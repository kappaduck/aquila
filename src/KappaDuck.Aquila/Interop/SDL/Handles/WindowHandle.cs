// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Handles;

namespace KappaDuck.Aquila.Interop.SDL.Handles;

internal sealed class WindowHandle() : SafeHandleZeroInvalid(ownsHandle: true)
{
    internal static WindowHandle Zero { get; } = new();

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
