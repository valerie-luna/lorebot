using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record SingleRollResult(decimal Value) : NumericRollResult(Value)
{
    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitSingle(this);
    }

    public static implicit operator SingleRollResult(decimal val) => new(val);
}
