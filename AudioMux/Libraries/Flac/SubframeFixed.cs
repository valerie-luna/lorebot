namespace AudioMux.Libraries.Flac;

public unsafe struct SubframeFixed
{
    public readonly EntropyCodingMethod EntropyCodingMethod;
    public readonly uint Order;
    public fixed long Warmup[4];
    public readonly int* Residual;
}
