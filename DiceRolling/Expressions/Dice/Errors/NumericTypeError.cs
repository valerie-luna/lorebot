using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Errors;

public sealed record NumericTypeError(NumericExpression Original, ExpressionType ActualType, 
    ExpressionType DesiredType) : MultipleNumericExpression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitTypeError(this);
}
