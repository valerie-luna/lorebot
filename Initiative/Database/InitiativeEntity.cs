namespace Initiative.Database;

internal class InitiativeEntity
{
    public int Id { get; set; }
    public ulong ServerId { get; set; }
    public ulong ChannelId { get; set; }
    public required string Name { get; set; }
    public decimal PrimaryValue { get; set; }
    public decimal SecondaryValue { get; set; }
    public decimal TeritaryValue { get; set; }
    public int TimesActed { get; set; }
    public ulong? PingUser { get; set; }
    public bool Hidden { get; set; }

    public ServerSettingsEntity ServerSettings { get; set; } = default!;
}
