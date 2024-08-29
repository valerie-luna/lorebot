using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record ArithmeticRollResult(NumericRollResult Left, NumericRollResult Right, ArithmeticType Type) : NumericRollResult(Type switch
{
    ArithmeticType.Add => Left.Total + Right.Total,
    ArithmeticType.Subtract => Left.Total - Right.Total,
    ArithmeticType.Multiply => Left.Total * Right.Total,
    ArithmeticType.Divide => Left.Total / Right.Total,
    ArithmeticType.Modulo => Left.Total % Right.Total,
    _ => throw new InvalidOperationException(),
})
{
    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitArithmetic(this);
    }
}
