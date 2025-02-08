// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Video.Displays;

/// <summary>
/// Defines a display mode.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct DisplayMode
{
    /// <summary>
    /// The display this mode is associated with.
    /// </summary>
    public uint DisplayId;

    /// <summary>
    /// The pixel format.
    /// </summary>
    public PixelFormat Format;

    /// <summary>
    /// The width of the display mode.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height of the display mode.
    /// </summary>
    public int Height;

    /// <summary>
    /// Scale converting size to pixels (e.g. a 1920x1080 mode with 2.0 scale would have 3840x2160 pixels).
    /// </summary>
    public float PixelDensity;

    /// <summary>
    /// The refresh rate of the display mode or 0.0f for unspecified.
    /// </summary>
    public float RefreshRate;

    /// <summary>
    /// Precise refresh rate numerator or 0 for unspecified.
    /// </summary>
    public int RefreshRateNumerator;

    /// <summary>
    /// Precise refresh rate denominator or 0 for unspecified.
    /// </summary>
    public int RefreshRateDenominator;

    private readonly nint _internal;
}
