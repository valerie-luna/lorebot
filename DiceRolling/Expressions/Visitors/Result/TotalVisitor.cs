using System.Diagnostics;
using System.Text;
using DiceRolling.Expressions.Results;

namespace DiceRolling.Expressions.Visitors.Result;

public class TotalVisitor : ResultVisitor<string>
{
    protected internal override string VisitGenesys(GenesysRollResult genesys)
    {
        return genesys.Total
            .GroupBy(g => g)
            .Aggregate(new StringBuilder(),
            (sb, g) => sb.Append($"{g.Key}: {g.Count()}, "), sb =>
            {
                if (sb.Length > 0)
                    sb.Length -= 2;
                return sb.ToString();
            });
    }

    protected internal override string VisitVersus(VersusResult vs)
    {
        switch (vs.Type)
        {
            case Dice.VersusType.Opposed:
            {
                var nethits = vs.AbsoluteResult;
                StringBuilder sb = new();
                sb.Append($"{vs.Left.Total} vs {vs.Right.Total}, ");
                if (vs.Draw)
                {
                    sb.Append("Draw");
                }
                else
                {
                    sb.Append($"{nethits} net hit{(nethits == 1 ? "" : "s")}");
                    if (vs.RightWinner)
                        sb.Append(" to opposition");
                }
                return sb.ToString();
            }
            case Dice.VersusType.Earthdawn:
            {
                var successes = Math.Max(0, vs.Total);
                if (vs.Total >= 0)
                {
                    if (successes == 1)
                    {
                        return $"{vs.Left.Total} vs {vs.Right.Total}, success with {successes} extra success";
                    }
                    else
                    {
                        return $"{vs.Left.Total} vs {vs.Right.Total}, success with {successes} extra successes";
                    }
                }
                else
                {
                    return $"{vs.Left.Total} vs {vs.Right.Total}, failure";
                }
            }
            default: throw new InvalidOperationException();
        }
    }

    protected internal override string VisitNumeric(NumericRollResult result)
    {
        return result.Total.ToString();
    }
}
