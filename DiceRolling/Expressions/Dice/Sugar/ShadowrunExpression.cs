using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Sugar;

public sealed record ShadowrunExpression(NumericExpression DiceCount, NumericExpression? Limit, bool Exploding) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitShadowrun(this);
}
