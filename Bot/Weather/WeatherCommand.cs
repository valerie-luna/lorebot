using Lore.Discord;
using DSharpPlus;
using DSharpPlus.Entities;
using Weather;
using Weather.Context;
using Lore.Initiative;
using Lore.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;

namespace Lore.Weather;

public class WeatherCommand : ICommand
{
    private readonly WeatherContext context;

    public WeatherCommand(WeatherContext context)
    {
        this.context = context;
    }

    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        var generate = new DiscordApplicationCommandOptionBuilder()
            .WithName("generate")
            .WithDescription("Generate random weather.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption("location", DiscordApplicationCommandOptionType.String, 
                description: "Where would you like the weather?", 
                isRequired: true)
            .AddOption("seed", DiscordApplicationCommandOptionType.Integer,
                description: "Random seed to use.",
                isRequired: false)
            .AddOption("mana", DiscordApplicationCommandOptionType.Number,
                description: "Fixed mana level.",
                isRequired: false);

        var list = new DiscordApplicationCommandOptionBuilder()
            .WithName("list")
            .WithDescription("List available weather regions.")
            .WithType(DiscordApplicationCommandOptionType.SubCommand);

        var distribution = new DiscordApplicationCommandOptionBuilder()
            .WithName("test")
            .WithDescription("Test the distribution of a given location")
            .WithType(DiscordApplicationCommandOptionType.SubCommand)
            .AddOption("location", DiscordApplicationCommandOptionType.String, 
                description: "Where would you like the weather?", 
                isRequired: true)
            .AddOption("mana", DiscordApplicationCommandOptionType.Number,
                description: "Fixed mana level.",
                isRequired: false)
            .AddOption("weather", DiscordApplicationCommandOptionType.String,
                description: "Filter only to given weather.",
                isRequired: false,
                choices: Enum.GetValues<WeatherEnum>()
                    .Where(e => e is not WeatherEnum.Unknown)
                    .Select(e => (e.ToPretty(), e.ToString()))
                    .ToArray())
            .AddOption("cloud", DiscordApplicationCommandOptionType.String,
                description: "Filter only to given cloud level.",
                isRequired: false,
                choices: Enum.GetValues<CloudLevel>()
                    .Where(e => e != CloudLevel.Unspecified)
                    .Select(e => (e.ToPretty(), e.ToString()))
                    .ToArray());

        builder.WithName("weather")
            .WithDescription("Generate random weather.")
            .AddOption(generate)
            .AddOption(list)
            .AddOption(distribution);

