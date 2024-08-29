using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

public class RollFinder : ExpressionVisitor<IEnumerable<Expression>>
{
    protected internal override IEnumerable<Expression> VisitRoll(RollExpression roll)
    {
        foreach (var expr in roll.Expressions)
        {
            if (expr is NumericExpression or GenesysExpression)
                yield return expr;
            else foreach (var subexp in Visit(expr))
                yield return subexp;
        }
    }

    protected internal override IEnumerable<Expression> VisitRepeatRoll(RepeatRollExpression repeat)
    {
        for (int i = 0; i < repeat.Count; i++)
        {
            if (repeat.Expression is NumericExpression or GenesysExpression)
                yield return repeat.Expression;
            else foreach (var subexp in Visit(repeat.Expression))
                yield return subexp;
        }
    }

    protected internal override IEnumerable<Expression> VisitRepeatRollOverflow(RepeatRollOverflowError error)
    {
        yield break;
    }
}
