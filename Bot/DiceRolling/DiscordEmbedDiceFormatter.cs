using System.Text;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;
using DiceRolling.Formatting;
using DSharpPlus.Entities;

namespace Lore.DiceRolling;

public sealed class DiscordEmbedDiceFormatter : DiceFormatter<DiscordInteractionResponseBuilder>
{
    protected override DiscordInteractionResponseBuilder FormatError(Expression expr)
    {
        var builder = new DiscordInteractionResponseBuilder();
        var embed = new DiscordEmbedBuilder();
        var rollvis = new DiscordRollVisitor(UnderlineErrors: false);
        embed.Title = rollvis.Visit(expr).ToString();
        rollvis = new DiscordRollVisitor(UnderlineErrors: true);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Errors were detected in the highlighted segments:");
        sb.AppendLine(rollvis.Visit(expr).ToString());
        var desc = new ErrorDescriber();
        foreach (var err in new ErrorFinder().Visit(expr))
        {
            sb.AppendLine(desc.Visit(err));
        }
        embed.Description = sb.ToString();
        embed.Color = DiscordColor.Red;
        return builder.AddEmbed(embed);
    }

    protected override DiscordInteractionResponseBuilder FormatMultiple(Expression fullExpr, IEnumerable<Roll> rolls, string? reason)
    {
        var builder = new DiscordInteractionResponseBuilder();
        List<(string Name, string Value)> fields = [];
        
        var rollvis = new DiscordRollVisitor(UnderlineErrors: false);
        var printer = new DiceSetPrinterVisitor(sort: true);
        var total = new TotalVisitor();
        foreach (var roll in rolls)
        {
            string name;
            if (roll.Desugared is not null)
            {
                name = $"`{rollvis.Visit(roll.Expression)} ({rollvis.Visit(roll.Desugared)})`";
            }
            else
            {
                name = $"`{rollvis.Visit(roll.Expression)}`";
            }
            StringBuilder sb = new();
            sb.AppendLine($"**Result:** `{total.Visit(roll.Result)}`");
            sb.Append($"-# `{printer.Visit(roll.Result)}`");
            fields.Add((name, sb.ToString()));
        }

        List<DiscordEmbedBuilder> embeds = [new DiscordEmbedBuilder()];
        if (rolls.Count() > 1)
            embeds[0].WithTitle($"`{rollvis.Visit(fullExpr)}`");
        if (reason is not null)
            embeds[0].Description = reason;
        int count = 0;
        const int maxCount = 25;
        foreach (var (Name, Value) in fields)
        {
            embeds.Last().AddField(Name, Value, false);
            if (++count == maxCount)
            {
                count = 0;
                embeds.Add(new());
            }
        }
        embeds.ForEach(e => e.Color = DiscordColor.CornflowerBlue);
        return builder.AddEmbeds(embeds.Select(e => e.Build()));
    }

    protected override DiscordInteractionResponseBuilder FormatSingle(Expression fullExpr, Roll roll, string? reason)
    {
        return FormatMultiple(fullExpr, [roll], reason);
    }
}
