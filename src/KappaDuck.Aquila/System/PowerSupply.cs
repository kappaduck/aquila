// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.SDL;

namespace KappaDuck.Aquila.System;

/// <summary>
/// Represents a power supply such as laptop battery.
/// </summary>
public sealed class PowerSupply
{
    private PowerSupply(int? seconds, int? percentage, PowerState state)
    {
        Seconds = seconds;
        Percentage = percentage;
        State = state;
    }

    /// <summary>
    /// Gets the remaining seconds of power left or <see langword="null"/> if the platform cannot determine it.
    /// </summary>
    public int? Seconds { get; }

    /// <summary>
    /// Gets the remaining percentage of power left or <see langword="null"/> if the platform cannot determine it.
    /// </summary>
    public int? Percentage { get; }

    /// <summary>
    /// Gets the current power state.
    /// </summary>
    public PowerState State { get; }

    /// <summary>
    /// Get the current power supply information.
    /// </summary>
    /// <remarks>
    /// You should never take a battery status as absolute truth.
    /// Batteries (especially failing batteries) are delicate hardware,
    /// and the values reported here are best estimates based on what that hardware reports.
    /// It's not uncommon for older batteries to lose stored power much faster than it reports,
    /// or completely drain when reporting it has 20 percent left, etc.
    /// Battery status can change at any time; if you are concerned with power state,
    /// you should call this function frequently, and perhaps ignore changes until they seem to be stable for a few seconds.
    /// It's possible a platform can only report battery percentage or time left but not both.
    /// </remarks>
    /// <returns>The current power supply information.</returns>
    internal static PowerSupply GetPowerInfo()
    {
        PowerState state = SDLNative.SDL_GetPowerInfo(out int seconds, out int percent);

        return new PowerSupply(seconds == -1 ? null : seconds, percent == -1 ? null : percent, state);
    }
}
