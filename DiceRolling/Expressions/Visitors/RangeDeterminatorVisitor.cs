using System.Diagnostics;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

public class RangeDeterminatorVisitor : ExpressionVisitor<DiceRange>
{
    protected internal override DiceRange VisitLiteral(LiteralExpression literal)
    {
        ExpressionType type;
        try
        {
            long litlong = (long)literal.Value;
            type = litlong != literal.Value ? ExpressionType.Decimal : ExpressionType.Integer;
        }
        catch (OverflowException) { type = ExpressionType.Decimal; }
        return new DiceRange(type, literal.Value, literal.Value);
    }

    protected internal override DiceRange VisitDice(DiceExpression dice)
    {
        var sides = Visit(dice.Sides);
        AssertIntegerPositive(sides);
        return new DiceRange(ExpressionType.Integer, 1, sides.High);
    }

    protected internal override DiceRange VisitMultiple(MultipleExpression mult)
    {
        var count = Visit(mult.Count);
        var roll = Visit(mult.Roll);
        AssertIntegerPositive(count);
        AssertIntegerPositive(roll);
        return new DiceRange(ExpressionType.Integer, count.Low * roll.Low, count.High * roll.High);
    }

    protected internal override DiceRange VisitArithmetic(ArithmeticExpression arith)
    {
        var left = Visit(arith.Left);
        var right = Visit(arith.Right);
        switch (arith.ArithType)
        {
            case ArithmeticType.Add:
                return new DiceRange(Utilities.DecimalCast(left.Type, right.Type),
                    left.Low + right.Low, left.High + right.High);
            case ArithmeticType.Subtract:
                return new DiceRange(Utilities.DecimalCast(left.Type, right.Type),
                    left.Low - right.High, left.High - right.Low);
            case ArithmeticType.Multiply:
            {
                var low = Utilities.Min(
                    left.Low * right.Low,
                    left.Low * right.High,
                    left.High * right.Low,
                    left.High * right.High
                );
                var high = Utilities.Max(
                    left.Low * right.Low,
                    left.Low * right.High,
                    left.High * right.Low,
                    left.High * right.High
                );
                return new DiceRange(Utilities.DecimalCast(left.Type, right.Type),
                    low, high);
            }
            case ArithmeticType.Divide:
            {
                Debug.Assert(!CrossesZero(right));
                var low = Utilities.Min(
                    left.Low / right.Low,
                    left.Low / right.High,
                    left.High / right.Low,
                    left.High / right.High
                );
                var high = Utilities.Max(
                    left.Low / right.Low,
                    left.Low / right.High,
                    left.High / right.Low,
                    left.High / right.High
                );
                return new DiceRange(ExpressionType.Decimal, low, high);
            }
            case ArithmeticType.Modulo:
            {
                Debug.Assert(!CrossesZero(right));
                // our range depends on the +/- value of left
                // if it can be negative, low extends to -right.High, otherwise 0
                // if it can be positive, high extends to right.High, otherwise 0
                // use ABSOLUTE VALUE of right.High, negative modulo is no different from positive
                bool canBeNegative = left.Low < 0;
                bool canBePositive = left.High > 0;
                return new DiceRange(Utilities.DecimalCast(left.Type, right.Type),
                    canBeNegative ? -SatDec.Abs(right.High): 0,
                    canBePositive ? SatDec.Abs(right.High): 0);
            }
            default:
                throw new InvalidOperationException();
        }
    }

    protected internal override DiceRange VisitExploding(ExplodingExpression explode)
    {
        var target = Visit(explode.Target);
        var roll = Visit(explode.Roll);
        AssertIntegerPositive(target);
        AssertIntegerPositive(roll);
        SatDec low, high;
        // situations
            // target type: OrLower
                // strictly less than: never explodes, value same as Roll
                // strictly greater than: always explodes, value same as Roll x2
            // target type Equal
                // strictly less than: never explodes
                // strictly greater than: never explodes
            // target type: OrHigher
                // strictly less than: always explodes
                // strictly greater than: never explodes
        if (!Overlaps(target, roll))
        {
            bool lessThan = target.High < roll.Low;
            if ((explode.Type is TargetType.OrLower && !lessThan)
                || (explode.Type is TargetType.OrHigher && lessThan))
            {
                low = roll.Low + roll.Low;
                high = roll.High + roll.High;
            }
            else
            {
                low = roll.Low;
                high = roll.High;
            }
        }
        else
        {
            if (explode.Type == TargetType.OrLower || Contains(target, roll.Low))
            {
                // if we roll minimum, it explodes
                // our lowest possible roll is therefore either the lowest possible explosion 
                // (max(target.low, roll.low) + explode.low)
                // or the lowest possible not-explosion
                // min(target.low + 1, roll.high)
                low = SatDec.Min(
                    SatDec.Max(target.Low, roll.Low) + roll.Low,
                    SatDec.Max(target.Low + 1, roll.High)
                );
            }
            else
            {
                low = roll.Low;
            }
            if (explode.Type == TargetType.OrHigher || Contains(target, roll.High))
            {
                // our maximum is easy, high x2
                high = roll.High + roll.High;
            }
            else
            {
                // highest is max target + max roll, as we can't roll higher on a single roll anyway
                high = target.High + roll.High;
            }
        }
        return new DiceRange(ExpressionType.Integer, low, high);
    }

