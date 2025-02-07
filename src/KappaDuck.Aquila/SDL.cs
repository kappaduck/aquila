// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop;
using KappaDuck.Aquila.System;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents global SDL functions.
/// </summary>
public static class SDL
{
    internal const string NativeLibrary = "SDL3.dll";

    /// <summary>
    /// Get the current system theme.
    /// </summary>
    /// <returns>The current system theme.</returns>
    public static SystemTheme GetSystemTheme() => Native.SDL_GetSystemTheme();

    /// <summary>
    /// Get the version of the SDL that is linked against your program.
    /// </summary>
    /// <returns>The version of the linked library.</returns>
    public static string GetVersion()
    {
        int version = Native.SDL_GetVersion();

        int major = version / 1000000;
        int minor = version / 1000 % 1000;
        int patch = version % 1000;

        return $"{major}.{minor}.{patch}";
    }

    /// <summary>
    /// Open a URL/URI in the browser or other appropriate external application.
    /// </summary>
    /// <param name="uri">The URL/URI to open.</param>
    /// <remarks>
    /// Open a URL in a separate, system-provided application. How this works will vary wildly depending on the platform.
    /// This will likely launch what makes sense to handle a specific URL's protocol (a web browser for http://, etc),
    /// but it might also be able to launch file managers for directories and other things.
    /// What happens when you open a URL varies wildly as well: your game window may lose
    /// focus(and may or may not lose focus if your game was Fullscreen or grabbing input at the time).
    /// On mobile devices, your app will likely move to the background or your process might be paused. Any given platform may or may not handle a given URL.
    /// If this is unimplemented (or simply unavailable) for a platform, this will fail with an error.
    /// A successful result does not mean the URL loaded, just that we launched something to handle it(or at least believe we did).
    /// All this to say: this function can be useful, but you should definitely test it on every platform you target.
    /// </remarks>
    /// <exception cref="SDLException">Failed to open the URL.</exception>
    public static void OpenUrl(Uri uri) => OpenUrl(uri.ToString());

    /// <inheritdoc cref="OpenUrl(Uri)"/>
    public static void OpenUrl(string uri)
    {
        if (!Native.SDL_OpenURL(uri))
            SDLException.Throw();
    }
}
