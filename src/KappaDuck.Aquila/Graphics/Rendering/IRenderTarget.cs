// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics.Primitives;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics.Rendering;

/// <summary>
/// Represents a render target that can be used to render graphics.
/// </summary>
public interface IRenderTarget
{
    /// <summary>
    /// Clears the render target with a default color.
    /// </summary>
    void Clear();

    /// <summary>
    /// Clears the render target with the specified color.
    /// </summary>
    /// <param name="color">The color to clear the render target with.</param>
    void Clear(Color color);

    /// <summary>
    /// Draws the specified drawable to the render target.
    /// </summary>
    /// <param name="drawable">The drawable to draw to the render target.</param>
    void Draw(IDrawable drawable);

    /// <summary>
    /// Draws a collection of drawable to the render target.
    /// </summary>
    /// <param name="drawable">The collection of drawable to draw to the render target.</param>
    void Draw(ReadOnlySpan<IDrawable> drawable);

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
    /// Draws a point to the render target.
    /// </summary>
    /// <param name="point">The point to draw to the render target.</param>
    /// <param name="color">The color to draw the points with.</param>
    void Draw(Point<float> point, Color? color);

    /// <summary>
    /// Draws a collection of points to the render target.
    /// </summary>
    /// <param name="points">The points to draw to the render target.</param>
    /// <param name="color">The color to draw the points with.</param>
    void Draw(in ReadOnlySpan<Point<float>> points, Color? color);
}
