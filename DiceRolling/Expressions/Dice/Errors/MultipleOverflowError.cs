using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Errors;

public record MultipleOverflowError(NumericExpression Multiple, int MaximumIterations) : MultipleNumericExpression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitMultipleOverflow(this);
}