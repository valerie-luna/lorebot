using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Errors;

public record RepeatRollOverflowError(Expression Expression, int MaximumIterations) : Expression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitRepeatRollOverflow(this);
    }
}
