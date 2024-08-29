using System.Numerics;
using DSharpPlus.Entities;

namespace Lore.Utilities;

public static class Utilities
{
    public static T GetOptionValue<T>(this DiscordInteractionDataOption opt, string name)
    {
        return opt.Options
            .Where(o => o.Name == name)
            .Select(o => (T)o.Value)
            .Single();
    }

    public static T? GetOptionValueOrDefault<T>(this DiscordInteractionDataOption opt, string name)
    {
        return opt.Options
            .Where(o => o.Name == name)
            .Select(o => (T)o.Value)
            .SingleOrDefault();
    }

    public static void Increment<T, U>(this Dictionary<T, U> dict, T element)
        where U : INumber<U>
        where T : notnull
    {
        if (dict.ContainsKey(element))
            dict[element] += U.One;
        else
            dict[element] = U.One;
    }

    public static T ParseEnum<T>(this string str) where T : struct, Enum => Enum.Parse<T>(str); 
}