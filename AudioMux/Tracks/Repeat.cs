namespace AudioMux.Tracks;

public sealed class Repeat<T> : ITrack<T>
    where T : unmanaged
{
    private readonly ITrack<T> track;
    private readonly TimeSpan seekTo;

    public Repeat(ITrack<T> track) : this(track, TimeSpan.Zero) { }

    public Repeat(ITrack<T> track, TimeSpan seekTo)
    {
        this.track = track;
        this.seekTo = seekTo;
    }

    public int SampleRateHz => track.SampleRateHz;

    public TimeSpan Length => track.Length;

    public TimeSpan CurrentPosition => track.CurrentPosition;

    public int Channels => track.Channels;

    public bool AdvanceMusicData()
    {
        bool result = track.AdvanceMusicData();
        if (!result)
        {
            track.SeekMusicData(seekTo);
            result = track.AdvanceMusicData();
            if (!result)
                return false;
        }
        return true;
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
        track.Signal(signal);
    }
}
