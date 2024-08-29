namespace AudioMux.Tracks;

public class PlayOnSignal<T> : ITrack<T>
    where T : unmanaged
{
    private readonly ITrack<T> track;
    private readonly int signal;
    private bool playing;

    public PlayOnSignal(ITrack<T> track, int signal)
    {
        this.track = track;
        this.signal = signal;
    }

    public int SampleRateHz => track.SampleRateHz;

    public TimeSpan Length => track.Length;

    public TimeSpan CurrentPosition => track.CurrentPosition;

    public int Channels => track.Channels;

    public bool AdvanceMusicData()
    {
        if (playing)
        {
            bool md = track.AdvanceMusicData();
            if (!md)
                playing = false;
            return md;
        }
        return false;
    }

    public void Dispose()
    {
        track.Dispose();
    }

    public MusicData<T> GetCurrentMusicData()
    {
        return track.GetCurrentMusicData();
    }

    public void SeekMusicData(TimeSpan span)
    {
        track.SeekMusicData(span);
    }

    public void Signal(int signal)
    {
        if (signal == this.signal)
        {
            SeekMusicData(TimeSpan.Zero);
            playing = true;
        }
        else
        {
            track.Signal(signal);
        }
    }
}
