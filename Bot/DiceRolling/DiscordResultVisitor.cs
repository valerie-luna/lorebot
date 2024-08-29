// See https://aka.ms/new-console-template for more information

using System.Text;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Genesys;

namespace Lore.DiceRolling;

public class DiscordResultVisitor
{

    private static string FromToken(GenesysToken token) => token switch
    {
        GenesysToken.Success => GenesysSuccess,
        GenesysToken.Failure => "<:genesys_failure:1239089372041707600>",
        GenesysToken.Advantage => GenesysAdvantage,
        GenesysToken.Threat => "<:genesys_threat:1239089374839177347>",
        GenesysToken.Triumph => GenesysTriumph,
        GenesysToken.Despair => "<:genesys_despair:1239089369181061120>",
        _ => throw new NotImplementedException(),
    };

    public const string GenesysSuccess = "<:genesys_success:1239089373308387398>";
    public const string GenesysAdvantage = "<:genesys_advantage:1239089376353452042>";
    public const string GenesysTriumph = "<:genesys_triumph:1239089370699534396>";

    private static string FromDice(GenesysDice dice) => dice switch
    {
        GenesysDice.Boost => "b",
        GenesysDice.Ability => "g",
        GenesysDice.Proficiency => "y",
        GenesysDice.Setback => "k",
        GenesysDice.Difficulty => "p",
        GenesysDice.Challenge => "r",
        _ => throw new NotImplementedException(),
    };
}
