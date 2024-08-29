using System.Collections.Immutable;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;

namespace DiceRolling.Expressions.Visitors.Base;

public abstract class ExpressionReplacer : ExpressionVisitor<Expression>
{
    // base expressions
    protected internal sealed override Expression VisitExpression(Expression expression) => throw new NotImplementedException();
    protected internal sealed override Expression VisitNumeric(NumericExpression numeric) => throw new NotImplementedException();
    protected internal sealed override Expression VisitMultipleNumeric(MultipleNumericExpression mult) => throw new NotImplementedException();
    protected internal sealed override Expression VisitGenesys(GenesysExpression genesys) => throw new NotImplementedException();

    // dice and literals
    protected internal sealed override Expression VisitLiteral(LiteralExpression literal)
    {
        return ReplaceLiteral(literal);
    }
    protected internal sealed override Expression VisitDice(DiceExpression dice)
    {
        DiceExpression newexpr = dice with
        {
            Sides = (NumericExpression)Visit(dice.Sides)
        };
        return ReplaceDice(newexpr);
    }
    protected internal sealed override Expression VisitMultiple(MultipleExpression mult)
    {
        MultipleExpression newexpr = mult with
        {
            Count = (NumericExpression)Visit(mult.Count),
            Roll = (NumericExpression)Visit(mult.Roll)   
        };
        return ReplaceMultiple(newexpr);
    }
    protected internal sealed override Expression VisitArithmetic(ArithmeticExpression arith)
    {
        ArithmeticExpression newexpr = arith with
        {
            Left = (NumericExpression)Visit(arith.Left),
            Right = (NumericExpression)Visit(arith.Right)
        };
        return ReplaceArithmetic(newexpr);
    }
    
    // dice modifiers
    protected internal sealed override Expression VisitExploding(ExplodingExpression explode)
    {
        ExplodingExpression newexpr = explode with
        {
            Target = (NumericExpression)Visit(explode.Target),
            Roll = (NumericExpression)Visit(explode.Roll)
        };
        return ReplaceExploding(newexpr);
    }
    protected internal sealed override Expression VisitStackingExploding(StackingExplodingExpression explode)
    {
        StackingExplodingExpression newexpr = explode with
        {
            Target = (NumericExpression)Visit(explode.Target),
            Roll = (NumericExpression)Visit(explode.Roll)
        };
        return ReplaceStackingExploding(newexpr);
    }
    protected internal sealed override Expression VisitIndefiniteExploding(IndefiniteExplodingExpression explode)
    {
        IndefiniteExplodingExpression newexpr = explode with
        {
            Target = (NumericExpression)Visit(explode.Target),
            Roll = (NumericExpression)Visit(explode.Roll)
        };
        return ReplaceIndefiniteExploding(newexpr);
    }
    protected internal sealed override Expression VisitTarget(TargetExpression target)
    {
        TargetExpression newexpr = target with
        {
            Target = (NumericExpression)Visit(target.Target),
            Set = (MultipleNumericExpression)Visit(target.Set)
        };
        return ReplaceTarget(newexpr);
    }
    protected internal sealed override Expression VisitLimit(LimitExpression limit)
    {
        LimitExpression newexpr = limit with
        {
            Value = (NumericExpression)Visit(limit.Value),
            Limit = (NumericExpression)Visit(limit.Limit)
        };
        return ReplaceLimit(newexpr);
    }
    protected internal sealed override Expression VisitKeep(KeepExpression keep)
    {
        KeepExpression newexpr = keep with
        {
            Keep = (NumericExpression)Visit(keep.Keep),
            Set = (MultipleNumericExpression)Visit(keep.Set)
        };
        return ReplaceKeep(newexpr);
    }

    // sugar dice
    protected internal sealed override Expression VisitStarWars(StarwarsExpression sw)
    {
        StarwarsExpression newexpr = sw with
        {
            DiceCount = (NumericExpression)Visit(sw.DiceCount)
        };
        return ReplaceStarWars(newexpr);
    }
    protected internal sealed override Expression VisitShadowrun(ShadowrunExpression sr)
    {
        ShadowrunExpression newexpr = sr with
        {
            DiceCount = (NumericExpression)Visit(sr.DiceCount),
            Limit = sr.Limit is null ? null : (NumericExpression)Visit(sr.Limit)
        };
        return ReplaceShadowrun(newexpr);
    }
    protected internal sealed override Expression VisitOldShadowrun(OldShadowrunExpression osr)
    {
        OldShadowrunExpression newexpr = osr with
        {
            DiceCount = (NumericExpression)Visit(osr.DiceCount),
            Target = osr.Target is null ? null : (NumericExpression)Visit(osr.Target)
        };
        return ReplaceOldShadowrun(newexpr);
    }
    protected internal sealed override Expression VisitEarthdawn(EarthdawnExpression ed)
    {
        EarthdawnExpression newexpr = ed with
        {
            Step = (NumericExpression)Visit(ed.Step)
        };
        return ReplaceEarthdawn(newexpr);
    }
    protected internal sealed override Expression VisitVersus(VersusExpression vs)
    {
        VersusExpression newvs = vs with
        {
            Left = (NumericExpression)Visit(vs.Left),
            Right = (NumericExpression)Visit(vs.Right)
        };
        return ReplaceVersus(newvs);
    }

