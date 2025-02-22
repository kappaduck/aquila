// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Win32;
using KappaDuck.Aquila.System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Interop.SDL;

internal static partial class SDLNative
{
    internal const string LibraryName = "SDL3";

    internal static unsafe void Free<T>(T* memory) where T : unmanaged
        => Free((nint)memory);

    internal static unsafe void Free<T>(T** memory) where T : unmanaged
        => Free((nint)memory);

    internal static void Free(nint memory) => SDL_free(memory);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_free(nint memory);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial ulong SDL_GetTicks();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int SDL_GetVersion();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_InitSubSystem(SubSystem subSystem);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_QuitSubSystem(SubSystem subSystem);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_Quit();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [SupportedOSPlatform("windows")]
    internal static partial void SDL_SetWindowsMessageHook(Win32Native.WindowMessageHook callback, nint data = default);
}
