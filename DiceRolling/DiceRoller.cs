using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Dice;
using DiceRolling.Expressions.Dice.Sugar;
using DiceRolling.Expressions.Errors;
using DiceRolling.Expressions.Genesys;
using DiceRolling.Expressions.Visitors;
using static DiceRolling.DiceRollingParser;
using NotNullAttribute = Antlr4.Runtime.Misc.NotNullAttribute;

namespace DiceRolling;

public class DiceRoller(int MaximumIterations = 1000) : IDiceRoller
{
    private class NoLexerErrorsAllowed : IAntlrErrorListener<int>
    {
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new InvalidOperationException("This shouldn't ever happen");
        }
    }

    public Expression Parse(string roll)
    {
        ICharStream stream = new AntlrInputStream(roll);
        var lexer = new DiceRollingLexer(stream);
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new NoLexerErrorsAllowed());
        var token = new CommonTokenStream(lexer);
        var parser = new DiceRollingParser(token, TextWriter.Null, TextWriter.Null)
        {
            ErrorHandler = new BailErrorStrategy()
        };
        RequestContext context;
        try
        {
            context = parser.request();
        }
        catch (ParseCanceledException pce)
        {
            Debug.Assert(pce.InnerException is not null);
            RecognitionException re = (RecognitionException)pce.InnerException;
            return new SyntaxError(roll, re.OffendingToken.StartIndex, re.OffendingToken.StopIndex + 1);
        }
        var last = token.Get(token.Size - 1);
        if (last.Type != -1)
        {
            // we didn't consume the whole stream
            return new SyntaxError(roll, last.StartIndex, last.StopIndex + 1);
        }
        var visitor = new DiceBuilderVisitor(MaximumIterations);
        var ast = visitor.Visit(context).AssertNotNull();
        var errorVisitor = new TypeCheckVisitor(MaximumIterations);
        return errorVisitor.Visit(ast);
    }

    private class DiceBuilderVisitor(int MaximumIterations) : DiceRollingParserBaseVisitor<Expression?>
    {
        public override Expression VisitRequest([NotNull] RequestContext context)
        {
            var rolls = context.diceRoll();
            var reason = context.reason()?.ANYTHING().Select(n => n.ToString());
            return new RollExpression(
                [.. rolls.Select(VisitDiceRoll)],
                reason is not null ? string.Concat(reason) : null
            );
        }

        public override Expression VisitDiceRoll([NotNull] DiceRollContext context)
        {
            Expression expr = VisitDicerollType(context.dicerollType());
            var multtext = context.multiplier()?.POSITIVE_INTEGER().GetText();
            if (multtext is not null)
            {
                if (int.TryParse(multtext, out int i))
                {
                    expr = new RepeatRollExpression(expr, i);
                }
                else
                {
                    expr = new RepeatRollOverflowError(expr, MaximumIterations);
                }
            }
            return expr;
        }

        public override Expression VisitDicerollType([NotNull] DicerollTypeContext context)
        {
            if (context.arithmetic() is not null)
                return VisitArithmetic(context.arithmetic());
            else if (context.nonarithmetic() is not null)
                return Visit(context.nonarithmetic()).AssertNotNull();
            else 
                throw new InvalidOperationException();
        }

        public override NumericExpression VisitArithmetic([NotNull] ArithmeticContext context)
        {
            if (context.value() is not null)
            {
                return VisitValue(context.value());
            }
            else if (context.OPENBRACKET() is not null)
            {
                return VisitArithmetic(context.arithmetic(0));
            }
            else
            {
                ITerminalNode node = context.PLUS() ?? context.MINUS() ?? context.MULTIPLY() ?? context.DIVIDE() ?? context.MODULO(); 
                Debug.Assert(node is not null);
                ArithmeticType type = node.Symbol.Text switch
                {
                    "+" => ArithmeticType.Add,
                    "-" => ArithmeticType.Subtract,
                    "*" => ArithmeticType.Multiply,
                    "/" => ArithmeticType.Divide,
                    "%" => ArithmeticType.Modulo,
                    _ => throw new NotImplementedException()
                };
                NumericExpression left = VisitArithmetic(context.arithmetic()[0]);
                NumericExpression right = VisitArithmetic(context.arithmetic()[1]);
                return new ArithmeticExpression(left, right, type);
            }
        }

        public override NumericExpression VisitValue([NotNull] ValueContext context)
        {
            if (context.dice() is not null)
            {
                return (NumericExpression)Visit(context.dice()).AssertNotNull();
            }
            else
            {
                decimal value = decimal.Parse((context.DECIMAL() ?? context.POSITIVE_INTEGER()).GetText());
                if (context.sign()?.MINUS() is not null)
                    value = -value;
                return new LiteralExpression(value);
            }
        }

        public override NumericExpression VisitDiceGroup([NotNull] DiceGroupContext context)
        {
            NumericExpression diceNumber = VisitDiceNumber(context.diceNumber())
                ?? new LiteralExpression(1);
            NumericExpression v = VisitSingleDiceRoll(context.singleDiceRoll());
            MultipleNumericExpression multValue = new MultipleExpression(diceNumber, v);
            if (context.groupKeep() is not null)
            {
                multValue = new KeepExpression(
                    multValue, 
                    VisitDiceNumber(context.groupKeep().diceNumber()),
                    context.groupKeep().sign().ParseKeepType()
                );
            }

            NumericExpression value;
            if (context.groupTarget() is not null)
            {
                value = new TargetExpression(multValue, 
                    VisitDiceNumber(context.groupTarget().diceNumber()),
                    context.groupTarget().sign().ParseTargetType()
                );
            }
            else
                value = multValue;
            
            if (context.groupLimit() is not null)
            {
                return new LimitExpression(
                    value,
                    VisitDiceNumber(context.groupLimit().diceNumber())
                );
            }
            else
            {
                return value;
            }
        }

        public override NumericExpression VisitSingleDiceRoll([NotNull] SingleDiceRollContext context)
        {
            NumericExpression diceNumber = VisitDiceNumber(context.diceNumber());
            NumericExpression dice = new DiceExpression(diceNumber);
            foreach (var modifier in context.diceModifiers())
            {
                dice = modifier switch
                {
                    DMExplosionContext explosion => new ExplodingExpression(
                        dice, VisitDiceNumber(explosion.diceNumber()), explosion.sign().ParseTargetType()
                    ),
                    DMIndefiniteExplosionContext iexplosion => new IndefiniteExplodingExpression(
                        dice, VisitDiceNumber(iexplosion.diceNumber()), iexplosion.sign().ParseTargetType()
                    ),
                    DMStackingExplosionContext sexplosion => new StackingExplodingExpression(
                        dice, VisitDiceNumber(sexplosion.diceNumber()), sexplosion.sign().ParseTargetType()
                    ),
                    _ => throw new NotImplementedException(modifier.GetType().Name)
                };
            }
            return dice;
        }

        [return: NotNullIfNotNull(nameof(context))]
        public override NumericExpression? VisitDiceNumber(DiceNumberContext? context)
        {
            if (context is null)
            {
                return null;
            }
            if (context.POSITIVE_INTEGER() is not null)
            {
                return new LiteralExpression(
                    decimal.Parse(context.POSITIVE_INTEGER().GetText())
                );
            }
            else
            {
                return VisitArithmetic(context.squaredValue().arithmetic());
            }
        }

        public override NumericExpression VisitSSShadowrun([NotNull] SSShadowrunContext context)
        {
            var expr = new ShadowrunExpression(
                VisitDiceNumber(context.diceNumber()),
                VisitDiceNumber(context.shadowrunLimit()?.diceNumber()),
                context.shadowrunExplode() is not null
            );
            if (context.value() is not null)
            {
                return new VersusExpression(expr, VisitValue(context.value()), VersusType.Opposed);
            }
            else
            {
                return expr;
            }
        }

        public override NumericExpression VisitSSOldShadowrun([NotNull] SSOldShadowrunContext context)
        {
            return new OldShadowrunExpression(
                VisitDiceNumber(context.diceNumber()),
                context.oldShadowrunTarget() is not null
                    ? VisitDiceNumber(context.oldShadowrunTarget().diceNumber())
                    : null
            );
        }

        public override NumericExpression VisitSSEarthdawn([NotNull] SSEarthdawnContext context)
        {
            var ed = new EarthdawnExpression(
                new LiteralExpression(
                    decimal.Parse(context.POSITIVE_INTEGER().GetText())
                )
            );
            if (context.value() is not null)
            {
                return new VersusExpression(ed, VisitValue(context.value()), VersusType.Earthdawn);
            }
            else
            {
                return ed;
            }
        }

        public override NumericExpression VisitSSStarwars([NotNull] SSStarwarsContext context)
        {
            return new StarwarsExpression(
                new LiteralExpression(
                    decimal.Parse(context.POSITIVE_INTEGER().GetText())
                )
            );
        }

        public override GenesysExpression VisitSSGenesys([NotNull] SSGenesysContext context)
        {
            return new GenesysSetExpression(context.genesysDice().Select(Visit)
                .Concat(context.genesysLiteral().Select(Visit))
                .Cast<GenesysExpression>().ToImmutableArray());
        }

        public override GenesysExpression VisitGenesysDice([NotNull] GenesysDiceContext context)
        {
            GenesysDice dice;
            if (context.YELLOW() is not null)
                dice = GenesysDice.Proficiency;
            else if (context.GREEN() is not null)
                dice = GenesysDice.Ability;
            else if (context.BLUE() is not null)
                dice = GenesysDice.Boost;
            else if (context.BLACK() is not null)
                dice = GenesysDice.Setback;
            else if (context.PURPLE() is not null)
                dice = GenesysDice.Difficulty;
            else if (context.RED() is not null)
                dice = GenesysDice.Challenge;
            else
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }
            return new GenesysDiceExpression(dice);
        }

        public override GenesysExpression VisitGenesysLiteral([NotNull] GenesysLiteralContext context)
        {
            GenesysToken token;
            if (context.SUCCESS() is not null)
                token = GenesysToken.Success;
            else if (context.FAILURE() is not null)
                token = GenesysToken.Failure;
            else if (context.ADVANTAGE() is not null)
                token = GenesysToken.Advantage;
            else if (context.THREAT() is not null)
                token = GenesysToken.Threat;
            else if (context.TRIUMPH() is not null)
                token = GenesysToken.Triumph;
            else if (context.DESPAIR() is not null)
                token = GenesysToken.Despair;
            else
            {
                Debug.Assert(false);
                throw new InvalidOperationException();
            }
            return new GenesysLiteralExpression(token);
        }
    }
}