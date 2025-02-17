// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Drawing;

namespace KappaDuck.Aquila.Graphics;

/// <summary>
/// Represents a render target that can be used to render graphics.
/// </summary>
public interface IRenderTarget
{
    /// <summary>
    /// Clears the render target.
    /// </summary>
    void Clear();

    /// <summary>
    /// Clears the render target with the specified color.
    /// </summary>
    /// <param name="color">The color to clear the render target with.</param>
    void Clear(Color color);
}
