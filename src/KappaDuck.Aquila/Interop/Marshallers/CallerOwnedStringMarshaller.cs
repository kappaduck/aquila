// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop.Marshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(CallerOwnedStringMarshaller))]
internal static class CallerOwnedStringMarshaller
{
    internal static string ConvertToManaged(nint unmanaged)
        => Marshal.PtrToStringUTF8(unmanaged) ?? string.Empty;

    internal static void Free(nint unmanaged) => SDLNative.Free(unmanaged);
}
