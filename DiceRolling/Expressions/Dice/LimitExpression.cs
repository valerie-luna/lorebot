using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record LimitExpression(NumericExpression Value, NumericExpression Limit) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitLimit(this);
}
