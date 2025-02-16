// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Interop.Win32.Extensions;

internal static class PointerExtensions
{
    internal static ushort Lower16bits(this nuint value)
        => (ushort)(value & 0xFFFF);

    internal static ushort Upper16bits(this nuint value)
        => (ushort)((value >> 16) & 0xFFFF);
}
