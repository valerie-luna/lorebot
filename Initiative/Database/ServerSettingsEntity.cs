namespace Initiative.Database;

internal class ServerSettingsEntity
{
    public ulong ServerId { get; set; }
    public InitiativeConfiguration Config { get; set; }
}
