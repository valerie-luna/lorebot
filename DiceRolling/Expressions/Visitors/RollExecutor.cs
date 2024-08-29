using System.Collections.Immutable;
using System.Diagnostics;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Results;
using DiceRolling.Expressions.Visitors.Base;
using static DiceRolling.Expressions.Genesys.GenesysToken;

namespace DiceRolling.Expressions.Visitors;

public class RollExecutor(Random random) : ExpressionVisitor<RollResult>
{
    const int IterationMax = 100;

    private readonly Random random = random;

    private new NumericRollResult VisitNumeric(NumericExpression num) => (NumericRollResult)Visit(num);

    protected internal override SingleRollResult VisitLiteral(LiteralExpression literal)
    {
        return literal.Value;
    }

    protected internal override SingleRollResult VisitDice(DiceExpression dice)
    {
        var sides = VisitNumeric(dice.Sides).TotalAsLong;
        return random.NextInt64(1, sides + 1);
    }

    protected internal override ResultSet VisitMultiple(MultipleExpression mult)
    {
        var count = VisitNumeric(mult.Count).TotalAsLong;
        return [.. Utilities.Repeat(mult.Roll, count).Select(VisitNumeric)];
    }

    protected internal override NumericRollResult VisitArithmetic(ArithmeticExpression arith)
    {
        return new ArithmeticRollResult(
            VisitNumeric(arith.Left),
            VisitNumeric(arith.Right),
            arith.ArithType
        );
    }

    protected internal override NumericRollResult VisitExploding(ExplodingExpression explode)
    {
        var first = VisitNumeric(explode.Roll);
        var target = VisitNumeric(explode.Target);
        if (Compare(first, target, explode.Type))
        {
            return (ResultSet)([first, VisitNumeric(explode.Roll)]);
        }
        else
        {
            return first;
        }
    }

    protected internal override NumericRollResult VisitIndefiniteExploding(IndefiniteExplodingExpression explode)
    {
        List<NumericRollResult> results = [VisitNumeric(explode.Roll)];
        var target = VisitNumeric(explode.Target);
        int iterations = 0;
        while (Compare(results.Last(), target, explode.Type) && iterations++ < IterationMax)
        {
            results.Add(VisitNumeric(explode.Roll));
        }
        return results.Count == 1 ? results.Single() : new ResultSet([.. results]);
    }

    protected internal override NumericRollResult VisitStackingExploding(StackingExplodingExpression explode)
    {
        decimal result = VisitNumeric(explode.Roll).Total;
        decimal last = result;
        var target = VisitNumeric(explode.Target);
        int iterations = 0;
        while (Compare(last, target.Total, explode.Type) && iterations++ < IterationMax)
        {
            last = VisitNumeric(explode.Roll).Total;
            result += last;
        }
        return (SingleRollResult)result;
    }

    private static bool Compare(NumericRollResult result, NumericRollResult target, TargetType type) => Compare(result.Total, target.Total, type);
    private static bool Compare(decimal result, decimal target, TargetType type)
    {
        return type switch
        {
            TargetType.Exact => result == target,
            TargetType.OrHigher => result >= target,
            TargetType.OrLower => result <= target,
            _ => throw new InvalidOperationException(),
        };
    }

    protected internal override NumericRollResult VisitTarget(TargetExpression target)
    {
        var set = (ResultSet)Visit(target.Set);
        var targetval = VisitNumeric(target.Target);
        var total = set.SelectMany(Recurse).Where(s => Compare(s, targetval, target.Type)).Count();
        return new TotalReplacementRollResult(set, total);

        static IEnumerable<NumericRollResult> Recurse(NumericRollResult result)
        {
            if (result is ResultSet rs)
                return rs.SelectMany(Recurse);
            return Utilities.Only(result);
        }
    }

    protected internal override NumericRollResult VisitLimit(LimitExpression limit)
    {
        var result = VisitNumeric(limit.Value);
        var lim = VisitNumeric(limit.Limit);
        return new TotalReplacementRollResult(result, Math.Min(result.Total, lim.Total));
    }

