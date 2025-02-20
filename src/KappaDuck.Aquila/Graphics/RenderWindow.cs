// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Graphics.Rendering;
using KappaDuck.Aquila.Video.Windows;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics;

/// <summary>
/// Represents a window that can be used to render graphics.
/// </summary>
public sealed class RenderWindow : BaseWindow, IRenderTarget
{
    private readonly IRenderEngine _engine;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderWindow"/> class.
    /// </summary>
    /// <param name="engine">The render engine to use for rendering. By default, a new instance of <see cref="Renderer"/> is used.</param>
    public RenderWindow(IRenderEngine? engine = null)
    {
        _engine = engine ?? new Renderer();

        _engine.Setup(Handle);
    }

    /// <summary>
    /// Initializes a create the window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The initial state of the window.</param>
    /// <param name="engine">The render engine to use for rendering. By default, a new instance of <see cref="Renderer"/> is used.</param>
    public RenderWindow(string title, int width, int height, WindowState state = WindowState.None, IRenderEngine? engine = null) : base(title, width, height, state)
    {
        _engine = engine ?? new Renderer();

        _engine.Setup(Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The render target is cleared with a black color.
    /// If you want to clear the render target with a different color, use <see cref="Clear(Color)"/> instead.
    /// </remarks>
    public void Clear() => Clear(Color.Black);

    /// <inheritdoc/>
    public void Clear(Color color) => _engine.Clear(color);

    /// <inheritdoc/>
    public void Draw(IDrawable drawable)
        => drawable.Draw(this);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices) => _engine.Draw(vertices);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices) => _engine.Draw(vertices, indices);

    /// <summary>
    /// Renders all the graphics to the window since the last call.
    /// </summary>
    public void Render() => _engine.Render();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _engine.Dispose();

        base.Dispose(disposing);
    }
}
