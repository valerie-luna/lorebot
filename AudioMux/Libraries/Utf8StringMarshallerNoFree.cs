using System.Runtime.InteropServices.Marshalling;

namespace AudioMux.Libraries;

[CustomMarshaller(typeof(string), MarshalMode.Default, typeof(Utf8StringMarshallerNoFree))]
internal static unsafe class Utf8StringMarshallerNoFree
{
    public static byte* ConvertToUnmanaged(string? managed)
    {
        return Utf8StringMarshaller.ConvertToUnmanaged(managed);
    }

    public static string? ConvertToManaged(byte* unmanaged)
    {
        return Utf8StringMarshaller.ConvertToManaged(unmanaged);
    }

    public static void Free(byte* unmanaged)
    {

    }
}
