// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Graphics;
using KappaDuck.Aquila.System;
using KappaDuck.Aquila.Video.Windows;
using System.Diagnostics;
using System.Drawing;

namespace Aquila.Demo.Examples.Graphics;

/// <summary>
/// Demonstrates how to clear the render target with a sine wave that smoothly transitions between colors.
/// </summary>
internal static class SineWaveClear
{
    internal static void Run()
    {
        // Initialize SDL with the video subsystem
        using SDL engine = SDL.Init(SubSystem.Video);

        // Create the window
        using Window2D window = new("Sine Wave", 1080, 720, WindowState.Resizable | WindowState.AlwaysOnTop);

        // Start a stopwatch for the sine wave
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Run the main loop
        while (window.IsOpen)
        {
            // Poll for events
            while (window.Poll(out SDLEvent e))
            {
                // Close the window if the user requests to quit
                if (e.RequestQuit())
                {
                    window.Close();
                    return;
                }
            }

            // Clear the window with a sine wave color
            window.Clear(SineWaveColor(stopwatch.Elapsed.TotalSeconds));

            // Render the graphics to the window since the last call
            window.Render();
        }
    }

    private static Color SineWaveColor(double seconds)
    {
        double red = 0.5 + (0.5 * Math.Sin(seconds));
        double green = 0.5 + (0.5 * Math.Sin(seconds + (Math.PI * 2 / 3)));
        double blue = 0.5 + (0.5 * Math.Sin(seconds + (Math.PI * 4 / 3)));

        return Color.FromArgb((byte)(red * 255), (byte)(green * 255), (byte)(blue * 255));
    }
}
