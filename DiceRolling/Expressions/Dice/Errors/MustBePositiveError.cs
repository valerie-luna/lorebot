using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Errors;

public sealed record MustBePositiveError(NumericExpression Expression) : MultipleNumericExpression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitMustBePositive(this);
}
