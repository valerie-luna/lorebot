
using DSharpPlus.Entities;

namespace Lore.Discord;

public interface IModal : IRegisterable
{
    static abstract string CustomId { get; }
    Task Execute(DiscordInteraction modal);
}
