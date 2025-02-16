// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop.SDL.Marshallers;

/// <summary>
/// Marshaller for strings that are owned by SDL.
/// </summary>
/// <remarks>
/// SDL responsible for freeing the memory.
/// </remarks>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(SDLOwnedStringMarshaller))]
internal static class SDLOwnedStringMarshaller
{
    internal static string? ConvertToManaged(nint unmanaged)
        => Marshal.PtrToStringUTF8(unmanaged);
}
