// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// A rectangle, with the origin at the upper left.
/// </summary>
/// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
/// <param name="x">The x-coordinate of the top-left corner of the rectangle.</param>
/// <param name="y">The y-coordinate of the top-left corner of the rectangle.</param>
/// <param name="width">The width of the rectangle.</param>
/// <param name="height">The height of the rectangle.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Rectangle<T>(T x, T y, T width, T height) : IEquatable<Rectangle<T>> where T : INumber<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle{T}"/> struct.
    /// </summary>
    public Rectangle() : this(T.Zero, T.Zero, T.Zero, T.Zero)
    {
    }

    /// <summary>
    /// The x-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public T X = x;

    /// <summary>
    /// The y-coordinate of the top-left corner of the rectangle.
    /// </summary>
    public T Y = y;

    /// <summary>
    /// The width of the rectangle.
    /// </summary>
    public T Width = width;

    /// <summary>
    /// The height of the rectangle.
    /// </summary>
    public T Height = height;

    /// <summary>
    /// Gets a value indicating whether the rectangle is empty.
    /// </summary>
    public readonly bool IsEmpty => Width <= T.Zero || Height <= T.Zero;

    /// <summary>
    /// Checks if two rectangles are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="other">The rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Rectangle<T> other)
    {
        if (IsEmpty && other.IsEmpty)
            return true;

        if (other is Rectangle<float> right)
            return CompareWithEpsilon(right);

        return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    }

    /// <summary>
    /// Checks if two rectangles are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="obj">The rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Rectangle<T> rectangle && Equals(rectangle);

    /// <summary>
    /// Gets the hash code of the rectangle.
    /// </summary>
    /// <returns>The hash code of the rectangle.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    /// <summary>
    /// Converts the rectangle to a string using this format (<see cref="X"/>, <see cref="Y"/>, <see cref="Width"/>, <see cref="Height"/>).
    /// </summary>
    /// <returns>The string representation of the rectangle.</returns>
    public override readonly string ToString() => $"({X}, {Y}, {Width}, {Height})";

    /// <summary>
    /// Checks if two rectangles are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="left">Left rectangle to compare.</param>
    /// <param name="right">Right rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Rectangle<T> left, Rectangle<T> right) => left.Equals(right);

    /// <summary>
    /// Checks if two rectangles are not equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="left">Left rectangle to compare.</param>
    /// <param name="right">Right rectangle to compare.</param>
    /// <returns><see langword="true"/> if the rectangles are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Rectangle<T> left, Rectangle<T> right) => !(left == right);

    private readonly bool CompareWithEpsilon(Rectangle<float> rectangle)
    {
        return Math.Approximately(float.CreateChecked(X), rectangle.X)
            && Math.Approximately(float.CreateChecked(Y), rectangle.Y)
            && Math.Approximately(float.CreateChecked(Width), rectangle.Width)
            && Math.Approximately(float.CreateChecked(Height), rectangle.Height);
    }
}
