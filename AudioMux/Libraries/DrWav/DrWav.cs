using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AudioMux.Libraries.DrWav;

internal static unsafe partial class DrWavLibrary
{
    [LibraryImport("dr_wav", EntryPoint = "drwav_init_file", StringMarshalling = StringMarshalling.Utf8)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InitFile(ref DrWav wav, string filename, ref AllocationCallbacks callbacks);
    [LibraryImport("dr_wav", EntryPoint = "drwav_init_memory")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InitMemory(ref DrWav mp3, ref byte data, nint dataSize, ref AllocationCallbacks callbacks);

    [LibraryImport("dr_wav", EntryPoint = "drwav_uninit")]
    public static partial DrwavResult Uninit(ref DrWav wav);
    [LibraryImport("dr_wav", EntryPoint = "drwav_get_length_in_pcm_frames")]
    public static partial DrwavResult GetLengthPcmFrames(ref DrWav wav, out ulong length);
    [LibraryImport("dr_wav", EntryPoint = "drwav_seek_to_pcm_frame")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SeekToPcmFrame(ref DrWav wav, ulong frameIndex);
    [LibraryImport("dr_wav", EntryPoint = "drwav_read_pcm_frames_f32")]
    public static partial ulong ReadPcmFrames(ref DrWav wav, ulong framesToRead, ref float buffer);
}

internal enum DrwavResult : int
{
    Success = 0,
    Error = -1,
    InvalidArgs = -2,
    InvalidOperation = -3,
    OutOfMemory = -4,
    OutOfRange = -5,
    AccessDenied = -6,
    DoesNotExist = -7,
    AlreadyExists = -8,
    TooManyOpenFiles = -9,
    InvalidFile = -10,
    TooBig = -11,
    PathTooLong = -12,
    NameTooLong = -13,
    NotDirectory = -14,
    IsDirectory = -15,
    DirectoryNotEmpty = -16,
    END_OF_FILE = -17,
    NO_SPACE = -18,
    BUSY = -19,
    IO_ERROR = -20,
    INTERRUPT = -21,
    UNAVAILABLE = -22,
    ALREADY_IN_USE = -23,
    BAD_ADDRESS = -24,
    BAD_SEEK = -25,
    BAD_PIPE = -26,
    DEADLOCK = -27,
    TOO_MANY_LINKS = -28,
    NOT_IMPLEMENTED = -29,
    NO_MESSAGE = -30,
    BAD_MESSAGE = -31,
    NO_DATA_AVAILABLE = -32,
    INVALID_DATA = -33,
    TIMEOUT = -34,
    NO_NETWORK = -35,
    NOT_UNIQUE = -36,
    NOT_SOCKET = -37,
    NO_ADDRESS = -38,
    BAD_PROTOCOL = -39,
    PROTOCOL_UNAVAILABLE = -40,
    PROTOCOL_NOT_SUPPORTED = -41,
    PROTOCOL_FAMILY_NOT_SUPPORTED = -42,
    ADDRESS_FAMILY_NOT_SUPPORTED = -43,
    SOCKET_NOT_SUPPORTED = -44,
    CONNECTION_RESET = -45,
    ALREADY_CONNECTED = -46,
    NOT_CONNECTED = -47,
    CONNECTION_REFUSED = -48,
    NO_HOST = -49,
    IN_PROGRESS = -50,
    CANCELLED = -51,
    MEMORY_ALREADY_MAPPED = -52,
    AT_END = -53,
}

internal unsafe struct AllocationCallbacks
{
    public void* UserData;
    public delegate* unmanaged<nint, void*, void*> Allocate;
    public delegate* unmanaged<void*, nint, void*, void*> Reallocate;
    public delegate* unmanaged<void*, void*, void> Free;
}

internal unsafe struct DrWav
{
    private delegate* unmanaged<void*, void*, nint, nint> OnRead;
    private delegate* unmanaged<void*, void*, nint, nint> OnWrite;
    private delegate* unmanaged<void*, void*, DrWavSeekOrigin, int> OnSeek;
    private void* UserData;
    private AllocationCallbacks AllocationCallbacks;
    private Container Container;
    private Format Format;
    public readonly uint SampleRate;
    public readonly ushort Channels;
    public readonly ushort BitsPerSample;
    private ushort TranslatedFormatTag;
    private ulong TotalPCMFrameCount;
    private ulong DataChunkDataSize;
    private ulong DataChunkDataPos;
    private ulong BytesRemaining;
    private ulong ReadCursorInPcmFrames;
    private ulong DataChunkDataSizeTargetWrite;
    private int IsSequentialWrite;
    private void* Metadata;
    private ulong MetadataCount;
    private DrwavMemoryStream MemoryStream;
    private DrwavMemoryStreamWrite MemoryStreamWrite;
    private ADPCMData MSADPCM;
    private IMAData IMA;
    private AIFFData AIFF;
}

internal unsafe struct Format
{
    private ushort FormatTag;
    private ushort Channels;
    private uint SampleRate;
    private uint AverageBytesPerSecond;
    private ushort BlockAlign;
    private ushort BitsPerSample;
    private ushort ExtendedSize;
    private ushort ValidBitsPerSample;
    private uint ChannelMask;
    private fixed byte SubFormat[16];
}

internal unsafe readonly struct DrwavMemoryStream
{
    private readonly byte* Data;
    private readonly nint DataSize;
    private readonly nint CurrentReadPos;
}

internal unsafe readonly struct DrwavMemoryStreamWrite
{
    private readonly void** Data;
    private readonly nint* DataSize;
    private readonly nint DataSizeCount;
    private readonly nint DataCapacity;
    private readonly nint CurrentWritePos;
}

internal unsafe struct ADPCMData
{
    private uint BytesRemainingInBlock;
    private fixed ushort Predictor[2];
    private fixed int Delta[2];
    private fixed int CachedFrames[4];
    private uint CachedFramesCount;
    private fixed int PreviousFrames[2 * 2];
}

internal readonly struct AIFFData
{
    private readonly bool IsLittleEndian;
}

internal unsafe struct IMAData
{
    private readonly uint BytesRemainingInBlock;
    private fixed int Predictor[2];
    private fixed int StepIndex[2];
    private fixed int CachedFrames[16];
    private readonly uint CachedFrameCount;
}

internal enum Container : int
{
    Riff,
    Rifx,
    W64,
    RF64,
    Aiff
}

internal enum DrWavSeekOrigin : int
{
    Start,
    Current
}