using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Formatting;

public class ReasonExtractor : ExpressionVisitor<string?>
{
    protected internal override string? VisitRoll(RollExpression roll)
    {
        return roll.Reason;
    }

    protected internal override string? VisitExpression(Expression expression)
    {
        return null;
    }
}