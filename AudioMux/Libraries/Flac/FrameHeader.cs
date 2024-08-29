using System.Runtime.InteropServices;

namespace AudioMux.Libraries.Flac;

public readonly struct FrameHeader
{
    public readonly uint Blocksize;
    public readonly uint SampleRate;
    public readonly uint Channels;
    public readonly ChannelAssignment ChannelAssignment;
    public readonly uint BitsPerSample;
    public readonly FrameNumberType NumberType;
    public readonly Union Number;
    public readonly byte Crc;

    [StructLayout(LayoutKind.Explicit)]
    public readonly struct Union
    {
        [FieldOffset(0)] public readonly uint FrameNumber;
        [FieldOffset(0)] public readonly ulong SampleNumber;
    }
}
