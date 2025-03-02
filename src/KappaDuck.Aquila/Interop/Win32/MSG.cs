// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.Win32;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct MSG
{
    private readonly nint _hwnd;

    public readonly uint Message;

    public readonly nuint WParam;

    public readonly nuint LParam;

    private readonly nuint _time;

    public readonly Vector2i Point;
}
