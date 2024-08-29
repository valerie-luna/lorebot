using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record StackingExplodingExpression(NumericExpression Roll, NumericExpression Target, TargetType Type) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitStackingExploding(this);
}
