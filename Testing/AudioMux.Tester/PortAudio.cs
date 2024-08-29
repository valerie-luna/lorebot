using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using AudioMux.Tracks;

namespace AudioMux;

public unsafe class AudioPlayer<T> : IDisposable
    where T : unmanaged
{
    private bool disposedValue;
    internal readonly ITrack<T> track;
    private readonly PaStream stream;
    private readonly GCHandle handle;
    internal uint dataIndex = 0;
    internal readonly ConcurrentQueue<int> queue;
    internal bool starting = true;

    public unsafe AudioPlayer(ITrack<T> track)
    {
        queue = new ConcurrentQueue<int>();
        PaSampleFormat format;
        delegate* unmanaged<
            void*, 
            void*, 
            ulong, 
            PaStreamCallbackTimeInfo*, 
            PaStreamCallbackFlags, 
            void*, 
            PaStreamCallbackResult> callback;
        delegate* unmanaged<void*, void> finish;
        if (typeof(T) == typeof(float))
        {
            callback = &AudioPlayerData.CallbackFloat;
            finish = &AudioPlayerData.FinishFloat;
            format = PaSampleFormat.Float32;
        }
        else if (typeof(T) == typeof(int))
        {
            callback = &AudioPlayerData.CallbackInteger;
            finish = &AudioPlayerData.FinishInteger;
            format = PaSampleFormat.Int32;
        }
        else if (typeof(T) == typeof(short))
        {
            callback = &AudioPlayerData.CallbackShort;
            finish = &AudioPlayerData.FinishShort;
            format = PaSampleFormat.Int16;
        }
        else if (typeof(T) == typeof(sbyte))
        {
            callback = &AudioPlayerData.CallbackSByte;
            finish = &AudioPlayerData.FinishSByte;
            format = PaSampleFormat.Int8;
        }
        else if (typeof(T) == typeof(byte))
        {
            callback = &AudioPlayerData.CallbackByte;
            finish = &AudioPlayerData.FinishByte;
            format = PaSampleFormat.UInt8;
        }
        else
            throw new InvalidOperationException("Wrong type of T - only supports int/short/sbyte/byte/float");
        this.track = track;
        var error = PortAudio.Pa_Initialize();
        if (error != default) throw new InvalidOperationException(error.ToString());
        handle = GCHandle.Alloc(this);
        error = PortAudio.Pa_OpenDefaultStream(
            out stream,
            0,
            track.Channels,
            format,
            track.SampleRateHz,
            PortAudio.FramesPerBufferUnspecified,
            callback,
            (void*)GCHandle.ToIntPtr(handle)
        );
        if (error != default) throw new InvalidOperationException(error.ToString());
    }

    public void PushSignal(int signal)
    {
        queue.Enqueue(signal);
    }

    public void StartStream()
    {
        var error = PortAudio.Pa_StartStream(stream);
        if (error != default) if (error != default) throw new InvalidOperationException(error.ToString());
    }

    public void StopStream()
    {
        var error = PortAudio.Pa_StopStream(stream);
        if (error != default) if (error != default) throw new InvalidOperationException(error.ToString());
    }

    public void CloseStream()
    {
        var error = PortAudio.Pa_CloseStream(stream);
        if (error != default) if (error != default) throw new InvalidOperationException(error.ToString());
    }

#region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                track.Dispose();
            }
            PortAudio.Pa_Terminate();
            disposedValue = true;
        }
    }

    ~AudioPlayer()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
#endregion
}

internal struct AudioPlayerData
{
    [UnmanagedCallersOnly]
    public static unsafe PaStreamCallbackResult CallbackFloat(void* input, void* output, ulong frameCount, 
        PaStreamCallbackTimeInfo* timeInfo, PaStreamCallbackFlags statusFlags, void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<float> player = (AudioPlayer<float>)handle.Target!;
        float* output_actual = (float*)output;
        return CommonCode(player, frameCount, output_actual);
    }

    [UnmanagedCallersOnly]
    public static unsafe PaStreamCallbackResult CallbackInteger(void* input, void* output, ulong frameCount, 
        PaStreamCallbackTimeInfo* timeInfo, PaStreamCallbackFlags statusFlags, void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<int> player = (AudioPlayer<int>)handle.Target!;
        int* output_actual = (int*)output;
        return CommonCode(player, frameCount, output_actual);
    }

