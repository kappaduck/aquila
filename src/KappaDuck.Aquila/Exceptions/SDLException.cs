// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Interop;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace KappaDuck.Aquila.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an SDL error occurs.
/// It uses <see href="https://wiki.libsdl.org/SDL3/SDL_GetError">SDL_GetError</see> to get the error message
/// then use <see href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</see> to clear the message.
/// </summary>
public sealed class SDLException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class.
    /// </summary>
    public SDLException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    public SDLException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SDLException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message from SDL or a custom message.</param>
    /// <param name="innerException">The inner exception.</param>
    public SDLException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition)
    {
        if (condition)
            Throw();
    }

    internal static void ThrowIfZero<T>(T value) where T : INumber<T>
    {
        if (T.IsZero(value))
            Throw();
    }

    internal static void ThrowIfNullOrEmpty([NotNull] string? value)
    {
        if (string.IsNullOrEmpty(value))
            Throw();
    }

    [DoesNotReturn]
    internal static void Throw()
    {
        string message = NativeMethods.SDL_GetError();
        NativeMethods.SDL_ClearError();

        throw new SDLException(message);
    }
}
