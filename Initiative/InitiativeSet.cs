using System.Diagnostics;

namespace Initiative;

public abstract record InitiativeSet(decimal Primary, decimal Secondary, decimal Teritary) : IComparable<InitiativeSet>
{
    public int CompareTo(InitiativeSet? other)
    {
        if (other is null) return 1;
        if (Primary != other.Primary) return Primary.CompareTo(other.Primary);
        if (Secondary != other.Secondary) return Secondary.CompareTo(other.Secondary);
        return Teritary.CompareTo(other.Teritary);
    }

    public static InitiativeSet operator +(InitiativeSet left, InitiativeSet right)
    {
        Debug.Assert(left.GetType() == right.GetType());
        return left.Add(right);
    }

    protected abstract InitiativeSet Add(InitiativeSet other);
}
