// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Pixels;
using KappaDuck.Aquila.Interop.SDL.Handles;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.SDL;

internal static partial class SDLNative
{
    [LibraryImport(LibraryName)]
    internal static unsafe partial SurfaceHandle SDL_CreateSurfaceFrom(int width, int height, PixelFormat format, void* pixels, int pitch);

    [LibraryImport(LibraryName)]
    internal static partial void SDL_DestroySurface(nint surface);
}
