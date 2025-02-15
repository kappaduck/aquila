// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using Win32 = System.ComponentModel;

namespace KappaDuck.Aquila.Exceptions;

/// <summary>
/// The exception that is thrown when a call to a Win32 function fails.
/// </summary>
public sealed class Win32Exception : Win32.Win32Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Exception"/> class.
    /// </summary>
    public Win32Exception()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Exception"/> class with the specified error.
    /// </summary>
    /// <param name="error">The Win32 error code.</param>
    public Win32Exception(int error) : base(error)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Exception"/> class with the specified error and message.
    /// </summary>
    /// <param name="error">The Win32 error code.</param>
    /// <param name="message">The error message.</param>
    public Win32Exception(int error, string? message) : base(error, message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Exception"/> class with the specified message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public Win32Exception(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Win32Exception"/> class with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public Win32Exception(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    [DoesNotReturn]
    internal static void Throw(string? message = null) => throw new Win32Exception(message);

    internal static void ThrowIf([DoesNotReturnIf(true)] bool condition, string? message = null)
    {
        if (condition)
            throw new Win32Exception(message);
    }
}
