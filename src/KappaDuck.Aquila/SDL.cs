// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Marshallers;
using KappaDuck.Aquila.System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents global SDL functions.
/// </summary>
public static partial class SDL
{
    internal const string NativeLibrary = "SDL3.dll";

    private static SubSystem _initializedSubSystems;

    /// <summary>
    /// Get the latest message with information about the specific error that occurred,
    /// or an empty string if there hasn't been an error message set since the last call to <see href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</see>.
    /// </summary>
    /// <remarks>It is possible for multiple errors to occur before calling this function. Only the last error is returned.
    /// The message is only applicable when an SDL function has signaled an error.
    /// </remarks>
    /// <returns>The last error message.</returns>
    public static string GetError() => SDL_GetError();

    /// <summary>
    /// Get the current system theme.
    /// </summary>
    /// <returns>The current system theme.</returns>
    public static SystemTheme GetSystemTheme() => SDL_GetSystemTheme();

    /// <summary>
    /// Get the version of the SDL that is linked against your program.
    /// </summary>
    /// <returns>The version of the linked library.</returns>
    public static string GetVersion()
    {
        int version = SDL_GetVersion();

        int major = version / 1000000;
        int minor = version / 1000 % 1000;
        int patch = version % 1000;

        return $"{major}.{minor}.{patch}";
    }

    /// <summary>
    /// Initializes the specified subsystems.
    /// </summary>
    /// <remarks>
    /// Initialized subsystems are stored and will be uninitialized on <see cref="Quit" /> by using <see cref="QuitSubSystem(SubSystem)"/>
    /// or call directly <see cref="QuitSubSystem(SubSystem)"/> to shut down specific subsystems.
    /// You can initialize the same subsystem multiple times. It will only initializes once.
    /// </remarks>
    /// <param name="subSystem">The subsystems to initialize.</param>
    /// <returns>Returns true on success or false on failure; call <see cref="GetError"/> for more information.</returns>
    public static bool Init(SubSystem subSystem)
    {
        if ((_initializedSubSystems & subSystem) != SubSystem.None)
        {
            return true;
        }

        if (SDL_InitSubSystem(subSystem))
        {
            _initializedSubSystems |= subSystem;
            return true;
        }

        return false;
    }

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
    /// <returns>True on success of false on failure; call <see cref="GetError"/> for more information.</returns>
    public static bool OpenUrl(Uri uri) => SDL_OpenURL(uri.ToString());

    /// <inheritdoc cref="OpenUrl(Uri)"/>
    public static bool OpenUrl(string url) => SDL_OpenURL(url);

    /// <summary>
    /// Clean up all initialized subsystems.
    /// </summary>
    /// <remarks>
    /// It will call <see cref="QuitSubSystem(SubSystem)"/> for initialized subsystems.
    /// It is safe to call this function even in the case of errors in initialization.
    /// </remarks>
    public static void Quit()
    {
        QuitSubSystem(_initializedSubSystems);
        SDL_Quit();
    }

    /// <summary>
    /// Shut down specific subsystems.
    /// </summary>
    /// <remarks>
    /// You still need to call <see cref="Quit" /> even if you close all subsystems.
    /// You can shut down the same subsystem multiple times. It will only shut down once.
    /// </remarks>
    /// <param name="subSystem">Any of the subsystem used by <see cref="Init(SubSystem)"/>.</param>
    public static void QuitSubSystem(SubSystem subSystem)
    {
        if ((_initializedSubSystems & subSystem) == SubSystem.None)
            return;

        SDL_QuitSubSystem(subSystem);
        _initializedSubSystems &= ~subSystem;
    }

    /// <summary>
    /// Get a mask of the specified subsystems which are currently initialized.
    /// </summary>
    /// <param name="subSystem">The subsystem to check if it was initialized or <see langword="null"/> to check all subsystems.</param>
    /// <returns>A mask of all initialized subsystems if <see cref="SubSystem"/> is <see langword="null"/>,
    /// otherwise it returns the initialization status of the specified subsystem.
    /// </returns>
    public static SubSystem WasInit(SubSystem? subSystem)
        => subSystem.HasValue ? (_initializedSubSystems & subSystem.Value) : _initializedSubSystems;

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    private static partial string SDL_GetError();

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial SystemTheme SDL_GetSystemTheme();

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_GetVersion();

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SDL_InitSubSystem(SubSystem subSystem);

    [LibraryImport(NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SDL_OpenURL(string url);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_QuitSubSystem(SubSystem subSystem);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_Quit();
}
