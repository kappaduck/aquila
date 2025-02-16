// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Graphics;
using KappaDuck.Aquila.Inputs;
using KappaDuck.Aquila.System;
using KappaDuck.Aquila.Video.Windows;

using SDL engine = SDL.Init(SubSystem.Video);

using Window2D window = new("Aquila Demo", 1080, 720, WindowState.Resizable);

while (window.IsOpen)
{
    while (window.Poll(out SDLEvent e))
    {
        if (e.RequestQuit())
        {
            window.Close();
            return;
        }

        if (e.IsKeyDown(Keyboard.Scancode.Space))
            Console.WriteLine("Space");

        if (e.IsMouseButtonDown(Mouse.Button.Left))
            window.Title = $"Aquila Demo - Left Mouse Button Down at {e.Mouse.Position}";
    }
}
