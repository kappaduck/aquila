// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Geometry;

internal static class Math
{
    /// <summary>
    /// The machine epsilon for <see cref="float"/>. SDL3 uses it.
    /// </summary>
    internal const float Epsilon = 1.192092896e-07f;

    internal static bool Approximately(float a, float b) => float.Abs(a - b) < Epsilon;
}
