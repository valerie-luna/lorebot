using System.Runtime.InteropServices;

namespace AudioMux;

internal static class Utilities
{
    public static unsafe T* Allocate<T>(int length)
        where T : unmanaged
    {
        return (T*)Marshal.AllocHGlobal(length * sizeof(T));
    }

    public static unsafe void Free<T>(T* ptr)
        where T : unmanaged
    {
        Marshal.FreeHGlobal((nint)ptr);
    }
}