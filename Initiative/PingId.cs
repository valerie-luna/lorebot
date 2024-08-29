namespace Initiative;

public record struct PingId(ulong Id)
{
    public static PingId? From(ulong? id) => id is null ? null : new PingId(id.Value);

    public override string ToString()
    {
        return Id.ToString();
    }
}
