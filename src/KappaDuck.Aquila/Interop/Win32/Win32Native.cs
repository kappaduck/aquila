// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Interop.Win32;

[SupportedOSPlatform("windows")]
internal static partial class Win32Native
{
    private const string LibraryName = "user32";

    [LibraryImport(LibraryName, SetLastError = true)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool DestroyMenu(nint menu);
}
