using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

public class TypeCheckVisitor(int MaximumIterations) : ExpressionReplacer
{
    private readonly RangeDeterminatorVisitor typevisitor = new();
    private readonly ErrorFinder error = new ErrorFinder();

    protected internal override Expression ReplaceDice(DiceExpression dice)
    {
        if (error.HasAny(dice)) return dice;
        var type = typevisitor.Visit(dice.Sides);
        if (type.Type != ExpressionType.Integer)
            dice = dice with { Sides = new NumericTypeError(dice.Sides, type.Type, ExpressionType.Integer) };
        if (error.HasAny(dice)) return dice;
        if (!RangeDeterminatorVisitor.PositiveOnly(type))
        {
            return new MustBePositiveError(dice);
        }
        return dice;
    }

    protected internal override Expression ReplaceMultiple(MultipleExpression mult)
    {
        if (error.HasAny(mult)) return mult;
        var count = typevisitor.Visit(mult.Count);
        var roll = typevisitor.Visit(mult.Roll);
        if (count.Type != ExpressionType.Integer)
            mult = mult with { Count = new NumericTypeError(mult.Count, count.Type, ExpressionType.Integer) };
        if (roll.Type != ExpressionType.Integer)
            mult = mult with { Roll = new NumericTypeError(mult.Roll, roll.Type, ExpressionType.Integer) };
        if (error.HasAny(mult)) return mult;
        if (!RangeDeterminatorVisitor.PositiveOnly(count))
        {
            return mult with { Count = new MustBePositiveError(mult.Count) };
        }
        if (!RangeDeterminatorVisitor.PositiveOnly(roll))
        {
            return mult with { Roll = new MustBePositiveError(mult.Roll) };
        }
        if (error.HasAny(mult)) return mult;
        if (count.High > MaximumIterations)
            return new MultipleOverflowError(mult, MaximumIterations);
        return mult;
    }

    protected internal override Expression ReplaceArithmetic(ArithmeticExpression arith)
    { // the TOUGH ONE
        if (error.HasAny(arith)) return arith;
        var left = typevisitor.Visit(arith.Left);
        var right = typevisitor.Visit(arith.Right);
        switch (arith.ArithType)
        {
            case ArithmeticType.Add: // cannot have opposite infinities
                if ((left.ReachesPositiveInfinity && right.ReachesNegativeInfinity)
                    || (left.ReachesNegativeInfinity && right.ReachesPositiveInfinity))
                {
                    arith = arith with
                    {
                        Left = new InfiniteNumericError(arith.Left),
                        Right = new InfiniteNumericError(arith.Right)
                    };
                }
                break;
            case ArithmeticType.Subtract: // cannot have the same infinities
                if ((left.ReachesPositiveInfinity && right.ReachesPositiveInfinity)
                    || (left.ReachesNegativeInfinity && right.ReachesNegativeInfinity))
                {
                    arith = arith with
                    {
                        Left = new InfiniteNumericError(arith.Left),
                        Right = new InfiniteNumericError(arith.Right)
                    };
                }
                break;
            case ArithmeticType.Multiply: // cannot have opposite infinities
                if ((left.ReachesPositiveInfinity && right.ReachesNegativeInfinity)
                    || (left.ReachesNegativeInfinity && right.ReachesPositiveInfinity))
                {
                    arith = arith with
                    {
                        Left = new InfiniteNumericError(arith.Left),
                        Right = new InfiniteNumericError(arith.Right)
                    };
                }
                break;
            case ArithmeticType.Divide: // types cannot both be infinite
                if (left.ReachesAnyInfinity && right.ReachesAnyInfinity)
                {
                    arith = arith with
                    {
                        Left = new InfiniteNumericError(arith.Left),
                        Right = new InfiniteNumericError(arith.Right)
                    };
                }
                break;
            case ArithmeticType.Modulo: // left cannot be infinite
                if (left.ReachesAnyInfinity)
                {
                    arith = arith with
                    {
                        Left = new InfiniteNumericError(arith.Left)
                    };
                }
                break;
            default:
                throw new NotImplementedException();
        }
        if (error.HasAny(arith)) return arith;
        if (arith.ArithType is ArithmeticType.Divide or ArithmeticType.Modulo)
        {
            if (RangeDeterminatorVisitor.CrossesZero(right))
            {
                return new PossibleDivZeroError(arith);
            }
        }
        return arith;
    }

