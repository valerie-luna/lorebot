using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice;

public sealed record KeepExpression(MultipleNumericExpression Set, NumericExpression Keep, KeepType Type) : MultipleNumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitKeep(this);
}
