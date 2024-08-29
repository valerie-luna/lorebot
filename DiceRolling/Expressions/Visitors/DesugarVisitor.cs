using System.Diagnostics;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

public class DesugarVisitor : ExpressionReplacer
{
    protected internal override Expression ReplaceShadowrun(ShadowrunExpression sr)
    {
        NumericExpression roll;
        if (sr.Exploding)
        {
            roll = new TargetExpression(
                new MultipleExpression(
                    sr.DiceCount,
                    new IndefiniteExplodingExpression(new DiceExpression(new LiteralExpression(6)), new LiteralExpression(6), TargetType.Exact)
                ), new LiteralExpression(5), TargetType.OrHigher
            ); 
        }
        else
        {
            roll = new TargetExpression(
                new MultipleExpression(
                    sr.DiceCount,
                    new DiceExpression(new LiteralExpression(6))
                ), new LiteralExpression(5), TargetType.OrHigher
            );
        }
        if (sr.Limit is not null)
        {
            roll = new LimitExpression(roll, sr.Limit);
        }
        return roll;
    }

    protected internal override Expression ReplaceOldShadowrun(OldShadowrunExpression osr)
    {
        MultipleNumericExpression roll = new MultipleExpression(
            osr.DiceCount,
            new StackingExplodingExpression(new DiceExpression(new LiteralExpression(6)), new LiteralExpression(6), TargetType.Exact)
        );
        if (osr.Target is not null)
        {
            return new TargetExpression(roll, 
                osr.Target, TargetType.OrHigher);
        }
        else
        {
            return new KeepExpression(roll, new LiteralExpression(1), KeepType.Highest);
        }
    }

    protected internal override Expression ReplaceEarthdawn(EarthdawnExpression ed)
    {
        if (ed.Step is not LiteralExpression steplit)
            Debug.Assert(false);
        return CalculateEarthdawn((long)((LiteralExpression)ed.Step).Value);
    }

    protected internal override Expression ReplaceStarWars(StarwarsExpression sw)
    {
        if (sw.DiceCount is not LiteralExpression dicelit)
            Debug.Assert(false);
        return CalculateStarWars((long)((LiteralExpression)sw.DiceCount).Value);
    }

    public static NumericExpression CalculateEarthdawn(long steps)
    {
        long d20s = (steps - 8) / 11;
        steps = ((steps - 8) % 11) + 8;
        NumericExpression roll = steps switch
        {
            1  => Subtract(D4(), Number(2)),
            2  => Subtract(D4(), Number(1)),
            3  => D4(),
            4  => D6(),
            5  => D8(),
            6  => D10(),
            7  => D12(),
            8  => TwoD6(),
            9  => Add(D8(), D6()),
            10 => TwoD8(),
            11 => Add(D10(), D8()),
            12 => TwoD10(),
            13 => Add(D12(), D10()),
            14 => TwoD12(),
            15 => Add(D12(), TwoD6()),
            16 => Add(D12(), Add(D8(), D6())),
            17 => Add(D12(), TwoD8()),
            18 => Add(D12(), Add(D10(), D8())),
            _ => throw new InvalidOperationException() // shouldn't ever happen of course
        };
        if (d20s > 0)
        {
            roll = new ArithmeticExpression(
                new MultipleExpression(new LiteralExpression(d20s), new DiceExpression(new LiteralExpression(20))),
                roll,
                ArithmeticType.Add
            );
        }
        return roll;

        static ArithmeticExpression Subtract(NumericExpression left, NumericExpression right)
        {
            return new ArithmeticExpression(left, right, ArithmeticType.Subtract);
        }
        static ArithmeticExpression Add(NumericExpression left, NumericExpression right)
        {
            return new ArithmeticExpression(left, right, ArithmeticType.Add);
        }
        static LiteralExpression Number(long num) => new(num);
        static IndefiniteExplodingExpression Exploding(long num) => new(new DiceExpression(Number(num)), Number(num), TargetType.Exact); 
        static IndefiniteExplodingExpression D4() => Exploding(4);
        static IndefiniteExplodingExpression D6() => Exploding(6);
        static IndefiniteExplodingExpression D8() => Exploding(8);
        static IndefiniteExplodingExpression D10() => Exploding(10);
        static IndefiniteExplodingExpression D12() => Exploding(12);
        static MultipleExpression TwoD6() => new(Number(2), D6());
        static MultipleExpression TwoD8() => new(Number(2), D8());
        static MultipleExpression TwoD10() => new(Number(2), D10());
        static MultipleExpression TwoD12() => new(Number(2), D12());

    }

    public static NumericExpression CalculateStarWars(long count)
    {
        NumericExpression roll;
        if (count == 1)
        {
            roll = new StackingExplodingExpression(new DiceExpression(
                new LiteralExpression(6)), new LiteralExpression(6), TargetType.Exact);
        }
        else
        {
            roll = new ArithmeticExpression(
                new MultipleExpression(new LiteralExpression(count - 1), new DiceExpression(new LiteralExpression(6))),
                new StackingExplodingExpression(new DiceExpression(new LiteralExpression(6)), new LiteralExpression(6), TargetType.Exact),
                ArithmeticType.Add
            );
        }
        return roll;
    }
}