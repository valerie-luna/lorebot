using System.Runtime.InteropServices;

namespace AudioMux.Libraries.Flac;

public readonly struct Subframe
{
    public readonly SubframeType Type;

    [StructLayout(LayoutKind.Explicit)]
    public readonly struct SubframeUnion
    {
        [FieldOffset(0)]
        public readonly SubframeConstant Constant;
        [FieldOffset(0)]
        public readonly SubframeFixed Fixed;
        [FieldOffset(0)]
        public readonly SubframeLPC LPC;
        [FieldOffset(0)]
        public readonly SubframeVerbatim Verbtaim;

    }
    public readonly SubframeUnion Union;
    public readonly uint WastedBits;
}
