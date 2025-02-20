// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Graphics.Pixels;

/// <summary>
/// Represents the format of a pixel.
/// </summary>
public enum PixelFormat : uint
{
    /// <summary>
    /// Unknown pixel format.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// 1LSB pixel format.
    /// </summary>
    Index1LSB = 0x11100100u,

    /// <summary>
    /// 1MSB pixel format.
    /// </summary>
    Index1MSB = 0x11200100u,

    /// <summary>
    /// 4LSB pixel format.
    /// </summary>
    Index4LSB = 0x12100400u,

    /// <summary>
    /// 4MSB pixel format.
    /// </summary>
    Index4MSB = 0x12200400u,

    /// <summary>
    /// 8-bit pixel format.
    /// </summary>
    Index8 = 0x13000801u,

    /// <summary>
    /// RGB332 pixel format.
    /// </summary>
    RGB332 = 0x14110801u,

    /// <summary>
    /// RGB444 pixel format.
    /// </summary>
    XRGB4444 = 0x15120c02u,

    /// <summary>
    /// XRGB1555 pixel format.
    /// </summary>
    XRGB1555 = 0x15130f02u,

    /// <summary>
    /// RGB565 pixel format.
    /// </summary>
    RGB565 = 0x15151002u,

    /// <summary>
    /// ARGB4444 pixel format.
    /// </summary>
    ARGB4444 = 0x15321002u,

    /// <summary>
    /// ARGB1555 pixel format.
    /// </summary>
    ARGB1555 = 0x15331002u,

    /// <summary>
    /// RGBA4444 pixel format.
    /// </summary>
    RGBA4444 = 0x15421002u,

    /// <summary>
    /// RGBA5551 pixel format.
    /// </summary>
    RGBA5551 = 0x15441002u,

    /// <summary>
    /// BGR444 pixel format.
    /// </summary>
    XBGR4444 = 0x15520c02u,

    /// <summary>
    /// XBGR1555 pixel format.
    /// </summary>
    XBGR1555 = 0x15530f02u,

    /// <summary>
    /// BGR565 pixel format.
    /// </summary>
    BGR565 = 0x15551002u,

    /// <summary>
    /// ABGR4444 pixel format.
    /// </summary>
    ABGR4444 = 0x15721002u,

    /// <summary>
    /// ABGR1555 pixel format.
    /// </summary>
    ABGR1555 = 0x15731002u,

    /// <summary>
    /// BGRA4444 pixel format.
    /// </summary>
    BGRA4444 = 0x15821002u,

    /// <summary>
    /// BGRA5551 pixel format.
    /// </summary>
    BGRA5551 = 0x15841002u,

    /// <summary>
    /// XRGB8888 pixel format.
    /// </summary>
    XRGB8888 = 0x16161804u,

    /// <summary>
    /// Alias for <see cref="XRGB8888"/>.
    /// </summary>
    XRGB32 = XRGB8888,

    /// <summary>
    /// XRGB2101010 pixel format.
    /// </summary>
    XRGB2101010 = 0x16172004u,

    /// <summary>
    /// RGBX8888 pixel format.
    /// </summary>
    RGBX8888 = 0x16261804u,

    /// <summary>
    /// Alias for <see cref="RGBX8888"/>.
    /// </summary>
    RGBX32 = RGBX8888,

    /// <summary>
    /// ARGB8888 pixel format.
    /// </summary>
    ARGB8888 = 0x16362004u,

    /// <summary>
    /// Alias for <see cref="ARGB8888"/>.
    /// </summary>
    ARGB32 = ARGB8888,

    /// <summary>
    /// ARGB2101010 pixel format.
    /// </summary>
    ARGB2101010 = 0x16372004u,

    /// <summary>
    /// RGBA8888 pixel format.
    /// </summary>
    RGBA8888 = 0x16462004u,

    /// <summary>
    /// Alias for <see cref="RGBA8888"/>.
    /// </summary>
    RGBA32 = RGBA8888,

    /// <summary>
    /// XBGR8888 pixel format.
    /// </summary>
    XBGR8888 = 0x16561804u,

    /// <summary>
    /// Alias for <see cref="XBGR8888"/>.
    /// </summary>
    XBGR32 = XBGR8888,

    /// <summary>
    /// XBGR2101010 pixel format.
    /// </summary>
    XBGR2101010 = 0x16572004u,

    /// <summary>
    /// BGRX8888 pixel format.
    /// </summary>
    BGRX8888 = 0x16661804u,

