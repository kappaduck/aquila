// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Graphics;

/// <summary>
/// Represents an object that can be drawn to a render target.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draws the object to the specified render target.
    /// </summary>
    /// <param name="target">The render target to draw the object to.</param>
    void Draw(IRenderTarget target);
}
