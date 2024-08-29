using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions;

public record RepeatRollExpression(Expression Expression, int Count) : Expression
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitRepeatRoll(this);
}
