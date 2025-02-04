// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Numerics;

namespace KappaDuck.Aquila.Geometry.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Point{T}"/>.
/// </summary>
public static class PointExtensions
{
    /// <summary>
    /// Checks whether a point resides in a rectangle.
    /// </summary>
    /// <remarks>
    /// A point is considered part of a rectangle if both are not empty, and <paramref name="point"/>.X and <paramref name="point"/>.Y
    /// coordinates are >= <paramref name="rectangle"/>'s top left corner, and &lt; the <paramref name="rectangle"/>'s x + width and y + height.
    /// So a 1x1 rectangle considers point (0, 0) as "inside" and (0, 1) as not.
    /// </remarks>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="point">Point to check.</param>
    /// <param name="rectangle">Rectangle to check.</param>
    /// <returns><see langword="true"/> if the point resides in the rectangle, otherwise <see langword="false"/>.</returns>
    public static bool Reside<T>(this Point<T> point, Rectangle<T> rectangle) where T : INumber<T>
    {
        if (rectangle.IsEmpty)
            return false;

        return point.X >= rectangle.X && point.X < rectangle.X + rectangle.Width &&
               point.Y >= rectangle.Y && point.Y < rectangle.Y + rectangle.Height;
    }
}
