using System.Collections.Immutable;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Genesys;

public sealed record GenesysSetExpression(ImmutableArray<GenesysExpression> Expressions) : GenesysExpression
{
    public sealed override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitGenesysSet(this);
}