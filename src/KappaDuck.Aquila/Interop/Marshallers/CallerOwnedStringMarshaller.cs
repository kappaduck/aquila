// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop.Marshallers;

/// <summary>
/// Marshaller for strings that are owned by the caller.
/// </summary>
/// <remarks>
/// Caller responsible for freeing the memory. It uses the <see cref="NativeMethods.Free(nint)"/> method.
/// </remarks>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(CallerOwnedStringMarshaller))]
internal static class CallerOwnedStringMarshaller
{
    internal static string? ConvertToManaged(nint unmanaged)
        => Marshal.PtrToStringUTF8(unmanaged);

    internal static void Free(nint unmanaged) => NativeMethods.Free(unmanaged);
}