    [UnmanagedCallersOnly]
    public static unsafe PaStreamCallbackResult CallbackShort(void* input, void* output, ulong frameCount, 
        PaStreamCallbackTimeInfo* timeInfo, PaStreamCallbackFlags statusFlags, void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<short> player = (AudioPlayer<short>)handle.Target!;
        short* output_actual = (short*)output;
        return CommonCode(player, frameCount, output_actual);
    }

    [UnmanagedCallersOnly]
    public static unsafe PaStreamCallbackResult CallbackByte(void* input, void* output, ulong frameCount, 
        PaStreamCallbackTimeInfo* timeInfo, PaStreamCallbackFlags statusFlags, void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<byte> player = (AudioPlayer<byte>)handle.Target!;
        byte* output_actual = (byte*)output;
        return CommonCode(player, frameCount, output_actual);
    }

    [UnmanagedCallersOnly]
    public static unsafe PaStreamCallbackResult CallbackSByte(void* input, void* output, ulong frameCount, 
        PaStreamCallbackTimeInfo* timeInfo, PaStreamCallbackFlags statusFlags, void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<sbyte> player = (AudioPlayer<sbyte>)handle.Target!;
        sbyte* output_actual = (sbyte*)output;
        return CommonCode(player, frameCount, output_actual);
    }


    private static unsafe PaStreamCallbackResult CommonCode<T>(AudioPlayer<T> player,
        ulong frameCount, T* output_actual)
        where T : unmanaged
    {
        while (player.queue.TryDequeue(out int signal))
        {
            player.track.Signal(signal);
        }
        if (player.starting)
        {
            player.starting = false;
            player.track.AdvanceMusicData();
        }
        MusicData<T> data = player.track.GetCurrentMusicData();
        uint out_index = 0;
        var act_original = output_actual;
        while (out_index < frameCount)
        {
            if (player.dataIndex >= data.Length)
            {
                player.dataIndex = 0;
                bool cont = player.track.AdvanceMusicData();
                if (!cont)
                {
                    return PaStreamCallbackResult.Abort;
                }
                data = player.track.GetCurrentMusicData();
            }
            *output_actual = data[(int)player.dataIndex * 2];
            output_actual++;
            *output_actual = data[(int)player.dataIndex * 2 + 1];
            output_actual++;
            player.dataIndex++;
            out_index++;
        }
        return PaStreamCallbackResult.Continue;
    }

    [UnmanagedCallersOnly]
    public static unsafe void FinishFloat(void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<float> player = (AudioPlayer<float>)handle.Target!;
        CommonFinishCode(player);
    }

    [UnmanagedCallersOnly]
    public static unsafe void FinishInteger(void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<int> player = (AudioPlayer<int>)handle.Target!;
        CommonFinishCode(player);
    }

    [UnmanagedCallersOnly]
    public static unsafe void FinishShort(void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<short> player = (AudioPlayer<short>)handle.Target!;
        CommonFinishCode(player);
    }

    [UnmanagedCallersOnly]
    public static unsafe void FinishByte(void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<byte> player = (AudioPlayer<byte>)handle.Target!;
        CommonFinishCode(player);
    }

    [UnmanagedCallersOnly]
    public static unsafe void FinishSByte(void* userData)
    {
        GCHandle handle = GCHandle.FromIntPtr((nint)userData);
        AudioPlayer<sbyte> player = (AudioPlayer<sbyte>)handle.Target!;
        CommonFinishCode(player);
    }

    private static unsafe void CommonFinishCode<T>(AudioPlayer<T> player)
        where T : unmanaged
    {

    }

}

internal unsafe static partial class PortAudio
{
    [LibraryImport("portaudio")]
    public static partial int Pa_GetVersion(); 

    [LibraryImport("portaudio",
        StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))
    ]
    public static partial string Pa_GetVersionText();

    [LibraryImport("portaudio")]
    public static partial PaVersionInfo* Pa_GetVersionInfo();

    [LibraryImport("portaudio",
        StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))
    ]
    public static partial string Pa_GetErrorText(PaErrorCode error);

    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_Initialize();

    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_Terminate();
    [LibraryImport("portaudio")]
    public static partial int Pa_GetHostApiCount();
    [LibraryImport("portaudio")]
    public static partial int Pa_GetDefaultHostApi();
    [LibraryImport("portaudio")]
    public static partial PaHostApiInfo* Pa_GetHostApiInfo(int hostApi);
    [LibraryImport("portaudio")]
    public static partial int Pa_HostApiTypeIdToHostApiIndex(PaHostApiTypeId type);
    [LibraryImport("portaudio")]
    public static partial int Pa_HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex);

    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_OpenDefaultStream(
        out PaStream stream,
        int numInputChannels,
        int numOutputChannels,
        PaSampleFormat sampleFormat,
        double sampleRate,
        ulong framesPerBuffer,
        delegate* unmanaged<
            void*, 
            void*, 
            ulong, 
            PaStreamCallbackTimeInfo*, 
            PaStreamCallbackFlags, 
            void*, 
            PaStreamCallbackResult> streamCallback,
        void* userData
    );

    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_CloseStream(PaStream stream);
    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_StartStream(PaStream stream);
    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_StopStream(PaStream stream);
    [LibraryImport("portaudio")]
    public static partial PaErrorCode Pa_SetStreamFinishedCallback(PaStream stream, delegate* unmanaged<void*, void> streamFinishedCallback);
    public const ulong FramesPerBufferUnspecified = 0;
}