    /// <summary>
    /// Alias for <see cref="BGRX8888"/>.
    /// </summary>
    BGRX32 = BGRX8888,

    /// <summary>
    /// ABGR8888 pixel format.
    /// </summary>
    ABGR8888 = 0x16762004u,

    /// <summary>
    /// Alias for <see cref="ABGR8888"/>.
    /// </summary>
    ABGR32 = ABGR8888,

    /// <summary>
    /// ABGR2101010 pixel format.
    /// </summary>
    ABGR2101010 = 0x16772004u,

    /// <summary>
    /// BGRA8888 pixel format.
    /// </summary>
    BGRA8888 = 0x16862004u,

    /// <summary>
    /// Alias for <see cref="BGRA8888"/>.
    /// </summary>
    BGRA32 = BGRA8888,

    /// <summary>
    /// RGB24 pixel format.
    /// </summary>
    RGB24 = 0x17101803u,

    /// <summary>
    /// BGR24 pixel format.
    /// </summary>
    BGR24 = 0x17401803u,

    /// <summary>
    /// RGB48 pixel format.
    /// </summary>
    RGB48 = 0x18103006u,

    /// <summary>
    /// RGBA64 pixel format.
    /// </summary>
    RGBA64 = 0x18204008u,

    /// <summary>
    /// ARGB64 pixel format.
    /// </summary>
    ARGB64 = 0x18304008u,

    /// <summary>
    /// BGR48 pixel format.
    /// </summary>
    BGR48 = 0x18403006u,

    /// <summary>
    /// BGRA64 pixel format.
    /// </summary>
    BGRA64 = 0x18504008u,

    /// <summary>
    /// ABGR64 pixel format.
    /// </summary>
    ABGR64 = 0x18604008u,

    /// <summary>
    /// RGB48Float pixel format.
    /// </summary>
    RGB48Float = 0x1a103006u,

    /// <summary>
    /// RGBA64Float pixel format.
    /// </summary>
    RGBA64Float = 0x1a204008u,

    /// <summary>
    /// ARGB64Float pixel format.
    /// </summary>
    ARGB64Float = 0x1a304008u,

    /// <summary>
    /// BGR48Float pixel format.
    /// </summary>
    BGR48Float = 0x1a403006u,

    /// <summary>
    /// BGRA64Float pixel format.
    /// </summary>
    BGRA64Float = 0x1a504008u,

    /// <summary>
    /// ABGR64Float pixel format.
    /// </summary>
    ABGR64Float = 0x1a604008u,

    /// <summary>
    /// RGB96Float pixel format.
    /// </summary>
    RGB96Float = 0x1b10600cu,

    /// <summary>
    /// RGBA128Float pixel format.
    /// </summary>
    RGBA128Float = 0x1b208010u,

    /// <summary>
    /// ARGB128Float pixel format.
    /// </summary>
    ARGB128Float = 0x1b308010u,

    /// <summary>
    /// BGR96Float pixel format.
    /// </summary>
    BGR96Float = 0x1b40600cu,

    /// <summary>
    /// BGRA128Float pixel format.
    /// </summary>
    BGRA128Float = 0x1b508010u,

    /// <summary>
    /// ABGR128Float pixel format.
    /// </summary>
    ABGR128Float = 0x1b608010u,

    /// <summary>
    /// 2LSB pixel format.
    /// </summary>
    Index2LSB = 0x1c100200u,

    /// <summary>
    /// 2MSB pixel format.
    /// </summary>
    Index2MSB = 0x1c200200u,

    /// <summary>
    /// External OES pixel format.
    /// </summary>
    ExternalOes = 0x2053454fu,

    /// <summary>
    /// P010 pixel format.
    /// </summary>
    P010 = 0x30313050u,

    /// <summary>
    /// NV21 pixel format.
    /// </summary>
    NV21 = 0x3132564eu,

    /// <summary>
    /// NV12 pixel format.
    /// </summary>
    NV12 = 0x3231564eu,

    /// <summary>
    /// YV12 pixel format.
    /// </summary>
    YV12 = 0x32315659u,

    /// <summary>
    /// YUY2 pixel format.
    /// </summary>
    YUY2 = 0x32595559u,

    /// <summary>
    /// YVYU pixel format.
    /// </summary>
    YVYU = 0x55595659u,

    /// <summary>
    /// IYUV pixel format.
    /// </summary>
    IYUV = 0x56555949u,

    /// <summary>
    /// UYVY pixel format.
    /// </summary>
    UYVY = 0x59565955u
}
