using System.Diagnostics;
using System.Numerics;

namespace DiceRolling.Expressions.Visitors;

// saturating decimal
public readonly struct SatDec(decimal d) : IComparable<SatDec>,
    IComparisonOperators<SatDec, SatDec, bool>,
    IAdditionOperators<SatDec, SatDec, SatDec>,
    ISubtractionOperators<SatDec, SatDec, SatDec>,
    IMultiplyOperators<SatDec, SatDec, SatDec>,
    IDivisionOperators<SatDec, SatDec, SatDec>,
    IModulusOperators<SatDec, SatDec, SatDec>,
    IUnaryNegationOperators<SatDec, SatDec>
{
    private readonly decimal d = d;

    public int CompareTo(SatDec other) => d.CompareTo(other.d);

    public static SatDec operator +(SatDec left, SatDec right)
    {
        Debug.Assert(left.d != decimal.MaxValue || right.d != decimal.MinValue);
        Debug.Assert(left.d != decimal.MinValue || right.d != decimal.MaxValue);
        if (left.d == decimal.MaxValue || right.d == decimal.MaxValue)
            return decimal.MaxValue;
        if (left.d == decimal.MinValue || right.d == decimal.MinValue)
            return decimal.MinValue;
        try
        {
            return left.d + right.d;
        }
        catch (OverflowException)
        {
            // +big + +big = +inf
            // -big + -big = -inf
            Debug.Assert(decimal.Sign(left.d) == decimal.Sign(right.d));
            return decimal.MaxValue * decimal.Sign(left.d);
        }
    }

    public static SatDec operator -(SatDec left, SatDec right)
    {
        Debug.Assert(left.d != decimal.MaxValue || right.d != decimal.MaxValue);
        Debug.Assert(left.d != decimal.MinValue || right.d != decimal.MinValue);
        if (left.d == decimal.MaxValue || right.d == decimal.MinValue)
            return decimal.MaxValue;
        if (left.d == decimal.MinValue || right.d == decimal.MaxValue)
            return decimal.MinValue;
        try
        {
            return left.d - right.d;
        }
        catch (OverflowException)
        {
            // +big - -big = +inf
            // -big - +big = -inf
            Debug.Assert(decimal.Sign(left.d) != decimal.Sign(right.d));
            return decimal.MaxValue * decimal.Sign(left.d);
        }
    }

    public static SatDec operator *(SatDec left, SatDec right)
    {
        Debug.Assert(left.d != decimal.MaxValue || right.d != decimal.MinValue);
        Debug.Assert(left.d != decimal.MinValue || right.d != decimal.MaxValue);
        if (left.d == decimal.MaxValue)
            return decimal.MaxValue * decimal.Sign(right.d);
        if (right.d == decimal.MaxValue)
            return decimal.MaxValue * decimal.Sign(left.d);
        if (left.d == decimal.MinValue)
            return decimal.MinValue * decimal.Sign(right.d);
        if (right.d == decimal.MinValue)
            return decimal.MinValue * decimal.Sign(left.d);
        try
        {
            return left.d * right.d;
        }
        catch (OverflowException)
        {
            // +big * +big = +inf
            // -big * -big = +inf
            // +big * -big = -inf
            // -big * +big = -inf
            return decimal.MaxValue * decimal.Sign(left.d) * decimal.Sign(right.d);  
        }
    }

    public static SatDec operator /(SatDec left, SatDec right)
    {
        Debug.Assert(!((left.d == decimal.MaxValue || left.d == decimal.MinValue)
         && (right.d == decimal.MaxValue || right.d == decimal.MinValue)));
        if (left.d == decimal.MaxValue)
            return decimal.MaxValue;
        if (right.d == decimal.MaxValue)
            return 0;
        if (left.d == decimal.MinValue)
            return decimal.MinValue;
        if (right.d == decimal.MinValue)
            return 0;
        return left.d / right.d;
    }

    public static SatDec operator %(SatDec left, SatDec right)
    {
        Debug.Assert(left.d != decimal.MaxValue && left.d != decimal.MinValue);
        if (right.d == decimal.MaxValue || right.d == decimal.MinValue)
            return left;
        return left.d % right.d;
    }

    public static bool operator ==(SatDec left, SatDec right) => left.d == right.d;
    public static bool operator !=(SatDec left, SatDec right) => left.d != right.d;
    public static bool operator <(SatDec left, SatDec right) => left.d < right.d;
    public static bool operator >(SatDec left, SatDec right) => left.d > right.d;
    public static bool operator <=(SatDec left, SatDec right) => left.d <= right.d;
    public static bool operator >=(SatDec left, SatDec right) => left.d >= right.d;

    public static implicit operator SatDec(decimal dec) => new(dec);
    public static explicit operator decimal(SatDec dec) => dec.d;

    public static SatDec operator -(SatDec value)
    {
        return -value.d;
    }

    public override bool Equals(object? obj) => obj is SatDec sat && this == sat;
    public override int GetHashCode() => d.GetHashCode();

    public static SatDec Max(SatDec l, SatDec r) => Math.Max(l.d, r.d);
    public static SatDec Min(SatDec l, SatDec r) => Math.Min(l.d, r.d);

    public override string ToString()
    {
        if (d == decimal.MinValue)
            return "-Inf";
        else if (d == decimal.MaxValue)
            return "+Inf";
        return d.ToString();
    }

    public static readonly SatDec NegInf = new SatDec(decimal.MinValue);
    public static readonly SatDec Inf = new SatDec(decimal.MaxValue);

    public static SatDec Abs(SatDec dec) => Math.Abs(dec.d);
    public static SatDec Floor(SatDec dec) => Math.Floor(dec.d);
}