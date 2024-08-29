using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Dice.Sugar;

public sealed record StarwarsExpression(NumericExpression DiceCount) : NumericExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitStarWars(this);
}
