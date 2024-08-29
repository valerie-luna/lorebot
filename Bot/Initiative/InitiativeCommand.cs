using System.Diagnostics;
using Lore.Discord;
using DSharpPlus;
using DSharpPlus.Entities;
using Initiative;
using Lore.Utilities;
using System.Text;
using Lore.DiceRolling;
using DiceRolling;
using System.ComponentModel.DataAnnotations;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;
using DiceRolling.Formatting;

namespace Lore.Initiative;

public class InitiativeCommand : ICommand
{
    private const DiscordPermissions AdvancedPermission = DiscordPermissions.Administrator;
    private readonly InitiativeModel init;

    public static string CustomId => "initiative_settings";

    public InitiativeCommand(InitiativeModel init)
    {
        this.init = init;
    }

    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        var reset = new DiscordApplicationCommandOptionBuilder()
            .WithName("reset")
            .WithDescription("Reset the initiative state.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .WithRequired(false);

        var next = new DiscordApplicationCommandOptionBuilder()
            .WithName("next")
            .WithDescription("Move to the next person in initiative order.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "ping",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to ping the current user",
                isRequired: false)
            .AddOption(
                name: "quiet",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to be quiet (as ephemeral message).",
                isRequired: false)
            .AddOption(
                name: "skip",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to skip all other turns and go to the next pass.",
                isRequired: false);

        var roll = new DiscordApplicationCommandOptionBuilder()
            .WithName("roll")
            .WithDescription("Roll initiative for a person.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "name",
                type: DiscordApplicationCommandOptionType.String,
                description: "The name of the person in initiative.",
                isRequired: true)
            .AddOption(
                name: "roll",
                type: DiscordApplicationCommandOptionType.String,
                description: "The initiative roll.",
                isRequired: true)
            .AddOption(
                name: "ping",
                type: DiscordApplicationCommandOptionType.User,
                description: "Who to ping when it's their turn",
                isRequired: false)
            .AddOption(
                name: "hidden",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to hide this roll from regular players.",
                isRequired: false);

        var mod = new DiscordApplicationCommandOptionBuilder()
            .WithName("modify")
            .WithDescription("Modify someone's initiative by the result of a roll.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "name",
                type: DiscordApplicationCommandOptionType.String,
                description: "The name of the person in initiative.",
                isRequired: true)
            .AddOption(
                name: "roll",
                type: DiscordApplicationCommandOptionType.String,
                description: "How to modify their score, as a dice roll.",
                isRequired: true);

        var check = new DiscordApplicationCommandOptionBuilder()
            .WithName("check")
            .WithDescription("Check initiative.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "ping",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to ping the current user",
                isRequired: false)
            .AddOption(
                name: "quiet",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to be quiet (as ephemeral message).",
                isRequired: false)
            .AddOption(
                name: "show-hidden",
                type: DiscordApplicationCommandOptionType.Boolean,
                description: "Whether to show hidden rolls.",
                isRequired: false);

        var delete = new DiscordApplicationCommandOptionBuilder()
            .WithName("delete")
            .WithDescription("Remove an entry from the tracker.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "name",
                type: DiscordApplicationCommandOptionType.String,
                description: "The name of the person in initiative.",
                isRequired: true);

        var reroll = new DiscordApplicationCommandOptionBuilder()
            .WithName("reroll")
            .WithDescription("Reroll your last initiative roll in this server.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand);

        var settings = new DiscordApplicationCommandOptionBuilder()
            .WithName("settings")
            .WithDescription("Configure settings.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "type",
                type: DiscordApplicationCommandOptionType.String,
                description: "The type of initiative to run with.",
                isRequired: true,
                choices: Enum.GetValues<InitiativeConfiguration>()
                    .Where(e => e != InitiativeConfiguration.Unknown)
                    .Select(e => (e.ToPretty(), e.ToString()))
                    .ToArray()
            );

        var unhide = new DiscordApplicationCommandOptionBuilder()
            .WithName("unhide")
            .WithDescription("Unhide a hidden roll.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption(
                name: "name",
                type: DiscordApplicationCommandOptionType.String,
                description: "The name of the person to unhide.",
                isRequired: true);


        builder.WithName("initiative")
            .WithDescription("Initiative management.")
            .WithDMPermission(false)
            .AddOption(check)
            .AddOption(reset)
            .AddOption(unhide)
            .AddOption(next)
            .AddOption(roll)
            .AddOption(mod)
            .AddOption(delete)
            .AddOption(reroll)
            .AddOption(settings);

        return "initiative";
    }

    public async Task Execute(DiscordInteraction command)
    {
        Task task;
        var option = command.Data.Options.Single();
        task = option.Name switch
        {
            "reset" => Reset(command),
            "next" => Next(command),
            "roll" => Roll(command),
            "modify" => Modify(command),
            "check" => Check(command),
            "delete" => Delete(command),
            "reroll" => Reroll(command),
            "settings" => Settings(command),
            "unhide" => Unhide(command),
            _ => throw new NotImplementedException()
        };
        await task;
    }

    private async Task Reset(DiscordInteraction command)
    {
        await CheckPermission(command, AdvancedPermission);

        var result = await init.Reset(new ChannelId(command.ChannelId, new(command.Guild.Id)));

        if (result.Result is ResultEnum.ConfigurationRequired)
            await command.RespondAsync("Please configure initiative with /initiative settings first.", ephemeral: true);
        else if (result.Result is ResultEnum.Success)
            await command.RespondAsync("Initiative reset in this channel.", ephemeral: true);
        else
        {
            Debug.Assert(false);
            throw new InvalidOperationException();
        }
    }

    private async Task Unhide(DiscordInteraction command)
    {
        await CheckPermission(command, AdvancedPermission);

        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("name").Trim();

        var result = await init.Unhide(
            name: name,
            channel: new (command.ChannelId, new (command.Guild.Id))
        );

        if (result.Result is ResultEnum.ConfigurationRequired)
            await command.RespondAsync("Please configure initiative with /initiative settings first.", ephemeral: true);
        else if (result.Result is ResultEnum.NotFound)
            await command.RespondAsync($"Could not find '{name}' in the initiative tracker.", ephemeral: true);
        else if (result.Result is ResultEnum.Success)
            await command.RespondAsync($"'{name}' has been unhidden.", ephemeral: true);
        else
        {
            Debug.Assert(false);
            throw new InvalidOperationException();
        }
    }

    private async Task Next(DiscordInteraction command)
    {
        await CheckPermission(command, AdvancedPermission);

        var subcommand = command.Data.Options.Single();
        var ping = subcommand.GetOptionValueOrDefault<bool?>("ping") ?? true;
        var quiet = subcommand.GetOptionValueOrDefault<bool?>("quiet") ?? false;
        var nextPass = subcommand.GetOptionValueOrDefault<bool?>("skip") ?? false;

        var result = await init.Next(
            new (command.ChannelId, new(command.Guild.Id)),
            nextPass
        );

        await PrintInitiative(command, ping, quiet, false, result.Initiative, result.Result);
    }

    private async Task Roll(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("name").Trim();
        var roll = subcommand.GetOptionValue<string>("roll").Trim();
        var userId = subcommand.GetOptionValueOrDefault<ulong?>("ping");
        var hidden = subcommand.GetOptionValueOrDefault<bool?>("hidden") ?? false;

        bool checkUserPermission = userId is not null && userId != command.User.Id;
        bool checkHiddenPermission = hidden;
        if (checkUserPermission || checkHiddenPermission)
        {
            await CheckPermission(command, AdvancedPermission);
        }

        var result = await init.Roll(
            channel: new(command.ChannelId, new(command.Guild.Id)),
            roll: roll,
            name: name,
            hidden: hidden,
            ping: PingId.From(userId),
            user: new(command.User.Id)
        );

        await PrintRoll(command, result, hidden);
    }

    private async Task Modify(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("name").Trim();
        var roll = subcommand.GetOptionValue<string>("roll");

        var result = await init.Modify(
            name: name,
            channel: new (command.ChannelId, new(command.Guild.Id)),
            roll: roll
        );

        await PrintRoll(command, result, false, message: "modified their initiative");
    }

    private async Task Check(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var ping = subcommand.GetOptionValueOrDefault<bool?>("ping") ?? false;
        var quiet = subcommand.GetOptionValueOrDefault<bool?>("quiet") ?? true;
        var showHidden = subcommand.GetOptionValueOrDefault<bool?>("show-hidden") ?? false;

        if (!quiet || showHidden)
        {
            await CheckPermission(command, AdvancedPermission);
        }

        var result = await init.Check(
            new(command.ChannelId, new (command.Guild.Id))
        );

        await PrintInitiative(command, ping, quiet, showHidden, result.Initiative, result.Result);
    }

    private async Task Delete(DiscordInteraction command)
    {
        await CheckPermission(command, AdvancedPermission);

        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("name").Trim();

        var result = await init.Delete(
            new(command.ChannelId, new(command.Guild.Id)),
            name
        );

        if (result.Result is ResultEnum.ConfigurationRequired)
            await command.RespondAsync("Please configure initiative with /initiative settings first.", ephemeral: true);
        else if (result.Result is ResultEnum.NotFound)
            await command.RespondAsync($"Could not find '{name}' in the initiative tracker.", ephemeral: true);
        else if (result.Result is ResultEnum.Success)
            await command.RespondAsync($"'{name}' has been removed from the initiative tracker.", ephemeral: true);
        else
        {
            Debug.Assert(false);
            throw new InvalidOperationException();
        }
    }

    private async Task Reroll(DiscordInteraction command)
    {
        var result = await init.Reroll(
            new(command.ChannelId, new(command.Guild.Id)),
            new(command.User.Id)
        );

        await PrintRoll(command, result, false);
    }

    private async Task Settings(DiscordInteraction command)
    {
        await CheckPermission(command, AdvancedPermission);

        var subcommand = command.Data.Options.Single();
        var config = subcommand.GetOptionValue<string>("type").ParseEnum<InitiativeConfiguration>();

        var result = await init.SetServerInitiativeType(new(command.Guild.Id), config);

        Debug.Assert(result is ResultEnum.Success);

        await command.RespondAsync("Initiative settings updated.", ephemeral: true);
    }

    private static async Task PrintRoll(DiscordInteraction command, InitiativeRollResult result, 
        bool hidden, string message = "rolled initiative")
    {
        if (result.Result is ResultEnum.ConfigurationRequired)
            await command.RespondAsync("Please configure initiative with /initiative settings first.", ephemeral: true);
        else if (result.Result is ResultEnum.NeedsGenesysRoll or ResultEnum.NeedsNumericalRoll or ResultEnum.MultipleRolls)
        {
            var msg = "Current roll settings do not support this kind of roll for initiative.\n";
            msg += result.Result switch
            {
                ResultEnum.MultipleRolls => "Cannot roll multiple rolls for initiative.",
                ResultEnum.NeedsGenesysRoll => "You need to roll a Genesys-type roll.",
                ResultEnum.NeedsNumericalRoll => "You need to roll a standard, numeric roll.",
                _ => throw new InvalidOperationException()
            };
            await command.RespondAsync(msg, ephemeral: true);
        }
        else if (result.Result is ResultEnum.RollError)
        {
            Debug.Assert(result.Expression is not null);
            var rollvis = new DiscordRollVisitor(UnderlineErrors: true);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Errors were detected in the highlighted segments:");
            sb.AppendLine(rollvis.Visit(result.Expression).ToString());
            var desc = new ErrorDescriber();
            foreach (var err in new ErrorFinder().Visit(result.Expression))
            {
                sb.AppendLine(desc.Visit(err));
            }
            await command.RespondAsync(sb.ToString());
        }
        else if (result.Result is ResultEnum.Success)
        {
            Debug.Assert(result.Roll is not null);
            var rollvis = new DiscordRollVisitor();
            var printer = new DiceSetPrinterVisitor(sort: true);
            FormattableString request = rollvis.Visit(result.Expression!);
            var desugared = new DesugarVisitor().Visit(result.Expression!);
            if (desugared != result.Expression)
                request = $"{request} ({rollvis.Visit(desugared)})";
            StringBuilder builder = new StringBuilder();
            builder.Append($"{result.Name} {message}: **Request:** `[{request}]` **Dice:** ");
            builder.Append(printer.Visit(result.Roll));
            builder.Append($" **Result:** ");
            builder.Append(new TotalVisitor().Visit(result.Roll));
            var reason = new ReasonExtractor().Visit(result.Expression!);
            if (reason is not null)
            {
                builder.Append($" **Reason**: {reason}");
            }
            await command.RespondAsync(builder.ToString(), ephemeral: hidden);
        }
        else
        {
            Debug.Assert(false);
            throw new InvalidOperationException();
        }
    }

    private static async Task PrintInitiative(DiscordInteraction command, bool ping, bool quiet, bool showHidden,
        InitiativeList initiative, ResultEnum result)
    {
        StringBuilder sb = new StringBuilder();
        string? content = null;
        PingId? mention = null;

        switch (result)
        {
            case ResultEnum.NextPass:
                sb.AppendLine("Next pass.");
            goto case ResultEnum.Success;
            case ResultEnum.Success:
                var first = GetFirst();
                if (first is null)
                    sb.Append("Turn over.");
                else
                {
                    if (first.Hidden && !showHidden)
                        sb.Append("It's a hidden character's turn.");
                    else
                    {
                        sb.Append($"It's {first.Name}'s turn.");
                        if (ping && first.Ping is not null)
                        {
                            mention = first.Ping;
                            content = $"<@{first.Ping}>";
                        }
                    }
                }
                break;
            case ResultEnum.ConfigurationRequired:
                await command.RespondAsync("Please configure initiative with /initiative settings first.", ephemeral: true);
                return;
            case ResultEnum.RerollRequired:
                sb.Append("Turn over.\nPlease reroll initiative.");
                break;
            default:
                Debug.Assert(false);
                throw new InvalidOperationException();
        }

        sb.AppendLine();
        sb.AppendLine();

        int i = 1;
        foreach (var init in initiative.Entries.Where(i => !i.Hidden || showHidden))
        {
            string hidden = init.Hidden ? " *(hidden)*" : "";
            string acted = init.HasActed ? "~~" : "";
            if (i == 100)
            {
                sb.AppendLine("Warning - too many initiative results to display. Initiative may be buggy.");
            }
            else
            {
                sb.AppendLine($"`({i++,2})` {acted}**{init.Value.ToDiscordString()}** {init.Name}{hidden}{acted}");
            }
        }

        var embed = new DiscordEmbedBuilder()
            .WithTitle($"Initiative: {command.Channel.Name}")
            .WithDescription(sb.ToString());

        var builder = new DiscordInteractionResponseBuilder()
            .AddEmbed(embed);
        
        if (content is not null)
            builder = builder.WithContent(content);

        if (mention is not null)
            builder.AddMention(new UserMention(mention.Value.Id));

        if (quiet)
            builder = builder.AsEphemeral();

        await command.CreateResponseAsync(
            DiscordInteractionResponseType.ChannelMessageWithSource,
            builder
        );

        InitiativeEntry? GetFirst()
        {
            return initiative.Entries.FirstOrDefault(i => !i.HasActed);
        }
    }

    private static async Task CheckPermission(DiscordInteraction command, DiscordPermissions permission)
    {
        var user = command.User as DiscordMember;
        Debug.Assert(user is not null);
        if ((user.Permissions & permission) == 0)
        {
            await command.RespondAsync("You are not permitted to use this command.", ephemeral: true);
            throw new CommandFinishedException();
        }
    }
}

public static class Utilities
{
    public static string ToDiscordString(this InitiativeSet set)
    {
        if (set is NumericalInitiative nu)
            return nu.Initiative.ToString();
        else if (set is GenesysInitiative gen)
        {
            StringBuilder sb = new();
            bool first = true;
            if (gen.Successes != 0)
            {
                PrintComma();
                sb.Append($"{DiscordResultVisitor.GenesysSuccess} {gen.Successes}");
            }
            if (gen.Advantages != 0)
            {
                PrintComma();
                sb.Append($"{DiscordResultVisitor.GenesysAdvantage} {gen.Advantages}");
            }
            if (gen.Triumphs != 0)
            {
                PrintComma();
                sb.Append($"{DiscordResultVisitor.GenesysTriumph} {gen.Triumphs}");
            }
            if (gen.Successes == 0 && gen.Advantages == 0 && gen.Triumphs == 0)
            {
                sb.Append("Nothing!");
            }
            return sb.ToString();
            void PrintComma()
            {
                if (first)
                    first = false;
                else
                    sb.Append(", ");
            }
        }
        else
        {
            Debug.Assert(false);
            throw new InvalidOperationException();
        }
    }

    public static string ToPretty(this InitiativeConfiguration config)
    {
        return config switch
        {
            InitiativeConfiguration.DungeonsAndDragons => "Dungeons & Dragons and other simple initiative systems",
            InitiativeConfiguration.ShadowrunStandard => "2e-3e, 5e Shadowrun",
            InitiativeConfiguration.ShadowrunLethal => "1e, alternate 5e Shadowrun",
            InitiativeConfiguration.Genesys => "Genesys",
            _ => throw new InvalidOperationException(),
        };
    }
}