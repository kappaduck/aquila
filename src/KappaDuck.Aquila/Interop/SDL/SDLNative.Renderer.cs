// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Interop.SDL.Handles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.SDL;

internal static partial class SDLNative
{
    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial RendererHandle SDL_CreateRenderer(WindowHandle window, string name = "");

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_DestroyRenderer(nint renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderClear(RendererHandle renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderGeometry(RendererHandle renderer, nint texture, ReadOnlySpan<Vertex> vertices, int numVertices, ReadOnlySpan<int> indices, int numIndices);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_RenderPresent(RendererHandle renderer);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_SetRenderDrawColor(RendererHandle renderer, byte r, byte g, byte b, byte a);
}
