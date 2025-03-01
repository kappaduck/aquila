// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Graphics.Rendering;
using KappaDuck.Aquila.System;
using KappaDuck.Aquila.Video.Windows;

namespace Aquila.Demo.Examples.Windows;

/// <summary>
/// Demonstrates how to create a minimal window.
/// </summary>
internal static class MinimalWindow
{
    internal static void Run()
    {
        // Initialize SDL with the video subsystem
        using SDL engine = SDL.Init(SubSystem.Video);

        // Create a resizable window with the title "Minimal Window" and size 1080x720
        using RenderWindow window = new("Minimal Window", 1080, 720, WindowState.Resizable | WindowState.AlwaysOnTop);

        // Run the main loop
        while (window.IsOpen)
        {
            // Poll events and handle them
            while (window.Poll(out SDLEvent e))
            {
                // If the user requested to quit the application, close the window and exit the loop
                // You can close the window by clicking the close button or pressing Esc key
                if (e.RequestQuit())
                {
                    window.Close();
                    return;
                }
            }
        }
    }
}
