// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Marshallers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Video.Displays;

/// <summary>
/// Represents a display such as monitor.
/// </summary>
public sealed partial class Display
{
    private const string HdrEnabledProperty = "SDL.display.HDR_enabled";

    internal Display(uint id) => Id = id;

    /// <summary>
    /// Gets the Display Id.
    /// </summary>
    public uint Id { get; init; }

    /// <summary>
    /// Gets the name of the display or <see cref="string.Empty"/> on failure; call <see cref="SDL.GetError"/> for more information.
    /// </summary>
    public string Name => SDL_GetDisplayName(Id);

    /// <summary>
    /// Gets information about the current display mode.
    /// </summary>
    /// <remarks>
    /// There's a difference between <see cref="CurrentMode"/> and <see cref="DesktopMode"/>.
    /// When SDL run fullscreen and has changed the resolution. In that case, <see cref="CurrentMode"/> will return the current resolution,
    /// and not the previous native display mode.
    /// </remarks>
    public unsafe DisplayMode CurrentMode => *SDL_GetCurrentDisplayMode(Id);

    /// <summary>
    /// Gets information about the desktop display mode.
    /// </summary>
    /// <remarks>
    /// There's a difference between <see cref="DesktopMode"/> and <see cref="CurrentMode"/>.
    /// When SDL run fullscreen and has changed the resolution. In that case, <see cref="DesktopMode"/> will return the previous native display mode,
    /// and not the current display mode.
    /// </remarks>
    public unsafe DisplayMode DesktopMode => *SDL_GetDesktopDisplayMode(Id);

    /// <summary>
    /// Gets a value indicating whether HDR is enabled on the display.
    /// </summary>
    public bool HdrEnabled => SDLProperties.Get(SDL_GetDisplayProperties(Id), HdrEnabledProperty, defaultValue: false);

    /// <summary>
    /// Gets the orientation of a display.
    /// </summary>
    public DisplayOrientation Orientation => SDL_GetCurrentDisplayOrientation(Id);

    /// <summary>
    /// Gets the orientation of a display when it is unrotated.
    /// </summary>
    public DisplayOrientation NaturalOrientation => SDL_GetNaturalDisplayOrientation(Id);

    /// <summary>
    /// Get a list of fullscreen display modes available for the display.
    /// </summary>
    /// <returns>A list of fullscreen display modes.</returns>
    public unsafe DisplayMode[] GetFullScreenModes()
    {
        DisplayMode** modes = SDL_GetFullscreenDisplayModes(Id, out int length);

        if (modes is null)
            return [];

        DisplayMode[] displayModes = new DisplayMode[length];

        for (int i = 0; i < length; i++)
            displayModes[i] = *modes[i];

        SDL.Free(modes);

        return displayModes;
    }

    /// <summary>
    /// Get a list of currently connected displays.
    /// </summary>
    /// <returns>A list of currently connected displays otherwise empty on failure; call <see cref="SDL.GetError"/>.</returns>
    public static unsafe Display[] GetDisplays()
    {
        uint* ids = SDL_GetDisplays(out int length);

        if (ids is null)
            return [];

        Display[] displays = new Display[length];

        for (int i = 0; i < length; i++)
            displays[i] = new Display(ids[i]);

        SDL.Free(ids);

        return displays;
    }

    /// <summary>
    /// Search the closest matching display mode for the display.
    /// </summary>
    /// <param name="width">The width in pixels of the desired display mode.</param>
    /// <param name="height">The height in pixels of the desired display mode.</param>
    /// <param name="refreshRate">The refresh rate of the desired display mode, or 0 for the desktop refresh rate.</param>
    /// <param name="includeHighDensityMode">include high density modes in the search.</param>
    /// <returns>The closest display mode equal to or larger than the desired mode.</returns>
    /// <exception cref="SDLException">Failed to search the display mode.</exception>
    public DisplayMode SearchDisplayMode(int width, int height, float refreshRate, bool includeHighDensityMode)
    {
        if (SDL_GetClosestFullscreenDisplayMode(Id, width, height, refreshRate, includeHighDensityMode, out DisplayMode displayMode) == 0)
            SDLException.Throw();

        return displayMode;
    }

    /// <summary>
    /// Get the primary display.
    /// </summary>
    /// <returns>The primary display.</returns>
    public static Display GetPrimaryDisplay() => new(SDL_GetPrimaryDisplay());

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_GetClosestFullscreenDisplayMode(uint display, int width, int height, float refreshRate, [MarshalAs(UnmanagedType.Bool)] bool includeHighDensityMode, out DisplayMode displayMode);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial DisplayMode* SDL_GetCurrentDisplayMode(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial DisplayMode* SDL_GetDesktopDisplayMode(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    private static partial string SDL_GetDisplayName(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial uint SDL_GetDisplayProperties(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial uint* SDL_GetDisplays(out int count);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial DisplayMode** SDL_GetFullscreenDisplayModes(uint display, out int count);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial uint SDL_GetPrimaryDisplay();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial DisplayOrientation SDL_GetCurrentDisplayOrientation(uint display);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial DisplayOrientation SDL_GetNaturalDisplayOrientation(uint display);
}
