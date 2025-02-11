// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Inputs;

/// <summary>
/// A mouse input.
/// </summary>
public static class Mouse
{
    /// <summary>
    /// Represents a mouse button.
    /// </summary>
    public enum Button : byte
    {
        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left = 1,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle = 2,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right = 3,

        /// <summary>
        /// The first extra mouse button.
        /// </summary>
        /// <remarks>
        /// Generally is the side mouse button.
        /// </remarks>
        X1 = 4,

        /// <summary>
        /// The second extra mouse button.
        /// </summary>
        /// <remarks>
        /// Generally is the side mouse button.
        /// </remarks>
        X2 = 5
    }

    /// <summary>
    /// Represents the state of the mouse buttons.
    /// </summary>
    [Flags]
    public enum ButtonState : uint
    {
        /// <summary>
        /// No mouse button.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left = 0x1,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle = 0x2,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right = 0x4,

        /// <summary>
        /// The first extra mouse button.
        /// </summary>
        /// <remarks>
        /// Generally is the side mouse button.
        /// </remarks>
        X1 = 0x08,

        /// <summary>
        /// The second extra mouse button.
        /// </summary>
        /// <remarks>
        /// Generally is the side mouse button.
        /// </remarks>
        X2 = 0x10
    }

    /// <summary>
    /// Represents a mouse wheel direction.
    /// </summary>
    public enum WheelDirection
    {
        /// <summary>
        /// The scroll direction is normal.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The scroll direction is flipped.
        /// </summary>
        Flipped = 1
    }
}
