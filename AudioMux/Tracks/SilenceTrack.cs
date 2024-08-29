using System.Runtime.CompilerServices;

namespace AudioMux.Tracks;

public class SilenceTrack : ITrack<float>
{
    private readonly MusicDataInternal<float> md;
    private bool disposedValue;

    public unsafe SilenceTrack()
    {
        md = new MusicDataInternal<float>(
            4096,
            2
        );
        md.AsSpan().Clear();
    }

    public int SampleRateHz => 44100;

    public TimeSpan Length => TimeSpan.FromMinutes(10);

    public TimeSpan CurrentPosition => TimeSpan.Zero;

    public int Channels => 2;

    public bool AdvanceMusicData()
    {
        return true;
    }

    public MusicData<float> GetCurrentMusicData()
    {
        return md.ToMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
    }

    public void Signal(int signal)
    {
    }

    protected unsafe virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            md.Unallocate();
            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~SilenceTrack()
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