    protected internal override Expression ReplaceExploding(ExplodingExpression explode)
    {
        if (error.HasAny(explode)) return explode;
        var target = typevisitor.Visit(explode.Target);
        var roll = typevisitor.Visit(explode.Roll);
        if (target.Type != ExpressionType.Integer)
            explode = explode with { Target = new NumericTypeError(explode.Target, target.Type, ExpressionType.Integer) };
        if (roll.Type != ExpressionType.Integer)
            explode = explode with { Roll = new NumericTypeError(explode.Roll, roll.Type, ExpressionType.Integer) };
        if (error.HasAny(explode)) return explode;
        if (!RangeDeterminatorVisitor.PositiveOnly(target))
        {
            explode = explode with { Target = new MustBePositiveError(explode.Target) };
        }
        if (!RangeDeterminatorVisitor.PositiveOnly(roll))
        {
            explode = explode with { Roll = new MustBePositiveError(explode.Roll) };
        }
        return explode;
    }

    protected internal override Expression ReplaceStackingExploding(StackingExplodingExpression explode)
    {
        if (error.HasAny(explode)) return explode;
        var target = typevisitor.Visit(explode.Target);
        var roll = typevisitor.Visit(explode.Roll);
        if (target.Type != ExpressionType.Integer)
            explode = explode with { Target = new NumericTypeError(explode.Target, target.Type, ExpressionType.Integer) };
        if (roll.Type != ExpressionType.Integer)
            explode = explode with { Roll = new NumericTypeError(explode.Roll, roll.Type, ExpressionType.Integer) }; 
        if (error.HasAny(explode)) return explode;
        if (!RangeDeterminatorVisitor.PositiveOnly(target))
        {
            explode = explode with { Target = new MustBePositiveError(explode.Target) };
        }
        if (!RangeDeterminatorVisitor.PositiveOnly(roll))
        {
            explode = explode with { Roll = new MustBePositiveError(explode.Roll) };
        }
        return explode;
    }

    protected internal override Expression ReplaceTarget(TargetExpression target)
    {
        if (error.HasAny(target)) return target;
        var set = typevisitor.Visit(target.Set);
        var ttype = typevisitor.Visit(target.Target);
        if (set.Type != ExpressionType.Integer)
            target = target with { Set = new NumericTypeError(target.Set, set.Type, ExpressionType.Integer) };
        if (ttype.Type != ExpressionType.Integer)
            target = target with { Target = new NumericTypeError(target.Target, ttype.Type, ExpressionType.Integer) }; 
        return target;
    }

    protected internal override Expression ReplaceIndefiniteExploding(IndefiniteExplodingExpression explode)
    {
        if (error.HasAny(explode)) return explode;
        var target = typevisitor.Visit(explode.Target);
        var roll = typevisitor.Visit(explode.Roll);
        if (target.Type != ExpressionType.Integer)
            explode = explode with { Target = new NumericTypeError(explode.Target, target.Type, ExpressionType.Integer) };
        if (roll.Type != ExpressionType.Integer)
            explode = explode with { Roll = new NumericTypeError(explode.Roll, roll.Type, ExpressionType.Integer) }; 
        if (error.HasAny(explode)) return explode;
        if (!RangeDeterminatorVisitor.PositiveOnly(target))
        {
            explode = explode with { Target = new MustBePositiveError(explode.Target) };
        }
        if (!RangeDeterminatorVisitor.PositiveOnly(roll))
        {
            explode = explode with { Roll = new MustBePositiveError(explode.Roll) };
        }
        return explode;
    }

    protected internal override Expression ReplaceLimit(LimitExpression limit)
    {
        if (error.HasAny(limit)) return limit;
        var value = typevisitor.Visit(limit.Value);
        var ltype = typevisitor.Visit(limit.Limit);
        if (value.Type != ExpressionType.Integer)
            limit = limit with { Value = new NumericTypeError(limit.Value, value.Type, ExpressionType.Integer) };
        if (ltype.Type != ExpressionType.Integer)
            limit = limit with { Limit = new NumericTypeError(limit.Limit, ltype.Type, ExpressionType.Integer) }; 
        return limit;
    }

