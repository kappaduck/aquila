// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.Handles;

namespace KappaDuck.Aquila.Interop.SDL.Handles;

internal sealed class RendererHandle() : SafeHandleZeroInvalid(ownsHandle: true)
{
    internal static RendererHandle Zero { get; } = new();

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            SDLNative.SDL_DestroyRenderer(handle);

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }
}
