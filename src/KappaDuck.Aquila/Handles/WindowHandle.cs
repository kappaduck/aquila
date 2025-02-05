// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Handles;

internal sealed partial class WindowHandle() : SafeHandle(IntPtr.Zero, ownsHandle: true)
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            SDL_DestroyWindow(handle);
            SetHandle(IntPtr.Zero);
        }

        return true;
    }

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_DestroyWindow(IntPtr window);
}
