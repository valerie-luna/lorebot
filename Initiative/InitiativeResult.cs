using System.Diagnostics.CodeAnalysis;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Results;
using DiceRolling.Expressions.Visitors;

namespace Initiative;

public record InitiativeResult(ResultEnum Result, InitiativeList Initiative);
public record InitiativeRollResult(ResultEnum Result, InitiativeList Initiative, 
    [property: NotNullIfNotNull(nameof(InitiativeRollResult.Roll))] string? Name, RollResult? Roll,
    [property: NotNullIfNotNull(nameof(InitiativeRollResult.Roll))] Expression? Expression);
