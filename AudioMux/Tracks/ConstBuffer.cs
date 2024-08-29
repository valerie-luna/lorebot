using System.Runtime.CompilerServices;

namespace AudioMux.Tracks;

public unsafe class ConstBuffer<T> : ITrack<T>
    where T : unmanaged
{
    private readonly ITrack<T> inner;
    private int innerDataIndex;
    private readonly MusicDataInternal<T> data;
    private bool disposedValue;
    private readonly Queue<int> signalQueue;
    private bool reacquireImmediately = true;
    public TimeSpan Chunksize { get; }

    public ConstBuffer(ITrack<T> inner, TimeSpan chunksize)
    {
        if (inner.Channels != 2)
            throw new InvalidOperationException("Expected 2 channels, got " + inner.Channels.ToString());
        signalQueue = new Queue<int>();
        this.inner = inner;
        Chunksize = chunksize;
        var elements = (int)(chunksize.TotalSeconds * inner.SampleRateHz);
        data = new MusicDataInternal<T>(
            elements,
            inner.Channels
        );
    }

    public int SampleRateHz => inner.SampleRateHz;

    public TimeSpan Length => inner.Length;

    public int Channels => inner.Channels;

    public TimeSpan CurrentPosition => inner.CurrentPosition + TimeSpan.FromSeconds(innerDataIndex / (double)SampleRateHz);

    private MusicData<T> Acquire()
    {
        while (signalQueue.TryDequeue(out int signal))
            inner.Signal(signal);
        innerDataIndex = 0;
        bool success = inner.AdvanceMusicData();
        if (success)
        {
            reacquireImmediately = false;
            return inner.GetCurrentMusicData();
        }
        else
        {
            reacquireImmediately = true;
            return default;
        }
    }

    public bool AdvanceMusicData()
    {
        MusicData<T> innerData = reacquireImmediately ? Acquire() : inner.GetCurrentMusicData();
        int outerDataIndex = 0;
        while (outerDataIndex < data.Length)
        {
            if (innerData.Length > 0 && innerDataIndex >= innerData.Length)
            {
                innerData = Acquire();
            }
            if (innerData.Length == 0)
            {
                data.AsSpan()[(outerDataIndex * 2)..].Clear();
                return true;
            }
            data[outerDataIndex * 2] = innerData[innerDataIndex * 2];
            data[outerDataIndex * 2 + 1] = innerData[innerDataIndex * 2 + 1];
            innerDataIndex++;
            outerDataIndex++;
        }
        return true;
    }

    public MusicData<T> GetCurrentMusicData()
    {
        return data.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        inner.SeekMusicData(span);
        innerDataIndex = 0;
        reacquireImmediately = true;
    }

    public void Signal(int signal)
    {
        signalQueue.Enqueue(signal);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                inner.Dispose();
            }
            data.Unallocate();
            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~ConstBuffer()
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
}