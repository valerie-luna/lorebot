using System.Collections.Immutable;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record GenesysDiceRollResult(ImmutableArray<GenesysToken> Result) : GenesysRollResult
{
    public override IEnumerable<GenesysToken> Total => Result;

    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitGenesysDice(this);
    }
}
