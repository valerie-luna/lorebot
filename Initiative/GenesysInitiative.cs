namespace Initiative;

public record GenesysInitiative(decimal Successes, decimal Advantages, decimal Triumphs) : InitiativeSet(Successes, Advantages, Triumphs)
{
    protected override InitiativeSet Add(InitiativeSet _other)
    {
        GenesysInitiative other = (GenesysInitiative)_other;
        return new GenesysInitiative(
            Successes + other.Successes,
            Advantages + other.Advantages,
            Triumphs + other.Triumphs
        );
    }
}

