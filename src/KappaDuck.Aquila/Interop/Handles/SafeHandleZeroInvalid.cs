// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.Handles;

internal abstract class SafeHandleZeroInvalid(bool ownsHandle) : SafeHandle(nint.Zero, ownsHandle)
{
    public override bool IsInvalid => handle == nint.Zero;
}
