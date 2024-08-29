// See https://aka.ms/new-console-template for more information

using System.Text;
using DiceRolling.Expressions.Dice;
using System.Diagnostics;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors.Base;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Dice.Sugar;

public class DiscordRollVisitor(bool AlwaysBrackets = false, bool UnderlineErrors = false) : ExpressionVisitor<FormattableString>
{
    protected override FormattableString VisitArithmetic(ArithmeticExpression arith)
    {
        char symbol = arith.ArithType switch
        {
            ArithmeticType.Add => '+',
            ArithmeticType.Subtract => '-',
            ArithmeticType.Multiply => '*',
            ArithmeticType.Divide => '/',
            ArithmeticType.Modulo => '%',
            _ => throw new InvalidOperationException(),
        };
        var left = PrintWithBrackets(arith.Left, arith.ArithType);
        var right = PrintWithBrackets(arith.Right, arith.ArithType);
        return $"{left} {symbol} {right}";
    }

    private FormattableString PrintWithBrackets(NumericExpression descriptor, ArithmeticType outerType)
    {
        bool skipBrackets = descriptor is not ArithmeticExpression arith 
            || !ArithmeticsNeedBrackets(outerType, arith.ArithType);

        if (AlwaysBrackets) skipBrackets = false;

        if (skipBrackets)
        {
            return descriptor.Accept(this);
        }
        else
        {
            return $"({descriptor.Accept(this)})";
        }
        static bool ArithmeticsNeedBrackets(ArithmeticType outer, ArithmeticType inner)
        {
            if (outer is ArithmeticType.Add or ArithmeticType.Subtract)
            {
                return inner is ArithmeticType.Multiply or ArithmeticType.Divide;
            }
            else
            {
                return inner is ArithmeticType.Add or ArithmeticType.Subtract;
            }
        }
    }

    protected override FormattableString VisitDice(DiceExpression dice)
    {
        return $"d{InnerDiceNumber(dice.Sides)}";
    }

    private FormattableString InnerDiceNumber(NumericExpression descriptor)
    {
        if (descriptor is LiteralExpression lit && lit.Value > 0)
        {
            return descriptor.Accept(this);
        }
        else
        {
            return $"[{descriptor.Accept(this)}]";
        }
    }

    protected override FormattableString VisitExploding(ExplodingExpression exploding)
    {
        return PrintExploding(exploding.Roll, exploding.Target, exploding.Type, " e");
    }

    protected override FormattableString VisitIndefiniteExploding(IndefiniteExplodingExpression exploding)
    {
        return PrintExploding(exploding.Roll, exploding.Target, exploding.Type, " ie");
    }

    private FormattableString PrintExploding(NumericExpression roll, NumericExpression target, 
        TargetType type, string explodeStr)
    {
        var res = Visit(roll);
        var idn = InnerDiceNumber(target);
        var t = type switch
        {
            TargetType.Exact => "",
            TargetType.OrHigher => "+",
            TargetType.OrLower => "-",
            _ => throw new InvalidOperationException(),
        };
        return $"{res}{explodeStr}{idn}{t}";
    }

    protected override FormattableString VisitKeep(KeepExpression keep)
    {
        var set = keep.Set.Accept(this);
        var idn = InnerDiceNumber(keep.Keep);
        var type = keep.Type switch
        {
            KeepType.Highest => '+',
            KeepType.Lowest => '-',
            _ => throw new InvalidOperationException(),
        };
        return $"{set} k{idn}{type}";
    }

    protected override FormattableString VisitLimit(LimitExpression limit)
    {
        var orig = limit.Value.Accept(this);
        var limnum = InnerDiceNumber(limit.Limit);
        return $"{orig} l{limnum}";
    }

    protected override FormattableString VisitMultiple(MultipleExpression multiple)
    {
        return $"{InnerDiceNumber(multiple.Count)}{Visit(multiple.Roll)}";
    }

    protected override FormattableString VisitRepeatRoll(RepeatRollExpression repeated)
    {
        return $"{repeated.Count}: {Visit(repeated.Expression)}";
    }

    protected override FormattableString VisitRoll(RollExpression request)
    {
        StringBuilder builder = new StringBuilder();
        foreach (var roll in request.Expressions)
        {
            builder.Append(roll.Accept(this));
            builder.Append(' ');
        }
        builder.Length -= 1;
        return $"{builder}";
    }

