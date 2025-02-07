// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.Marshallers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila;

internal static partial class SDLProperties
{
    internal static bool Get(uint propertiesId, string name, bool defaultValue)
        => SDL_GetBooleanProperty(propertiesId, name, defaultValue) != 0;

    internal static float Get(uint propertiesId, string name, float defaultValue)
        => SDL_GetFloatProperty(propertiesId, name, defaultValue);

    internal static long Get(uint propertiesId, string name, long defaultValue)
        => SDL_GetNumberProperty(propertiesId, name, long.CreateChecked(defaultValue));

    internal static string Get(uint propertiesId, string name, string defaultValue)
        => SDL_GetStringProperty(propertiesId, name, defaultValue);

    internal static void Set<T>(uint propertiesId, string name, T value)
    {
        byte result = value switch
        {
            string s => SDL_SetStringProperty(propertiesId, name, s),
            float f => SDL_SetFloatProperty(propertiesId, name, f),
            bool b => SDL_SetBooleanProperty(propertiesId, name, b),
            long l => SDL_SetNumberProperty(propertiesId, name, l),
            _ => throw new NotSupportedException($"The type {typeof(T)} is not supported.")
        };

        if (result == 0)
            SDLException.Throw();
    }

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_GetBooleanProperty(uint propertiesId, string name, [MarshalAs(UnmanagedType.Bool)] bool defaultValue);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial long SDL_GetNumberProperty(uint propertiesId, string name, long defaultValue);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial float SDL_GetFloatProperty(uint propertiesId, string name, float defaultValue);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(OwnedStringMarshaller))]
    private static partial string SDL_GetStringProperty(uint propertiesId, string name, string defaultValue);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_SetBooleanProperty(uint propertiesId, string name, [MarshalAs(UnmanagedType.Bool)] bool value);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_SetNumberProperty(uint propertiesId, string name, long value);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_SetFloatProperty(uint propertiesId, string name, float value);

    [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_SetStringProperty(uint propertiesId, string name, string value);
}
