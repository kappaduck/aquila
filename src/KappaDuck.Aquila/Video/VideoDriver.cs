// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Marshallers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Video;

/// <summary>
/// Represents global video driver functions.
/// </summary>
public static partial class VideoDriver
{
    /// <summary>
    /// Gets the number of video drivers compiled into SDL.
    /// </summary>
    public static int Count { get; } = SDL_GetNumVideoDrivers();

    /// <summary>
    /// Gets the name of the currently initialized video driver.
    /// </summary>
    public static string Current { get; } = SDL_GetCurrentVideoDriver();

    /// <summary>
    /// Gets the name of a built in video driver.
    /// </summary>
    /// <param name="index">The index of a video driver.</param>
    /// <returns>Name of the video driver with the given index.</returns>
    public static string Get(int index) => SDL_GetVideoDriver(index);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_GetNumVideoDrivers();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    private static partial string SDL_GetCurrentVideoDriver();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    private static partial string SDL_GetVideoDriver(int index);
}
