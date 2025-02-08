// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.Marshallers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop;

internal static partial class NativeMethods
{
    internal static int Peek(Span<SdlEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int peekedEvents = SDL_PeepEvents(events, events.Length, EventAction.Peek, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(peekedEvents);

        return peekedEvents;
    }

    internal static int Push(Span<SdlEvent> events)
    {
        int added = SDL_PeepEvents(events, events.Length, EventAction.Add, EventType.None, EventType.LastEvent);

        SDLException.ThrowIfNegative(added);

        return added;
    }

    internal static int Retrieve(Span<SdlEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int retrievedEvents = SDL_PeepEvents(events, events.Length, EventAction.Get, minType, maxType ?? minType);

        SDLException.ThrowIfNegative(retrievedEvents);

        return retrievedEvents;
    }

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_EventEnabled(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_FlushEvent(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_FlushEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_HasEvent(EventType type);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_HasEvents(EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_PeepEvents(Span<SdlEvent> events, int count, EventAction action, EventType minType, EventType maxType);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_PollEvent(out SdlEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_PumpEvents();

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_PushEvent(ref SdlEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SDL_SetEventEnabled(EventType type, [MarshalAs(UnmanagedType.Bool)] bool enabled);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_WaitEvent(out SdlEvent e);

    [LibraryImport(LibraryName)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(BoolMarshaller))]
    internal static partial bool SDL_WaitEventTimeout(out SdlEvent e, int timeout);

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
