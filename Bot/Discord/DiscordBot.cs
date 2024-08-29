using DSharpPlus;
using Microsoft.Extensions.Hosting;

namespace Lore.Discord;

public class DiscordBot(DiscordClient client) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await client.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.DisconnectAsync();
    }
}
