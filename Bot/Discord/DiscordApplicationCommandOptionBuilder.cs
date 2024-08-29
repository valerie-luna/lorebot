using DSharpPlus;
using DSharpPlus.Entities;

namespace Lore.Discord;

public class DiscordApplicationCommandOptionBuilder
{
    private string? name;
    private bool? required;
    private DiscordApplicationCommandOptionType? type;
    private List<DiscordApplicationCommandOption> suboptions = new List<DiscordApplicationCommandOption>();
    private string? description;
    private (string name, string value)[]? choices;

    public DiscordApplicationCommandOption Build()
    {
        if (name is null || description is null || type is null)
            throw new InvalidOperationException();
        return new DiscordApplicationCommandOption(
            name, description, type.Value, required, options: suboptions, choices: choices?.Select(c => new DiscordApplicationCommandOptionChoice(c.name, c.value)).ToArray()!
        );
    }

    internal DiscordApplicationCommandOptionBuilder AddOption(string name, DiscordApplicationCommandOptionType type, 
        string description, bool isRequired, (string name, string value)[]? choices = null)
    {
        var builder = new DiscordApplicationCommandOptionBuilder()
            .WithName(name)
            .WithDescription(description)
            .WithType(type)
            .WithRequired(isRequired);

        if (choices is not null && type is not DiscordApplicationCommandOptionType.String)
            throw new InvalidOperationException();

        if (choices?.Length > 25)
            throw new InvalidOperationException();

        if (choices?.Length > 0)
            builder.WithChoices(choices);

        this.suboptions.Add(builder.Build());
        return this;
    }

    private DiscordApplicationCommandOptionBuilder WithChoices((string name, string value)[] choices)
    {
        this.choices = choices;
        return this;
    }

    internal DiscordApplicationCommandOptionBuilder WithDescription(string description)
    {
        this.description = description;
        return this;
    }

    internal DiscordApplicationCommandOptionBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }

    internal DiscordApplicationCommandOptionBuilder WithRequired(bool required)
    {
        this.required = required;
        return this;
    }

    internal DiscordApplicationCommandOptionBuilder WithType(DiscordApplicationCommandOptionType type)
    {
        this.type = type;
        return this;
    }
}