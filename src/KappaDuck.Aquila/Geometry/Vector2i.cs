// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// Represents an integer two-dimensional vector.
/// </summary>
/// <param name="x">The x-coordinate of the vector.</param>
/// <param name="y">The y-coordinate of the vector.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Vector2i(int x, int y) : IEquatable<Vector2i>,
    IAdditionOperators<Vector2i, Vector2i, Vector2i>,
    ISubtractionOperators<Vector2i, Vector2i, Vector2i>,
    IMultiplyOperators<Vector2i, int, Vector2i>,
    IDivisionOperators<Vector2i, int, Vector2i>,
    IUnaryNegationOperators<Vector2i, Vector2i>
{
    /// <summary>
    /// Gets or sets the x-coordinate of the vector.
    /// </summary>
    public int X = x;

    /// <summary>
    /// Gets or sets the y-coordinate of the vector.
    /// </summary>
    public int Y = y;

    /// <summary>
    /// Gets a value indicating whether the vector is zero.
    /// </summary>
    /// <remarks>
    /// It compares the squared length of the vector to a small tolerance.
    /// </remarks>
    public readonly bool IsZero => X == 0 && Y == 0;

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    /// <remarks>
    /// For performance reasons when comparing, it is recommended to use <see cref="LengthSquared"/> instead of <see cref="Length"/> to avoid the square root operation.
    /// </remarks>
    public readonly float Length => MathF.Sqrt(LengthSquared);

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    /// <remarks>
    /// It is more efficient to compare the squared length of two vectors than the length.
    /// </remarks>
    public readonly int LengthSquared => (X * X) + (Y * Y);

    /// <summary>
    /// Gets a vector pointing along the X-axis (1, 0).
    /// </summary>
    public static Vector2i UnitX { get; } = new(1, 0);

    /// <summary>
    /// Gets a vector pointing along the Y-axis (0, 1).
    /// </summary>
    public static Vector2i UnitY { get; } = new(0, 1);

    /// <summary>
    /// Gets an origin vector.
    /// </summary>
    public static Vector2i Zero { get; } = new(0, 0);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">Left vector to add.</param>
    /// <param name="right">Right vector to add.</param>
    /// <returns>A new vector that is the sum of the two vectors.</returns>
    public static Vector2i operator +(Vector2i left, Vector2i right) => new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">Left vector to subtract.</param>
    /// <param name="right">Right vector to subtract.</param>
    /// <returns>A new vector that is the difference of the two vectors.</returns>
    public static Vector2i operator -(Vector2i left, Vector2i right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">Scalar to multiply by.</param>
    /// <returns>A new vector that is the product of the vector and the scalar.</returns>
    public static Vector2i operator *(Vector2i left, int right) => new(left.X * right, left.Y * right);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">Scalar to multiply.</param>
    /// <param name="right">The vector to multiply by.</param>
    /// <returns>A new vector that is the product of the scalar and the vector.</returns>
    public static Vector2i operator *(int left, Vector2i right) => right * left;

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to divide.</param>
    /// <param name="right">Scalar to divide by.</param>
    /// <returns>A new vector that is the division of the vector and the scalar.</returns>
    public static Vector2i operator /(Vector2i left, int right) => new(left.X / right, left.Y / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A new vector that is the negation of the vector.</returns>
    public static Vector2i operator -(Vector2i value) => new(-value.X, -value.Y);

    /// <summary>
    /// Compares two vectors are equal.
    /// </summary>
    /// <param name="left">Left vector to compare.</param>
    /// <param name="right">Right vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Vector2i left, Vector2i right) => left.Equals(right);

    /// <summary>
    /// Compares two vectors are not equal.
    /// </summary>
    /// <param name="left">Left vector to compare.</param>
    /// <param name="right">Right vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Vector2i left, Vector2i right) => !(left == right);

    /// <summary>
    /// Converts a <see cref="Vector2i"/> to a <see cref="Vector2"/>.
    /// </summary>
    /// <param name="vector">The vector to convert.</param>
    public static implicit operator Vector2(Vector2i vector) => new(vector.X, vector.Y);

    /// <summary>
    /// Converts a <see cref="Vector2"/> to a <see cref="Vector2i"/>.
    /// </summary>
    /// <returns>The converted vector.</returns>
    public readonly Vector2 ToVector2() => this;

    /// <summary>
    /// Compares two vectors are equal.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Vector2i other) => X == other.X && Y == other.Y;

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector2i vector && Equals(vector);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    public override readonly string ToString() => $"({X}, {Y})";
}
