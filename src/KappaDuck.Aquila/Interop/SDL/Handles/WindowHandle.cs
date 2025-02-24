// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Handles;

namespace KappaDuck.Aquila.Interop.SDL.Handles;

/// <summary>
/// Represents a handle to a window.
/// </summary>
public sealed class WindowHandle : SafeHandleZeroInvalid
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowHandle"/> class.
    /// </summary>
    public WindowHandle() : base(ownsHandle: true)
    {
    }

    internal WindowHandle(WindowHandle window) : base(ownsHandle: false)
        => SetHandle(window.handle);

    internal static WindowHandle Zero { get; } = new();

    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            SDLNative.SDL_DestroyWindow(handle);

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }
}
