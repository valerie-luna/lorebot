namespace AudioMux.Libraries.Soxr;

public struct IoSpec
{
    public Datatype InputType;
    public Datatype OutputType;
    public double Scale;
    private readonly IntPtr internal_value;
    public ulong Flags;
}
