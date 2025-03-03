// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry;

/// <summary>
/// Represents an angle.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct Angle : IEquatable<Angle>, IAdditionOperators<Angle, Angle, Angle>, ISubtractionOperators<Angle, Angle, Angle>, IUnaryNegationOperators<Angle, Angle>
{
    internal Angle(float radians)
    {
        Radians = radians;
        Degrees = (float)(radians * 180f / Math.PI);

        Sin = MathF.Sin(radians);
        Cos = MathF.Cos(radians);
        Tan = MathF.Tan(radians);
    }

    /// <summary>
    /// Gets an angle in radians.
    /// </summary>
    public float Radians { get; }

    /// <summary>
    /// Gets an angle in degrees.
    /// </summary>
    public float Degrees { get; }

    /// <summary>
    /// Gets the computed sine of the angle.
    /// </summary>
    public float Sin { get; }

    /// <summary>
    /// Gets the computed cosine of the angle.
    /// </summary>
    public float Cos { get; }

    /// <summary>
    /// Gets the computed tangent of the angle.
    /// </summary>
    public float Tan { get; }

    /// <summary>
    /// Adds two angles.
    /// </summary>
    /// <param name="left">The first angle.</param>
    /// <param name="right">The second angle.</param>
    /// <returns>The sum of the angles.</returns>
    public static Angle operator +(Angle left, Angle right) => new(left.Radians + right.Radians);

    /// <summary>
    /// Subtracts two angles.
    /// </summary>
    /// <param name="left">The first angle.</param>
    /// <param name="right">The second angle.</param>
    /// <returns>The difference of the angles.</returns>
    public static Angle operator -(Angle left, Angle right) => new(left.Radians - right.Radians);

    /// <summary>
    /// Negates an angle.
    /// </summary>
    /// <param name="value">The angle to negate.</param>
    /// <returns>The negated angle.</returns>
    public static Angle operator -(Angle value) => new(-value.Radians);

    /// <summary>
    /// Compares two angles are equal.
    /// </summary>
    /// <param name="left">The first angle.</param>
    /// <param name="right">The second angle.</param>
    /// <returns><see langword="true"/> if the angles are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Angle left, Angle right) => left.Equals(right);

    /// <summary>
    /// Compares two angles are not equal.
    /// </summary>
    /// <param name="left">The first angle.</param>
    /// <param name="right">The second angle.</param>
    /// <returns><see langword="true"/> if the angles are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Angle left, Angle right) => !(left == right);

    /// <summary>
    /// Creates an angle from degrees.
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <returns>The angle.</returns>
    public static Angle FromDegrees(float degrees) => new((float)(degrees * Math.PI / 180f));

    /// <summary>
    /// Creates an angle from radians.
    /// </summary>
    /// <param name="radians">The angle in radians.</param>
    /// <returns>The angle.</returns>
    public static Angle FromRadians(float radians) => new(radians);

    /// <summary>
    /// Normalizes the angle to a range.
    /// </summary>
    /// <remarks>
    /// By default, the range is from 0 to 360 degrees.
    /// </remarks>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The normalized angle.</returns>
    public Angle Normalize(float min = 0f, float max = 360f)
    {
        float range = max - min;
        return FromDegrees(((((Degrees - min) % range) + range) % range) + min);
    }

    /// <summary>
    /// Compares two angles are equal.
    /// </summary>
    /// <param name="other">The angle to compare.</param>
    /// <returns><see langword="true"/> if the angles are equal; otherwise, <see langword="false"/>.</returns>
    public readonly bool Equals(Angle other) => FloatingPoint.IsNearlyZero(Radians - other.Radians);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj)
        => obj is Angle angle && Equals(angle);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => Radians.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => $"{Degrees}°";
}
