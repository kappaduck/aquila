// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using KappaDuck.Aquila.Video;
using KappaDuck.Aquila.Video.Windows;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics.Rendering;

/// <summary>
/// Represents a window that can be used to render 2D graphics using SDL Renderer API.
/// </summary>
public sealed class RenderWindow : BaseWindow, IRenderTarget
{
    private readonly RendererHandle _renderer;

    /// <summary>
    /// Creates an empty window.
    /// </summary>
    /// <remarks>
    /// Use <see cref="BaseWindow.Create(string, int, int, WindowState)"/> to create a window.
    /// </remarks>
    /// <param name="name">The name of the rendering driver to initialize, or <see langword="null"/> to let SDL choose one.</param>
    /// <remarks>
    /// A list of available renderers can be obtained by calling <see cref="VideoDriver.Get(int)"/> with an index from 0 to <see cref="VideoDriver.Count"/> - 1.
    /// </remarks>
    public RenderWindow(string? name = null) => _renderer = SDLNative.SDL_CreateRenderer(Handle, name);

    /// <summary>
    /// Creates a new window with the specified title, width, height, and state.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The state of the window.</param>
    /// <param name="name">The name of the rendering driver to initialize, or <see langword="null"/> to let SDL choose one.</param>
    /// <remarks>
    /// A list of available renderers can be obtained by calling <see cref="VideoDriver.Get(int)"/> with an index from 0 to <see cref="VideoDriver.Count"/> - 1.
    /// </remarks>
    public RenderWindow(string title, int width, int height, WindowState state = WindowState.None, string? name = null) : base(title, width, height, state)
        => _renderer = SDLNative.SDL_CreateRenderer(Handle, name);

    /// <inheritdoc/>
    /// <remarks>
    /// The render target is cleared with a black color.
    /// If you want to clear the render target with a different color, use <see cref="Clear(Color)"/> instead.
    /// </remarks>
    public void Clear() => Clear(Color.Black);

    /// <inheritdoc/>
    public void Clear(Color color)
    {
        SDLNative.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDLNative.SDL_RenderClear(_renderer);
    }

    /// <inheritdoc/>
    public void Draw(IDrawable drawable) => drawable.Draw(this);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices)
        => SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, [], 0);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices)
        => SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, indices, indices.Length);

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
}
