// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Marshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(OwnedStringMarshaller))]
internal static class OwnedStringMarshaller
{
    internal static string ConvertToManaged(IntPtr unmanaged)
        => Marshal.PtrToStringUTF8(unmanaged) ?? string.Empty;
}
