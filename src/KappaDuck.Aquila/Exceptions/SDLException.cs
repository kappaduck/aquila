// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Exceptions;

/// <summary>
/// Represents an exception that is thrown when an SDL error occurs.
/// It uses <see cref="SDL.GetError"/> to get the error message
/// then use <see href="https://wiki.libsdl.org/SDL3/SDL_ClearError">SDL_ClearError</see> to clear the message.
/// </summary>
public sealed partial class SDLException : Exception
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

    [DoesNotReturn]
    internal static void Throw()
    {
        string message = SDL.GetError();
        SDL_ClearError();

        throw new SDLException(message);
    }

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_ClearError();
}
