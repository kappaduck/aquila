// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Interop.SDL;

internal static partial class SDLNative
{
    internal static int Peek(Span<SDLEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int peekedEvents = SDL_PeepEvents(events, events.Length, EventAction.Peek, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(peekedEvents);

        return peekedEvents;
    }

    internal static int Push(Span<SDLEvent> events)
    {
        int added = SDL_PeepEvents(events, events.Length, EventAction.Add, EventType.None, EventType.LastEvent);

        SDLException.ThrowIfNegative(added);

        return added;
    }

    internal static int Retrieve(Span<SDLEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int retrievedEvents = SDL_PeepEvents(events, events.Length, EventAction.Get, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(retrievedEvents);

        return retrievedEvents;
    }

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_EventEnabled(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_FlushEvent(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_FlushEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_HasEvent(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_HasEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_PeepEvents(Span<SDLEvent> events, int count, EventAction action, EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_PollEvent(out SDLEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_PumpEvents();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_PushEvent(ref SDLEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_SetEventEnabled(EventType type, [MarshalAs(UnmanagedType.Bool)] bool enabled);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_WaitEvent(out SDLEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool SDL_WaitEventTimeout(out SDLEvent e, int timeout);

    private static void ThrowIfGreaterThan([DoesNotReturnIf(true)] bool condition, string paramName)
    {
        if (condition)
            throw new ArgumentOutOfRangeException(paramName, "minType must be less than or equal to maxType.");
    }

    private enum EventAction
    {
        Add = 0,
        Peek = 1,
        Get = 2,
    }
}
