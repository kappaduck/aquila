// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Represents an SDL event.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct SdlEvent
{
    /// <summary>
    /// The type of the event.
    /// </summary>
    [FieldOffset(0)]
    public EventType Type;

    /// <summary>
    /// The display event data.
    /// </summary>
    [FieldOffset(0)]
    public DisplayEvent Display;

    [FieldOffset(0)]
    private unsafe fixed byte _padding[128];
}
