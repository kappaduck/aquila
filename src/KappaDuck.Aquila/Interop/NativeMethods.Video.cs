// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Interop.Marshallers;
using KappaDuck.Aquila.Video.Displays;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop;

internal static partial class NativeMethods
{
    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_GetClosestFullscreenDisplayMode(uint display, int width, int height, float refreshRate, [MarshalUsing(typeof(BoolMarshaller))] bool includeHighDensityMode, out DisplayMode displayMode);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial DisplayMode* SDL_GetCurrentDisplayMode(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial DisplayOrientation SDL_GetCurrentDisplayOrientation(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    internal static partial string SDL_GetCurrentVideoDriver();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial DisplayMode* SDL_GetDesktopDisplayMode(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial uint* SDL_GetDisplays(out int count);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_GetDisplayBounds(uint display, out Rectangle<int> bounds);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial float SDL_GetDisplayContentScale(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial uint SDL_GetDisplayForPoint(Point<int>* point);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial uint SDL_GetDisplayForRect(Rectangle<int>* rectangle);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    internal static partial string? SDL_GetDisplayName(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial uint SDL_GetDisplayProperties(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_GetDisplayUsableBounds(uint display, out Rectangle<int> bounds);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial DisplayMode** SDL_GetFullscreenDisplayModes(uint display, out int count);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial DisplayOrientation SDL_GetNaturalDisplayOrientation(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int SDL_GetNumVideoDrivers();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial uint SDL_GetPrimaryDisplay();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    internal static partial string SDL_GetVideoDriver(int index);
}
