// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Rendering;

namespace KappaDuck.Aquila.Graphics.Drawing;

/// <summary>
/// Represents an object that self-draws to a render target.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Draws the object to the specified render target.
    /// </summary>
    /// <param name="target">The render target to draw the object to.</param>
    /// <param name="state">The render state to use when drawing the object.</param>
    void Draw(IRenderTarget target, RenderState state);
}
