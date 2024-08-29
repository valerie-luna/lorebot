using DiceRolling.Expressions;

namespace DiceRolling;

public interface IDiceRoller
{
    Expression Parse(string roll);
}
