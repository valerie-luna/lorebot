using Lore.Cat;
using Lore.DiceRolling;
using Lore.Discord;
using Lore.Initiative;
using Lore.Tarot;
using Lore.Weather;
using Lore.Music;
using DSharpPlus.VoiceNext;
using Initiative;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weather.Context;
using Microsoft.Extensions.Configuration;
using Initiative.Database;
using DiceRolling.Formatting;
using DSharpPlus.Extensions;
using DSharpPlus.Entities;

namespace Lore;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(l =>
            {
                l.ClearProviders();
                l.AddConsole();
            })
            .ConfigureServices(Configure)
            .Build();
        using (var scope = host.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<InitiativeContext>();
            var mctx = scope.ServiceProvider.GetRequiredService<MusicContext>();
            var wctx = scope.ServiceProvider.GetRequiredService<WeatherContext>();

            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var dbtype = config["DATABASE_TYPE"]?.ToLowerInvariant();

            if (dbtype == "sqlite")
            {
                ctx.Database.EnsureDeleted();
                mctx.Database.EnsureDeleted();
                wctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                mctx.Database.EnsureCreated();
                wctx.Database.EnsureCreated();
            }
            else if (dbtype == "postgres")
            {
                ctx.Database.Migrate();
                mctx.Database.Migrate();
                wctx.Database.Migrate();
            }
            else
            {
                throw new InvalidOperationException("No database type configured");
            }
        }

        host.Run();
    }

    private static void Configure(HostBuilderContext context, IServiceCollection services)
    {
        var token = context.Configuration["TOKEN"] ?? throw new InvalidOperationException("No token");
        services.AddDiscordBot(token, cfg => cfg.UseVoiceNext(new VoiceNextConfiguration
        {
            PacketQueueSize = 7
        })).AddRegisterable<CatCommand>()
            .AddRegisterable<WeatherCommand>()
            .AddRegisterable<RollCommand, RollCommandOptions>(cfg => cfg.MaxRollIterations = 500)
            .AddRegisterable<InitiativeCommand>()
            .AddRegisterable<TarotCommand>()
            .AddRegisterable<MusicCommand>()
            ;

        services.ConfigureEventHandlers(a => a.AddEventHandlers<EventsHandler>(ServiceLifetime.Singleton));

        services.AddTransient<DiceFormatter<DiscordInteractionResponseBuilder>, 
            SimpleDiscordWrappingDiceFormatter>();

        services.AddHttpClient();

        var config = context.Configuration["DATABASE_TYPE"]?.ToLowerInvariant();
        if (config == "sqlite")
        {
            ConfigureSqlite(context.Configuration, services);
        }
        else if (config == "postgres")
        {
            ConfigurePostgres(context.Configuration, services);
        }
        else
        {
            throw new InvalidOperationException("No database type configured");
        }

        services.AddScoped<InitiativeModel>();
        services.AddScoped<MusicModel>();
        
        services.AddSingleton<MusicThreadHandler>();
    }

    private static void ConfigureSqlite(IConfiguration config, IServiceCollection services)
    {
        services.AddDbContext<InitiativeContext>(ic => 
            ic.UseSqlite("Data Source=initiatives.db;")
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
        services.AddDbContext<MusicContext>(ic => 
            ic.UseSqlite("Data Source=music.db;")
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
        services.AddDbContext<WeatherContext>(ic => 
            ic.UseSqlite("Data Source=weather.db;")
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
    }

    private static void ConfigurePostgres(IConfiguration config, IServiceCollection services)
    {
        string username = config["POSTGRES_USERNAME"] ?? throw new InvalidOperationException("No username");
        string? password = config["POSTGRES_PASSWORD"];
        string server = config["POSTGRES_SERVER"] ?? throw new InvalidOperationException("No server");
        string port = config["POSTGRES_PORT"] ?? throw new InvalidOperationException("No port");

        services.AddDbContext<InitiativeContext>(ic => 
            ic.UseNpgsql($"Host={server};Port={port};Database=lorebot_initiative;Username={username};" + (password == null ? "" : $"Password={password};"))
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
        services.AddDbContext<MusicContext>(ic => 
            ic.UseNpgsql($"Host={server};Port={port};Database=lorebot_music;Username={username};" + (password == null ? "" : $"Password={password};"))
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
        services.AddDbContext<WeatherContext>(ic => 
            ic.UseNpgsql($"Host={server};Port={port};Database=lorebot_weather;Username={username};" + (password == null ? "" : $"Password={password};"))
#if DEBUG
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
#endif
        );
    }
}