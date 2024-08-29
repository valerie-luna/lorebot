// See https://aka.ms/new-console-template for more information

using DiceRolling;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;

const string Roll = "sr12 vs sr12";
const int MaxIterations = 1000;

var seed = 1972962;
Console.WriteLine($"Seed: {seed}");
var rand = new Random(seed);

var error = new ErrorFinder();
var rollvisitor = new DiscordRollVisitor();

Console.WriteLine("Original roll: " + Roll);

Expression result = new DiceRoller(MaxIterations).Parse(Roll);
Console.WriteLine();
Console.WriteLine($"Parsed Roll: {rollvisitor.Visit(result)}");
if (!error.HasAny(result))
{
    foreach (var subroll in new RollFinder().Visit(result))
    {
        FormattableString roll = rollvisitor.Visit(subroll);
        var desugared = new DesugarVisitor().Visit(subroll);
        if (desugared is not null)
            roll = $"{roll} ({rollvisitor.Visit(desugared)})";
        desugared ??= subroll;
        var executed = new RollExecutor(rand).Visit(desugared);
        string total = new TotalVisitor().Visit(executed);
        string dice = new DiceSetPrinterVisitor(sort: true).Visit(executed);
        Console.WriteLine($"Roll: {roll} Dice: {dice} Total: {total}");
    }
}
else
{
    rollvisitor = new DiscordRollVisitor(UnderlineErrors: true);
    var errorroll = rollvisitor.Visit(result);

    Console.WriteLine($"Error roll: {errorroll}"); 

    var desc = new ErrorDescriber();
    foreach (var e in new ErrorFinder().Visit(result))
        Console.WriteLine(desc.Visit(e));
}