    // genesys
    protected internal sealed override Expression VisitGenesysLiteral(GenesysLiteralExpression lit)
    {
        return ReplaceGenesysLiteral(lit);
    }
    protected internal sealed override Expression VisitGenesysDice(GenesysDiceExpression dice)
    {
        return ReplaceGenesysDice(dice);
    }
    protected internal sealed override Expression VisitGenesysSet(GenesysSetExpression set)
    {
        GenesysSetExpression newexpr = set with
        {
            Expressions = set.Expressions.Select(Visit).Cast<GenesysExpression>().ToImmutableArray()
        };
        return ReplaceGenesysSet(newexpr);
    }

    // metaroll
    protected internal sealed override Expression VisitRoll(RollExpression roll)
    {
        RollExpression newexpr = roll with
        {
            Expressions = roll.Expressions.Select(Visit).ToImmutableArray()
        };
        return ReplaceRoll(newexpr);
    }
    protected internal sealed override Expression VisitRepeatRoll(RepeatRollExpression repeat)
    {
        RepeatRollExpression newexpr = repeat with
        {
            Expression = Visit(repeat.Expression)
        };
        return ReplaceRepeatRoll(newexpr);
    }

    // errors
    protected internal sealed override Expression VisitTypeError(NumericTypeError error) => ReplaceTypeError(error);
    protected internal sealed override Expression VisitDivZero(PossibleDivZeroError error) => ReplaceDivZero(error);
    protected internal sealed override Expression VisitRepeatRollOverflow(RepeatRollOverflowError error) => ReplaceRepeatRollOverflow(error);
    protected internal sealed override Expression VisitMultipleOverflow(MultipleOverflowError error) => ReplaceMultipleOverflow(error);
    protected internal sealed override Expression VisitMustBePositive(MustBePositiveError error) => ReplaceMustBePositive(error);
    protected internal sealed override Expression VisitInfiniteNumeric(InfiniteNumericError error) => ReplaceInfiniteNumeric(error);

    // base expressions
    protected internal virtual Expression ReplaceExpression(Expression expression) => expression;
    protected internal virtual Expression ReplaceNumeric(NumericExpression numeric) => ReplaceExpression(numeric);
    protected internal virtual Expression ReplaceMultipleNumeric(MultipleNumericExpression mult) => ReplaceNumeric(mult);
    protected internal virtual Expression ReplaceGenesys(GenesysExpression genesys) => ReplaceExpression(genesys);

    // dice and literals
    protected internal virtual Expression ReplaceLiteral(LiteralExpression literal) => ReplaceNumeric(literal);
    protected internal virtual Expression ReplaceDice(DiceExpression dice) => ReplaceNumeric(dice);
    protected internal virtual Expression ReplaceMultiple(MultipleExpression mult) => ReplaceMultipleNumeric(mult);
    protected internal virtual Expression ReplaceArithmetic(ArithmeticExpression arith) => ReplaceNumeric(arith);
    
    // dice modifiers
    protected internal virtual Expression ReplaceExploding(ExplodingExpression explode) => ReplaceMultipleNumeric(explode);
    protected internal virtual Expression ReplaceStackingExploding(StackingExplodingExpression explode) => ReplaceNumeric(explode);
    protected internal virtual Expression ReplaceIndefiniteExploding(IndefiniteExplodingExpression explode) => ReplaceMultipleNumeric(explode);
    protected internal virtual Expression ReplaceTarget(TargetExpression target) => ReplaceNumeric(target);
    protected internal virtual Expression ReplaceLimit(LimitExpression limit) => ReplaceNumeric(limit);
    protected internal virtual Expression ReplaceKeep(KeepExpression keep) => ReplaceMultipleNumeric(keep);

    // sugar dice
    protected internal virtual Expression ReplaceStarWars(StarwarsExpression sw) => ReplaceNumeric(sw);
    protected internal virtual Expression ReplaceShadowrun(ShadowrunExpression sr) => ReplaceNumeric(sr);
    protected internal virtual Expression ReplaceOldShadowrun(OldShadowrunExpression osr) => ReplaceNumeric(osr);
    protected internal virtual Expression ReplaceEarthdawn(EarthdawnExpression ed) => ReplaceNumeric(ed);
    protected internal virtual Expression ReplaceVersus(VersusExpression vs) => ReplaceNumeric(vs);

    // genesys
    protected internal virtual Expression ReplaceGenesysLiteral(GenesysLiteralExpression lit) => ReplaceGenesys(lit);
    protected internal virtual Expression ReplaceGenesysDice(GenesysDiceExpression dice) => ReplaceGenesys(dice);
    protected internal virtual Expression ReplaceGenesysSet(GenesysSetExpression set) => ReplaceGenesys(set);

    // metaroll
    protected internal virtual Expression ReplaceRoll(RollExpression roll) => ReplaceExpression(roll);
    protected internal virtual Expression ReplaceRepeatRoll(RepeatRollExpression repeat) => ReplaceExpression(repeat);

    // errors
    protected internal virtual Expression ReplaceTypeError(NumericTypeError error) => ReplaceExpression(error);
    protected internal virtual Expression ReplaceDivZero(PossibleDivZeroError error) => ReplaceExpression(error);
    protected internal virtual Expression ReplaceRepeatRollOverflow(RepeatRollOverflowError error) => ReplaceExpression(error);
    protected internal virtual Expression ReplaceMultipleOverflow(MultipleOverflowError error) => ReplaceExpression(error);
    protected internal virtual Expression ReplaceMustBePositive(MustBePositiveError error) => ReplaceExpression(error);
    protected internal virtual Expression ReplaceInfiniteNumeric(InfiniteNumericError error) => ReplaceExpression(error);
}
