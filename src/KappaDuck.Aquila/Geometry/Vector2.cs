// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Geometry.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// Represents a floating-point two-dimensional vector.
/// </summary>
/// <param name="x">The x-coordinate of the vector.</param>
/// <param name="y">The y-coordinate of the vector.</param>
[StructLayout(LayoutKind.Sequential)]
public struct Vector2(float x, float y) : IEquatable<Vector2>,
    IAdditionOperators<Vector2, Vector2, Vector2>,
    ISubtractionOperators<Vector2, Vector2, Vector2>,
    IMultiplyOperators<Vector2, float, Vector2>,
    IDivisionOperators<Vector2, float, Vector2>,
    IUnaryNegationOperators<Vector2, Vector2>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2"/> struct.
    /// </summary>
    public Vector2() : this(0f, 0f)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2"/> struct using polar coordinates.
    /// </summary>
    /// <param name="radius">The magnitude of the vector.</param>
    /// <param name="angle">The direction of the vector.</param>
    public Vector2(float radius, Angle angle) : this(radius * angle.Cos, radius * angle.Sin)
    {
    }

    /// <summary>
    /// Gets or sets the x-coordinate of the vector.
    /// </summary>
    public float X = x;

    /// <summary>
    /// Gets or sets the y-coordinate of the vector.
    /// </summary>
    public float Y = y;

    /// <summary>
    /// Gets a value indicating whether the vector is normalized.
    /// </summary>
    public readonly bool IsNormalized => FloatingPoint.IsNearlyZero(LengthSquared - 1f);

    /// <summary>
    /// Gets a value indicating whether the vector is zero.
    /// </summary>
    /// <remarks>
    /// It compares the squared length of the vector to a small tolerance.
    /// </remarks>
    public readonly bool IsZero => LengthSquared < (float.Epsilon * float.Epsilon);

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
    public readonly float LengthSquared => (X * X) + (Y * Y);

    /// <summary>
    /// Gets the normalized vector.
    /// </summary>
    public readonly Vector2 Normalized
    {
        get
        {
            if (IsNormalized)
                return this;

            float length = Length;

            if (length > float.Epsilon)
            {
                return this / length;
            }

            return Zero;
        }
    }

    /// <summary>
    /// Gets the perpendicular vector.
    /// </summary>
    public readonly Vector2 Perpendicular => new(-Y, X);

    /// <summary>
    /// Gets a vector pointing along the X-axis (1, 0).
    /// </summary>
    public static Vector2 UnitX { get; } = new(1f, 0f);

    /// <summary>
    /// Gets a vector pointing along the Y-axis (0, 1).
    /// </summary>
    public static Vector2 UnitY { get; } = new(0f, 1f);

    /// <summary>
    /// Gets an origin vector.
    /// </summary>
    public static Vector2 Zero { get; } = new(0f, 0f);

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">Left vector to add.</param>
    /// <param name="right">Right vector to add.</param>
    /// <returns>A new vector that is the sum of the two vectors.</returns>
    public static Vector2 operator +(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);

    /// <summary>
    /// Subtracts two vectors.
    /// </summary>
    /// <param name="left">Left vector to subtract.</param>
    /// <param name="right">Right vector to subtract.</param>
    /// <returns>A new vector that is the difference of the two vectors.</returns>
    public static Vector2 operator -(Vector2 left, Vector2 right) => new(left.X - right.X, left.Y - right.Y);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">Scalar to multiply by.</param>
    /// <returns>A new vector that is the product of the vector and the scalar.</returns>
    public static Vector2 operator *(Vector2 left, float right) => new(left.X * right, left.Y * right);

    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="left">Scalar to multiply.</param>
    /// <param name="right">The vector to multiply by.</param>
    /// <returns>A new vector that is the product of the scalar and the vector.</returns>
    public static Vector2 operator *(float left, Vector2 right) => right * left;

    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="left">The vector to divide.</param>
    /// <param name="right">Scalar to divide by.</param>
    /// <returns>A new vector that is the division of the vector and the scalar.</returns>
    public static Vector2 operator /(Vector2 left, float right) => new(left.X / right, left.Y / right);

    /// <summary>
    /// Negates a vector.
    /// </summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>A new vector that is the negation of the vector.</returns>
    public static Vector2 operator -(Vector2 value) => new(-value.X, -value.Y);

    /// <summary>
    /// Compares two vectors are equal.
    /// </summary>
    /// <param name="left">Left vector to compare.</param>
    /// <param name="right">Right vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Vector2 left, Vector2 right) => left.Equals(right);

    /// <summary>
    /// Compares two vectors are not equal.
    /// </summary>
    /// <param name="left">Left vector to compare.</param>
    /// <param name="right">Right vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

    /// <summary>
    /// Gets the angle between two vectors.
    /// </summary>
    /// <param name="from">The first vector.</param>
    /// <param name="to">The second vector.</param>
    /// <returns>The angle between the two vectors.</returns>
    public static Angle Angle(Vector2 from, Vector2 to)
    {
        float dot = Dot(from.Normalized, to.Normalized);

        dot = Math.Clamp(dot, -1f, 1f);

        return new(MathF.Acos(dot));
    }

    /// <summary>
    /// Gets the distance between two vectors.
    /// </summary>
    /// <param name="left">The left vector to calculate the distance to.</param>
    /// <param name="right">The right vector to calculate the distance to.</param>
    /// <returns>The distance between the two vectors.</returns>
    public static float Distance(Vector2 left, Vector2 right)
    {
        float x = right.X - left.X;
        float y = right.Y - left.Y;

        return MathF.Sqrt((x * x) + (y * y));
    }

    /// <summary>
    /// Produces a dot product of two vectors.
    /// </summary>
    /// <param name="left">Left vector to multiply.</param>
    /// <param name="right">Right vector to multiply.</param>
    /// <returns>A scalar that is the dot product of the two vectors.</returns>
    public static float Dot(Vector2 left, Vector2 right) => (left.X * right.X) + (left.Y * right.Y);

    /// <summary>
    /// Linearly interpolates between two vectors.
    /// </summary>
    /// <param name="start">The start vector.</param>
    /// <param name="end">The end vector.</param>
    /// <param name="interpolationFactor">The interpolation factor. The value is clamped between 0 and 1.</param>
    /// <returns>A new vector interpolated between the start and end vectors.</returns>
    public static Vector2 Lerp(Vector2 start, Vector2 end, float interpolationFactor)
    {
        interpolationFactor = Math.Clamp(interpolationFactor, 0f, 1f);

        float x = start.X + ((end.X - start.X) * interpolationFactor);
        float y = start.Y + ((end.Y - start.Y) * interpolationFactor);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Reflects the current vector across the given normal vector.
    /// </summary>
    /// <remarks>
    /// If the normal vector is not normalized, it will be normalized for the calculation.
    /// </remarks>
    /// <param name="vector">The vector to reflect.</param>
    /// <param name="normal">The normal vector to reflect across. Must be normalized.</param>
    /// <returns>The reflected vector.</returns>
    public static Vector2 Reflect(Vector2 vector, Vector2 normal)
    {
        Vector2 normalized = normal.Normalized;
        return vector - (2 * Dot(vector, normalized) * normalized);
    }

    /// <summary>
    /// Gets the angle between two vectors.
    /// </summary>
    /// <param name="to">The second vector.</param>
    /// <returns>The angle between the two vectors.</returns>
    public readonly Angle Angle(Vector2 to) => Angle(this, to);

    /// <summary>
    /// Clamps the vector to a maximum length.
    /// </summary>
    /// <param name="maxLength">The maximum length to clamp to.</param>
    /// <returns>A new vector that is the clamped vector.</returns>
    public readonly Vector2 Clamp(float maxLength)
    {
        if (LengthSquared > maxLength * maxLength)
        {
            return Normalized * maxLength;
        }

        return this;
    }

    /// <summary>
    /// Gets the distance between two vectors.
    /// </summary>
    /// <param name="other">The other vector to calculate the distance to.</param>
    /// <returns>The distance between the two vectors.</returns>
    public readonly float Distance(Vector2 other) => Distance(this, other);

    /// <summary>
    /// Produces a dot product of two vectors.
    /// </summary>
    /// <param name="right">Right vector to multiply.</param>
    /// <returns>A scalar that is the dot product of the two vectors.</returns>
    public readonly float Dot(Vector2 right) => Dot(this, right);

    /// <summary>
    /// Linearly interpolates between the current vector and the target vector by the given interpolation factor.
    /// </summary>
    /// <param name="target">The target vector to interpolate towards.</param>
    /// <param name="interpolationFactor">The interpolation factor. The value is clamped between 0 and 1.</param>
    /// <returns>A new vector interpolated between the current vector and the target vector.</returns>
    public readonly Vector2 Lerp(Vector2 target, float interpolationFactor) => Lerp(this, target, interpolationFactor);

    /// <summary>
    /// Reflects the current vector across the given normal vector.
    /// </summary>
    /// <remarks>
    /// If the normal vector is not normalized, it will be normalized for the calculation.
    /// </remarks>
    /// <param name="vector">The normal vector to reflect across. Must be normalized.</param>
    /// <returns>The reflected vector.</returns>
    public readonly Vector2 Reflect(Vector2 vector) => Reflect(this, vector);

    /// <summary>
    /// Rotates the vector by the given angle.
    /// </summary>
    /// <param name="angle">The angle to rotate by.</param>
    /// <returns>The rotated vector.</returns>
    public readonly Vector2 Rotate(Angle angle)
        => new((X * angle.Cos) - (Y * angle.Sin), (X * angle.Sin) + (Y * angle.Cos));

    /// <summary>
    /// Compares two vectors are equal.
    /// </summary>
    /// <param name="other">The vector to compare.</param>
    /// <returns><see langword="true"/> if the vectors are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Vector2 other)
    {
        return FloatingPoint.IsNearlyZero(X - other.X)
            && FloatingPoint.IsNearlyZero(Y - other.Y);
    }

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is Vector2 vector && Equals(vector);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    public override readonly string ToString() => $"({X}, {Y})";
}
