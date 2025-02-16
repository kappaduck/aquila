// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.Handles;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Interop.Win32.Handles;

[SupportedOSPlatform("windows")]
internal sealed class MenuHandle() : SafeHandleZeroInvalid(ownsHandle: true)
{
    internal static MenuHandle Zero { get; } = new();

    protected override bool ReleaseHandle()
    {
        if (!IsInvalid)
        {
            if (!Win32Native.DestroyMenu(handle))
            {
                Win32Exception.Throw("Failed to destroy menu bar.");
                return false;
            }

            SetHandle(nint.Zero);
            SetHandleAsInvalid();
        }

        return true;
    }
}
