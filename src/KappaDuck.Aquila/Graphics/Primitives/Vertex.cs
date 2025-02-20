// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics.Pixels;
using System.Drawing;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Graphics.Primitives;

/// <summary>
/// Represents a vertex with a position and color.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    /// <summary>
    /// Creates a vertex with the specified position and color.
    /// </summary>
    /// <param name="position">The position of the vertex.</param>
    /// <param name="color">The color of the vertex.</param>
    public Vertex(Point<float> position, Color color)
    {
        Position = position;
        Color = color;
    }

    /// <summary>
    /// Creates a vertex with the specified position and color.
    /// </summary>
    /// <param name="x">The x-coordinate of the vertex.</param>
    /// <param name="y">The y-coordinate of the vertex.</param>
    /// <param name="color">The color of the vertex.</param>
    public Vertex(float x, float y, Color color) : this(new Point<float>(x, y), color)
    {
    }

    /// <summary>
    /// Creates a vertex with the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the vertex.</param>
    /// <param name="y">The y-coordinate of the vertex.</param>
    public Vertex(float x, float y) : this(new Point<float>(x, y), Color.White)
    {
    }

    /// <summary>
    /// Creates a vertex with the specified position.
    /// </summary>
    /// <param name="position">The position of the vertex.</param>
    public Vertex(Point<float> position) : this(position, Color.White)
    {
    }

    /// <summary>
    /// Creates a vertex with the specified color.
    /// </summary>
    /// <param name="color">The color of the vertex.</param>
    public Vertex(Color color) : this(new Point<float>(0, 0), color)
    {
    }

    /// <summary>
    /// Gets or sets the position of the vertex.
    /// </summary>
    public Point<float> Position;

    private SDL_FColor _color;
    private Point<float> _texCoord;

    /// <summary>
    /// Gets or sets the color of the vertex.
    /// </summary>
    public Color Color
    {
        readonly get => Color.FromArgb((int)_color.A * 255, (int)_color.R * 255, (int)_color.G * 255, (int)_color.B * 255);
        set => _color = new SDL_FColor(value.R, value.G, value.B, value.A);
    }
}
