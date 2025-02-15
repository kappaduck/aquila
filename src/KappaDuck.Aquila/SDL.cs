// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.System;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents the SDL engine which is used to initialize and quit the SDL <see cref="SubSystem"/>.
/// </summary>
public sealed class SDL : IDisposable
{
    private static readonly Lock _lock = new();

    private static SDL? _instance;
    private static SubSystem _subSystems = SubSystem.None;
    private static int _refCount;

    private SDL()
    {
    }

    /// <summary>
    /// Clean up all initialized subsystems.
    /// </summary>
    public void Dispose()
    {
        lock (_lock)
        {
            if (Interlocked.Decrement(ref _refCount) > 0)
                return;

            SDLNative.SDL_QuitSubSystem(_subSystems);
            SDLNative.SDL_Quit();

            Cleanup();
        }
    }

    /// <summary>
    /// Get the version of the SDL that is linked against your program.
    /// </summary>
    /// <returns>The version of the linked library.</returns>
    public static string GetVersion()
    {
        int version = SDLNative.SDL_GetVersion();

        int major = version / 1000000;
        int minor = version / 1000 % 1000;
        int patch = version % 1000;

        return $"{major}.{minor}.{patch}";
    }

    /// <summary>
    /// Get whether the specified <see cref="SubSystem"/> is initialized.
    /// </summary>
    /// <param name="subSystem">The subsystem to compare.</param>
    /// <returns><see langword="true"/> if the subsystem is initialized; otherwise, <see langword="false"/>.</returns>
    public static bool Has(SubSystem subSystem)
        => (_subSystems & subSystem) == subSystem;

    /// <summary>
    /// Initialize SDL with the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// Initialized subsystems are stored and will be uninitialized on <see cref="Dispose" />
    /// or call directly <see cref="QuitSubSystem(SubSystem)"/> to shut down specific subsystems.
    /// You can initialize the same subsystem multiple times. It will only initializes once.
    /// </remarks>
    /// <param name="subSystem">The subsystem to initialize.</param>
    /// <returns>An instance of <see cref="SDL"/>.</returns>
    /// <exception cref="SDLException">Failed to initialize the subsystem.</exception>
    public static SDL Init(SubSystem subSystem)
    {
        lock (_lock)
        {
            _instance ??= new SDL();

            InternalInit(subSystem);

            return _instance;
        }
    }

    /// <summary>
    /// Initialize SDL with the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// You should call <see cref="Init(SubSystem)"/> before calling this method to make sure the SDL is initialized.
    /// </remarks>
    /// <param name="subSystem">The subsystem to initialize.</param>
    /// <exception cref="SDLException">Failed to initialize the subsystem.</exception>
    /// <exception cref="InvalidOperationException">The SDL is not initialized.</exception>
    public static void InitSubSystem(SubSystem subSystem)
    {
        ThrowIfInstanceNull();

        lock (_lock)
            InternalInit(subSystem);
    }

    /// <summary>
    /// Quit the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// <para>You should call <see cref = "Init(SubSystem)" /> before calling this method to make sure the SDL is initialized.</para>
    /// <para>You can shut down the same subsystem multiple times. It will only shut down once.</para>
    /// You still need to call <see cref="Dispose" /> or <see langword="using"/> even if you close all subsystems.
    /// </remarks>
    /// <param name="subSystem">The subsystem to quit.</param>
    /// <exception cref="InvalidOperationException">The SDL is not initialized.</exception>
    public static void QuitSubSystem(SubSystem subSystem)
    {
        ThrowIfInstanceNull();

        lock (_lock)
        {
            if (!Has(subSystem))
                return;

            SDLNative.SDL_QuitSubSystem(subSystem);

            _subSystems &= ~subSystem;
        }
    }

    private static void InternalInit(SubSystem subSystem)
    {
        if (Has(subSystem))
            return;

        if (!SDLNative.SDL_InitSubSystem(subSystem))
            SDLException.Throw();

        Interlocked.Increment(ref _refCount);

        _subSystems |= subSystem;
    }

    private static void Cleanup()
    {
        _instance = null;
        _subSystems = SubSystem.None;
    }

    private static void ThrowIfInstanceNull()
    {
        if (_instance is null)
            throw new InvalidOperationException("SDL is not initialized.");
    }
}
