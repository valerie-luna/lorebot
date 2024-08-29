using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AudioMux.Libraries.DrMp3;


internal static unsafe partial class DrMp3Library
{
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_init_file", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InitFile(ref DrMp3 mp3, string filePath, ref AllocationCallbacks callbacks);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_init_memory")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InitMemory(ref DrMp3 mp3, ref byte data, nint dataSize, ref AllocationCallbacks callbacks);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_uninit")]
    public static partial void Uninit(ref DrMp3 mp3);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_read_pcm_frames_f32")]
    public static partial ulong ReadPcmFrames(ref DrMp3 mp3, ulong framesToRead, ref float bufferOut);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_seek_to_pcm_frame")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SeekToPcmFrame(ref DrMp3 mp3, ulong frameIndex);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_get_pcm_frame_count")]
    public static partial ulong GetPcmFrameCount(ref DrMp3 mp3);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_calculate_seek_points")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool CalculateSeekPoints(ref DrMp3 mp3, ref uint SeekPointCount, ref SeekPoint SeekPoints);
    [LibraryImport("dr_mp3", EntryPoint = "drmp3_bind_seek_table")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool BindSeekTable(ref DrMp3 mp3, uint SeekPointCount, SeekPoint* SeekPoints);
    
}

internal unsafe struct AllocationCallbacks
{
    public void* UserData;
    public delegate* unmanaged<nint, void*, void*> Allocate;
    public delegate* unmanaged<void*, nint, void*, void*> Reallocate;
    public delegate* unmanaged<void*, void*, void> Free;
}

internal enum DrMp3SeekOrigin : int
{
    Start,
    Current
}

internal unsafe struct DrMp3
{
    private DrMp3Decoder Decoder;
    public readonly uint Channels;
    public readonly uint SampleRate;
    private delegate* unmanaged<void*, void*, nint, nint> OnRead;
    private delegate* unmanaged<void*, int, DrMp3SeekOrigin, int>  OnSeek;
    private void* UserData;
    private AllocationCallbacks AllocationCallbacks;
    private uint Mp3FrameChannels;
    private uint Mp3FrameSampleRate;
    private uint PcmFramesConsumedInMp3Frame;
    private uint PcmFramesRemainingInMp3Frame;
    private fixed byte PcmFrames[sizeof(float) * MaxSamplesPerFrame];
    private ulong CurrentPcmFrame;
    private ulong StreamCursor;
    private SeekPoint* SeekPoints;
    private uint SeekPointCount;
    private nint DataSize;
    private nint DataCapacity;
    private nint DataConsumed;
    private byte* Data;
    private bool AtEnd;
    private byte* MemoryData;
    private nint MemoryDataSize;
    private nint MemoryCurrentReadPos;

    private const int MaxSamplesPerFrame = MaxPcmFramesPerMp3Frame * 2;
    private const int MaxPcmFramesPerMp3Frame = 1152;
}

internal unsafe struct DrMp3Decoder
{
    private fixed float Overlap[2 * 9 * 32];
    private fixed float QmfState[15 * 2 * 32];
    private int Reserved;
    private int FreeFormatBytes;
    private fixed byte Header[4];
    private fixed byte ReservedBuffer[511];
}

internal unsafe struct SeekPoint
{
    private ulong SeekPosInBytes;
    private ulong PcmFrameIndex;
    private ushort Mp3FramesToDiscard;
    private short PcmFramesToDiscard;
}