using System.Diagnostics;

namespace DiceRolling.Expressions.Visitors;

public readonly struct DiceRange
{
    private readonly SatDec low;
    private readonly SatDec high;
    private readonly ExpressionType type;
    public DiceRange(ExpressionType Type, SatDec Low, SatDec High)
    {
        Debug.Assert(Low <= High);
        this.low = Low;
        this.high = High;
        this.type = Type;
    }

    public readonly SatDec Low => low;
    public readonly SatDec High => high;
    public readonly ExpressionType Type => type;

    public override string ToString()
    {
        return $"{{{Type}: {Low} - {High}}}";
    }

    public readonly bool ReachesPositiveInfinity => High == SatDec.Inf;
    public readonly bool ReachesNegativeInfinity => Low == SatDec.NegInf;
    public readonly bool ReachesAnyInfinity => ReachesPositiveInfinity || ReachesNegativeInfinity;
}
