// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Marshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(CallerOwnedStringMarshaller))]
internal static partial class CallerOwnedStringMarshaller
{
    internal static string ConvertToManaged(IntPtr unmanaged)
        => Marshal.PtrToStringUTF8(unmanaged) ?? string.Empty;

    internal static void Free(IntPtr unmanaged) => SDL_free(unmanaged);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_free(IntPtr mem);
}
