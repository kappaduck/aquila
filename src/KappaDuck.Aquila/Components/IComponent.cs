// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Components;

/// <summary>
/// Represents an component that can be attached to a window.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Attaches the component to the window.
    /// </summary>
    /// <param name="window">The window handle.</param>
    internal void Attach(nint window);

    /// <summary>
    /// Detaches the component from the window.
    /// </summary>
    /// <param name="window">The window handle.</param>
    internal void Detach(nint window);
}
