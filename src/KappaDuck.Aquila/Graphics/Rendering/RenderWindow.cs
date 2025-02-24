// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics.Drawing;
using KappaDuck.Aquila.Graphics.Primitives;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using KappaDuck.Aquila.Video.Windows;
using System.Drawing;

namespace KappaDuck.Aquila.Graphics.Rendering;

/// <summary>
/// Represents a window that can be used to render 2D graphics using SDL Renderer API.
/// </summary>
public sealed class RenderWindow : BaseWindow, IRenderTarget
{
    private RendererHandle _renderer = RendererHandle.Zero;

    /// <summary>
    /// Creates an empty window.
    /// </summary>
    /// <remarks>
    /// Use <see cref="BaseWindow.Create(string, int, int, WindowState)"/> to create a window.
    /// </remarks>
    public RenderWindow()
    {
    }

    /// <summary>
    /// Creates a new window with the specified title, width, height, and state.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The state of the window.</param>
    public RenderWindow(string title, int width, int height, WindowState state = WindowState.None) : base(title, width, height, state)
    {
    }

    /// <summary>
    /// Gets or sets the blend mode for drawing operations (fill and line).
    /// </summary>
    /// <remarks>
    /// If the blend mode is not supported by the renderer, the closest supported mode is chosen.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the renderer blend mode.</exception>
    public BlendMode BlendMode
    {
        get;
        set
        {
            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SDL_SetRenderDrawBlendMode(_renderer, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets the clipping rectangle for rendering.
    /// </summary>
    /// <remarks>
    /// Setting the clipping rectangle to <see langword="null"/> will disable clipping.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the renderer clip.</exception>
    public Rectangle<int>? Clip
    {
        get;
        set
        {
            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SetRenderClip(_renderer, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets the color scale used by the renderer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The color scale is an additional scale multiplied into the pixel color value while rendering. This can be used
    /// to adjust the brightness of colors during HDR rendering, or changing HDR video brightness when playing on an SDR display.
    /// </para>
    /// <para>
    /// It does not affect the alpha channel, only the color brightness.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The value is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the renderer color scale.</exception>
    public float ColorScale
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SDL_SetRenderColorScale(_renderer, value))
                SDLException.Throw();
        }
    }

    = 1f;

    /// <summary>
    /// Gets the current output size in pixels of a rendering context.
    /// </summary>
    /// <remarks>
    /// If a rendering target is active, this will return the size of the rendering target in pixels,
    /// otherwise if a logical size is set, it will return the logical size,
    /// otherwise it will return the value of <see cref="OutputSize"/>.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the current renderer output size.</exception>
    public (int Width, int Height) CurrentOutputSize
    {
        get
        {
            if (_renderer.IsInvalid)
                return (0, 0);

            if (!SDLNative.SDL_GetCurrentRenderOutputSize(_renderer, out int w, out int h))
                SDLException.Throw();

            return (w, h);
        }
    }

    /// <summary>
    /// Gets or sets the color used for drawing operations.
    /// </summary>
    /// <remarks>
    /// Set the color used for drawing operations (rect, line, points, and clear).
    /// </remarks>
    public Color DrawColor
    {
        get;
        set
        {
            field = value;

            if (_renderer.IsInvalid)
                return;

            SDLNative.SDL_SetRenderDrawColor(_renderer, value.R, value.G, value.B, value.A);
        }
    }

    = Color.Black;

    /// <summary>
    /// Gets a value indicating whether the renderer has a clipping rectangle set.
    /// </summary>
    public bool HasClip => Clip.HasValue;

    /// <summary>
    /// Gets the output size in pixels of a rendering context.
    /// </summary>
    /// <remarks>
    /// It return the true output size in pixels, ignoring any render targets or logical size and presentation.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the renderer output size.</exception>
    public (int Width, int Height) OutputSize
    {
        get
        {
            if (_renderer.IsInvalid)
                return (0, 0);

            if (!SDLNative.SDL_GetRenderOutputSize(_renderer, out int w, out int h))
                SDLException.Throw();

            return (w, h);
        }
    }

    /// <summary>
    /// Gets or sets a device independent resolution and presentation mode for rendering.
    /// </summary>
    /// <remarks>
    /// It sets the width and height of the logical rendering output.
    /// The renderer will act as if the window is always the requested dimensions, scaling to the actual window resolution as necessary.
    /// This can be useful for games that expect a fixed size, but would like to scale the output to whatever is available,
    /// regardless of how a user resizes a window, or if the display is high DPI.
    /// You can disable logical coordinates by setting the mode to <see cref="LogicalPresentation.Disabled"/>,
    /// and in that case you get the full pixel resolution of the output window;
    /// it is safe to toggle logical presentation during the rendering of a frame: perhaps most of the rendering is done to specific dimensions
    /// but to make fonts look sharp, the app turns off logical presentation while drawing text.
    /// Letterboxing will only happen if logical presentation is enabled during <see cref="Render"/>; be sure to reenable it first if you were using it.
    /// You can convert coordinates in an event into rendering coordinates using <see cref="ConvertEventToCoordinates(SDLEvent)"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the logical presentation.</exception>
    public (int Width, int Height, LogicalPresentation Mode) Presentation
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SDL_SetRenderLogicalPresentation(_renderer, value.Width, value.Height, value.Mode))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the final presentation rectangle for rendering.
    /// </summary>
    /// <remarks>
    /// It returns the calculated rectangle used for logical presentation, based on the presentation
    /// mode and output size. If logical presentation is <see cref="LogicalPresentation.Disabled"/>, it will fill
    /// the rectangle with the output size, in pixels.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the renderer presentation rectangle.</exception>
    public Rectangle<float> PresentationRectangle
    {
        get
        {
            if (_renderer.IsInvalid)
                return default;

            if (!SDLNative.SDL_GetRenderLogicalPresentationRect(_renderer, out Rectangle<float> rectangle))
                SDLException.Throw();

            return rectangle;
        }
    }

    /// <summary>
    /// Gets the name of the rendering driver used by the renderer.
    /// </summary>
    /// <remarks>
    /// A list of available renderers can be obtained by calling <see cref="RendererDriver.GetAll"/>.
    /// </remarks>
    public string? RendererName
    {
        get
        {
            if (_renderer.IsInvalid)
                return null;

            return field ?? SDLNative.SDL_GetRendererName(_renderer);
        }
        init;
    }

    /// <summary>
    /// Gets the safe area for rendering within the current viewport.
    /// </summary>
    /// <remarks>
    /// Some devices have portions of the screen which are partially obscured or not interactive,
    /// possibly due to on-screen controls, curved edges, camera notches, TV overscan, etc.
    /// This function provides the area of the current viewport which is safe to have interactable content.
    /// You should continue rendering into the rest of the render target, but it should not contain visually important or interactable content.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the renderer safe area.</exception>
    public Rectangle<int> RenderingSafeArea
    {
        get
        {
            if (_renderer.IsInvalid)
                return default;

            if (!SDLNative.SDL_GetRenderSafeArea(_renderer, out Rectangle<int> rect))
                SDLException.Throw();

            return rect;
        }
    }

    /// <summary>
    /// Gets or sets the drawing scale for rendering on the current target.
    /// </summary>
    /// <remarks>
    /// The drawing coordinates are scaled by the x/y scaling factors before they are used by the renderer.
    /// This allows resolution independent drawing with a single coordinate system.
    /// If this results in scaling or subpixel drawing by the rendering backend,
    /// it will be handled using the appropriate quality hints.For best results use integer scaling factors.
    /// </remarks>
    public Point<float> Scale
    {
        get;
        set
        {
            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SDL_SetRenderScale(_renderer, value.X, value.Y))
                SDLException.Throw();
        }
    }

    = new Point<float>(1, 1);

    /// <summary>
    /// Gets or sets the vertical synchronization (VSync) of the renderer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When a renderer is created, VSync defaults to <see cref="VSync.Disabled"/> which means that VSync is disabled.
    /// </para>
    /// <para>
    /// The value can be 1 to synchronize present with every vertical refresh, 2 to synchronize present with every other vertical refresh, and so on.
    /// <see cref="VSync.Adaptive"/> can be used for adaptive VSync or <see cref="VSync.Disabled"/> to disable. Not every value is supported by every driver.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the renderer VSync.</exception>
    public int VSync
    {
        get;
        set
        {
            if (field == value)
                return;

            field = value;

            if (_renderer.IsInvalid)
                return;

            if (!SDLNative.SDL_SetRenderVSync(_renderer, value))
                SDLException.Throw();
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// The render target is cleared with a black color.
    /// If you want to clear the render target with a different color, use <see cref="Clear(Color)"/> instead.
    /// </remarks>
    public void Clear() => Clear(Color.Black);

    /// <inheritdoc/>
    public void Clear(Color color)
    {
        SDLNative.SDL_SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        SDLNative.SDL_RenderClear(_renderer);

        DrawColor = color;
    }

    /// <inheritdoc/>
    public void Draw(IDrawable drawable) => drawable.Draw(this);

    /// <inheritdoc/>
    public void Draw(ReadOnlySpan<IDrawable> drawable)
    {
        foreach (IDrawable item in drawable)
            item.Draw(this);
    }

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices)
        => SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, [], 0);

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Vertex> vertices, ReadOnlySpan<int> indices)
        => SDLNative.SDL_RenderGeometry(_renderer, nint.Zero, vertices, vertices.Length, indices, indices.Length);

