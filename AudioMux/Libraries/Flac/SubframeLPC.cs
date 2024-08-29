namespace AudioMux.Libraries.Flac;

public unsafe struct SubframeLPC
{
    public readonly EntropyCodingMethod EntropyCodingMethod;
    public readonly uint Order;
    public readonly uint QlpCoeffPrecision;
    public readonly int QuantizationLevel;
    public fixed int QlpCoeff[4];
    public fixed int Warmup[4];
    public readonly int* Residual; 
}
