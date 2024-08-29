using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors.Base;

namespace DiceRolling.Expressions.Visitors;

[Obsolete("Use ErrorFinder", true)]
public class HasErrorVisitor : ExpressionVisitor<bool>
{
    protected internal override bool VisitExpression(Expression expression) => throw new NotImplementedException();
    protected internal override bool VisitLiteral(LiteralExpression literal)
    {
        return false;
    }
    protected internal override bool VisitDice(DiceExpression dice)
    {
        return Visit(dice.Sides);
    }
    protected internal override bool VisitMultiple(MultipleExpression mult)
    {
        return Visit(mult.Count) || Visit(mult.Roll);
    }
    protected internal override bool VisitArithmetic(ArithmeticExpression arith)
    {
        return Visit(arith.Left) || Visit(arith.Right);
    }
    protected internal override bool VisitExploding(ExplodingExpression explode)
    {
        return Visit(explode.Roll) || Visit(explode.Target);
    }
    protected internal override bool VisitIndefiniteExploding(IndefiniteExplodingExpression explode)
    {
        return Visit(explode.Roll) || Visit(explode.Target);
    }
    protected internal override bool VisitStackingExploding(StackingExplodingExpression explode)
    {
        return Visit(explode.Roll) || Visit(explode.Target);
    }
    protected internal override bool VisitTarget(TargetExpression target)
    {
        return Visit(target.Set) || Visit(target.Target);
    }
    protected internal override bool VisitLimit(LimitExpression limit)
    {
        return Visit(limit.Limit) || Visit(limit.Value);
    }
    protected internal override bool VisitKeep(KeepExpression keep)
    {
        return Visit(keep.Set) || Visit(keep.Keep);
    }
    protected internal override bool VisitGenesysLiteral(GenesysLiteralExpression lit)
    {
        return false;
    }
    protected internal override bool VisitGenesysDice(GenesysDiceExpression dice)
    {
        return false;
    }
    protected internal override bool VisitGenesysSet(GenesysSetExpression set)
    {
        return set.Expressions.Any(Visit);
    }
    protected internal override bool VisitRoll(RollExpression roll)
    {
        return roll.Expressions.Any(Visit);
    }
    protected internal override bool VisitRepeatRoll(RepeatRollExpression repeat)
    {
        return Visit(repeat.Expression);
    }

    protected internal override bool VisitStarWars(StarwarsExpression sw)
    {
        return Visit(sw.DiceCount);
    }
    protected internal override bool VisitShadowrun(ShadowrunExpression sr)
    {
        return Visit(sr.DiceCount) || (sr.Limit is not null && Visit(sr.Limit));
    }
    protected internal override bool VisitOldShadowrun(OldShadowrunExpression osr)
    {
        return Visit(osr.DiceCount) || (osr.Target is not null && Visit(osr.Target));
    }
    protected internal override bool VisitEarthdawn(EarthdawnExpression ed)
    {
        return Visit(ed.Step);
    }

    protected internal override bool VisitTypeError(NumericTypeError error) => true;
    protected internal override bool VisitDivZero(PossibleDivZeroError error) => true;
    protected internal override bool VisitMultipleOverflow(MultipleOverflowError error) => true;
    protected internal override bool VisitRepeatRollOverflow(RepeatRollOverflowError error) => true;
    protected internal override bool VisitMustBePositive(MustBePositiveError error) => true;
    protected internal override bool VisitInfiniteNumeric(InfiniteNumericError error) => true;
}
