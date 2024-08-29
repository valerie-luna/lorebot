using System.Text;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Results;

namespace DiceRolling.Expressions.Visitors.Result;

public class DiceSetPrinterVisitor(bool sort) : ResultVisitor<string>
{
    private readonly bool sort = sort;
    protected internal override string VisitSingle(SingleRollResult single)
    {
        return single.Total.ToString();
    }

    protected internal override string VisitSet(ResultSet set)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        IEnumerable<NumericRollResult> tmp = set;
        if (sort)
            tmp = tmp.OrderByDescending(t => t.Total);
        sb = tmp.Select(Visit)
            .Aggregate(sb, (sb, s) => sb.Append($"{s}, "));
        if (tmp.Any())
            sb.Length -= 2;
        sb.Append(']');
        return sb.ToString();
    }

    protected internal override string VisitArithmetic(ArithmeticRollResult arith)
    {
        StringBuilder sb = new StringBuilder();
        if (ShouldBracket(arith.Left))
            sb.Append($"({Visit(arith.Left)})");
        else
            sb.Append(Visit(arith.Left));

        sb.Append(arith.Type switch
        {
            ArithmeticType.Add => " + ",
            ArithmeticType.Subtract => " - ",
            ArithmeticType.Multiply => " * ",
            ArithmeticType.Divide => " / ",
            ArithmeticType.Modulo => " % ",
            _ => throw new InvalidOperationException(),
        });

        if (ShouldBracket(arith.Right))
            sb.Append($"({Visit(arith.Right)})");
        else
            sb.Append(Visit(arith.Right));
        return sb.ToString();

        bool ShouldBracket(RollResult inner) => inner is ArithmeticRollResult innerarith
            && SB(arith.Type, innerarith.Type);
        static bool SB(ArithmeticType outer, ArithmeticType inner) =>
            outer is ArithmeticType.Modulo 
            || inner is ArithmeticType.Modulo
            || (outer is ArithmeticType.Add or ArithmeticType.Subtract && inner is ArithmeticType.Multiply or ArithmeticType.Divide)
            || (outer is ArithmeticType.Multiply or ArithmeticType.Divide && inner is ArithmeticType.Add or ArithmeticType.Subtract);
    }

    protected internal override string VisitReplacementTotal(TotalReplacementRollResult total)
    {
        return Visit(total.Inner);
    }

    protected internal override string VisitVersus(VersusResult vs)
    {
        return $"{Visit(vs.Left)} vs. {Visit(vs.Right)}";
    }

    protected internal override string VisitGenesysSet(GenesysSetRollResult set)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        sb = set.Elements.Select(Visit)
            .Aggregate(sb, (sb, s) => sb.Append($"{s}, "));
        sb.Length -= 2;
        sb.Append(']');
        return sb.ToString();
    }

    protected internal override string VisitGenesysDice(GenesysDiceRollResult dice)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        sb = dice.Result.Aggregate(sb, (sb, s) => sb.Append($"{s}, "));
        if (sb.Length > 1)
            sb.Length -= 2;
        sb.Append(']');
        return sb.ToString();
    }

    protected internal override string VisitGenesysLiteral(GenesysLiteralRollResult lit)
    {
        return lit.Token.ToString();
    }

} 
