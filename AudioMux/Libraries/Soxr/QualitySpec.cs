namespace AudioMux.Libraries.Soxr;

public readonly struct QualitySpec
{
    public readonly double Precision;
    public readonly double PhaseResponse;
    public readonly double PassbandEnd;
    public readonly double StopbandBegin;
    private readonly IntPtr internal_value;
    public readonly ulong Flags;
}
