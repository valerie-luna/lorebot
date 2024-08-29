using System.Runtime.InteropServices;

namespace AudioMux.Libraries.Soxr;

public unsafe static partial class SoxrLibrary
{
    [LibraryImport("soxr", EntryPoint = "soxr_version", StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))]
    public static partial string SoxrVersion();

    [LibraryImport("soxr", EntryPoint = "soxr_create", StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))]
    public static partial SoxrPtr SoxrCreate(
        double InputRate,
        double OutputRate,
        uint NumChannels,
        out string? error,
        ref IoSpec iospec,
        ref QualitySpec qualityspec,
        ref RuntimeSpec runtimespec
    );

    [LibraryImport("soxr", EntryPoint = "soxr_process", StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))]
    public static partial string SoxrProcess(SoxrPtr resampler, void* input, ulong inputLength, 
        out ulong inputUsed, void* output, ulong outputLength, out ulong outputUsed);

    [LibraryImport("soxr", EntryPoint = "soxr_delete")]
    public static partial void SoxrDelete(SoxrPtr ptr);
    [LibraryImport("soxr", EntryPoint = "soxr_clear", StringMarshallingCustomType = typeof(Utf8StringMarshallerNoFree))]
    public static partial string SoxrClear(SoxrPtr ptr);
}
