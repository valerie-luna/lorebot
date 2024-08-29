using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Genesys;

public sealed record GenesysLiteralExpression(GenesysToken Value) : GenesysExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitGenesysLiteral(this);
}
