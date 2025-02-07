// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop;

namespace KappaDuck.Aquila.System;

/// <summary>
/// Represents operating system related functionality.
/// </summary>
public static class OperatingSystem
{
    /// <summary>
    /// Gets the current power supply information.
    /// </summary>
    public static PowerSupply PowerSupply => PowerSupply.GetPowerInfo();

    /// <summary>
    /// Gets or sets a value indicating whether the screen saver is enabled.
    /// </summary>
    /// <remarks>
    /// If you disable the screensaver, it is automatically re-enabled when SDL quits.
    /// The screensaver is disabled by default, but this may by changed by SDL_HINT_VIDEO_ALLOW_SCREENSAVER.
    /// </remarks>
    /// <exception cref="SDLException">Failed to enable or disable the screensaver.</exception>
    public static bool ScreenSaver
    {
        get => SDLNative.SDL_ScreenSaverEnabled();
        set
        {
            if (value)
            {
                if (!SDLNative.SDL_EnableScreenSaver())
                    SDLException.Throw();

                return;
            }

            if (!SDLNative.SDL_DisableScreenSaver())
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the current system theme.
    /// </summary>
    public static SystemTheme Theme => SDLNative.SDL_GetSystemTheme();

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
    public static void OpenUri(Uri uri) => OpenUrl(uri.ToString());

    /// <inheritdoc cref="OpenUri(Uri)"/>
    public static void OpenUrl(string uri)
    {
        if (!SDLNative.SDL_OpenURL(uri))
            SDLException.Throw();
    }
}
