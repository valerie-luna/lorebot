using System.Diagnostics;
using System.Reflection;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lore.Discord;

public class EventsHandler(IOptions<DiscordBotCommandConfig> options,
        ILogger<EventsHandler> logger,
        IServiceProvider provider
    ) : IEventHandler<SessionCreatedEventArgs>,
        IEventHandler<InteractionCreatedEventArgs>,
        IEventHandler<ModalSubmittedEventArgs>
{
    private readonly Dictionary<string, Type> commandMap = []; 
    private readonly Dictionary<string, Type> modalMap = []; 

    public async Task HandleEventAsync(DiscordClient sender, SessionCreatedEventArgs eventArgs)
    {
        await RegisterCommands(sender);
        await RegisterModals(sender);
    }

    public async Task HandleEventAsync(DiscordClient sender, InteractionCreatedEventArgs args)
    {
        try
        {
            using var scope = provider.CreateScope();
            var type = commandMap[args.Interaction.Data.Name];
            var service = (ICommand)scope.ServiceProvider.GetRequiredService(type);
            await service.Execute(args.Interaction);
        }
        catch (NotFoundException)
        {
            logger.LogWarning("Attempted to respond to an interaction after it had expired.");
        }
        catch (CommandFinishedException)
        {
            // basically a nonlocal cancel
        }
        catch (Exception e)
        {
#warning todo: carry some data through, can't just serialize the whole thing
            logger.LogCritical(e, "Unhandled exception in command: {} serving: {}",
                args.Interaction.Data.Name, args.Interaction);
            await args.Interaction.RespondAsync("<:rei:1086948649633857606>");
        }
    }

    public async Task HandleEventAsync(DiscordClient sender, ModalSubmittedEventArgs args)
    {
        try
        {
            using var scope = provider.CreateScope();
            var type = modalMap[args.Interaction.Data.CustomId];
            var service = (IModal)scope.ServiceProvider.GetRequiredService(type);
            await service.Execute(args.Interaction);
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Unhandled exception in modal: {} serving: {}", 
                args.Interaction.Data.CustomId, args.Interaction);
            await args.Interaction.RespondAsync("<:rei:1086948649633857606>");
        }
    }

    private async Task RegisterCommands(DiscordClient client)
    {
        foreach (Type type in options.Value.Commands)
        {
            MethodInfo configureMethod = type.GetMethod(nameof(ICommand.Configure), BindingFlags.Static | BindingFlags.Public)
                ?? throw new InvalidOperationException();
            DiscordApplicationCommandBuilder builder = new DiscordApplicationCommandBuilder();
            object? name = configureMethod.Invoke(null, [builder]);
            Debug.Assert(name is not null and string);
            commandMap[(string)name] = type;
            var command = builder.Build();
            await client.CreateGlobalApplicationCommandAsync(command);
        }
    }

    private Task RegisterModals(DiscordClient client)
    {
        foreach (Type type in options.Value.Modals)
        {
            PropertyInfo customIdProperty = type.GetProperty(nameof(IModal.CustomId), BindingFlags.Static | BindingFlags.Public)
                ?? throw new InvalidOperationException();
            string id = customIdProperty.GetMethod?.Invoke(null, []) as string
                ?? throw new InvalidOperationException();
            modalMap[id] = type;
        }
        return Task.CompletedTask;
    }
}