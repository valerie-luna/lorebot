using System.Collections.Immutable;
using System.Diagnostics;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Errors;

public record SyntaxError(string Roll, int Start, int End) : Expression, IExpressionError
{
    public override T Accept<T>(ExpressionVisitor<T> visitor)
    {
        return visitor.VisitSyntaxError(this);
    }
}