    protected internal override DiceRange VisitIndefiniteExploding(IndefiniteExplodingExpression explode)
    {
        // thankfully this is easier than exploding
        // the max is always infinite
        var target = Visit(explode.Target);
        var roll = Visit(explode.Roll);
        return VisitUnlimitedExplosionType(target, roll, explode.Type);
    }

    protected internal override DiceRange VisitStackingExploding(StackingExplodingExpression explode)
    { // calculation is identical to indefinite, it just changes the roll set thing
        var target = Visit(explode.Target);
        var roll = Visit(explode.Roll);
        return VisitUnlimitedExplosionType(target, roll, explode.Type);
    }

    private DiceRange VisitUnlimitedExplosionType(DiceRange target, DiceRange roll, TargetType type)
    {
        AssertIntegerPositive(target);
        AssertIntegerPositive(roll);
        SatDec low, high;
        if (!Overlaps(target, roll))
        {
            bool lessThan = target.High < roll.Low;
            if ((type is TargetType.OrLower && !lessThan)
                || (type is TargetType.OrHigher && lessThan))
            {
                low = SatDec.Inf;
                high = SatDec.Inf;
            }
            else
            {
                low = roll.Low;
                high = roll.High;
            }
        }
        else
        {
            high = SatDec.Inf;
            if (type == TargetType.OrLower || Contains(target, roll.Low))
            {
                // the lowest roll may explode
                // our lowest possible is therefore the lowest roll possible roll that may not explode
                // this is (target.low + 1)
                if (!Contains(roll, target.Low + 1))
                    low = SatDec.Inf;
                else
                    low = target.Low + 1;
            }
            else
            {
                // the lowest roll cannot explode, so it is our lowest for sure
                low = roll.Low;
            }
        }
        return new DiceRange(ExpressionType.Integer, low, high);
    }

    protected internal override DiceRange VisitTarget(TargetExpression target)
    { // target changes the roll to a 0/1 based on if it matches or not
        var setcount = GetSetCount(target.Set);
        Debug.Assert(setcount.Low >= 0 && setcount.High >= 0);
        return new DiceRange(ExpressionType.Integer, 0, setcount.High);
    }

    protected internal override DiceRange VisitLimit(LimitExpression limit)
    {
        var inner = Visit(limit.Value);
        var limitrange = Visit(limit.Limit);
        return new DiceRange(ExpressionType.Integer, 
            SatDec.Min(inner.Low, limitrange.Low),
            SatDec.Min(inner.High, limitrange.High));
    }

    protected internal override DiceRange VisitKeep(KeepExpression keep)
    {
        var inner = GetIndividualRange(keep.Set);
        var innercount = GetSetCount(keep.Set);
        var count = Visit(keep.Keep);
        AssertIntegerPositive(count);
        return new DiceRange(ExpressionType.Integer, 
            inner.Low * SatDec.Min(count.Low, innercount.Low), 
            inner.High * SatDec.Min(count.High, innercount.High));
    }

    private DiceRange GetIndividualRange(MultipleNumericExpression mnum)
    {
        return mnum switch
        {
            ExplodingExpression ex => Visit(ex.Roll),
            IndefiniteExplodingExpression idef => Visit(idef.Roll),
            KeepExpression ke => GetIndividualRange(ke.Set),
            MultipleExpression mul => Visit(mul.Roll),
            _ => throw new InvalidOperationException()
        };
    }

    private DiceRange GetSetCount(MultipleNumericExpression mnum)
    {
        switch (mnum)
        {
            case ExplodingExpression ex:
            {
                Debug.Assert(Overlaps(Visit(ex.Roll), Visit(ex.Target)));
                return new DiceRange(ExpressionType.Integer, 1, 2);
            }
            case IndefiniteExplodingExpression iex: 
            {
                Debug.Assert(Overlaps(Visit(iex.Roll), Visit(iex.Target)));
                return new DiceRange(ExpressionType.Integer, 1, SatDec.Inf);
            }
            case KeepExpression ke:
            {
                var innercount = GetSetCount(ke.Set);
                var keepamount = Visit(ke.Keep);
                return new DiceRange(ExpressionType.Integer, 
                    SatDec.Min(innercount.Low, keepamount.Low), keepamount.High);
            }
            case MultipleExpression mul:
            {
                var count = Visit(mul.Count);
                if (mul.Roll is MultipleNumericExpression inner)
                {
                    var incount = GetSetCount(inner);
                    count = new DiceRange(ExpressionType.Integer,
                        count.Low * incount.Low, count.High * incount.High
                    );
                }
                return count;
            }
            case MultipleOverflowError:
                throw new InvalidOperationException();
            default: throw new NotImplementedException();
        }
    }

