// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents global SDL functions.
/// </summary>
public static class SDL
{
    internal const string NativeLibrary = "SDL3.dll";

    /// <summary>
    /// Get the version of the SDL that is linked against your program.
    /// </summary>
    /// <returns>The version of the linked library.</returns>
    public static string GetVersion()
    {
        int version = SDLNative.SDL_GetVersion();

        int major = version / 1000000;
        int minor = version / 1000 % 1000;
        int patch = version % 1000;

        return $"{major}.{minor}.{patch}";
    }
}
