using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public record VersusExpression(NumericExpression Left, NumericExpression Right, VersusType Type) : NumericExpression
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitVersus(this);
}