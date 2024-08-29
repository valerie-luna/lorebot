using System.Collections.Immutable;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions;

public record RollExpression(ImmutableArray<Expression> Expressions, string? Reason) : Expression
{
    public override T Accept<T>(ExpressionVisitor<T> visitor) => visitor.VisitRoll(this);
}