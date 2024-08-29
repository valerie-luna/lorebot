using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Errors;

public sealed record PossibleDivZeroError(ArithmeticExpression Arith) : NumericExpression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitDivZero(this);
}
