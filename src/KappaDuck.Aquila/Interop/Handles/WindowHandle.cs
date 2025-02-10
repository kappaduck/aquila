// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.Handles;

internal sealed class WindowHandle() : SafeHandle(nint.Zero, ownsHandle: true)
{
    public override bool IsInvalid => handle == nint.Zero;

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            NativeMethods.SDL_DestroyWindow(handle);

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }
}
