using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Errors;

public sealed record InfiniteNumericError(NumericExpression Original) : MultipleNumericExpression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitInfiniteNumeric(this);
}