// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics;
using KappaDuck.Aquila.Interop.Handles;
using KappaDuck.Aquila.Interop.Marshallers;
using KappaDuck.Aquila.Video.Displays;
using KappaDuck.Aquila.Video.Windows;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop;

internal static partial class NativeMethods
{
    internal static unsafe bool SetWindowFullscreenMode(WindowHandle handle, DisplayMode? value)
    {
        if (value is null)
            return SDL_SetWindowFullscreenMode(handle, mode: null);

        DisplayMode mode = value.Value;
        return SDL_SetWindowFullscreenMode(handle, &mode);
    }

    internal static unsafe bool SetWindowMouseRect(WindowHandle handle, Rectangle<int>? value)
    {
        if (value is null)
            return SDL_SetWindowMouseRect(handle, rectangle: null);

        Rectangle<int> rect = value.Value;
        return SDL_SetWindowMouseRect(handle, &rect);
    }

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial WindowHandle SDL_CreateWindow(string title, int width, int height, WindowState state);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_DestroyWindow(nint window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_FlashWindow(WindowHandle window, FlashState state);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial uint SDL_GetDisplayForWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_GetWindowBordersSize(WindowHandle window, out int top, out int left, out int bottom, out int right);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial float SDL_GetWindowDisplayScale(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial DisplayMode* SDL_GetWindowFullscreenMode(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial uint SDL_GetWindowID(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial Rectangle<int>* SDL_GetWindowMouseRect(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial float SDL_GetWindowPixelDensity(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial PixelFormat SDL_GetWindowPixelFormat(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_GetWindowPosition(WindowHandle window, out int x, out int y);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_GetWindowSafeArea(WindowHandle window, out Rectangle<int> area);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_HideWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_MaximizeWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_MinimizeWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_RaiseWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_RestoreWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowAlwaysOnTop(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool onTop);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowAspectRatio(WindowHandle window, float min, float max);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowBordered(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool bordered);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowFocusable(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool focusable);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static unsafe partial bool SDL_SetWindowFullscreen(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool mode);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    private static unsafe partial bool SDL_SetWindowFullscreenMode(WindowHandle window, DisplayMode* mode);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowKeyboardGrab(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool grabbed);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowMaximumSize(WindowHandle window, int width, int height);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowMinimumSize(WindowHandle window, int width, int height);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowMouseGrab(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool grabbed);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    private static unsafe partial bool SDL_SetWindowMouseRect(WindowHandle window, Rectangle<int>* rectangle);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowOpacity(WindowHandle window, float opacity);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowPosition(WindowHandle window, int x, int y);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowResizable(WindowHandle window, [MarshalUsing(typeof(BoolMarshaller))] bool resizable);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowSize(WindowHandle window, int width, int height);

    [LibraryImport(LibraryName, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SetWindowTitle(WindowHandle window, string title);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_ShowWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_SyncWindow(WindowHandle window);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_WarpMouseInWindow(WindowHandle handle, float x, float y);
}
