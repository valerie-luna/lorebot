using System.Diagnostics;

namespace DiceRolling.Expressions.Results;

public abstract record NumericRollResult(decimal Total) : RollResult
{
    internal long TotalAsLong
    {
        get
        {
            Debug.Assert((long)Total == Total);
            return (long)Total;
        }
    }
}
