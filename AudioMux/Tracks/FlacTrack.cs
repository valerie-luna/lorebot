using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AudioMux.Libraries.Flac;

namespace AudioMux.Tracks;

public unsafe class FlacTrack : ITrack<float>
{
    private readonly StreamDecoderPtr ptr;
    private readonly UnmanagedFlacTrack* data;
    private bool beenDisposed;

    public FlacTrack(string filename)
    {
        PreInit(out data, out ptr);
        var status = FlacLibrary.StreamDecoderInitFile(
            ptr,
            filename,
            &WriteCallback,
            &MetadataCallback,
            &ErrorCallback,
            data
        );
        PostInit(status);
    }

    private static void PreInit(out UnmanagedFlacTrack* data, out StreamDecoderPtr ptr)
    {
        data = Utilities.Allocate<UnmanagedFlacTrack>(1);
        ptr = FlacLibrary.StreamDecoderNew();
    }

    private void PostInit(StreamDecoderInitStatus status)
    {
        if (status != StreamDecoderInitStatus.Ok)
        {
            throw FlacDecoderFailureException.Create(status);
        }
        bool result = FlacLibrary.StreamDecoderProcessUntilEndOfMetadata(ptr);
        if (!result)
        {
            StateFailure();
        }
        if (data->Channels != 2)
        {
            throw FlacDecoderFailureException.Create("Invalid channels - only 2-channel supported right now");
        }
    }

    public FlacTrack(byte* buffer, int length)
    {
        PreInit(out data, out ptr);
        data->Buffer = buffer;
        data->BufferLength = length;
        data->BufferReadIndex = 0;
        var status = FlacLibrary.StreamDecoderInit(
            ptr,
            &ReadCallback,
            &SeekCallback,
            &TellCallback,
            &LengthCallback,
            &EOFCallback,
            &WriteCallback,
            &MetadataCallback,
            &ErrorCallback,
            data
        );
        PostInit(status);
    }

    public TimeSpan Length => TimeSpan.FromSeconds(data->TotalNumberOfSamples / (double)data->SampleRate);

    public int SampleRateHz => (int)data->SampleRate;

    public int Channels => (int)data->Channels;

    public TimeSpan CurrentPosition => TimeSpan.FromSeconds(data->CurrentPosition / (double)data->SampleRate);

    [DoesNotReturn]
    private void StateFailure()
    {
        throw FlacDecoderFailureException.Create(FlacLibrary.StreamDecoderGetState(ptr));
    }

    public bool AdvanceMusicData()
    {
        var state = FlacLibrary.StreamDecoderGetState(ptr);
        if (state is StreamDecoderState.ReadFrame or StreamDecoderState.SearchForFrameSync)
        {
            if (data->Seeked)
            {
                data->Seeked = false;
                return true;
            }
            bool result = FlacLibrary.StreamDecoderProcessSingle(ptr);
            if (!result)
                StateFailure();
            state = FlacLibrary.StreamDecoderGetState(ptr);
            return state != StreamDecoderState.EndOfStream;
        }
        else if (state == StreamDecoderState.EndOfStream)
        {
            data->CurrentBlockSize = 0;
            return false;
        }
        else
        {
            StateFailure();
            Debug.Assert(false);
            return false; // should not happen
        }
    }

    public unsafe MusicData<float> GetCurrentMusicData()
    {
        return new MusicData<float>(
            (int)data->CurrentBlockSize,
            ref *data->Ptr,
            (int)data->Channels
        );
    }

    public void SeekMusicData(TimeSpan span)
    {
        ulong sample = (ulong)(data->SampleRate * span.TotalSeconds);
        data->CurrentPosition = sample;
        data->NextPositionAdd = 0;
        data->Seeked = true;
        bool result = FlacLibrary.SeekAbsolute(ptr, sample);
        if (!result)
            StateFailure();
    }


    public void Signal(int signal)
    {
    }

    private struct UnmanagedFlacTrack
    {
        public ulong TotalNumberOfSamples;
        public ulong CurrentPosition;
        public ulong NextPositionAdd;
        public uint SampleRate;
        public uint Channels;
        public uint BitsPerSample;
        public uint MaxBlockSize;
        public uint CurrentBlockSize;
        public float* Ptr;
        public bool Seeked;
        public byte* Buffer;
        public int BufferLength;
        public int BufferReadIndex;
    }

