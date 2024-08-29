using System.Collections;
using System.Diagnostics;

namespace Lore.Discord;

public class DiscordBotCommandConfig
{
    private HashSet<Type> commands = new HashSet<Type>();
    private HashSet<Type> modals = new HashSet<Type>();
    public void AddRegisterable(Type type)
    {
        Debug.Assert(type.IsAssignableTo(typeof(IRegisterable)));
        if (type.IsAssignableTo(typeof(IModal)))
            modals.Add(type);
        if (type.IsAssignableTo(typeof(ICommand)))
            commands.Add(type);
    }

    public IEnumerable<Type> Commands => commands;
    public IEnumerable<Type> Modals => modals;
}
