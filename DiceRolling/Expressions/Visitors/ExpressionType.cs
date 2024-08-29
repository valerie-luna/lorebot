namespace DiceRolling.Expressions.Visitors;

[Flags]
public enum ExpressionType
{
    Integer          = 0b00001,
    Decimal          = 0b00010,
}
