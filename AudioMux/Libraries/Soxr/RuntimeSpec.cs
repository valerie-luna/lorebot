namespace AudioMux.Libraries.Soxr;

public readonly struct RuntimeSpec 
{
    public readonly uint Log2MinDftSize;
    public readonly uint Log2LargeDftSize;
    public readonly uint CoefSizeKBytes;
    public readonly uint OmpNumThreads;
    private readonly IntPtr internal_value;
    public readonly ulong Flags;
}