namespace AudioMux.Libraries.Flac;

public readonly struct Int24
{
    public Int24(int i)
    {
        a = (byte)(i & 0xFF);
        b = (byte)((i >> 8) & 0xFF);
        c = (byte)((i >> 16) & 0xFF);
    }

    private readonly byte a;
    private readonly byte b;
    private readonly byte c;

    public static explicit operator Int24(long l) => new Int24((int)l);
    public static explicit operator Int24(int l) => new Int24(l);
}
