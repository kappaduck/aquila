// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop;

namespace KappaDuck.Aquila.Video;

/// <summary>
/// Represents global video driver functions.
/// </summary>
public static class VideoDriver
{
    /// <summary>
    /// Gets the number of video drivers compiled into SDL.
    /// </summary>
    public static int Count { get; } = NativeMethods.SDL_GetNumVideoDrivers();

    /// <summary>
    /// Gets the name of the currently initialized video driver.
    /// </summary>
    public static string Current { get; } = NativeMethods.SDL_GetCurrentVideoDriver();

    /// <summary>
    /// Gets the name of a built in video driver.
    /// </summary>
    /// <param name="index">The index of a video driver.</param>
    /// <returns>Name of the video driver with the given index.</returns>
    public static string Get(int index) => NativeMethods.SDL_GetVideoDriver(index);
}
