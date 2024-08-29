using System.Collections.Immutable;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Result;
using static DiceRolling.Expressions.Genesys.GenesysToken;

namespace DiceRolling.Expressions.Results;

public record GenesysSetRollResult(ImmutableArray<GenesysRollResult> Elements) : GenesysRollResult
{
    public override IEnumerable<GenesysToken> Total => BalanceSet();

    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitGenesysSet(this);
    }

    private IEnumerable<GenesysToken> BalanceSet()
    {
        int SuccessFailures = 0;
        int AdvantagesThreats = 0;
        int Triumphs = 0;
        int Despairs = 0;
        foreach (GenesysToken token in Elements.SelectMany(e => e.Total))
        {
            switch (token)
            {
                case Success:
                    SuccessFailures++;
                    break;
                case Failure:
                    SuccessFailures--;
                    break;
                case Advantage:
                    AdvantagesThreats++;
                    break;
                case Threat:
                    AdvantagesThreats--;
                    break;
                case Triumph:
                    Triumphs++;
                    SuccessFailures++;
                    break;
                case Despair:
                    Despairs++;
                    SuccessFailures--;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        if (SuccessFailures > 0)
            foreach (int _ in Enumerable.Range(0, SuccessFailures)) yield return Success;
        if (Triumphs > 0)
            foreach (int _ in Enumerable.Range(0, Triumphs)) yield return Triumph;
        if (SuccessFailures < 0)
            foreach (int _ in Enumerable.Range(0, -SuccessFailures)) yield return Failure;
        if (Despairs > 0)
            foreach (int _ in Enumerable.Range(0, Despairs)) yield return Despair;
        if (AdvantagesThreats > 0)
            foreach (int _ in Enumerable.Range(0, AdvantagesThreats)) yield return Advantage;
        if (AdvantagesThreats < 0)
            foreach (int _ in Enumerable.Range(0, -AdvantagesThreats)) yield return Threat;
    }
}
