using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record DiceExpression(NumericExpression Sides) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitDice(this);
}