public readonly struct PaStream
{
    private readonly IntPtr ptr;
}

public readonly unsafe struct PaVersionInfo
{
    public readonly int VersionMajor;
    public readonly int VersionMinor;
    public readonly int VersionSubMinor;
    private readonly byte* _VersionControlRevision;
    private readonly byte* _VersionText;

    public string? VersionControlRevision => Utf8StringMarshaller.ConvertToManaged(_VersionControlRevision);
    public string? VersionText => Utf8StringMarshaller.ConvertToManaged(_VersionText);
}

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8StringMarshallerNoFree))]
internal static unsafe class Utf8StringMarshallerNoFree
{
    public static byte* ConvertToUnmanaged(string? managed)
    {
        return Utf8StringMarshaller.ConvertToUnmanaged(managed);
    }

    public static string? ConvertToManaged(byte* unmanaged)
    {
        return Utf8StringMarshaller.ConvertToManaged(unmanaged);
    }

    public static void Free(byte* unmanaged)
    {

    }
}

public enum PaErrorCode : int
{
    NoError = 0,

    NotInitialized = -10000,
    UnanticipatedHostError,
    InvalidChannelCount,
    InvalidSampleRate,
    InvalidDevice,
    InvalidFlag,
    SampleFormatNotSupported,
    BadIODeviceCombination,
    InsufficientMemory,
    BufferTooBig,
    BufferTooSmall,
    NullCallback,
    BadStreamPtr,
    TimedOut,
    InternalError,
    DeviceUnavailable,
    IncompatibleHostApiSpecificStreamInfo,
    StreamIsStopped,
    StreamIsNotStopped,
    InputOverflowed,
    OutputUnderflowed,
    HostApiNotFound,
    InvalidHostApi,
    CanNotReadFromACallbackStream,
    CanNotWriteToACallbackStream,
    CanNotReadFromAnOutputOnlyStream,
    CanNotWriteToAnInputOnlyStream,
    IncompatibleStreamHostApi,
    BadBufferPtr
}

public enum PaHostApiTypeId : int
{
    InDevelopment=0, /* use while developing support for a new host API */
    DirectSound=1,
    MME=2,
    ASIO=3,
    SoundManager=4,
    CoreAudio=5,
    OSS=7,
    ALSA=8,
    AL=9,
    BeOS=10,
    WDMKS=11,
    JACK=12,
    WASAPI=13,
    AudioScienceHPI=14
}

public readonly unsafe struct PaHostApiInfo
{
    public readonly int StructVersion;
    public readonly PaHostApiTypeId Type;
    private readonly byte* _Name;
    public string? Name => Utf8StringMarshaller.ConvertToManaged(_Name);
    public readonly int DeviceCount;
    public readonly int DefaultInputDevice;
    public readonly int DefaultOutputDevice;
}

public readonly struct PaStreamCallbackTimeInfo
{
    public readonly double InputBufferAdcTime;
    public readonly double CurrentTime;
    public readonly double OutputBufferDacTime;
}

[Flags]
enum PaStreamCallbackFlags : ulong
{
    InputUnderflow = 0x00000001,
    InputOverflow = 0x00000002,
    OutputUnderflow = 0x00000004,
    OutputOverflow = 0x00000008,
    PrimingOutput = 0x00000010
}

public enum PaStreamCallbackResult : int
{
    Continue = 0,
    Complete = 1,
    Abort = 2
}

public enum PaSampleFormat : ulong
{
    Float32 = 0x00000001,
    Int32 = 0x00000002,
    Int24 = 0x00000004,
    Int16 = 0x00000008,
    Int8 = 0x00000010,
    UInt8 = 0x00000020,
    CustomFormat = 0x00010000,
    NonInterleaved = 0x80000000
}