// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.Handles;

/// <summary>
/// Represents a safe handle that is always invalid when the handle is zero.
/// </summary>
/// <param name="ownsHandle"><see lang="true"/> if the handle is owned by this instance; otherwise, <see lang="false"/>.</param>
public abstract class SafeHandleZeroInvalid(bool ownsHandle) : SafeHandle(nint.Zero, ownsHandle)
{
    /// <summary>
    /// Gets a value indicating whether the handle value is invalid.
    /// </summary>
    public override bool IsInvalid => handle == nint.Zero;
}
