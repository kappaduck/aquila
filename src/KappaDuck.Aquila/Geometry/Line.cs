// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// Represents a line in a two-dimensional plane.
/// </summary>
/// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
/// <param name="start">The start point of the line.</param>
/// <param name="end">The end point of the line.</param>
[StructLayout(LayoutKind.Auto)]
public struct Line<T>(Point<T> start, Point<T> end) : IEquatable<Line<T>> where T : INumber<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Line{T}"/> struct.
    /// </summary>
    /// <param name="startX">The x-coordinate of the start point.</param>
    /// <param name="startY">The y-coordinate of the start point.</param>
    /// <param name="endX">The x-coordinate of the end point.</param>
    /// <param name="endY">The y-coordinate of the end point.</param>
    public Line(T startX, T startY, T endX, T endY) : this(new Point<T>(startX, startY), new Point<T>(endX, endY))
    {
    }

    /// <summary>
    /// Gets or sets the start point of the line.
    /// </summary>
    public Point<T> Start { readonly get; set; } = start;

    /// <summary>
    /// Gets or sets the end point of the line.
    /// </summary>
    public Point<T> End { readonly get; set; } = end;

    /// <summary>
    /// Checks if two lines are equal.
    /// </summary>
    /// <param name="other">The line to compare.</param>
    /// <returns><see langword="true"/> if the lines are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Line<T> other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Checks if two lines are equal.
    /// </summary>
    /// <param name="obj">The line to compare.</param>
    /// <returns><see langword="true"/> if the lines are equal; otherwise, <see langword="false"/>.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Line<T> line && Equals(line);

    /// <summary>
    /// Gets the hash code for the line.
    /// </summary>
    /// <returns>The hash code of the line.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Converts the line to a string using this format Start = (x, y), End = (x, y).
    /// </summary>
    /// <returns>The string representation of the line.</returns>
    public override readonly string ToString() => $"Start = {Start}, End = {End}";

    /// <summary>
    /// Checks if two lines are equal.
    /// </summary>
    /// <param name="left">Left line to compare.</param>
    /// <param name="right">Right line to compare.</param>
    /// <returns><see langword="true"/> if the lines are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Line<T> left, Line<T> right) => left.Equals(right);

    /// <summary>
    /// Checks if two lines are not equal.
    /// </summary>
    /// <param name="left">Left line to compare.</param>
    /// <param name="right">Right line to compare.</param>
    /// <returns><see langword="true"/> if the lines are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Line<T> left, Line<T> right) => !(left == right);
}
