using System.Diagnostics;
using System.Text.Json;
using Lore.Discord;
using DSharpPlus.Entities;

namespace Lore.Cat;

public class CatCommand : ICommand
{
    private readonly IHttpClientFactory clientFactory;

    public CatCommand(IHttpClientFactory clientFactory)
    {
        this.clientFactory = clientFactory;
    }

    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        builder.WithName("cat")
            .WithDescription("Cat.");

        return "cat";
    }

    public async Task Execute(DiscordInteraction command)
    {
        try
        {
            var client = clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(2);
            var response = await client.GetAsync("https://api.thecatapi.com/v1/images/search");
            var stream = await response.Content.ReadAsStreamAsync();
            var url = await JsonSerializer.DeserializeAsync<Cat[]>(stream);
            Debug.Assert(url is not null);
            await command.RespondAsync(url[0].url.ToString());
        }
        catch
        {
            await command.RespondAsync(":cat:");
        }
    }

    private record Cat(Uri url);
}