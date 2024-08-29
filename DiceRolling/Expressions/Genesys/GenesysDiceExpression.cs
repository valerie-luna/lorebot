using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Genesys;

public sealed record GenesysDiceExpression(GenesysDice Dice) : GenesysExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitGenesysDice(this);
}
