using DiceRolling.Expressions.Genesys;

namespace DiceRolling.Expressions.Results;

public abstract record GenesysRollResult : RollResult
{
    public abstract IEnumerable<GenesysToken> Total { get; }
}
