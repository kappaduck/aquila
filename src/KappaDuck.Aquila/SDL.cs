// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents global SDL functions.
/// </summary>
public static partial class SDL
{
    internal const string NativeLibrary = "SDL3.dll";

    private static SubSystem _initializedSubSystems;

    /// <summary>
    /// Initializes the specified subsystems.
    /// </summary>
    /// <remarks>
    /// Initialized subsystems are stored and will be uninitialized on <see cref="Quit" /> by using <see cref="QuitSubSystem(SubSystem)"/>
    /// or call directly <see cref="QuitSubSystem(SubSystem)"/> to shut down specific subsystems.
    /// You can initialize the same subsystem multiple times. It will only initializes once.
    /// </remarks>
    /// <param name="subSystem">The subsystems to initialize.</param>
    /// <returns>Returns true on success or false on failure.</returns>
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
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SDL_InitSubSystem(SubSystem subSystem);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_QuitSubSystem(SubSystem subSystem);

    [LibraryImport(NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_Quit();
}
