// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Geometry;

internal static class FloatingPoint
{
    /// <summary>
    /// The machine epsilon for <see cref="float"/>. It is based on C++'s FLT_EPSILON.
    /// </summary>
    internal const float MachineEpsilon = 1.192092896e-07f;

    internal static bool IsNearlyZero(float value) => MathF.Abs(value) < MachineEpsilon;
}
