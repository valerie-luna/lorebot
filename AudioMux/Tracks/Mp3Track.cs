using System.Runtime.CompilerServices;
using AudioMux.Libraries.DrMp3;

namespace AudioMux.Tracks;

public unsafe class Mp3Track : ITrack<float>
{
    private DrMp3 data;
    private readonly SeekPoint* seekPoints;
    private readonly ulong frameCount;
    private ulong currentFrame;
    private bool disposedValue;
    private MusicDataInternal<float> musicdata;

    public int SampleRateHz => (int)data.SampleRate;

    public TimeSpan Length => TimeSpan.FromSeconds((double)frameCount / SampleRateHz);

    public TimeSpan CurrentPosition => TimeSpan.FromSeconds((double)currentFrame / SampleRateHz);

    public int Channels => (int)data.Channels;

    private const int BufferSize = 4096; 

    public Mp3Track(string filename)
    {
        bool success = DrMp3Library.InitFile(ref data, filename, ref Unsafe.NullRef<AllocationCallbacks>());
        if (!success) throw new InvalidOperationException();
        Init(out seekPoints, out frameCount);
    }

    public Mp3Track(byte* buffer, int length)
    {
        bool success = DrMp3Library.InitMemory(ref data, ref buffer[0], length, ref Unsafe.NullRef<AllocationCallbacks>());
        if (!success) throw new InvalidOperationException();
        Init(out seekPoints, out frameCount);
    }

    private void Init(out SeekPoint* seekPoints, out ulong frameCount)
    {
        seekPoints = Utilities.Allocate<SeekPoint>(100);
        uint seekPointCount = 100;
        bool success = DrMp3Library.CalculateSeekPoints(ref data, ref seekPointCount, ref *seekPoints);
        if (!success) throw new InvalidOperationException();
        success = DrMp3Library.BindSeekTable(ref data, seekPointCount, seekPoints);
        if (!success) throw new InvalidOperationException();
        frameCount = DrMp3Library.GetPcmFrameCount(ref data);
        musicdata = new MusicDataInternal<float>(
            BufferSize, Channels
        );
    }


    public void SeekMusicData(TimeSpan span)
    {
        ulong sample = (ulong)(data.SampleRate * span.TotalSeconds);
        currentFrame = sample;
        bool success = DrMp3Library.SeekToPcmFrame(ref data, sample);
        if (!success) throw new InvalidOperationException();
    }

    public void Signal(int signal)
    {
    }

    public bool AdvanceMusicData()
    {
        ulong framesRead = DrMp3Library.ReadPcmFrames(ref data, BufferSize, ref musicdata.Ref);
        musicdata.Length = (int)framesRead;
        return framesRead > 0;
    }

    public MusicData<float> GetCurrentMusicData()
    {
        return musicdata.ToMusicData();
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            DrMp3Library.Uninit(ref data);
            Utilities.Free(seekPoints);
            disposedValue = true;
            musicdata.Unallocate();
        }
    }

    ~Mp3Track()
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

