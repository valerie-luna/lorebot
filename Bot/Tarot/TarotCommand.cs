using System.Diagnostics;
using System.Numerics;
using Lore.Discord;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Lore.Tarot;

public class TarotCommand : ICommand
{
    public static string Configure(DiscordApplicationCommandBuilder builder)
    {
        var deckOption = new DiscordApplicationCommandOptionBuilder()
            .WithName("suits")
            .WithDescription("What suits to roll? Examples: 'cups', 'major', 'fire earth', 'clubs, diamonds'")
            .WithType(DiscordApplicationCommandOptionType.String)
            .WithRequired(false);

        builder.WithName("tarot")
            .WithDescription("Draw a card from a tarot deck.")
            .AddOption(deckOption);

        return "tarot";
    }

    public async Task Execute(DiscordInteraction command)
    {
        TarotSuit suitsActual;
        if (command.Data.Options?.SingleOrDefault()?.Value is not string suits)
            suitsActual = TarotSuit.Minor | TarotSuit.Major;
        else
            suitsActual = Parse(suits);
        var gen = new TarotGenerator(suitsActual);
        await command.RespondAsync(gen.Generate());
    }

    private static readonly IReadOnlyDictionary<string, TarotSuit> suits 
        = new Dictionary<string, TarotSuit>()
    {
        ["wands"]     = TarotSuit.Batons,
        ["batons"]    = TarotSuit.Batons,
        ["clubs"]     = TarotSuit.Batons,
        ["staves"]    = TarotSuit.Batons,
        ["fire"]      = TarotSuit.Batons,
        ["pentacles"] = TarotSuit.Coins,
        ["coins"]     = TarotSuit.Coins,
        ["disks"]     = TarotSuit.Coins,
        ["rings"]     = TarotSuit.Coins,
        ["diamonds"]  = TarotSuit.Coins,
        ["earth"]     = TarotSuit.Coins,
        ["cups"]      = TarotSuit.Cups,
        ["chalices"]  = TarotSuit.Cups,
        ["goblets"]   = TarotSuit.Cups,
        ["vessels"]   = TarotSuit.Cups,
        ["hearts"]    = TarotSuit.Cups,
        ["water"]     = TarotSuit.Cups,
        ["swords"]    = TarotSuit.Blades,
        ["blades"]    = TarotSuit.Blades,
        ["spades"]    = TarotSuit.Blades,
        ["air"]       = TarotSuit.Blades,
        ["minor"]     = TarotSuit.Minor,
        ["major"]     = TarotSuit.Major   
    };

    private static TarotSuit Parse(string input)
    {
        string[] strs = input.Split(new[] {" ", ",", "+"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        TarotSuit suit = default;
        foreach (string str in strs)
        {
            if (suits.TryGetValue(str, out TarotSuit outv))
                suit |= outv;
        }
        return suit;
    }
}

public record TarotGenerator(TarotSuit AcceptableSuits)
{
    private readonly Random random = new Random();
    private bool HasMajor => (AcceptableSuits & TarotSuit.Major) == TarotSuit.Major;
    private TarotSuit Minors => AcceptableSuits & TarotSuit.Minor;
    private bool HasAnyMinor => Minors != 0;

    private readonly static string[] Minor = new[]
    {
        "Ace",
        "Two",
        "Three",
        "Four",
        "Five",
        "Six",
        "Seven",
        "Eight",
        "Nine",
        "Ten",
        "Page",
        "Knight",
        "Queen",
        "King"
    };

    private readonly static string[] ShadowrunMajor = new[]
    {
        "The Matrix",
        "The High Priestess",
        "Aes Sidhe Banrigh",
        "The Chief Executive",
        "The Higher Power",
        "The Avatars",
        "The Ride",
        "Discipline",
        "The Hermit",
        "The Wheel of Fortune",
        "The Vigilante",
        "The Hanged Man",
        "404",
        "Threshold",
        "The Dragon",
        "The Tower",
        "The Comet",
        "The Shadows",
        "The Eclipse",
        "Karma",
        "The Awakened World",
        "The Bastard"
    };

    public string Generate()
    {
        int minors = BitOperations.PopCount((uint)Minors);
        bool major = HasMajor;
        int count = minors * Minor.Length + (major ? ShadowrunMajor.Length : 0);
        int card = random.Next(0, count);
        int suit = card / Minor.Length;
        if (suit > (minors - 1)) // it's a major
        {
            int majorCard = (card - minors * Minor.Length);
            string majorString = ShadowrunMajor[majorCard];
            return $"**{ToRomanNumeral(majorCard + 1)}: {majorString}**";
        }
        else
        {
            TarotSuit actualSuit = GetMinorSuit(suit);
            string cardString = Minor[card % Minor.Length];
            return $"**{cardString} of {actualSuit}**";
        }
    }

    private static string ToRomanNumeral(int number)
    {
        return number switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            4 => "IIII",
            5 => "V",
            6 => "VI",
            7 => "VII",
            8 => "VIII",
            9 => "VIIII",
            10 => "X",
            11 => "XI",
            12 => "XII",
            13 => "XIII",
            14 => "XIIII",
            15 => "XV",
            16 => "XVI",
            17 => "XVII",
            18 => "XVIII",
            19 => "XVIIII",
            20 => "XX",
            21 => "XXI",
            22 => "O",
            _ => throw new NotImplementedException()
        };
    }

    private TarotSuit GetMinorSuit(int index)
    {
        Debug.Assert(index <= BitOperations.PopCount((uint)Minors));
        int found = 0;
        foreach (TarotSuit suit in new[] { TarotSuit.Blades, TarotSuit.Coins, TarotSuit.Batons, TarotSuit.Cups})
        {
            if ((Minors & suit) == suit)
                found++;
            if (found > index)
                return suit;
        }
        throw new InvalidOperationException();
    }
}

[Flags]
public enum TarotSuit
{
    Major = 0b1,
    Blades = 0b10,
    Coins = 0b100,
    Batons = 0b1000,
    Cups = 0b10000,
    Minor = Blades | Coins | Batons | Cups,
}