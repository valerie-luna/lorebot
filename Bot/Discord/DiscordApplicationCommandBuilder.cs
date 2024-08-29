using DSharpPlus;
using DSharpPlus.Entities;

namespace Lore.Discord;

public class DiscordApplicationCommandBuilder
{
    private string? name;
    private bool dmPermission = false;
    private string? description;
    private List<DiscordApplicationCommandOption> options = new List<DiscordApplicationCommandOption>();

    public DiscordApplicationCommand Build()
    {
        if (name is null || description is null)
            throw new InvalidOperationException();
        return new DiscordApplicationCommand(
            name, description, options, allowDMUsage: dmPermission
        );
    }

    internal DiscordApplicationCommandBuilder AddOption(string optionName, 
        DiscordApplicationCommandOptionType optionType, string description, bool isRequired = true)
    {
        var builder = new DiscordApplicationCommandOptionBuilder()
            .WithName(optionName)
            .WithType(optionType)
            .WithDescription(description)
            .WithRequired(isRequired);
        return this.AddOption(builder);
    }

    internal DiscordApplicationCommandBuilder AddOption(DiscordApplicationCommandOptionBuilder builder)
    {
        this.options.Add(builder.Build());
        return this;
    }

    internal DiscordApplicationCommandBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    internal DiscordApplicationCommandBuilder WithDMPermission(bool hasPermission)
    {
        this.dmPermission = hasPermission;
        return this;
    }

    internal DiscordApplicationCommandBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }
}
