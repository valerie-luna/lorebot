namespace Initiative.Database;

internal class LastRollEntity
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public ulong ChannelId { get; set; }
    public string Name { get; set; } = default!;
    public string Roll { get; set; } = default!;
    public ulong? PingUser { get; set; } = default!;
    public bool Hidden { get; set; }
}
