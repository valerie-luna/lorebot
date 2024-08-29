using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Sugar;

public sealed record EarthdawnExpression(NumericExpression Step) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitEarthdawn(this);
}