using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions;

public abstract record Expression
{
    public abstract T Accept<T>(ExpressionVisitor<T> visitor); 
}
