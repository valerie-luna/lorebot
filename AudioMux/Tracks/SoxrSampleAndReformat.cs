using System.Diagnostics;
using System.Runtime.CompilerServices;
using AudioMux.Libraries.Soxr;

namespace AudioMux.Tracks;

public unsafe class SoxrSampleAndReformat : ITrack<short>
{
    private readonly ITrack<float> input;
    private MusicDataInternal<short> data;
    private readonly SoxrPtr ptr;
    private bool beenDisposed;
    private readonly double ratio;
    public SoxrSampleAndReformat(ITrack<float> input, int newSampleRate)
    {
        this.input = input;
        SampleRateHz = newSampleRate;
        ratio = (double)newSampleRate/input.SampleRateHz;
        var spec = new IoSpec()
        {
            InputType = Datatype.Float32Interleaved,
            OutputType = Datatype.Int16Interleved,
            Scale = 1
        };
        ptr = SoxrLibrary.SoxrCreate(
            input.SampleRateHz,
            newSampleRate,
            (uint)input.Channels,
            out string? s,
            ref spec,
            ref Unsafe.NullRef<QualitySpec>(),
            ref Unsafe.NullRef<RuntimeSpec>()
        );
        data = new MusicDataInternal<short>(input.Channels);
        if (s != null)
            throw new InvalidOperationException(s);
    }

    public int SampleRateHz { get; }

    public TimeSpan Length => input.Length;

    public int Channels => input.Channels;

    public TimeSpan CurrentPosition => input.CurrentPosition;

    public bool AdvanceMusicData()
    {
        bool next = input.AdvanceMusicData();
        if (!next)
        {
            return false;
        }
        MusicData<float> nextdata = input.GetCurrentMusicData();
        var ndspan = nextdata.AsSpan();
        int newMaxSamples = (int)(nextdata.Length * ratio) + 1;
        if (newMaxSamples > data.Length)
            data.Reallocate(newMaxSamples);
        ulong read, written;
        string response;
        fixed (void* ndptr = ndspan)
        {
            response = SoxrLibrary.SoxrProcess(
                ptr,
                ndptr,
                (ulong)nextdata.Length,
                out read,
                Unsafe.AsPointer(ref data.Ref),
                (ulong)data.Length,
                out written
            );
        }
        if (response is not null)
        {
            throw new InvalidOperationException(response);
        }
        Debug.Assert((int)read == nextdata.Length);
        data.Length = (int)written;
        return true;
    }

    public MusicData<short> GetCurrentMusicData()
    {
        return data.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        input.SeekMusicData(span);
    }

    public void Signal(int signal)
    {
        input.Signal(signal);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!beenDisposed)
        {
            if (disposing)
            {
                input.Dispose();
            }
            data.Unallocate();
            SoxrLibrary.SoxrDelete(ptr);
            beenDisposed = true;
        }
    }

    ~SoxrSampleAndReformat()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

