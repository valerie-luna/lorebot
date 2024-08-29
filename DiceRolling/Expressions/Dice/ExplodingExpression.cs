using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record ExplodingExpression(NumericExpression Roll, NumericExpression Target, TargetType Type) : MultipleNumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitExploding(this);
}