// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Video.Windows;

/// <summary>
/// Represents the flash state of a window.
/// </summary>
public enum FlashState
{
    /// <summary>
    /// Cancel any window flash state.
    /// </summary>
    Cancel = 0,

    /// <summary>
    /// Flash the window briefly.
    /// </summary>
    Briefly = 1,

    /// <summary>
    /// Flash the window until it receives focus.
    /// </summary>
    UntilFocused = 2,
}
