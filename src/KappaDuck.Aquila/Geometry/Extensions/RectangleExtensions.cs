// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Numerics;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Geometry.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Rectangle{T}"/>.
/// </summary>
public static class RectangleExtensions
{
    /// <summary>
    /// Checks whether a rectangle encloses a set of points.
    /// </summary>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="clip">The clipped rectangle to test if any points are inside.</param>
    /// <param name="points">points to test.</param>
    /// <returns><see langword="true"/> if the rectangle encloses any of theses points, otherwise <see langword="false"/>.</returns>
    public static bool Encloses<T>(this Rectangle<T> clip, ReadOnlySpan<Point<T>> points) where T : INumber<T>
    {
        if (clip.IsEmpty || points.IsEmpty)
            return false;

        T epsilon = Epsilon<T>();

        T right = clip.X + clip.Width - epsilon;
        T bottom = clip.Y + clip.Height - epsilon;

        for (int i = 0; i < points.Length; i++)
        {
            if (Contains(points[i], clip.X, clip.Y, right, bottom))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether a rectangle encloses a set of points.
    /// </summary>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="clip">The clipped rectangle to test if any points are inside.</param>
    /// <param name="points">points to test.</param>
    /// <returns><see langword="true"/> if the rectangle encloses any of theses points, otherwise <see langword="false"/>.</returns>
    public static bool Encloses<T>(this Rectangle<T> clip, IEnumerable<Point<T>> points) where T : INumber<T>
    {
        if (clip.IsEmpty)
            return false;

        if (points is Point<T>[] array)
            Encloses(clip, array);

        if (points is List<Point<T>> list)
            Encloses(clip, CollectionsMarshal.AsSpan(list));

        T epsilon = Epsilon<T>();

        T right = clip.X + clip.Width - epsilon;
        T bottom = clip.Y + clip.Height - epsilon;

        using IEnumerator<Point<T>> enumerator = points.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (Contains(enumerator.Current, clip.X, clip.Y, right, bottom))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether two rectangles intersect.
    /// </summary>
    /// <remarks>
    /// If one of the rectangles is empty, the method returns <see langword="false"/>.
    /// </remarks>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="left">Left rectangle.</param>
    /// <param name="right">Right rectangle.</param>
    /// <returns><see langword="true"/> if the rectangles intersect, otherwise <see langword="false"/>.</returns>
    public static bool HasIntersection<T>(this Rectangle<T> left, Rectangle<T> right) where T : INumber<T>
    {
        if (left.IsEmpty || right.IsEmpty)
            return false;

        T epsilon = Epsilon<T>();

        return left.X + epsilon < right.X + right.Width
            && left.X + left.Width - epsilon > right.X
            && left.Y + epsilon < right.Y + right.Height
            && left.Y + left.Height - epsilon > right.Y;
    }

    /// <summary>
    /// Calculates the intersection of two rectangles.
    /// </summary>
    /// <remarks>
    /// If both rectangles are empty, the method returns an empty rectangle.
    /// </remarks>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="left">Left rectangle.</param>
    /// <param name="right">Right rectangle.</param>
    /// <returns>The intersections of two rectangles or an empty rectangle if there is no intersection.</returns>
    public static Rectangle<T> Intersects<T>(this Rectangle<T> left, Rectangle<T> right) where T : INumber<T>
    {
        if (left.IsEmpty || right.IsEmpty)
            return default;

        T x = T.Max(left.X, right.X);
        T y = T.Max(left.Y, right.Y);
        T width = T.Min(left.X + left.Width, right.X + right.Width) - x;
        T height = T.Min(left.Y + left.Height, right.Y + right.Height) - y;

        return new Rectangle<T>
        {
            X = x,
            Y = y,
            Width = width,
            Height = height
        };
    }

    /// <summary>
    /// Converts a <see cref="int"/> rectangle to a <see cref="float"/> rectangle.
    /// </summary>
    /// <remarks>
    /// If the rectangle is empty, it returns an empty rectangle.
    /// </remarks>
    /// <param name="rectangle">The rectangle to convert.</param>
    /// <returns>A converted rectangle.</returns>
    public static Rectangle<float> ToFloat(this Rectangle<int> rectangle)
    {
        if (rectangle.IsEmpty)
            return default;

        return new Rectangle<float>
        {
            X = float.CreateChecked(rectangle.X),
            Y = float.CreateChecked(rectangle.Y),
            Width = float.CreateChecked(rectangle.Width),
            Height = float.CreateChecked(rectangle.Height)
        };
    }

    /// <summary>
    /// Converts a <see cref="float"/> rectangle to a <see cref="int"/> rectangle.
    /// </summary>
    /// <remarks>
    /// If the rectangle is empty, it returns an empty rectangle.
    /// </remarks>
    /// <param name="rectangle">The rectangle to convert.</param>
    /// <returns>A converted rectangle.</returns>
    public static Rectangle<int> ToInt(this Rectangle<float> rectangle)
    {
        if (rectangle.IsEmpty)
            return default;

        return new Rectangle<int>
        {
            X = int.CreateChecked(rectangle.X),
            Y = int.CreateChecked(rectangle.Y),
            Width = int.CreateChecked(rectangle.Width),
            Height = int.CreateChecked(rectangle.Height)
        };
    }

    /// <summary>
    /// Calculates the union of two rectangles.
    /// </summary>
    /// <remarks>
    /// If both rectangles are empty, it returns an empty rectangle.
    /// If one of the rectangles is empty, it returns the other rectangle.
    /// </remarks>
    /// <typeparam name="T"><see cref="int"/> or <see cref="float"/>.</typeparam>
    /// <param name="left">Left rectangle.</param>
    /// <param name="right">Right rectangle.</param>
    /// <returns>The union of two rectangles.</returns>
    public static Rectangle<T> Union<T>(this Rectangle<T> left, Rectangle<T> right) where T : INumber<T>
    {
        if (left.IsEmpty && right.IsEmpty)
            return default;

        if (left.IsEmpty)
            return right;

        if (right.IsEmpty)
            return left;

        T x = T.Min(left.X, right.X);
        T y = T.Min(left.Y, right.Y);

        T width = T.Max(left.X + left.Width, right.X + right.Width) - x;
        T height = T.Max(left.Y + left.Height, right.Y + right.Height) - y;

        return new Rectangle<T>
        {
            X = x,
            Y = y,
            Width = width,
            Height = height
        };
    }

    private static T Epsilon<T>() where T : INumber<T>
        => typeof(T) == typeof(int) ? T.One : T.Zero;

    private static bool Contains<T>(Point<T> point, T x, T y, T right, T bottom) where T : INumber<T>
        => point.X >= x && point.X <= right && point.Y >= y && point.Y <= bottom;
}
