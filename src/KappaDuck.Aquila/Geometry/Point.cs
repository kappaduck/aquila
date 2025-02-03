// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// Represents a point in a two-dimensional plane.
/// </summary>
/// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
/// <param name="x">The x-coordinate of the point.</param>
/// <param name="y">The y-coordinate of the point.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Point<T>(T x, T y) : IEquatable<Point<T>>, IAdditionOperators<Point<T>, Point<T>, Point<T>>, ISubtractionOperators<Point<T>, Point<T>, Point<T>>, IMultiplyOperators<Point<T>, T, Point<T>>, IDivisionOperators<Point<T>, T, Point<T>>
    where T : INumber<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Point{T}"/> struct.
    /// </summary>
    public Point() : this(T.Zero, T.Zero)
    {
    }

    /// <summary>
    /// The x-coordinate of the point.
    /// </summary>
    public T X = x;

    /// <summary>
    /// The y-coordinate of the point.
    /// </summary>
    public T Y = y;

    /// <summary>
    /// Checks if two points are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="other">The point to compare.</param>
    /// <returns><see langword="true"/> if the points are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Point<T> other)
    {
        if (other is Point<float> point)
            return CompareWithEpsilon(point);

        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Checks if two points are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="obj">The point to compare.</param>
    /// <returns><see langword="true"/> if the points are equal; otherwise, <see langword="false"/>.</returns>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is Point<T> point && Equals(point);

    /// <summary>
    /// Gets the hash code of the point.
    /// </summary>
    /// <returns>The hash code of the point.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Converts the point to a string using this format (<see cref="X"/>, <see cref="Y"/>).
    /// </summary>
    /// <returns>The string representation of the point.</returns>
    public override readonly string ToString() => $"({X}, {Y})";

    /// <summary>
    /// Adds two points together.
    /// </summary>
    /// <param name="left">Left point.</param>
    /// <param name="right">Right point.</param>
    /// <returns>The sum of the two points.</returns>
    public static Point<T> operator +(Point<T> left, Point<T> right) => new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts two points.
    /// </summary>
    /// <param name="left">Left point.</param>
    /// <param name="right">Right point.</param>
    /// <returns>The difference between the two points.</returns>
    public static Point<T> operator -(Point<T> left, Point<T> right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies a point by a scalar.
    /// </summary>
    /// <param name="left">Left point.</param>
    /// <param name="right">Right point.</param>
    /// <returns>The product of the point and the scalar.</returns>
    public static Point<T> operator *(Point<T> left, T right) => new(left.X * right, left.Y * right);

    /// <summary>
    /// Divides a point by a scalar.
    /// </summary>
    /// <param name="left">Left point.</param>
    /// <param name="right">Right point.</param>
    /// <returns>The quotient of the point and the scalar.</returns>
    /// <exception cref="DivideByZeroException">Thrown when the scalar is zero.</exception>
    public static Point<T> operator /(Point<T> left, T right)
    {
        if (T.IsZero(right))
            throw new DivideByZeroException();

        return new(left.X / right, left.Y / right);
    }

    /// <summary>
    /// Checks if two points are equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="left">Left point to compare.</param>
    /// <param name="right">Right point to compare.</param>
    /// <returns><see langword="true"/> if the points are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Point<T> left, Point<T> right) => left.Equals(right);

    /// <summary>
    /// Checks if two points are not equal.
    /// </summary>
    /// <remarks>
    /// If <typeparamref name="T"/> is <see cref="float"/>, the comparison is within a small tolerance.
    /// </remarks>
    /// <param name="left">Left point to compare.</param>
    /// <param name="right">Right point to compare.</param>
    /// <returns><see langword="true"/> if the points are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Point<T> left, Point<T> right) => !(left == right);

    private readonly bool CompareWithEpsilon(Point<float> right)
    {
        float leftX = float.CreateChecked(X);
        float leftY = float.CreateChecked(Y);

        return Math.Approximately(leftX, right.X) && Math.Approximately(leftY, right.Y);
    }
}
