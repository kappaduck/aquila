// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using KappaDuck.Aquila.Video.Windows;

namespace KappaDuck.Aquila.Graphics;

/// <summary>
/// Represents a 2D window which can be used to render 2D graphics.
/// </summary>
public sealed class Window2D : BaseWindow
{
    private RendererHandle _renderer = RendererHandle.Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="Window2D"/> class.
    /// </summary>
    public Window2D()
    {
    }

    /// <summary>
    /// Initializes a create the window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The initial state of the window.</param>
    /// <exception cref="SDLException">An error occurred while creating the window.</exception>
    public Window2D(string title, int width, int height, WindowState state = WindowState.None) : base(title, width, height, state)
    {
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _renderer.Dispose();

        base.Dispose(disposing);
    }

    internal override void OnCreate(WindowHandle window) => _renderer = SDLNative.SDL_CreateRenderer(window);
}
