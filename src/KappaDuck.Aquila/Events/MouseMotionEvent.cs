// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Inputs;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Represents a mouse motion event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct MouseMotionEvent
{
    private readonly EventType _type;
    private readonly uint _reserved;
    private readonly ulong _timestamp;

    /// <summary>
    /// The window with mouse focus.
    /// </summary>
    public readonly uint WindowId;

    /// <summary>
    /// The mouse instance id in relative mode.
    /// </summary>
    public readonly uint Which;

    /// <summary>
    /// The state of the mouse buttons.
    /// </summary>
    public readonly Mouse.ButtonState State;

    private readonly float _x;
    private readonly float _y;
    private readonly float _xRel;
    private readonly float _yRel;

    /// <summary>
    /// Gets the position of the mouse, relative to window.
    /// </summary>
    public Point<float> Position => new(_x, _y);

    /// <summary>
    /// Gets the relative position of the mouse.
    /// </summary>
    public Point<float> RelativePosition => new(_xRel, _yRel);
}
