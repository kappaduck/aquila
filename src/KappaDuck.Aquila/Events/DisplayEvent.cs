// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Display state change event data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct DisplayEvent
{
    private readonly EventType _type;
    private readonly uint _reserved;
    private readonly ulong _timestamp;

    /// <summary>
    /// The associated display.
    /// </summary>
    public uint Id;

    /// <summary>
    /// The event data1.
    /// </summary>
    public int Data1;

    /// <summary>
    /// The event data2.
    /// </summary>
    public int Data2;
}
