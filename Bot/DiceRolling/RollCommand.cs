using Lore.Discord;
using DiceRolling;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using DiceRolling.Formatting;

namespace Lore.DiceRolling;
public class RollCommand(IOptions<RollCommandOptions> options, 
    DiceFormatter<DiscordInteractionResponseBuilder> formatter) : ICommand
{
    private readonly int MaxRollIterations = options.Value.MaxRollIterations;
    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        builder.WithName("roll")
            .WithDescription("Roll dice!")
            .AddOption("message",
                DiscordApplicationCommandOptionType.String,
                description: "The dice to roll");

        return "roll";
    }

    public async Task Execute(DiscordInteraction command)
    {
        var roll = command.Data.Options.SingleOrDefault()?.Value as string;
        roll = roll!.Trim();
        var expression = new DiceRoller(MaxRollIterations).Parse(roll);
        if (expression is null)
        {
            Debug.Assert(false);
            await command.RespondAsync("Syntax error - and bug in the application!");
            return;
        }
        var result = formatter.Format(expression);
        await command.CreateResponseAsync(
            DiscordInteractionResponseType.ChannelMessageWithSource,
            result
        );
    }
}
