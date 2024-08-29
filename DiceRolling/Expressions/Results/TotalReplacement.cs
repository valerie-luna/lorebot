using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record TotalReplacementRollResult(NumericRollResult Inner, decimal ReplacementTotal) : NumericRollResult(ReplacementTotal)
{
    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitReplacementTotal(this);
    }
}