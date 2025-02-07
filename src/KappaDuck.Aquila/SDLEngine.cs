// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop;
using KappaDuck.Aquila.System;

namespace KappaDuck.Aquila;

/// <summary>
/// Represents the SDL engine which is used to initialize and quit the SDL <see cref="SubSystem"/>.
/// </summary>
public sealed class SDLEngine : IDisposable
{
    private static readonly Lock _lock = new();

    private static SDLEngine? _instance;
    private static SubSystem _subSystems = SubSystem.None;
    private static int _refCount;

    private SDLEngine()
    {
    }

    /// <summary>
    /// Initialize SDL with the specified <see cref="SubSystem"/>.
    /// </summary>
    /// <remarks>
    /// Initialized subsystems are stored and will be uninitialized on <see cref="Dispose" />
    /// or call directly <see cref="QuitSubSystem(SubSystem)"/> to shut down specific subsystems.
    /// You can initialize the same subsystem multiple times. It will only initializes once.
    /// </remarks>
    /// <param name="subSystem">The subsystem to initialize.</param>
    /// <returns>An instance of <see cref="SDLEngine"/>.</returns>
    /// <exception cref="SDLException">Failed to initialize the subsystem.</exception>
    public static SDLEngine Init(SubSystem subSystem)
    {
        lock (_lock)
        {
            _instance ??= new SDLEngine();

            InternalInit(subSystem);

            return _instance;
        }
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

            Native.SDL_QuitSubSystem(_subSystems);
            Native.SDL_Quit();

            Cleanup();
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

            Native.SDL_QuitSubSystem(subSystem);

            _subSystems &= ~subSystem;
        }
    }

    /// <summary>
    /// Get whether the specified <see cref="SubSystem"/> is initialized.
    /// </summary>
    /// <param name="subSystem">The subsystem to compare.</param>
    /// <returns><see langword="true"/> if the subsystem is initialized; otherwise, <see langword="false"/>.</returns>
    public static bool Has(SubSystem subSystem)
        => (_subSystems & subSystem) == subSystem;

    private static void InternalInit(SubSystem subSystem)
    {
        if (Has(subSystem))
            return;

        if (!Native.SDL_InitSubSystem(subSystem))
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
