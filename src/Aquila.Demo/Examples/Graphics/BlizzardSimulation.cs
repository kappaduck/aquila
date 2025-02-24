// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics;
using KappaDuck.Aquila.Graphics.Rendering;
using KappaDuck.Aquila.System;
using KappaDuck.Aquila.Video.Windows;
using System.Drawing;

namespace Aquila.Demo.Examples.Graphics;

/// <summary>
/// Simulates a blizzard with snowflakes using points.
/// </summary>
internal static class BlizzardSimulation
{
    private const int Width = 1080;
    private const int Height = 720;

    internal static void Run()
    {
        // Initialize SDL with the video subsystem
        using SDL engine = SDL.Init(SubSystem.Video);

        Blizzard blizzard = new(Width, Height);

        // Create the window
        using RenderWindow window = new("Blizzard simulation!", Width, Height, WindowState.Resizable | WindowState.AlwaysOnTop);

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

            // Clear the window with black color
            window.Clear();

            // Draw the blizzard
            window.Draw(blizzard);

            // Render the graphics to the window since the last call
            window.Render();
        }
    }
}

/// <summary>
/// A blizzard simulation that renders snowflakes falling from the top of the window.
/// It uses <see cref="IDrawable"/> which allows it to be drawn on a <see cref="IRenderTarget"/>.
/// </summary>
file sealed class Blizzard : IDrawable
{
    /// <summary>
    /// Maximum number of snowflakes to render.
    /// </summary>
    private const int MaxSnowflakes = 1000;

    /// <summary>
    /// Minimum and maximum number of snowflakes to render per second.
    /// </summary>
    private const float MinimumSnowflakesPerSecond = 45;
    private const float MaximumSnowflakesPerSecond = 90;

    private readonly int _height;
    private readonly Point<float>[] _snowflakes = new Point<float>[MaxSnowflakes];
    private readonly float[] _speeds = new float[MaxSnowflakes];

    private double _lastUpdate;

    public Blizzard(int width, int height)
    {
        _height = height;
        _lastUpdate = SDL.GetTicks().TotalMilliseconds;

        for (int i = 0; i < MaxSnowflakes; i++)
        {
            _snowflakes[i] = new Point<float>(Random.Shared.Next(0, width), Random.Shared.Next(0, height));
            _speeds[i] = MinimumSnowflakesPerSecond + (Random.Shared.NextSingle() * (MaximumSnowflakesPerSecond - MinimumSnowflakesPerSecond));
        }
    }

    public void Draw(IRenderTarget target)
    {
        double now = SDL.GetTicks().TotalMilliseconds;
        double delta = (now - _lastUpdate) / 1000;

        _lastUpdate = now;

        for (int i = 0; i < MaxSnowflakes; i++)
        {
            _snowflakes[i].Y += (float)(_speeds[i] * delta);
            if (_snowflakes[i].Y > _height)
            {
                _snowflakes[i].Y = 0;
            }
        }

        target.Draw(_snowflakes, Color.White);
    }
}
