// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Graphics;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SDL_FColor
{
    internal SDL_FColor(byte r, byte g, byte b, byte a)
    {
        R = r / 255f;
        G = g / 255f;
        B = b / 255f;
        A = a / 255f;
    }

    public readonly float R;
    public readonly float G;
    public readonly float B;
    public readonly float A;
}
