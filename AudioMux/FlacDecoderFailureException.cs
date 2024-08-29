namespace AudioMux;

public class FlacDecoderFailureException : Exception
{
    public static FlacDecoderFailureException Create<T>(T t)
        => new FlacDecoderFailureException(t?.ToString());

    private FlacDecoderFailureException(string message) : base(message) {}
}

