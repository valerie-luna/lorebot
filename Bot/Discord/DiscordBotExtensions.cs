using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lore.Discord;

public static class DiscordBotExtensions
{
    public static IServiceCollection AddDiscordBot(this IServiceCollection services, string token, Action<DiscordClientBuilder>? cfg = null)
    {
        services.AddHostedService<DiscordBot>();

        services.AddSingleton(s => 
        {
            var builder = DiscordClientBuilder.CreateDefault(token, DiscordIntents.AllUnprivileged, services);
            if (cfg is not null)
                cfg(builder);
            return builder.Build();
        });

        return services;
    }

    public static IServiceCollection AddRegisterable<T>(this IServiceCollection services)
        where T : class, IRegisterable
    {
        return services.Configure<DiscordBotCommandConfig>(c => c.AddRegisterable(typeof(T)))
            .AddScoped<T>();
    }

    public static IServiceCollection AddRegisterable<T, TOptions>(this IServiceCollection services, Action<TOptions> cfg)
        where T : class, IRegisterable where TOptions : class
    {
        return services.Configure<DiscordBotCommandConfig>(c => c.AddRegisterable(typeof(T)))
            .AddScoped<T>()
            .Configure(cfg);
    }

    public static Task RespondAsync(this DiscordInteraction interaction, string response, 
        bool ephemeral = false)
    {
        var builder = new DiscordInteractionResponseBuilder()
            .WithContent(response)
            .AsEphemeral(ephemeral);

        return interaction.CreateResponseAsync(
            DiscordInteractionResponseType.ChannelMessageWithSource,
            builder
        );
    }

    public static Task RespondWithModalAsync(this DiscordInteraction interaction, DiscordInteractionResponseBuilder builder)
    {
        return interaction.CreateResponseAsync(
            DiscordInteractionResponseType.Modal,
            builder
        );
    }
}