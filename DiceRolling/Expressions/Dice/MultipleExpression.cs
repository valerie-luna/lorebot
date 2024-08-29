using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record MultipleExpression(NumericExpression Count, NumericExpression Roll) : MultipleNumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitMultiple(this);
}
