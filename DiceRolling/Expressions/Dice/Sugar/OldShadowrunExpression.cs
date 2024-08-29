using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Sugar;

public sealed record OldShadowrunExpression(NumericExpression DiceCount, NumericExpression? Target) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitOldShadowrun(this);
}
