using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Antlr4.Runtime.Tree;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Results;
using DiceRolling.Expressions.Visitors;
using static DiceRolling.DiceRollingParser;

namespace DiceRolling;

public static class Utilities
{

    public static ExpressionType DecimalCast(ExpressionType left, ExpressionType right)
    {
        if (left is ExpressionType.Decimal || right is ExpressionType.Decimal)
            return ExpressionType.Decimal;
        Debug.Assert(left is ExpressionType.Integer && right is ExpressionType.Integer);
        return ExpressionType.Integer;
    }

#if DEBUG
    public static T AssertNotNull<T>(this T? tee, [CallerFilePath] string fp = "", 
        [CallerLineNumber] int linenum = 0, [CallerArgumentExpression(nameof(tee))] string arg = "")
        where T : class
    {
        Debug.Assert(tee is not null, $"{arg} was null ({fp}:line {linenum})");
        return tee;
    }
#else
    public static T AssertNotNull<T>(this T? tee) => tee!;
#endif

    public static int ToInt(this ITerminalNode node)
    {
        return int.Parse(node.GetText());
    } 

    //public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? tee) => tee ?? [];

    public static KeepType ParseKeepType(this SignContext sign)
    {
        Debug.Assert(sign.GetText() is "+" or "-");
        return sign.GetText() switch
        {
            "+" => KeepType.Highest,
            "-" => KeepType.Lowest,
            _ => throw new NotImplementedException()
        };
    }

    public static TargetType ParseTargetType(this SignContext? sign)
    {
        Debug.Assert(sign is null || sign.GetText() is "+" or "-");
        return sign?.GetText() switch
        {
            null => TargetType.Exact,
            "+" => TargetType.OrHigher,
            "-" => TargetType.OrLower,
            _ => throw new NotImplementedException()
        };
    }

    public static IEnumerable<T> Only<T>(T tee)
    {
        yield return tee;
    }

    public static IEnumerable<T> Repeat<T>(T element, long count)
    {
        if (count < int.MaxValue)
            return Enumerable.Repeat(element, (int)count);
        return Internal();
        IEnumerable<T> Internal()
        {
            for (long l = 0; l < count; l++)
                yield return element;
        } 
    }

    public static IEnumerable<T> Take<T>(this IEnumerable<T> tee, long count)
    {
        if ((int)count == count)
        { // stupid premature optimization, but it's fine
            return Enumerable.Take(tee, (int)count);
        }
        return Internal(); 
        IEnumerable<T> Internal()
        {
            long i = 0;
            foreach (T item in tee)
            {
                if (++i > count)
                    yield break;
                yield return item;
            }
        }
    }

    public static SatDec Min(SatDec a, SatDec b,
        SatDec c, SatDec d)
    {
        return SatDec.Min(SatDec.Min(a, b), SatDec.Min(c, d));
    }

    public static SatDec Max(SatDec a, SatDec b,
        SatDec c, SatDec d)
    {
        return SatDec.Max(SatDec.Max(a, b), SatDec.Max(c, d));
    }

}

internal static class ResultSetBuilder
{
    internal static ResultSet Create(ReadOnlySpan<NumericRollResult> values) => new(ImmutableArray.Create(values));
}
