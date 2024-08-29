using AudioMux.Tracks;

namespace AudioMux.Crossfade;

public class NoMix<T> : ICrossfade<T>
    where T : unmanaged
{
    public static ITrackData<T> Mix(ITrack<T> start, ITrack<T> end, TimeSpan diff)
    {
        if (diff != TimeSpan.Zero)
            throw new InvalidOperationException();
        return EmptyTrack.Instance;
    }

    private class EmptyTrack : ITrackData<T>
    {
        public static ITrackData<T> Instance { get; } = new EmptyTrack();


        public bool AdvanceMusicData()
        {
            return false;
        }

        public void Dispose()
        {
        }

        public MusicData<T> GetCurrentMusicData()
        {
            return default;
        }
    }
}
