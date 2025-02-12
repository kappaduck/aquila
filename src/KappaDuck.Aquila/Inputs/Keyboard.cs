// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop;

namespace KappaDuck.Aquila.Inputs;

/// <summary>
/// A keyboard input.
/// </summary>
public sealed partial class Keyboard
{
    internal Keyboard(uint id)
    {
        Id = id;
        Name = NativeMethods.SDL_GetKeyboardNameForID(id);
    }

    /// <summary>
    /// Gets the instance id of the mouse.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Gets the name of the mouse.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Get a list of currently connected keyboards.
    /// </summary>
    /// <remarks>
    /// This will include any device or virtual driver that includes keyboard functionality,
    /// including some mice, KVM switches, motherboard power buttons, etc. You should wait for input from a device
    /// before you consider it actively in use.
    /// </remarks>
    /// <returns>The list of connected keyboards.</returns>
    public static Keyboard[] GetKeyboards()
    {
        return [];
        //Span<uint> ids = NativeMethods.SDL_GetKeyboards(out _);

        //if (ids.IsEmpty)
        //    return [];

        //Keyboard[] keyboards = new Keyboard[ids.Length];

        //for (int i = 0; i < ids.Length; i++)
        //    keyboards[i] = new Keyboard(ids[i]);

        //return keyboards;
    }
}

// https://wiki.libsdl.org/SDL3/SDL_GetKeyboardNameForID
// https://wiki.libsdl.org/SDL3/SDL_GetKeyboards
// https://wiki.libsdl.org/SDL3/SDL_GetKeyboardState
// https://wiki.libsdl.org/SDL3/SDL_GetKeyFromName
// https://wiki.libsdl.org/SDL3/SDL_GetKeyFromScancode
// https://wiki.libsdl.org/SDL3/SDL_GetKeyName
// https://wiki.libsdl.org/SDL3/SDL_GetModState
// https://wiki.libsdl.org/SDL3/SDL_GetScancodeFromKey
// https://wiki.libsdl.org/SDL3/SDL_GetScancodeFromName
// https://wiki.libsdl.org/SDL3/SDL_GetScancodeName
// https://wiki.libsdl.org/SDL3/SDL_HasKeyboard
// https://wiki.libsdl.org/SDL3/SDL_HasScreenKeyboardSupport
// https://wiki.libsdl.org/SDL3/SDL_ResetKeyboard
// https://wiki.libsdl.org/SDL3/SDL_SetModState
// https://wiki.libsdl.org/SDL3/SDL_SetScancodeName

// https://wiki.libsdl.org/SDL3/SDL_ScreenKeyboardShown
