// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Handles;

namespace KappaDuck.Aquila.Interop.SDL.Handles;

internal sealed class SurfaceHandle() : SafeHandleZeroInvalid(ownsHandle: true)
{
    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            SDLNative.SDL_DestroySurface(handle);

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }
}