    protected internal override Expression ReplaceKeep(KeepExpression keep)
    {
        if (error.HasAny(keep)) return keep;
        var set = typevisitor.Visit(keep.Set);
        var ktype = typevisitor.Visit(keep.Keep);
        if (set.Type != ExpressionType.Integer)
            keep = keep with { Set = new NumericTypeError(keep.Set, set.Type, ExpressionType.Integer) };
        if (ktype.Type != ExpressionType.Integer)
            keep = keep with { Keep = new NumericTypeError(keep.Keep, ktype.Type, ExpressionType.Integer) };
        if (error.HasAny(keep)) return keep;
        if (!RangeDeterminatorVisitor.PositiveOnly(ktype))
        {
            return keep with { Keep = new MustBePositiveError(keep.Keep) };
        }
        return keep;
    }

    protected internal override Expression ReplaceShadowrun(ShadowrunExpression sr)
    {
        if (error.HasAny(sr)) return sr;
        var dicecount = typevisitor.Visit(sr.DiceCount);
        DiceRange? limit = sr.Limit is null ? null : typevisitor.Visit(sr.Limit);
        if (dicecount.Type != ExpressionType.Integer)
            sr = sr with { DiceCount = new NumericTypeError(sr.DiceCount, dicecount.Type, ExpressionType.Integer) };
        if (limit is not null && limit.Value.Type != ExpressionType.Integer)
            sr = sr with { Limit = new NumericTypeError(sr.Limit!, limit.Value.Type, ExpressionType.Integer) }; 
        if (error.HasAny(sr)) return sr;
        if (!RangeDeterminatorVisitor.PositiveOnly(dicecount))
        {
            return new MustBePositiveError(sr);
        }
        if (error.HasAny(sr)) return sr;
        if (dicecount.High > MaximumIterations)
            return new MultipleOverflowError(sr, MaximumIterations);
        return sr;
    }

    protected internal override Expression ReplaceOldShadowrun(OldShadowrunExpression osr)
    {
        if (error.HasAny(osr)) return osr;
        var dicecount = typevisitor.Visit(osr.DiceCount);
        DiceRange? target = osr.Target is null ? null : typevisitor.Visit(osr.Target);
        if (dicecount.Type != ExpressionType.Integer)
            osr = osr with { DiceCount = new NumericTypeError(osr.DiceCount, dicecount.Type, ExpressionType.Integer) };
        if (target is not null && target.Value.Type != ExpressionType.Integer)
            osr = osr with { Target = new NumericTypeError(osr.Target!, target.Value.Type, ExpressionType.Integer) }; 
        if (error.HasAny(osr)) return osr;
        if (!RangeDeterminatorVisitor.PositiveOnly(dicecount))
        {
            return new MustBePositiveError(osr);
        }
        if (error.HasAny(osr)) return osr;
        if (dicecount.High > MaximumIterations)
            return new MultipleOverflowError(osr, MaximumIterations);
        return osr;
    }

    protected internal override Expression ReplaceStarWars(StarwarsExpression sw)
    {
        if (error.HasAny(sw)) return sw;
        var dicecount = typevisitor.Visit(sw.DiceCount);
        if (dicecount.Type != ExpressionType.Integer)
            sw = sw with { DiceCount = new NumericTypeError(sw.DiceCount, dicecount.Type, ExpressionType.Integer) };
        if (error.HasAny(sw)) return sw;        
        if (!RangeDeterminatorVisitor.PositiveOnly(dicecount))
        {
            return new MustBePositiveError(sw);
        }
        if (error.HasAny(sw)) return sw;        
        if (dicecount.High > MaximumIterations)
            return new MultipleOverflowError(sw, MaximumIterations);
        return sw;
    }

    protected internal override Expression ReplaceEarthdawn(EarthdawnExpression ed)
    {
        if (error.HasAny(ed)) return ed;
        var step = typevisitor.Visit(ed.Step);
        if (step.Type != ExpressionType.Integer)
            ed = ed with { Step = new NumericTypeError(ed.Step, step.Type, ExpressionType.Integer) };
        if (error.HasAny(ed)) return ed;
        if (!RangeDeterminatorVisitor.PositiveOnly(step))
        {
            return new MustBePositiveError(ed);
        }
        if (error.HasAny(ed)) return ed;
        if (step.High > MaximumIterations)
            return new MultipleOverflowError(ed, MaximumIterations);
        return ed;
    }

    protected internal override Expression ReplaceRepeatRoll(RepeatRollExpression repeat)
    {
        if (error.HasAny(repeat)) return repeat;
        if (repeat.Count > MaximumIterations)
            return new RepeatRollOverflowError(repeat.Expression, MaximumIterations);
        return repeat;
    }
}
