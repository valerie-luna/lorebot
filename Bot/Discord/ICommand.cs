
using DSharpPlus.Entities;

namespace Lore.Discord;

public interface ICommand : IRegisterable
{
    static abstract string Configure(DiscordApplicationCommandBuilder builder);
    Task Execute(DiscordInteraction command);
}
