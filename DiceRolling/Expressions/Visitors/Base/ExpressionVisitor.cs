using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Errors;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;

namespace DiceRolling.Expressions.Visitors.Base;

public abstract class ExpressionVisitor<T>
{
    public T Visit(Expression expression) => expression.Accept(this);

    // base expressions
    protected internal virtual T VisitExpression(Expression expression) => throw new NotImplementedException();
    protected internal virtual T VisitNumeric(NumericExpression numeric) => VisitExpression(numeric);
    protected internal virtual T VisitMultipleNumeric(MultipleNumericExpression mult) => VisitNumeric(mult);
    protected internal virtual T VisitGenesys(GenesysExpression genesys) => VisitExpression(genesys);
    protected internal virtual T VisitError(Expression error) => VisitExpression(error);

    // dice and literals
    protected internal virtual T VisitLiteral(LiteralExpression literal) => VisitNumeric(literal);
    protected internal virtual T VisitDice(DiceExpression dice) => VisitNumeric(dice);
    protected internal virtual T VisitMultiple(MultipleExpression mult) => VisitMultipleNumeric(mult);
    protected internal virtual T VisitArithmetic(ArithmeticExpression arith) => VisitNumeric(arith);
    
    // dice modifiers
    protected internal virtual T VisitExploding(ExplodingExpression explode) => VisitMultipleNumeric(explode);
    protected internal virtual T VisitStackingExploding(StackingExplodingExpression explode) => VisitNumeric(explode);
    protected internal virtual T VisitIndefiniteExploding(IndefiniteExplodingExpression explode) => VisitMultipleNumeric(explode);
    protected internal virtual T VisitTarget(TargetExpression target) => VisitNumeric(target);
    protected internal virtual T VisitLimit(LimitExpression limit) => VisitNumeric(limit);
    protected internal virtual T VisitKeep(KeepExpression keep) => VisitMultipleNumeric(keep);

    // sugar dice
    protected internal virtual T VisitStarWars(StarwarsExpression sw) => VisitNumeric(sw);
    protected internal virtual T VisitShadowrun(ShadowrunExpression sr) => VisitNumeric(sr);
    protected internal virtual T VisitOldShadowrun(OldShadowrunExpression osr) => VisitNumeric(osr);
    protected internal virtual T VisitEarthdawn(EarthdawnExpression ed) => VisitNumeric(ed);
    protected internal virtual T VisitVersus(VersusExpression vs) => VisitNumeric(vs);

    // genesys
    protected internal virtual T VisitGenesysLiteral(GenesysLiteralExpression lit) => VisitGenesys(lit);
    protected internal virtual T VisitGenesysDice(GenesysDiceExpression dice) => VisitGenesys(dice);
    protected internal virtual T VisitGenesysSet(GenesysSetExpression set) => VisitGenesys(set);

    // metaroll
    protected internal virtual T VisitRoll(RollExpression roll) => VisitExpression(roll);
    protected internal virtual T VisitRepeatRoll(RepeatRollExpression repeat) => VisitExpression(repeat);

    // errors
    protected internal virtual T VisitTypeError(NumericTypeError error) => VisitError(error);
    protected internal virtual T VisitDivZero(PossibleDivZeroError error) => VisitError(error);
    protected internal virtual T VisitRepeatRollOverflow(RepeatRollOverflowError error) => VisitError(error);
    protected internal virtual T VisitMultipleOverflow(MultipleOverflowError error) => VisitError(error);
    protected internal virtual T VisitMustBePositive(MustBePositiveError error) => VisitError(error);
    protected internal virtual T VisitInfiniteNumeric(InfiniteNumericError error) => VisitError(error);
    protected internal virtual T VisitSyntaxError(SyntaxError error) => VisitError(error);
}
