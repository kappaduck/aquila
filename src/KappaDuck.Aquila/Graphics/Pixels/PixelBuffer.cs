// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using System.Buffers;

namespace KappaDuck.Aquila.Graphics.Pixels;

/// <summary>
/// Represents a low-level pixel buffer, stored in CPU memory, that can be used to manipulate pixels.
/// </summary>
/// <remarks>
/// <para>
/// It is your responsibility to manipulate the pixel buffer correctly with the correct pixel format.
/// </para>
/// <para>
/// The buffer is created by doing <see cref="Width"/> * <see cref="Height"/> * bytes per pixel.
/// </para>
/// </remarks>
/// <typeparam name="T">The type of the pixel buffer.</typeparam>
public sealed class PixelBuffer<T> : IDisposable where T : unmanaged
{
    private readonly int _bytesPerPixel;
    private readonly Memory<T> _pixels;

    /// <summary>
    /// Create a buffer with the specified width, height, and format.
    /// </summary>
    /// <param name="width">The width of the pixel buffer.</param>
    /// <param name="height">The height of the pixel buffer.</param>
    /// <param name="format">The format of the pixel buffer.</param>
    public unsafe PixelBuffer(int width, int height, PixelFormat format)
    {
        int byteCount = GetByteCountFromFormat(format);
        _bytesPerPixel = byteCount / sizeof(T);

        Pitch = width * byteCount;
        _pixels = new T[width * height * _bytesPerPixel];

        using (MemoryHandle handle = _pixels.Pin())
        {
            Handle = SDLNative.SDL_CreateSurfaceFrom(width, height, format, handle.Pointer, Pitch);
        }

        Width = width;
        Height = height;
        Format = format;
    }

    /// <summary>
    /// Gets or sets the a byte from the pixel at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the pixel.</param>
    /// <param name="y">The y-coordinate of the pixel.</param>
    /// <param name="index">The index of the pixel.</param>
    /// <returns>The byte from the pixel at the specified position.</returns>
    public T this[int x, int y, int index = 0]
    {
        get
        {
            int pixel = ((y * Width) + x) * _bytesPerPixel;
            return _pixels.Span[pixel + index];
        }

        set
        {
            int pixel = ((y * Width) + x) * _bytesPerPixel;
            _pixels.Span[pixel + index] = value;
        }
    }

    /// <summary>
    /// Gets the format of the pixel buffer.
    /// </summary>
    public PixelFormat Format { get; }

    /// <summary>
    /// Gets the height of the pixel buffer.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the pitch of the pixel buffer.
    /// </summary>
    public int Pitch { get; }

    /// <summary>
    /// Gets the width of the pixel buffer.
    /// </summary>
    public int Width { get; }

    internal SurfaceHandle Handle { get; }

    /// <inheritdoc/>
    public void Dispose() => Handle.Dispose();

    private static int GetByteCountFromFormat(PixelFormat format)
    {
        return format switch
        {
            PixelFormat.Index1LSB or PixelFormat.Index1MSB => 1,
            PixelFormat.Index4LSB or PixelFormat.Index4MSB => 1,
            PixelFormat.Index8 or PixelFormat.RGB332 => 1,

            PixelFormat.XRGB4444 or PixelFormat.XRGB1555 or PixelFormat.RGB565
            or PixelFormat.ARGB4444 or PixelFormat.ARGB1555 or PixelFormat.RGBA4444
            or PixelFormat.RGBA5551 or PixelFormat.XBGR4444 or PixelFormat.XBGR1555
            or PixelFormat.BGR565 or PixelFormat.ABGR4444 or PixelFormat.ABGR1555
            or PixelFormat.BGRA4444 or PixelFormat.BGRA5551 => 2,

            PixelFormat.RGB24 or PixelFormat.BGR24 => 3,

            PixelFormat.XRGB8888 or PixelFormat.RGBX8888 or PixelFormat.ARGB8888
            or PixelFormat.RGBA8888 or PixelFormat.RGBX32 or PixelFormat.XRGB32
            or PixelFormat.RGBA32 or PixelFormat.ARGB32 or PixelFormat.XRGB2101010
            or PixelFormat.ARGB2101010 or PixelFormat.XBGR2101010 or PixelFormat.ABGR2101010 => 4,

            PixelFormat.RGB48 or PixelFormat.BGR48 => 6,

            PixelFormat.RGBA64 or PixelFormat.ARGB64 or PixelFormat.BGRA64 or PixelFormat.ABGR64 => 8,

            PixelFormat.RGB96Float or PixelFormat.BGR96Float => 12,

            PixelFormat.RGBA128Float or PixelFormat.ARGB128Float or PixelFormat.BGRA128Float
            or PixelFormat.ABGR128Float => 16,

            _ => throw new NotSupportedException($"Unsupported pixel format: {format}")
        };
    }
}
