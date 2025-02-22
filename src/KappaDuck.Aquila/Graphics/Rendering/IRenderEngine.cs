// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Interop.SDL.Handles;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics.Rendering;

/// <summary>
/// Represents a renderer that can be used to render graphics.
/// </summary>
public interface IRenderEngine : IDisposable
{
    /// <summary>
    /// Clears the render target with the specified color.
    /// </summary>
    /// <param name="color">The color to clear the render target with.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws the specified vertices to the render target.
    /// </summary>
    /// <param name="vertices">The vertices to draw to the render target.</param>
    void Draw(in ReadOnlySpan<Vertex> vertices);

    /// <summary>
    /// Draws the specified vertices to the render target.
    /// </summary>
    /// <param name="vertices">The vertices to draw to the render target.</param>
    /// <param name="indices">The indices to draw the vertices in the specified order.</param>
    void Draw(in ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices);

    /// <summary>
    /// Renders all the draw calls to the render target.
    /// </summary>
    void Render();

    /// <summary>
    /// Sets up the renderer with the specified window.
    /// </summary>
    /// <param name="window">The window to render to.</param>
    internal void Setup(WindowHandle window);
}
