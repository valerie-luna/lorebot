using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record VersusResult(NumericRollResult Left, NumericRollResult Right, decimal WinningTotal, VersusType Type)
    : NumericRollResult(WinningTotal)
{
    public decimal AbsoluteResult => Math.Abs(WinningTotal);
    public bool Draw => WinningTotal == 0;
    public bool RightWinner => WinningTotal < 0;
    public bool LeftWinner => !RightWinner;

    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitVersus(this);
    }
}