    protected override FormattableString VisitStackingExploding(StackingExplodingExpression stacking)
    {
        return PrintExploding(stacking.Roll, stacking.Target, stacking.Type, " se");
    }

    protected override FormattableString VisitTarget(TargetExpression target)
    {
        var orig = target.Set.Accept(this);
        var idn = InnerDiceNumber(target.Target);
        var type = target.Type switch
        {
            TargetType.Exact => "",
            TargetType.OrHigher => "+",
            TargetType.OrLower => "-",
            _ => throw new InvalidOperationException(),
        };
        return $"{orig} t{idn}{type}";
    }

    protected override FormattableString VisitShadowrun(ShadowrunExpression shadowrun)
    {
        FormattableString str = $"sr{InnerDiceNumber(shadowrun.DiceCount)}";
        if (shadowrun.Exploding)
        {
            str = $"{str} ie";
        }
        if (shadowrun.Limit is not null)
        {
            str = $"{str} l{InnerDiceNumber(shadowrun.Limit)}";
        }
        return str;
    }

    protected override FormattableString VisitOldShadowrun(OldShadowrunExpression osr)
    {
        FormattableString str = $"osr{InnerDiceNumber(osr.DiceCount)}";
        if (osr.Target is not null)
        {
            str = $"{str} t{InnerDiceNumber(osr.Target)}";
        }
        return str;
    }

    protected override FormattableString VisitStarWars(StarwarsExpression sw)
    {
        return $"sw{InnerDiceNumber(sw.DiceCount)}";
    }

    protected override FormattableString VisitEarthdawn(EarthdawnExpression ed)
    {
        return $"ed{InnerDiceNumber(ed.Step)}";
    }

    protected override FormattableString VisitVersus(VersusExpression vs)
    {
        return $"{Visit(vs.Left)} vs. {Visit(vs.Right)}";
    }

    protected override FormattableString VisitGenesysSet(GenesysSetExpression set)
    {
        // todo: can we formattable string this
        // also, do we care
        StringBuilder sb = new StringBuilder();
        foreach (GenesysExpression desc in set.Expressions)
        {
            sb.Append(desc.Accept(this));
        }
        return $"g: {sb}";
    }

    protected override FormattableString VisitGenesysDice(GenesysDiceExpression roll)
    {
        return $"{roll.Dice switch
        {
            GenesysDice.Boost => 'b',
            GenesysDice.Ability => 'g',
            GenesysDice.Proficiency => 'y',
            GenesysDice.Setback => 'k',
            GenesysDice.Difficulty => 'p',
            GenesysDice.Challenge => 'r',
            _ => throw new InvalidOperationException()
        }}";
    }

    protected override FormattableString VisitGenesysLiteral(GenesysLiteralExpression lit)
    {
        return $"{lit.Value switch
        {
            GenesysToken.Success => 's',
            GenesysToken.Failure => 'f',
            GenesysToken.Advantage => 'a',
            GenesysToken.Threat => 't',
            GenesysToken.Triumph => 'i',
            GenesysToken.Despair => 'd',
            _ => throw new InvalidOperationException()
        }}";
    }

    protected override FormattableString VisitLiteral(LiteralExpression literal)
    {
        return $"{literal.Value}";
    }

    protected override FormattableString VisitDivZero(PossibleDivZeroError error) => VisitError(error.Arith);
    protected override FormattableString VisitMultipleOverflow(MultipleOverflowError error) => VisitError(error.Multiple);
    protected override FormattableString VisitTypeError(NumericTypeError error) => VisitError(error.Original);
    protected override FormattableString VisitMustBePositive(MustBePositiveError error) => VisitError(error.Expression);
    protected override FormattableString VisitRepeatRollOverflow(RepeatRollOverflowError error) => VisitError(error.Expression);
    protected override FormattableString VisitInfiniteNumeric(InfiniteNumericError error) => VisitError(error.Original);
    protected override FormattableString VisitSyntaxError(SyntaxError error)
    {
        if (UnderlineErrors)
        {
            return $"{error.Roll[..error.Start]}__{error.Roll[error.Start..error.End]}__{error.Roll[error.End..]}";
        }
        else
        {
            return $"{error.Roll}";
        }
    }

    private new FormattableString VisitError(Expression expr)
    {
        if (UnderlineErrors)
        {
            return $"__{Visit(expr)}__";
        }
        else
        {
            return Visit(expr);
        }
    }
}
