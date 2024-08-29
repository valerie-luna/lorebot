using System.Collections.Immutable;
using DiceRolling.Expressions.Results;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Visitors;

public record SetReplacementRollResult(ImmutableArray<NumericRollResult> Set, ResultSet Original) : ResultSet(Set)
{
    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitReplacementSet(this);
    }
}
