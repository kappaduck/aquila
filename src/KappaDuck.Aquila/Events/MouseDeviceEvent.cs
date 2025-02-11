// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Represents a mouse device event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct MouseDeviceEvent
{
    private readonly EventType _type;
    private readonly uint _reserved;
    private readonly ulong _timestamp;

    /// <summary>
    /// The mouse instance id which was added or removed.
    /// </summary>
    public readonly uint Which;
}
