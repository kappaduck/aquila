// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop.Marshallers;

[CustomMarshaller(typeof(bool), MarshalMode.Default, typeof(BoolMarshaller))]
internal static class BoolMarshaller
{
    internal static byte ConvertToUnmanaged(bool managed)
        => managed ? (byte)1 : (byte)0;

    internal static bool ConvertToManaged(byte unmanaged)
        => unmanaged != 0;
}
