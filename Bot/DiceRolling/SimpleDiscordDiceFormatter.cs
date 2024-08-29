using System.Text;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;
using DiceRolling.Formatting;
using DSharpPlus.Entities;

namespace Lore.DiceRolling;

public sealed class SimpleDiscordDiceFormatter : DiceFormatter<DiscordInteractionResponseBuilder>
{
    protected override DiscordInteractionResponseBuilder FormatError(Expression expr)
    {
        var rollvis = new DiscordRollVisitor(UnderlineErrors: true);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Errors were detected in the highlighted segments:");
        sb.AppendLine(rollvis.Visit(expr).ToString());
        var desc = new ErrorDescriber();
        foreach (var err in new ErrorFinder().Visit(expr))
        {
            sb.AppendLine(desc.Visit(err));
        }
        return new DiscordInteractionResponseBuilder()
            .WithContent(sb.ToString());
    }

    protected override DiscordInteractionResponseBuilder FormatSingle(Expression fullExpr, Roll roll, string? reason)
    {
        var rollvis = new DiscordRollVisitor();
        var printer = new DiceSetPrinterVisitor(sort: true);
        var total = new TotalVisitor();
        FormattableString request = rollvis.Visit(roll.Expression);
        if (roll.Desugared is not null)
            request = $"{request} ({rollvis.Visit(roll.Desugared)})";
        var final = $"**Request:** `[{request}]` "
            + $"**Dice:** `{printer.Visit(roll.Result)}` "
            + $"**Result:** `{total.Visit(roll.Result)}`";
        if (reason is not null)
            final = $"{final} **Reason:** {reason}";
        return new DiscordInteractionResponseBuilder()
            .WithContent(final);
    }

    protected override DiscordInteractionResponseBuilder FormatMultiple(Expression fullExpr, IEnumerable<Roll> rolls, string? reason)
    {
        var rollvis = new DiscordRollVisitor();
        var printer = new DiceSetPrinterVisitor(sort: true);
        var total = new TotalVisitor();
        StringBuilder sb = new StringBuilder();
        sb.Append($"**Request**: {rollvis.Visit(fullExpr)}");
        if (reason is not null)
            sb.Append($" **Reason:** {reason}");
        sb.AppendLine($"\nResults for {rolls.Count()} rolls");
        foreach (var (Expression, Desugared, Result, Seed) in rolls)
        {
            FormattableString request = rollvis.Visit(Expression);
            if (Desugared is not null)
                request = $"{request} ({rollvis.Visit(Desugared)})";
            sb.AppendLine($"**Request:** `[{request}]` "
            + $"**Dice:** `{printer.Visit(Result)}` "
            + $"**Result:** `{total.Visit(Result)}`");
        }
        return new DiscordInteractionResponseBuilder()
            .WithContent(sb.ToString());
    }
}
