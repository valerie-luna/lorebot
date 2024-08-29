namespace AudioMux.Tracks;

public interface ITrack<T> : ITrackData<T>
    where T : unmanaged
{
    int SampleRateHz { get; }
    // you must call AdvanceMusicData after this
    void SeekMusicData(TimeSpan span);
    void Signal(int signal);
    // Convenience only, not guaranteed to be constant, but sets the upper bound of seek
    TimeSpan Length { get; }
    // refers to start position of GetCurrentMusicData();
    TimeSpan CurrentPosition { get; }
    int Channels { get; }
}
