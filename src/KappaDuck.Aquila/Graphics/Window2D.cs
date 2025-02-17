// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using KappaDuck.Aquila.Video.Windows;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics;

/// <summary>
/// Represents a window which can be used to render 2D graphics.
/// </summary>
public sealed class Window2D : BaseWindow, IRenderTarget
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
    /// <remarks>
    /// The render target is cleared with a black color.
    /// </remarks>
    public void Clear() => Clear(Color.Black);

    /// <inheritdoc/>
    public void Clear(Color color)
    {
        SDLNative.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDLNative.SDL_RenderClear(_renderer);
    }

    /// <summary>
    /// Renders all the graphics to the window since the last call.
    /// </summary>
    public void Render() => SDLNative.SDL_RenderPresent(_renderer);

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _renderer.Dispose();

        base.Dispose(disposing);
    }

    internal override void OnCreate(WindowHandle window) => _renderer = SDLNative.SDL_CreateRenderer(window);
}
