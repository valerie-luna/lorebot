using System.Diagnostics;
using DiceRolling;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Results;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;
using Initiative.Database;
using Microsoft.EntityFrameworkCore;
using static Initiative.InitiativeConfiguration;

namespace Initiative;

public class InitiativeModel : IInitiativeModel
{
    private readonly InitiativeContext ctx;

    public InitiativeModel(InitiativeContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<InitiativeResult> Check(ChannelId channel)
    {
        return new InitiativeResult(ResultEnum.Success, new(await GetInitiatives(channel)));
    }

    public async Task<InitiativeResult> Delete(ChannelId channel, string name)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel);

        var init = await ctx.Initiatives
            .Where(i => i.Name == name)
            .Where(i => i.ChannelId == channel.Id)
            .SingleOrDefaultAsync();

        if (init is null)
            return await Result(ResultEnum.NotFound, channel);

        ctx.Remove(init);
        await ctx.SaveChangesAsync();

        return await Result(ResultEnum.Success, channel);
    }

    public async Task<InitiativeRollResult> Modify(ChannelId channel, string name, string roll)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel, null, null, name);
        
        var init = await ctx.Initiatives
            .Where(i => i.Name == name)
            .Where(i => i.ChannelId == channel.Id)
            .SingleOrDefaultAsync();

        if (init is null)
            return await Result(ResultEnum.NotFound, channel, null, null, name);

        var haserrorvis = new ErrorFinder();
        var fullrollexpr = new DiceRoller().Parse(roll);

        if (haserrorvis.HasAny(fullrollexpr))
            return await Error(channel, fullrollexpr, name);

        var subrolls = new RollFinder().Visit(fullrollexpr).ToArray();
        
        if (subrolls.Length != 1)
            return await Result(ResultEnum.MultipleRolls, channel, null, fullrollexpr, name);

        var rollexpr = subrolls.Single();
        var result = new RollExecutor(new Random()).Visit(rollexpr);
        var set = new InitiativeSetVisitor().Visit(result);
        
        if (settings.Config is Genesys && set is not GenesysInitiative 
            || settings.Config is not Genesys && set is not NumericalInitiative)
        {
            var type = set is GenesysInitiative
                ? ResultEnum.NeedsNumericalRoll
                : ResultEnum.NeedsGenesysRoll;
            return await Result(type, channel, null, fullrollexpr, name);
        }

        set = ToInitiativeSet(init, settings.Config) + set;
        (init.PrimaryValue, init.SecondaryValue, init.TeritaryValue) = set;
        await ctx.SaveChangesAsync();

        return await Result(ResultEnum.Success, channel, result, fullrollexpr, name);
    }

    public async Task<InitiativeResult> Next(ChannelId channel, bool skipToEnd)
    {
        var settings = await GetSettings(channel.Server);
        
        if (settings.Config == Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel);

        var initiatives = await ctx.Initiatives
            .Where(i => i.ChannelId == channel.Id)
            .OrderByDescending(o => o.PrimaryValue)
            .ThenByDescending(o => o.SecondaryValue)
            .ThenByDescending(o => o.TeritaryValue)
            .ToListAsync();

        var currentInitiative = GetCurrentInitiative(initiatives, settings.Config);

        if (currentInitiative == null || skipToEnd)
        {
            // we are at the end of the initiative round
            // ShadowrunLethal: it's time to re-roll
            // ShadowrunStandard: if all inits < 0, turn over reroll time
            // otherwise pass over, subtract all initiatives by 10
            // D&D: restart from start, no need to reroll
            // Genesys: restart from start, no need to reroll
            if (initiatives.Count == 0)
                return await Result(ResultEnum.Success, channel);
            switch (settings.Config)
            {
                case ShadowrunStandard:
                    initiatives.ForEach(i => i.TimesActed = 0);
                    initiatives.ForEach(i => i.PrimaryValue -= 10);
                    initiatives.ForEach(i => { if (i.PrimaryValue <= 0) i.TimesActed = 1; });
                    await ctx.SaveChangesAsync();
                    return initiatives.Any(i => i.PrimaryValue > 0)
                        ? await Result(ResultEnum.NextPass, channel)
                        : await Result(ResultEnum.RerollRequired, channel);
                case ShadowrunLethal:
                    return await Result(ResultEnum.RerollRequired, channel);
                case DungeonsAndDragons:
                case Genesys:
                    initiatives.ForEach(i => i.TimesActed = 0);
                    currentInitiative = GetCurrentInitiative(initiatives, settings.Config);
                    Debug.Assert(currentInitiative is not null);
                    await ctx.SaveChangesAsync();
                    return await Result(ResultEnum.Success, channel);
                case Unknown:
                default:
                    Debug.Assert(false);
                    throw new InvalidOperationException();
            }
        }

        currentInitiative.TimesActed++;
        await ctx.SaveChangesAsync();

        return await Result(ResultEnum.Success, channel);

        static InitiativeEntity? GetCurrentInitiative(IEnumerable<InitiativeEntity> initiatives, InitiativeConfiguration config)
        {
            if (config == ShadowrunLethal)
            {
                InitiativeEntity? flag = null;
                decimal highestFlag = 0;
                foreach (var init in initiatives)
                {
                    if (init.TimesActed > 0)
                    {
                        decimal virtualInit = init.PrimaryValue - (10 * init.TimesActed);
                        if (virtualInit > 0 && virtualInit > highestFlag)
                        {
                            highestFlag = virtualInit;
                            flag = init;
                        }
                    }
                    else
                    {
                        if (init.PrimaryValue > highestFlag)
                            return init;
                        else
                            return flag;
                    }
                }
                return highestFlag > 0 ? flag : null;
            }
            else if (config == ShadowrunStandard)
            {
                return initiatives.Where(i => i.PrimaryValue > 0).FirstOrDefault(i => i.TimesActed == 0);
            }
            else
            {
                return initiatives.FirstOrDefault(i => i.TimesActed == 0);
            }
        }
    }

    public async Task<InitiativeRollResult> Reroll(ChannelId channel, UserId user)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel, null, null, null);

        var last = await ctx.LastRolls
            .Where(r => r.UserId == user.Id)
            .Where(r => r.ChannelId == channel.Id)
            .SingleOrDefaultAsync();

        if (last is null)
            return await Result(ResultEnum.NotFound, channel, null, null, null);

        var haserrorvis = new ErrorFinder();
        var fullrollexpr = new DiceRoller().Parse(last.Roll);

        if (haserrorvis.HasAny(fullrollexpr))
            return await Error(channel, fullrollexpr, last.Name);

        var subrolls = new RollFinder().Visit(fullrollexpr).ToArray();
        
        if (subrolls.Length != 1)
            return await Result(ResultEnum.MultipleRolls, channel, null, fullrollexpr, last.Name);

        var rollexpr = subrolls.Single();
        var result = new RollExecutor(new Random()).Visit(rollexpr);
        var set = new InitiativeSetVisitor().Visit(result);

        var init = await GetOrCreate(last.Name, channel);

        if (settings.Config is Genesys && set is not GenesysInitiative 
            || settings.Config is not Genesys && set is not NumericalInitiative)
        {
            var type = set is GenesysInitiative
                ? ResultEnum.NeedsNumericalRoll
                : ResultEnum.NeedsGenesysRoll;
            return await Result(type, channel, null, fullrollexpr, last.Name);
        }

        (init.PrimaryValue, init.SecondaryValue, init.TeritaryValue) = set;
        init.TimesActed = 0;
        init.Hidden = last.Hidden;
        init.PingUser = last.PingUser;

        await ctx.SaveChangesAsync();
        return await Result(ResultEnum.Success, channel, result, fullrollexpr, last.Name);
    }

    public async Task<InitiativeResult> Reset(ChannelId channel)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel);

        var items = await ctx.Initiatives
            .Where(i => i.ChannelId == channel.Id)
            .ToListAsync();

        ctx.RemoveRange(items);

        await ctx.SaveChangesAsync();

        return await Result(ResultEnum.Success, channel);
    }

    public async Task<InitiativeRollResult> Roll(ChannelId channel, string roll, string name, bool hidden,
        PingId? ping, UserId user)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel, null, null, name);

        var haserrorvis = new ErrorFinder();
        var fullrollexpr = new DiceRoller().Parse(roll);

        if (haserrorvis.HasAny(fullrollexpr))
            return await Error(channel, fullrollexpr, name);

        var subrolls = new RollFinder().Visit(fullrollexpr).ToArray();
        
        if (subrolls.Length != 1)
            return await Result(ResultEnum.MultipleRolls, channel, null, fullrollexpr, name);

        var rollexpr = subrolls.Single();
        var result = new RollExecutor(new Random()).Visit(rollexpr);
        var set = new InitiativeSetVisitor().Visit(result);

        var init = await GetOrCreate(name, channel);

        if (settings.Config is Genesys && set is not GenesysInitiative 
            || settings.Config is not Genesys && set is not NumericalInitiative)
        {
            var type = set is GenesysInitiative
                ? ResultEnum.NeedsNumericalRoll
                : ResultEnum.NeedsGenesysRoll;
            return await Result(type, channel, null, fullrollexpr, name);
        }

        (init.PrimaryValue, init.SecondaryValue, init.TeritaryValue) = set;
        init.TimesActed = 0;
        init.Hidden = hidden;
        init.PingUser = ping?.Id;
        init.ServerId = channel.Server.Id;

        var last = await ctx.LastRolls
            .Where(r => r.UserId == user.Id)
            .Where(r => r.ChannelId == channel.Id)
            .SingleOrDefaultAsync();

        if (last is null)
        {
            last = new LastRollEntity
            {
                UserId = user.Id,
                ChannelId = channel.Id
            };
            ctx.Add(last);
        }

        last.Name = name;
        last.Roll = roll;
        last.Hidden = hidden;
        last.PingUser = ping?.Id;

        await ctx.SaveChangesAsync();
        return await Result(ResultEnum.Success, channel, result, fullrollexpr, name);
    }

    public async Task<ResultEnum> SetServerInitiativeType(ServerId server, InitiativeConfiguration config)
    {
        Debug.Assert(config is not Unknown && Enum.IsDefined(config));
        var settings = await GetSettings(server);

        settings.Config = config;

        var initiatives = await ctx.Initiatives
            .Where(i => i.ServerId == server.Id)
            .ToArrayAsync();

        ctx.RemoveRange(initiatives);
        
        await ctx.SaveChangesAsync();
        
        return ResultEnum.Success;
    }

    public async Task<InitiativeResult> Unhide(ChannelId channel, string name)
    {
        var settings = await GetSettings(channel.Server);

        if (settings.Config is Unknown)
            return await Result(ResultEnum.ConfigurationRequired, channel);

        var init = await ctx.Initiatives
            .Where(i => i.Name == name)
            .Where(i => i.ChannelId == channel.Id)
            .SingleOrDefaultAsync();

        if (init is null)
            return await Result(ResultEnum.NotFound, channel);

        init.Hidden = false;

        await ctx.SaveChangesAsync();

        return await Result(ResultEnum.Success, channel);
    }

    private async Task<InitiativeResult> Result(ResultEnum result, ChannelId channel)
        => new InitiativeResult(result, new(await GetInitiatives(channel)));

    private async Task<InitiativeRollResult> Result(ResultEnum result, ChannelId channel, RollResult? roll, 
        Expression? expr, string? name)
        => new InitiativeRollResult(result, new(await GetInitiatives(channel)), name, roll, expr);

    private async Task<InitiativeRollResult> Error(ChannelId channel, Expression expr, string name)
        => new InitiativeRollResult(ResultEnum.RollError, new(await GetInitiatives(channel)), name, null, expr);

    private async Task<ServerSettingsEntity> GetSettings(ServerId serverId)
    {
        var settings = await ctx.ServerSettings.SingleOrDefaultAsync(s => s.ServerId == serverId.Id);

        if (settings is null)
        {
            settings = new ServerSettingsEntity
            {
                ServerId = serverId.Id,
                Config = Unknown
            };
            ctx.Add(settings);
            await ctx.SaveChangesAsync();
        }

        return settings;
    }

    private async Task<List<InitiativeEntry>> GetInitiatives(ChannelId channel)
    {
        return await ctx.Initiatives
            .Where(i => i.ChannelId == channel.Id)
            .Include(e => e.ServerSettings)
            .AsAsyncEnumerable()
            .SelectMany(e => Entry(e, e.ServerSettings.Config))
            .OrderByDescending(o => o)
            .Take(100)
            .ToListAsync();

#pragma warning disable CS1998
        static async IAsyncEnumerable<InitiativeEntry> Entry(InitiativeEntity entity, 
            InitiativeConfiguration config)
#pragma warning restore CS1998
        {
            Debug.Assert(config is not Unknown);
            if (config is ShadowrunLethal)
            {
                Debug.Assert(entity.SecondaryValue == 0);
                Debug.Assert(entity.TeritaryValue == 0);
                decimal value = entity.PrimaryValue;
                int times = 0;
                while (value > 0)
                {
                    if (times >= 19)
                    {
                        yield return new InitiativeEntry(
                            $"{entity.Name} (overflow - any extra passes have been truncated)",
                            new NumericalInitiative(value),
                            entity.TimesActed > times++,
                            entity.Hidden,
                            PingId.From(entity.PingUser)
                        );
                        yield break;
                    }
                    yield return new InitiativeEntry(
                        entity.Name,
                        new NumericalInitiative(value),
                        entity.TimesActed > times++,
                        entity.Hidden,
                        PingId.From(entity.PingUser)
                    );
                    value -= 10;
                }
            }
            else
            {
                yield return new InitiativeEntry(
                    Name: entity.Name,
                    Value: ToInitiativeSet(entity, config),
                    HasActed: entity.TimesActed > 0,
                    Hidden: entity.Hidden,
                    Ping: PingId.From(entity.PingUser)
                );
            }
        }
    }

    private async Task<InitiativeEntity> GetOrCreate(string name, ChannelId channel)
    {
        var init = await ctx.Initiatives.SingleOrDefaultAsync(i => i.Name == name && i.ChannelId == channel.Id);

        if (init is null)
        {
            init = new InitiativeEntity
            {
                Name = name,
                ChannelId = channel.Id
            };
            ctx.Add(init);
        }

        return init;
    }

    private static InitiativeSet ToInitiativeSet(InitiativeEntity entity, InitiativeConfiguration config)
    {
        switch (config)
        {
            case DungeonsAndDragons:
            case ShadowrunStandard:
            case ShadowrunLethal:
                Debug.Assert(entity.SecondaryValue == 0);
                Debug.Assert(entity.TeritaryValue == 0);
                return new NumericalInitiative(entity.PrimaryValue);
            case Genesys:
                return new GenesysInitiative(
                    Successes: entity.PrimaryValue,
                    Advantages: entity.SecondaryValue,
                    Triumphs: entity.TeritaryValue
                );
            case Unknown:
            default:
                throw new InvalidOperationException();
        }
    }

    private class InitiativeSetVisitor : ResultVisitor<InitiativeSet>
    {
        protected override InitiativeSet VisitGenesys(GenesysRollResult genesys)
        {
            return new GenesysInitiative(
                Successes: genesys.Total
                    .Where(dr => dr is GenesysToken.Success or GenesysToken.Failure)
                    .Select(dr => dr == GenesysToken.Success ? 1 : -1)
                    .Sum(),
                Advantages: genesys.Total
                    .Where(dr => dr is GenesysToken.Advantage or GenesysToken.Threat)
                    .Select(dr => dr == GenesysToken.Advantage ? 1 : -1)
                    .Sum(),
                Triumphs: genesys.Total
                    .Where(dr => dr is GenesysToken.Triumph or GenesysToken.Despair)
                    .Select(dr => dr == GenesysToken.Triumph ? 1 : -1)
                    .Sum()
            );
        }

        protected override InitiativeSet VisitNumeric(NumericRollResult result)
        {
            return new NumericalInitiative(result.Total);
        }
    }
}