    protected internal override NumericRollResult VisitKeep(KeepExpression keep)
    {
        var original = (ResultSet)Visit(keep.Set);
        var count = VisitNumeric(keep.Keep).TotalAsLong;
        IEnumerable<NumericRollResult> newset;
        if (keep.Type == KeepType.Highest)
            newset = original.OrderByDescending(d => d.Total);
        else
            newset = original.OrderBy(d => d.Total);
        return new SetReplacementRollResult([.. newset.Take(count)], original);
    }

    protected internal override NumericRollResult VisitShadowrun(ShadowrunExpression sr)
    {
        throw new InvalidOperationException(); // will always be desugared
    }

    protected internal override NumericRollResult VisitOldShadowrun(OldShadowrunExpression osr)
    {
        throw new InvalidOperationException(); // will always be desugared
    }


    protected internal override NumericRollResult VisitStarWars(StarwarsExpression sw)
    {
        Debug.Assert(sw.DiceCount is LiteralExpression);
        var count = VisitNumeric(sw.DiceCount).TotalAsLong;
        NumericExpression roll = DesugarVisitor.CalculateStarWars(count);
        return VisitNumeric(roll);
    }

    protected internal override NumericRollResult VisitEarthdawn(EarthdawnExpression ed)
    {
        Debug.Assert(ed.Step is LiteralExpression);
        long steps = VisitNumeric(ed.Step).TotalAsLong;
        var roll = DesugarVisitor.CalculateEarthdawn(steps);
        return VisitNumeric(roll);
    }

    protected internal override RollResult VisitVersus(VersusExpression vs)
    {
        var left = VisitNumeric(vs.Left);
        var right = VisitNumeric(vs.Right);
        var result = vs.Type switch
        {
            VersusType.Opposed => left.Total - right.Total,
            VersusType.Earthdawn => Math.Floor((left.Total - right.Total) / 5),
        };
        return new VersusResult(left, right, result, vs.Type);
    }

    protected internal override GenesysRollResult VisitGenesysSet(GenesysSetExpression set)
    {
        return new GenesysSetRollResult([.. set.Expressions.Select(Visit).Cast<GenesysRollResult>()]);
    }

    protected internal override GenesysRollResult VisitGenesysDice(GenesysDiceExpression dice)
    {
        ImmutableArray<ImmutableArray<GenesysToken>> arr = dice.Dice switch
        {
            GenesysDice.Boost => Boost,
            GenesysDice.Ability => Ability,
            GenesysDice.Proficiency => Proficiency,
            GenesysDice.Setback => Setback,
            GenesysDice.Difficulty => Difficulty,
            GenesysDice.Challenge => Challenge,
            _ => throw new InvalidOperationException(),
        };
        var index = random.Next(0, arr.Length);
        return new GenesysDiceRollResult(arr[index]);
    }

    protected internal override GenesysRollResult VisitGenesysLiteral(GenesysLiteralExpression lit)
    {
        return new GenesysLiteralRollResult(lit.Value);
    }

    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Boost = [
        [],
        [],
        [Success],
        [Advantage, Success],
        [Advantage, Advantage],
        [Advantage]
    ];
    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Setback = [
        [],
        [],
        [Failure],
        [Failure],
        [Threat],
        [Threat]
    ];
    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Ability =
    [
        [],
        [Success],
        [Success],
        [Success, Success],
        [Advantage],
        [Advantage],
        [Advantage, Success],
        [Advantage, Advantage]
    ];
    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Difficulty = [
        [],
        [Failure],
        [Failure, Failure],
        [Threat],
        [Threat],
        [Threat],
        [Threat, Threat],
        [Failure, Threat]
    ];
    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Proficiency = [
        [],
        [Success],
        [Success],
        [Success, Success],
        [Success, Success],
        [Advantage],
        [Advantage, Success],
        [Advantage, Success],
        [Advantage, Success],
        [Advantage, Advantage],
        [Advantage, Advantage],
        [Triumph]
    ];
    private static readonly ImmutableArray<ImmutableArray<GenesysToken>> Challenge = [
        [],
        [Failure],
        [Failure],
        [Failure, Failure],
        [Failure, Failure],
        [Threat],
        [Threat],
        [Failure, Threat],
        [Failure, Threat],
        [Threat, Threat],
        [Threat, Threat],
        [Despair]
    ];
}
