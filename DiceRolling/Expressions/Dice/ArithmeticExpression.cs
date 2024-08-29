using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record ArithmeticExpression(NumericExpression Left, NumericExpression Right, ArithmeticType ArithType) 
    : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitArithmetic(this);
}
