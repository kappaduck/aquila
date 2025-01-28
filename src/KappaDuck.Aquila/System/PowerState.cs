// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.System;

/// <summary>
/// Represents the power state of a device.
/// </summary>
public enum PowerState
{
    /// <summary>
    /// An error occurred while determining the power state.
    /// </summary>
    Error = -1,

    /// <summary>
    /// Cannot determine power state.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Not plugged in, running on the battery.
    /// </summary>
    OnBattery = 1,

    /// <summary>
    /// Plugged in, no battery available.
    /// </summary>
    NoBattery = 2,

    /// <summary>
    /// Plugged in, charging the battery.
    /// </summary>
    Charging = 3,

    /// <summary>
    /// Plugged in, battery is charged.
    /// </summary>
    Charged = 4
}
