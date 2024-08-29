using AudioMux.Tracks;

namespace AudioMux.Crossfade;

public class BasicCrossfade : ICrossfade<float>
{
    public static ITrackData<float> Mix(ITrack<float> start, ITrack<float> end, TimeSpan diff)
    {
        return new CrossfadeImpl(start, end, diff);
    }

    private class CrossfadeImpl : ITrackData<float>
    {
        private const int maxSamplesPerBuffer = 1024; 
        private readonly ITrack<float> start;
        private readonly ITrack<float> end;
        private readonly int samples;
        private MusicDataInternal<float> data;
        private int readSoFar;
        private int left_index;
        private int right_index;
        private bool disposedValue;
        private bool opening = true;

        public unsafe CrossfadeImpl(ITrack<float> start, ITrack<float> end, TimeSpan diff)
        {
            if (start.SampleRateHz != end.SampleRateHz)
                throw new InvalidOperationException();
            if (start.Channels != 2 || end.Channels != 2)
                throw new InvalidOperationException();
            this.start = start;
            this.end = end;
            this.samples = start.SampleRateHz * diff.Seconds;
            this.data = new MusicDataInternal<float>(
                maxSamplesPerBuffer,
                start.Channels
            );
        }

        public unsafe bool AdvanceMusicData()
        {
            if (opening)
            {
                start.AdvanceMusicData();
                end.SeekMusicData(start.CurrentPosition);
                bool ad = end.AdvanceMusicData();
                if (!ad) throw new InvalidOperationException();
                opening = false;
            }
            bool exiting = readSoFar >= samples;
            MusicData<float> crossLeft = start.GetCurrentMusicData();
            MusicData<float> crossRight = end.GetCurrentMusicData();
            int index = 0;
            while (index < maxSamplesPerBuffer)
            {
                if (left_index >= crossLeft.Length)
                {
                    start.AdvanceMusicData();
                    crossLeft = start.GetCurrentMusicData();
                    left_index = 0;
                }
                if (right_index >= crossRight.Length)
                {
                    end.AdvanceMusicData();
                    crossRight = end.GetCurrentMusicData();
                    right_index = 0;
                    if (exiting)
                        return false;
                }
                if (readSoFar < samples)
                {
                    float coeff = readSoFar/(float)samples;
                    data[index * 2] = (crossLeft[left_index * 2] * (1 - coeff))
                        + (crossRight[right_index * 2] * coeff);
                    data[index * 2 + 1] = (crossLeft[left_index * 2 + 1] * (1 - coeff))
                        + (crossRight[right_index * 2 + 1] * coeff);
                    left_index++;
                }
                else
                {
                    data[index * 2] = crossRight[right_index * 2];
                    data[index * 2 + 1] = crossRight[right_index * 2 + 1];
                }
                right_index++;
                index++;
                readSoFar++;
            }
            return true;
        }

        public MusicData<float> GetCurrentMusicData()
        {
            return data.ToMusicData();
        }

        protected unsafe virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                data.Unallocate();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~CrossfadeImpl()
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
}
