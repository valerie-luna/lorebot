// See https://aka.ms/new-console-template for more information

using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors.Base;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Errors;

public class ErrorDescriber() : ExpressionVisitor<string>
{
    private readonly DiscordRollVisitor rollv = new DiscordRollVisitor();

    protected override string VisitTypeError(NumericTypeError error)
    {
        return $"Type error in: `{rollv.Visit(error.Original)}`, expected '{error.DesiredType}' but got '{error.ActualType}'\n"
            + "This usually happens because you're trying to roll a dice with a decimal number of sides - dice can only be rolled with whole number sides.";
    }

    protected override string VisitDivZero(PossibleDivZeroError error)
    {
        return $"Potential divide by zero: `{rollv.Visit(error.Arith)}`\n"
            + "You can't use any values that might be zero when performing division or modulo.";
    }

    protected override string VisitRepeatRollOverflow(RepeatRollOverflowError error)
    {
        return $"Too many rolls: `{rollv.Visit(error)}`\n"
            + $"You may only roll up to {error.MaximumIterations} sets of dice.";
    }

    protected override string VisitMultipleOverflow(MultipleOverflowError error)
    {
        return $"Too many rolls: `{rollv.Visit(error.Multiple)}`\n"
            + $"You may only roll up to {error.MaximumIterations} sets of dice.";
    }

    protected override string VisitInfiniteNumeric(InfiniteNumericError error)
    {
        return $"Potential infinite numeric calculaton: `{rollv.Visit(error.Original)}`";
    }

    protected override string VisitMustBePositive(MustBePositiveError error)
    {
        return $"Value must be positive: `{rollv.Visit(error.Expression)}`\n"
            + "If the inner expression can evaluate to 0 (or negative numbers) in any circumstance, this error will occur.";
    }

    protected override string VisitSyntaxError(SyntaxError error)
    {
        return $"Syntax error: `{error.Roll[error.Start..error.End]}`\n"
            + "The dice roller didn't expect this to be here.";
    }
}
