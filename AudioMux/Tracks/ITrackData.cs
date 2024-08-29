namespace AudioMux.Tracks;

public interface ITrackData<T> : IDisposable
    where T : unmanaged
{
    // true if data succesful, false if track ended
    bool AdvanceMusicData();
    MusicData<T> GetCurrentMusicData();
}
