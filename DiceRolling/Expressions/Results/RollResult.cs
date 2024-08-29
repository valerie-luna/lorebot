using DiceRolling.Expressions.Visitors.Result;

namespace DiceRolling.Expressions.Results;

public abstract record RollResult
{
    public abstract T Accept<T>(ResultVisitor<T> visitor);    
}
