using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public record GenesysLiteralRollResult(GenesysToken Token) : GenesysRollResult
{
    public override IEnumerable<GenesysToken> Total { get { yield return Token; } }

    public override T Accept<T>(ResultVisitor<T> visitor)
    {
        return visitor.VisitGenesysLiteral(this);
    }
}
