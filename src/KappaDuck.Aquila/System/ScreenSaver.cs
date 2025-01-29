// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.System;

/// <summary>
/// Represents screen saver functions.
/// </summary>
public static partial class ScreenSaver
{
    /// <summary>
    /// Prevent the screen from being blanked by a screen saver.
    /// </summary>
    /// <remarks>
    /// If you disable the screensaver, it is automatically re-enabled when SDL quits.
    /// The screensaver is disabled by default, but this may by changed by SDL_HINT_VIDEO_ALLOW_SCREENSAVER.
    /// </remarks>
    /// <returns><see langword="true"/> on success or <see langword="false"/> on failure; call <see cref="SDL.GetError"/> for more information.</returns>
    public static bool Disable() => SDL_DisableScreenSaver() != 0;

    /// <summary>
    /// Allow the screen to be blanked by a screen saver.
    /// </summary>
    /// <returns>Returns <see langword="true"/> on success or <see langword="false"/> on failure; call <see cref="SDL.GetError"/> for more information.</returns>
    public static bool Enable() => SDL_EnableScreenSaver() != 0;

    /// <summary>
    /// Check whether the screensaver is currently enabled.
    /// </summary>
    /// <remarks>
    /// The screensaver is disabled by default, but this may by changed by SDL_HINT_VIDEO_ALLOW_SCREENSAVER.
    /// </remarks>
    /// <returns>Returns <see langword="true"/> on success or <see langword="false"/> on failure; call <see cref="SDL.GetError"/> for more information.</returns>
    public static bool IsEnabled() => SDL_ScreenSaverEnabled() != 0;

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_DisableScreenSaver();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_EnableScreenSaver();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_ScreenSaverEnabled();
}
