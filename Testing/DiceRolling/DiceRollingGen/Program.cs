using System.Diagnostics;
using DiceRolling;
using DiceRolling.Expressions.Visitors;
using DiceRolling.Expressions.Visitors.Result;

const int tests = 10_000;
const int depth = 14;

DirectoryInfo dir = new DirectoryInfo("tests");
if (!dir.Exists)
    dir.Create();
bool print = false;
IEnumerable<(string, string)> TestStream;

if (args.Any(a => a == "unlimited"))
{
    var proc = new Process();
    proc.StartInfo.FileName = "/home/redacted/LoreBot/Testing/DiceRolling/pythonvenv/bin/grammarinator-generate";
    proc.StartInfo.Arguments = "DiceRollingGenerator.DiceRollingGenerator" + 
        " --sys-path " +
        " /home/redacted/LoreBot/Testing/DiceRolling/DiceRollingGen " +
        " -r diceRoll " +
        " -d " + $" {depth} " +
        " --stdout " +
        " -n " + $" inf";
    proc.StartInfo.UseShellExecute = false;
    proc.StartInfo.RedirectStandardOutput = true;

    IEnumerable<(string, string)> local()
    {
        proc.Start();
        long l = 1;
        while (true)
        {
            var str = proc.StandardOutput.ReadLine();
            yield return (str, $"Testcase {l++} => {str}");
        }
    }
    TestStream = local();

}
else if (args.Any(a => a == "purge"))
{
    dir.EnumerateFiles().Select(f => { f.Delete(); return 0; }).ToArray();

    var proc = Process.Start("/home/redacted/LoreBot/Testing/DiceRolling/pythonvenv/bin/grammarinator-generate", 
        "DiceRollingGenerator.DiceRollingGenerator" + 
        " --sys-path " +
        " /home/redacted/LoreBot/Testing/DiceRolling/DiceRollingGen " +
        " -r diceRoll " +
        " -d " + $" {depth}" +
        " -n " + $" {tests}");

    proc.WaitForExit();

    TestStream = dir.EnumerateFiles("test_*").OrderByDescending(o => int.Parse(o.Name[5..])).Select(s =>
    {
        using var f = s.OpenText();
        return (f.ReadToEnd(), s.Name);
    })!;
}
else if (args.Any(a => a == "override"))
{
    print = true;
    var str = args.Where(a => a != "override").Single();
    TestStream = Enumerable.Repeat((str, str), 1);
}
else if (args.Any())
{
    TestStream = dir.EnumerateFiles(args.First()).OrderByDescending(o => int.Parse(o.Name[5..])).Select(s =>
    {
        using var f = s.OpenText();
        return (f.ReadToEnd(), s.Name);
    })!;
}
else
{
    TestStream = dir.EnumerateFiles("test_*").OrderByDescending(o => int.Parse(o.Name[5..])).Select(s =>
    {
        using var f = s.OpenText();
        return (f.ReadToEnd(), s.Name);
    })!;
}

Random rand = new(5277563);

Stopwatch sw = new Stopwatch();
foreach ((var text, var info) in TestStream)
{
    sw.Restart();
    Console.WriteLine($"{DateTime.Now} BEGIN {info} {text}");
    try
    {
        Check(text, print);
    }
    catch (InvalidOperationException e) when (e.Message.Contains("FAILURE"))
    {
        Console.WriteLine($"{e.Message} ON: {text}");
        continue;
    }
    catch
    {
        Console.WriteLine(info);
        return;
    }
    Console.WriteLine($"{DateTime.Now} END {info} time {sw.Elapsed}");
}


void Check(string s, bool print = false)
{
    try
    {
        var result = DiceRoller.Parse(s)!;
        var error = new HasErrorVisitor();
        if (error.Visit(result)) return;

        foreach (var subroll in new RollFinder().Visit(result))
        {
            FormattableString roll = new DiscordRollVisitor().Visit(subroll);
            var desugared = new DesugarVisitor().Visit(subroll);
            if (desugared is not null)
                roll = $"{roll} ({new DiscordRollVisitor().Visit(desugared)})";
            desugared ??= subroll;
            var executed = new RollExecutor(rand).Visit(desugared);
            string total = new TotalVisitor().Visit(executed);
            string dice = new DiceSetPrinterVisitor(sort: false).Visit(executed);

            var tdv = new RangeDeterminatorVisitor().Visit(desugared);

            if (print)
            {
                Console.WriteLine(roll);
                Console.WriteLine(total);
                Console.WriteLine(dice);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        Console.WriteLine(s);
        throw;
    }
}