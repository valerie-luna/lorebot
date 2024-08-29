using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

[CollectionBuilder(typeof(ResultSetBuilder), nameof(ResultSetBuilder.Create))]
public record ResultSet(ImmutableArray<NumericRollResult> Set) : NumericRollResult(Set.Aggregate(0m, (a, b) => a + b.Total)), IEnumerable<NumericRollResult>
{
    public IEnumerator<NumericRollResult> GetEnumerator() => ((IEnumerable<NumericRollResult>)Set).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Set).GetEnumerator();

    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitSet(this);
    }
}
