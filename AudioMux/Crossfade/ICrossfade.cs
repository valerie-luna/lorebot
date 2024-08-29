using AudioMux.Tracks;

namespace AudioMux.Crossfade;

public interface ICrossfade<T>
    where T : unmanaged
{
    static abstract ITrackData<T> Mix(ITrack<T> start, ITrack<T> end, TimeSpan diff);
}
