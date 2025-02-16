// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

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
}