    [UnmanagedCallersOnly]
    private static unsafe StreamDecoderWriteStatus WriteCallback(
        StreamDecoderPtr ptr,
        Frame* frame,
        int** buffer,
        void* clientData
    )
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)clientData;
        data->CurrentPosition += data->NextPositionAdd;
        uint blocklength = frame->Header.Blocksize;
        data->NextPositionAdd = blocklength;
        float divisor = data->BitsPerSample switch
        {
            8 => 128,
            16 => 32_768,
            24 => 8_388_608,
            32 => 2_147_483_648,
            _ => throw new InvalidOperationException() // i don't know what this'll do in unmanaged code!
        };
        data->CurrentBlockSize = blocklength;
        uint ptrindex = 0;
        for (uint i = 0; i < blocklength; i++)
        {
            data->Ptr[ptrindex++] = buffer[0][i] / divisor;
            data->Ptr[ptrindex++] = buffer[1][i] / divisor;
        }
        return StreamDecoderWriteStatus.Continue;
    }

    [UnmanagedCallersOnly]
    private static unsafe void MetadataCallback(
        StreamDecoderPtr ptr,
        StreamMetadataStreamInfo* info,
        void* clientData
    )
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)clientData;
        data->TotalNumberOfSamples = info->TotalSamples;
        data->SampleRate = info->SampleRate;
        data->Channels = info->Channels;
        data->BitsPerSample = info->BitsPerSample;
        data->MaxBlockSize = info->MaxBlocksize;
        data->CurrentBlockSize = 0;
        data->CurrentPosition = 0;
        data->NextPositionAdd = 0;
        data->Seeked = false;
        data->Ptr = Utilities.Allocate<float>((int)data->MaxBlockSize * sizeof(float) * 2);
    }

    [UnmanagedCallersOnly]
    private static unsafe void ErrorCallback(StreamDecoderPtr ptr, StreamDecoderErrorStatus error)
    {
        throw new NotImplementedException();
    }

    [UnmanagedCallersOnly]
    private static unsafe StreamDecoderReadStatus ReadCallback(StreamDecoderPtr ptr, byte* buffer, nint* bytes, void* _data)
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)_data;
        int startIndex = data->BufferReadIndex;
        int readLength = Math.Min((int)*bytes, data->BufferLength - startIndex);
        if (readLength <= 0) return StreamDecoderReadStatus.EndOfStream;
        *bytes = readLength;
        Span<byte> inbound = new Span<byte>(data->Buffer + startIndex, readLength);
        Span<byte> outbound = new Span<byte>(buffer, readLength);
        inbound.CopyTo(outbound);
        data->BufferReadIndex = startIndex + readLength;
        return StreamDecoderReadStatus.Continue; 
    }

    [UnmanagedCallersOnly]
    private static unsafe StreamDecoderSeekStatus SeekCallback(StreamDecoderPtr ptr, ulong offset, void* _data)
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)_data;
        if (offset < 0 || offset > (ulong)data->BufferLength)
            return StreamDecoderSeekStatus.Error;
        data->BufferReadIndex = (int)offset;
        return StreamDecoderSeekStatus.Ok;
    }

    [UnmanagedCallersOnly]
    private static unsafe StreamDecoderTellStatus TellCallback(StreamDecoderPtr ptr, ulong* offset, void* _data)
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)_data;
        *offset = (ulong)data->BufferReadIndex;
        return StreamDecoderTellStatus.Ok;
    }

    [UnmanagedCallersOnly]
    private static unsafe StreamDecoderLengthStatus LengthCallback(StreamDecoderPtr ptr, ulong* length, void* _data)
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)_data;
        *length = (ulong)data->BufferLength;
        return StreamDecoderLengthStatus.Ok;
    }

    [UnmanagedCallersOnly]
    private static unsafe bool EOFCallback(StreamDecoderPtr ptr, void* _data)
    {
        UnmanagedFlacTrack* data = (UnmanagedFlacTrack*)_data;
        return data->BufferReadIndex >= data->BufferLength;
    }

#region disposal
    protected virtual void Dispose(bool disposing)
    {
        if (!beenDisposed)
        {
            StreamDecoderState state = FlacLibrary.StreamDecoderGetState(ptr);
            if (state != StreamDecoderState.Uninitialized)
            {
                FlacLibrary.StreamDecoderFinish(ptr);
            }
            FlacLibrary.StreamDecoderDelete(ptr);
            if (data->Ptr != null)
            {
                Utilities.Free(data->Ptr);
            }
            Utilities.Free(data);
            beenDisposed = true;
        }
    }

    ~FlacTrack()
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
    #endregion
}

