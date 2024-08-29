namespace Lore.Music;

internal class MusicEntryEntity
{
    public int Id { get; set; }
    public ulong ServerId { get; set; }
    public required string Name { get; set; }
    public required byte[] Stream { get; set; }
    public MusicType Type { get; set; }
}