    protected internal override DiceRange VisitEarthdawn(EarthdawnExpression ed)
    {
        var steprange = Visit(ed.Step);
        AssertIntegerPositive(steprange);
        decimal low = (decimal)steprange.Low switch
        {
            <= 0 => throw new InvalidOperationException(),
            1 => -1,
            2 => 0,
            <= 7 => 1,
            <= 15 => 2,
            _ => (((decimal)steprange.Low - 15) / 11) + 3 
        };
        return new DiceRange(ExpressionType.Integer, low, SatDec.Inf);
    }

    protected internal override DiceRange VisitVersus(VersusExpression vs)
    {
        var leftrange = Visit(vs.Left);
        var rightrange = Visit(vs.Right);
        switch (vs.Type)
        {
            case VersusType.Opposed:
            {
                var min = leftrange.Low - rightrange.High;
                var max = leftrange.High - rightrange.Low;
                var type = (leftrange.Type is ExpressionType.Integer 
                    && rightrange.Type is ExpressionType.Integer)
                    ? ExpressionType.Integer
                    : ExpressionType.Decimal;
                return new DiceRange(type, min, max);
            }
            case VersusType.Earthdawn:
            {
                var min = leftrange.Low - rightrange.High;
                min = SatDec.Floor(min / 5);
                var max = leftrange.High - rightrange.Low;
                max = SatDec.Floor(max / 5);
                return new DiceRange(ExpressionType.Integer, min, max);
            }
            default: throw new InvalidOperationException();
        }
    }

    protected internal override DiceRange VisitShadowrun(ShadowrunExpression sr)
    {
        SatDec max;
        if (sr.Exploding)
        {
            max = SatDec.Inf;
        }
        else
        {
            max = Visit(sr.DiceCount).High;
        }
        if (sr.Limit is not null)
        {
            max = SatDec.Min(Visit(sr.Limit).High, max);
        }
        return new DiceRange(ExpressionType.Integer, 0, max);
    }

    protected internal override DiceRange VisitOldShadowrun(OldShadowrunExpression osr)
    {
        if (osr.Target != null)
        {
            // our range is from 0-setcount
            var set = Visit(osr.DiceCount);
            return new DiceRange(ExpressionType.Integer, 0, set.High);
        }
        else
        {
            // our range is 1-inf
            return new DiceRange(ExpressionType.Integer, 1, SatDec.Inf);
        }
    }

    protected internal override DiceRange VisitStarWars(StarwarsExpression sw)
    {
        return new DiceRange(ExpressionType.Integer, Visit(sw.DiceCount).Low, SatDec.Inf);
    }

    protected internal override DiceRange VisitDivZero(PossibleDivZeroError error)
    {
        throw new InvalidOperationException();
    }

    protected internal override DiceRange VisitTypeError(NumericTypeError error)
    {
        throw new InvalidOperationException();
    }

    protected internal override DiceRange VisitMultipleOverflow(MultipleOverflowError error)
    {
        throw new InvalidOperationException();
    }

    public static bool PositiveOnly(DiceRange range)
    {
        return range.Low > 0;
    }

    public static bool CrossesZero(DiceRange range)
    {
        // this isn't for efficiency this is to fix the non-range checks
        if (range.Low == 0 || range.High == 0)
            return true;
        return range.Low <= 0 && range.High >= 0;
    }

    public static bool Overlaps(DiceRange left, DiceRange right)
    {
        return left.High >= right.Low && right.High >= left.Low;
    }

    public static bool Contains(DiceRange range, SatDec value)
    {
        return range.Low <= value && range.High >= value;
    }

    [Conditional("DEBUG")]
    private void AssertIntegerPositive(DiceRange range)
    {
        Debug.Assert(range.Type == ExpressionType.Integer);
        try
        {
            if (range.Low != SatDec.Inf)
            {
                Debug.Assert((long)(decimal)range.Low == range.Low);
                Debug.Assert(range.Low > 0);
            }
            if (range.High != SatDec.Inf)
            {
                Debug.Assert((long)(decimal)range.High == range.High);
                Debug.Assert(range.High > 0);
            }
        }
        catch (OverflowException)
        {
            Debug.Assert(false);
        }
    }

    protected internal override DiceRange VisitGenesys(GenesysExpression genesys)
    {
        return default;
    }
}
