namespace Initiative;

public record InitiativeEntry(string Name, InitiativeSet Value, bool HasActed,
    bool Hidden, PingId? Ping) : IComparable<InitiativeEntry>
{
    public int CompareTo(InitiativeEntry? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}

