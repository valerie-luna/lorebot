using System.Diagnostics;
using System.Reflection;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

public class ErrorFinder : ExpressionVisitor<IEnumerable<Expression>>
{
    protected internal override IEnumerable<Expression> VisitExpression(Expression expression)
    {
        if (expression is IExpressionError)
            yield return expression;
        Type type = expression.GetType();
        foreach (var property in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => f.FieldType.IsAssignableTo(typeof(Expression)) 
                || f.FieldType.IsAssignableTo(typeof(IEnumerable<Expression>)))
            .Select(f => f.GetValue(expression))
            .Where(f => f is not null))
        {
            if (property is Expression e)
            {
                foreach (var v in Visit(e))
                    yield return v;
            }
            else if (property is IEnumerable<Expression> en)
            {
                foreach (var x in en)
                    foreach (var v in Visit(x))
                        yield return v;
            }
            else Debug.Assert(false);
        }
    }

    public bool HasAny(Expression expression) => Visit(expression).Any();
}