    /// <inheritdoc/>
    public void Draw(Point<float> point, Color? color)
    {
        if (color.HasValue)
            SDLNative.SDL_SetRenderDrawColor(_renderer, color.Value.R, color.Value.G, color.Value.B, color.Value.A);

        SDLNative.SDL_RenderPoint(_renderer, point.X, point.Y);
    }

    /// <inheritdoc/>
    public void Draw(in ReadOnlySpan<Point<float>> points, Color? color)
    {
        if (color.HasValue)
            SDLNative.SDL_SetRenderDrawColor(_renderer, color.Value.R, color.Value.G, color.Value.B, color.Value.A);

        SDLNative.SDL_RenderPoints(_renderer, points, points.Length);
    }

    /// <summary>
    /// Renders all the graphics to the window since the last call.
    /// </summary>
    public void Render() => SDLNative.SDL_RenderPresent(_renderer);

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _renderer.Dispose();

        base.Dispose(disposing);
    }

    /// <inheritdoc/>
    protected override void OnWindowCreated(WindowHandle window)
    {
        _renderer = SDLNative.SDL_CreateRenderer(window, RendererName);

        SDLException.ThrowIf(_renderer.IsInvalid);

        SDLNative.SDL_SetRenderVSync(_renderer, VSync);
        SDLNative.SetRenderClip(_renderer, Clip);
        SDLNative.SDL_SetRenderDrawBlendMode(_renderer, BlendMode);
        SDLNative.SDL_SetRenderColorScale(_renderer, ColorScale);
        SDLNative.SDL_SetRenderDrawColor(_renderer, DrawColor.R, DrawColor.G, DrawColor.B, DrawColor.A);
        SDLNative.SDL_SetRenderLogicalPresentation(_renderer, Presentation.Width, Presentation.Height, Presentation.Mode);
        SDLNative.SDL_SetRenderScale(_renderer, Scale.X, Scale.Y);
    }
}
