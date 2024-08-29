using DiceRolling.Expressions.Results;

namespace DiceRolling.Expressions.Visitors.Result;

public abstract class ResultVisitor<T>
{
    public T Visit(RollResult result) => result.Accept(this);
    protected internal virtual T VisitResult(RollResult result) => throw new InvalidOperationException();
    protected internal virtual T VisitNumeric(NumericRollResult result) => VisitResult(result);
    protected internal virtual T VisitGenesys(GenesysRollResult genesys) => VisitResult(genesys);

    protected internal virtual T VisitSingle(SingleRollResult single) => VisitNumeric(single);
    protected internal virtual T VisitSet(ResultSet set) => VisitNumeric(set);
    protected internal virtual T VisitArithmetic(ArithmeticRollResult arith) => VisitNumeric(arith);
    protected internal virtual T VisitReplacementSet(SetReplacementRollResult set) => VisitSet(set);
    protected internal virtual T VisitReplacementTotal(TotalReplacementRollResult total) => VisitNumeric(total);
    protected internal virtual T VisitVersus(VersusResult vs) => VisitNumeric(vs);

    protected internal virtual T VisitGenesysLiteral(GenesysLiteralRollResult lit) => VisitGenesys(lit);
    protected internal virtual T VisitGenesysDice(GenesysDiceRollResult dice) => VisitGenesys(dice);
    protected internal virtual T VisitGenesysSet(GenesysSetRollResult set) => VisitGenesys(set);
}