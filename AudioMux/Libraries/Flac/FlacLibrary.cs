using System.Runtime.InteropServices;
using System.Numerics;

namespace AudioMux.Libraries.Flac;

internal unsafe static partial class FlacLibrary
{
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_new")]
    public static partial StreamDecoderPtr StreamDecoderNew();

    [LibraryImport("FLAC", StringMarshalling = StringMarshalling.Utf8, EntryPoint = "FLAC__stream_decoder_init_file")]
    public static partial StreamDecoderInitStatus StreamDecoderInitFile(
        StreamDecoderPtr decoder,
        string filename,
        delegate* unmanaged<
            StreamDecoderPtr,
            Frame*,
            int**,
            void*,
            StreamDecoderWriteStatus
        > WriteCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            StreamMetadataStreamInfo*,
            void*,
            void
        > MetadataCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            StreamDecoderErrorStatus,
            void
        > ErrorCallback,
        void* ClientData
    );
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_init_stream")]
    public static partial StreamDecoderInitStatus StreamDecoderInit(
        StreamDecoderPtr decoder,
        delegate* unmanaged<
            StreamDecoderPtr,
            byte*,
            nint*,
            void*,
            StreamDecoderReadStatus
        > ReadCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            ulong,
            void*,
            StreamDecoderSeekStatus
        > SeekCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            ulong*,
            void*,
            StreamDecoderTellStatus
        > TellCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            ulong*,
            void*,
            StreamDecoderLengthStatus
        > LengthCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            void*,
            bool
        > EOFCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            Frame*,
            int**,
            void*,
            StreamDecoderWriteStatus
        > WriteCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            StreamMetadataStreamInfo*,
            void*,
            void
        > MetadataCallback,
        delegate* unmanaged<
            StreamDecoderPtr,
            StreamDecoderErrorStatus,
            void
        > ErrorCallback,
        void* ClientData
    );
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_process_single")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StreamDecoderProcessSingle(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_process_until_end_of_metadata")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StreamDecoderProcessUntilEndOfMetadata(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_process_until_end_of_stream")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StreamDecoderProcessUntilEndOfStream(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_finish")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StreamDecoderFinish(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_delete")]
    public static partial void StreamDecoderDelete(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_get_state")]
    public static partial StreamDecoderState StreamDecoderGetState(StreamDecoderPtr ptr);
    [LibraryImport("FLAC", EntryPoint = "FLAC__stream_decoder_seek_absolute")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SeekAbsolute(StreamDecoderPtr ptr, ulong sample);
}
