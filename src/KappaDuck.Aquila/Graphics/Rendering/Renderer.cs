// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics.Rendering;

/// <summary>
/// The 2D rendering engine.
/// </summary>
/// <remarks>
/// Based on <see href="https://wiki.libsdl.org/SDL3/CategoryRender">SDL 2D rendering</see>.
/// </remarks>
public sealed class Renderer : IRenderEngine
{
    private RendererHandle _handle = RendererHandle.Zero;

    /// <inheritdoc/>
    public void Clear(Color color)
    {
        SDLNative.SDL_SetRenderDrawColor(_handle, color.R, color.G, color.B, color.A);
        SDLNative.SDL_RenderClear(_handle);
    }

    /// <inheritdoc/>
    public void Dispose() => _handle.Dispose();

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices)
        => SDLNative.SDL_RenderGeometry(_handle, nint.Zero, vertices, vertices.Length, [], 0);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices)
        => SDLNative.SDL_RenderGeometry(_handle, nint.Zero, vertices, vertices.Length, indices, indices.Length);

    /// <inheritdoc/>
    public void Render() => SDLNative.SDL_RenderPresent(_handle);

    /// <inheritdoc/>
    void IRenderEngine.Setup(WindowHandle window) => _handle = SDLNative.SDL_CreateRenderer(window);
}
