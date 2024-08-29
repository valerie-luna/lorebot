using System.Diagnostics;
using System.Numerics;

namespace AudioMux.Tracks;

public unsafe class Volume : ITrack<float>
{
    private readonly ITrack<float> track;
    private readonly float factor;
    private MusicDataInternal<float> musicData;
    private int bufferSize = 0;
    private bool disposedValue;

    public Volume(ITrack<float> track, float factor)
    {
        this.track = track;
        this.factor = factor;
        musicData = new MusicDataInternal<float>(track.Channels);
    }

    public int SampleRateHz => track.SampleRateHz;

    public TimeSpan Length => track.Length;

    public TimeSpan CurrentPosition => track.CurrentPosition;

    public int Channels => track.Channels;

    public bool AdvanceMusicData()
    {
        bool success = track.AdvanceMusicData();
        if (!success)
            return false;
        var data = track.GetCurrentMusicData();
        if (data.BufferLength > bufferSize)
        {
            musicData.Reallocate(data.BufferLength);
        }
        musicData.Length = data.Length;
        var outSpan = musicData.AsSpan();
        var inSpan = data.AsSpan();
        Debug.Assert(outSpan.Length == inSpan.Length);
        int remaining = outSpan.Length % Vector<float>.Count;
        for (int i = 0; i < (outSpan.Length - remaining); i += Vector<float>.Count)
        {
            var v = new Vector<float>(inSpan[i..]);
            (v * factor).CopyTo(outSpan[i..]);
        }

        for (int i = outSpan.Length - remaining; i < outSpan.Length; i++)
        {
            outSpan[i] = inSpan[i] * factor;
        }

        return true;
    }

    public MusicData<float> GetCurrentMusicData()
    {
        return musicData.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        track.SeekMusicData(span);
    }

    public void Signal(int signal)
    {
        track.Signal(signal);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                track.Dispose();
            }

            musicData.Unallocate();

            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~Volume()
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
