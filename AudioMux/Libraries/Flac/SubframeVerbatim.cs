using System.Runtime.InteropServices;

namespace AudioMux.Libraries.Flac;

[StructLayout(LayoutKind.Explicit)]
public readonly unsafe struct SubframeVerbatim
{
    [FieldOffset(0)]
    public readonly uint* Int32;
    [FieldOffset(0)]
    public readonly ulong* Int64;
    [FieldOffset(8)]
    public readonly VerbatimSubframeDataType DataType;
}