        return "weather";
    }

    public async Task Execute(DiscordInteraction command)
    {
        var option = command.Data.Options.Single();
        var task = option.Name switch
        {
            "generate" => Generate(command),
            "list" => List(command),
            "test" => Test(command),
            _ => throw new NotImplementedException()
        };
        await task;
    }

    public async Task Generate(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("location").Trim().ToLowerInvariant();
        var seed = subcommand.GetOptionValueOrDefault<long?>("seed");
        var manaforce = subcommand.GetOptionValueOrDefault<double?>("mana");

        var settings = await context.Names
            .Where(n => n.Name == name)
            .Select(n => n.Settings)
            .SingleOrDefaultAsync();

        if (settings is null)
        {
            await command.RespondAsync($"Unrecognized location: {name}", ephemeral: true);
            return;
        }

        Random rand = seed is not null ? new Random((int)seed.Value) : new Random();
        var weather = WeatherGenerator.Generate(settings, rand, setManaLevel: manaforce);
        await command.RespondAsync(weather.ToString());
    }

    public async Task List(DiscordInteraction command)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        var names = await context.Names
            .GroupBy(n => n.WeatherSettingsId, sel => culture.TextInfo.ToTitleCase(sel.Name))
            .ToListAsync();

        var sb = new StringBuilder();
        foreach (var group in names)
        {
            bool first = true;
            foreach (var name in group)
            {
                if (first)
                {
                    sb.Append($"**{name}**");
                    first = false;
                }
                else
                {
                    sb.Append($", {name}");
                }
            }
            sb.AppendLine();
        }

        await command.RespondAsync(sb.ToString(), ephemeral: true);
        
    }

    const int iterations = 100_000;
    public async Task Test(DiscordInteraction command)
    {
        var subcommand = command.Data.Options.Single();
        var name = subcommand.GetOptionValue<string>("location").Trim().ToLowerInvariant();
        var manaforce = subcommand.GetOptionValueOrDefault<double?>("mana");
        var weatherFilter = subcommand.GetOptionValueOrDefault<string?>("weather")?.ParseEnum<WeatherEnum>();
        var cloudFilter = subcommand.GetOptionValueOrDefault<string?>("cloud")?.ParseEnum<CloudLevel>();

        var settings = await context.Names
            .Where(n => n.Name == name)
            .Select(n => n.Settings)
            .SingleOrDefaultAsync();

        if (settings is null)
        {
            await command.RespondAsync($"Unrecognized location: {name}", ephemeral: true);
            return;
        }

        Dictionary<WeatherEnum, int> weathers = new();
        Dictionary<CloudLevel, int> clouds = new();
        Dictionary<AirQualityEnum, int> aq = new();
        Dictionary<string, int> events = new();
        IncrementalStatistics temp = new();
        IncrementalStatistics airp = new();
        IncrementalStatistics wind = new();
        IncrementalStatistics humid = new();
        IncrementalStatistics mana = new();
        IncrementalStatistics aqi = new();
        int count = 0;

        Random rand = new Random();
        for (int i = 0; i < iterations; i++)
        {
            var weather = WeatherGenerator.Generate(settings, rand, setManaLevel: manaforce);
            if (weatherFilter is not null && !weather.Weather.Matches(weatherFilter.Value))
                continue;
            if (cloudFilter is not null && !weather.CloudLevel.Matches(cloudFilter.Value))
                continue;
            count++;
            weathers.Increment(weather.Weather);
            clouds.Increment(weather.CloudLevel);
            aq.Increment(weather.AirQuality);
            foreach (var ev in weather.CustomEvents)
                events.Increment(ev);
            temp.Add(weather.Temperature);
            airp.Add(weather.AirPressure);
            wind.Add(weather.Windspeed);
            humid.Add(weather.Humidity);
            mana.Add(weather.ManaLevel);
            aqi.Add(weather.AQI);
        }

        if (count == 0)
        {
            await command.RespondAsync("Selected filters didn't return any values. Your event is either impossible or very unlikely!", ephemeral: true);
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"**Statistics for {name}** ({count} iterations)");
        if (count < 10_000)
        {
            sb.AppendLine("**WARNING:** Filters limited to a small number of events. Estimates may be inaccurate.");
        }
        sb.Append($"**Avg. Temperature:** {temp.Mean:0.00} C");
        sb.AppendLine($" ({temp.Bottom5:0.00} - {temp.Top5:0.00}) err. ±{temp.ConfidenceInterval:0.00}");
        sb.Append($"**Avg. Air Pressure:** {airp.Mean:0.00} hPa");
        sb.AppendLine($" ({airp.Bottom5:0.00} - {airp.Top5:0.00}) err. ±{airp.ConfidenceInterval:0.00}");
        sb.Append($"**Avg. Windspeed:** {wind.Mean:0.00} kph");
        sb.AppendLine($" ({wind.Bottom5:0.00} - {wind.Top5:0.00}) err. ±{wind.ConfidenceInterval:0.00}");
        sb.Append($"**Avg. Humidity:** {humid.Mean:0.00}%");
        sb.AppendLine($" ({humid.Bottom5:0.00} - {humid.Top5:0.00}) err. ±{humid.ConfidenceInterval:0.00}");
        sb.Append($"**Avg. Mana Level:** {mana.Mean:0.00}%");
        sb.AppendLine($" ({mana.Bottom5:0.00} - {mana.Top5:0.00}) err. ±{mana.ConfidenceInterval:0.00}");
        sb.AppendLine();
        sb.AppendLine("**Weather**");
        foreach (var wp in weathers.OrderByDescending(w => w.Value))
            sb.AppendLine($"*{wp.Key.ToPretty()}*: {wp.Value / (double)count * 100:0.00}%");
        sb.AppendLine("**Cloud Level**");
        foreach (var wp in clouds.OrderByDescending(w => w.Value))
            sb.AppendLine($"*{wp.Key.ToPretty()}*: {wp.Value / (double)count * 100:0.00}%");
        sb.AppendLine("**Air Quality**");
        foreach (var wp in aq.OrderByDescending(w => w.Value))
            sb.AppendLine($"*{wp.Key.ToPretty()}*: {wp.Value / (double)count * 100:0.00}%");
        if (events.Any())
        {
            sb.AppendLine("**Events**");
            foreach (var wp in events.OrderBy(w => w.Key))
                sb.AppendLine($"*{wp.Key}*: {wp.Value / (double)count * 100:0.00}%");
        }

        await command.RespondAsync(sb.ToString(), ephemeral: true);
    }
}

public struct IncrementalStatistics
{
    public IncrementalStatistics()
    {
        count = 0;
        mean = 0;
        dSquared = 0;
    }
    private int count;
    private double mean;
    private double dSquared;
    
    public void Add(double value)
    {
        count++;
        double meanDiff = (value - mean) / count;
        double newMean = mean + meanDiff;
        double dSquaredIncrement = (value - newMean) * (value - mean);
        double dSquaredNew = dSquared + dSquaredIncrement;
        this.dSquared = dSquaredNew;
        this.mean = newMean;
    }

    public readonly double Mean => mean;
    private readonly double Variance => dSquared / count;
    public readonly double StdDev => Math.Sqrt(Variance);


    public readonly double Bottom5 => Mean - StdDev * 2; 
    public readonly double Top5 => Mean + StdDev * 2; 
    public readonly double ConfidenceInterval => ZLevel * StdDev / Math.Sqrt(count);
    private const double ZLevel = 2.807;
}