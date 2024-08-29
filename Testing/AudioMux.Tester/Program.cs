using System.Diagnostics.Tracing;
using AudioMux;
using AudioMux.Crossfade;
using AudioMux.Tracks;

internal class Program
{
    private unsafe static void Main(string[] args)
    {
        ITrack<float> mp3_1 = new Mp3Track("./battle01.mp3");
        ITrack<float> flac1 = new FlacTrack("./tbs.flac");
        var mix = new TrackSwitcher<float, BasicCrossfade>(mp3_1, flac1, 1, TimeSpan.FromSeconds(2));
        //var floattrack = new Mix(TimeSpan.FromMilliseconds(20), MixStrategy.Nothing, flac1, flac2);
        ITrack<short> transformed = new SoxrSampleAndReformat(mix, 48000);

        AudioPlayer<short> player = new AudioPlayer<short>(transformed);
        Console.WriteLine("oh no");
        player.StartStream();

        while (true)
        {
            Console.ReadKey();
            player.PushSignal(1);
        }
    }

    /*private void Save<T>(ITrack<T> track)
        where T : unmanaged
    {
        Console.WriteLine($"Sample rate: {track.SampleRateHz}");
        Console.WriteLine($"Channels: {track.Channels}");
        using FileStream fs = new FileStream("out.raw", FileMode.Create);
        int written = 0;
        while (track.AdvanceMusicData())
        {
            MusicData<T> data = track.GetCurrentMusicData();
            var span = data.AsSpan();
            fs.Write(span);
            written += span.Length;
            if (written > (44100*8))
                break;
        }
    }*/
}