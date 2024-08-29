namespace Initiative;

public record NumericalInitiative(decimal Initiative) : InitiativeSet(Initiative, 0, 0)
{
    protected override InitiativeSet Add(InitiativeSet other)
    {
        return new NumericalInitiative(
            Initiative + ((NumericalInitiative)other).Initiative
        );
    }
}

