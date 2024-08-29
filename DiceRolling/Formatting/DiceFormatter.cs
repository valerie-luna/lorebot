using DiceRolling.Expressions;
using System.Diagnostics;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Results;

namespace DiceRolling.Formatting;

public abstract class DiceFormatter<T>
{
    public T Format(Expression expr)
    {
        Debug.Assert(expr is not null);
        var error = new ErrorFinder();
        if (error.HasAny(expr))
            return FormatError(expr);
        var rollExpressions = new RollFinder().Visit(expr).ToArray();
        var reason = new ReasonExtractor().Visit(expr);
        var desugar = new DesugarVisitor();
        var rolls = rollExpressions.Select(r =>
        {
            var seed = Random.Shared.Next();
            var rand = new Random(seed);
            var executor = new RollExecutor(rand);
            var desugared = desugar.Visit(r);
            if (desugared == r) desugared = null;
            var result = executor.Visit(desugared ?? r);
            return new Roll(r, desugared, result, seed);
        }).ToArray();
        Debug.Assert(rolls.Length != 0);
        if (rolls.Length == 1)
            return FormatSingle(expr, rolls[0], reason);
        else
            return FormatMultiple(expr, rolls, reason);
    }

    protected record Roll(Expression Expression, Expression? Desugared, RollResult Result, int Seed);

    protected abstract T FormatError(Expression expr);
    protected abstract T FormatSingle(Expression fullExpr, Roll roll, string? reason);
    protected abstract T FormatMultiple(Expression fullExpr, IEnumerable<Roll> rolls, string? reason);
}
