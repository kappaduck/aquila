// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop;

namespace KappaDuck.Aquila.System;

internal sealed class Properties : IDisposable
{
    private readonly uint _id;

    internal Properties()
    {
        _id = NativeMethods.SDL_CreateProperties();

        SDLException.ThrowIfZero(_id);
    }

    public void Dispose() => NativeMethods.SDL_DestroyProperties(_id);

    internal T Get<T>(string name, T defaultValue) => Get(_id, name, defaultValue);

    internal void Set<T>(string name, T value) => Set(_id, name, value);

    internal static T Get<T>(uint propertiesId, string name, T defaultValue)
    {
        return defaultValue switch
        {
            bool boolean => (T)(object)NativeMethods.SDL_GetBooleanProperty(propertiesId, name, boolean),
            float floating => (T)(object)NativeMethods.SDL_GetFloatProperty(propertiesId, name, floating),
            long number => (T)(object)NativeMethods.SDL_GetNumberProperty(propertiesId, name, number),
            string str => (T)(object)NativeMethods.SDL_GetStringProperty(propertiesId, name, str),
            _ => throw new NotSupportedException($"The type {typeof(T)} is not supported.")
        };
    }

    internal static void Set<T>(uint propertiesId, string name, T value)
    {
        bool isSet = value switch
        {
            bool boolean => NativeMethods.SDL_SetBooleanProperty(propertiesId, name, boolean),
            float floating => NativeMethods.SDL_SetFloatProperty(propertiesId, name, floating),
            long number => NativeMethods.SDL_SetNumberProperty(propertiesId, name, number),
            string str => NativeMethods.SDL_SetStringProperty(propertiesId, name, str),
            _ => throw new NotSupportedException($"The type {typeof(T)} is not supported.")
        };

        SDLException.ThrowIf(!isSet);
    }
}
