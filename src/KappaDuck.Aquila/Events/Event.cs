// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Global SDL event functions.
/// </summary>
public static partial class Event
{
    /// <summary>
    /// Disable events of a specific type.
    /// </summary>
    /// <param name="type">The event type to disable.</param>
    public static void Disable(EventType type) => SDL_SetEventEnabled(type, enabled: false);

    /// <summary>
    /// enable events of a specific type.
    /// </summary>
    /// <param name="type">The event type to enable.</param>
    public static void Enable(EventType type) => SDL_SetEventEnabled(type, enabled: true);

    /// <summary>
    /// Clear events of a range of types from the event queue.
    /// </summary>
    /// <param name="type">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <remarks>
    /// This will unconditionally remove any events from the queue that are in the range of <paramref name="type"/> to <paramref name="maxType"/>, inclusive.
    /// It's also normal to just ignore events you don't care about in your event loop without calling this function.
    /// This function only affects currently queued events. If you want to make sure that all pending OS events are flushed,
    /// you can call <see cref="Pump"/> on the main thread immediately before the flush call.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is greater than <paramref name="maxType"/>.</exception>
    public static void Flush(EventType type, EventType? maxType = null)
    {
        ThrowIfGreaterThan(type > maxType, nameof(type));

        SDL_FlushEvents(type, maxType ?? type);
    }

    /// <summary>
    /// Check for the existence of a certain event types in the event queue.
    /// </summary>
    /// <param name="type">The type to check if exists.</param>
    /// <returns>True if events matching type are present, or false if events matching type are not present.</returns>
    public static bool Has(EventType type) => SDL_HasEvent(type) != 0;

    /// <summary>
    /// Check for the existence of certain event types in the event queue.
    /// </summary>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive.</param>
    /// <returns>True if events with type >= <paramref name="minType"/> and &lt;= <paramref name="maxType"/> are present, or false if not.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    public static bool Has(EventType minType, EventType maxType)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        return SDL_HasEvents(minType, maxType) != 0;
    }

    /// <summary>
    /// Query the state of processing events by type.
    /// </summary>
    /// <param name="type">The event type to check.</param>
    /// <returns>True if the event is being processed, false otherwise.</returns>
    public static bool IsEnabled(EventType type) => SDL_EventEnabled(type) != 0;

    /// <summary>
    /// Retrieve events from the event queue without removing them.
    /// </summary>
    /// <param name="events">Destination buffer for the events at the front of the event queue.</param>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <returns>The number of events actually stored or <see langword="null"/> on failure; call <see cref="SDL.GetError()"/>.</returns>
    /// <remarks>
    /// You may have to call <see cref="Pump"/> before calling this function. Otherwise, the events may not be ready to be filtered.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    public static int? Peek(Span<SdlEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int peekedEvents = SDL_PeepEvents(events, events.Length, EventAction.Peek, minType, maxType ?? minType);

        return peekedEvents == -1 ? null : peekedEvents;
    }

    /// <summary>
    /// Polls for currently pending events.
    /// </summary>
    /// <param name="e">The next filled event from the queue.</param>
    /// <returns>True if this got an event or false if there are none available.</returns>
    public static bool Poll(out SdlEvent e) => SDL_PollEvent(out e) != 0;

    /// <summary>
    /// Pump the event loop, gathering events from the input devices.
    /// </summary>
    /// <remarks>
    /// This function updates the event queue and internal input device state.
    /// Gathers all the pending input information from devices and places it in the event queue.
    /// Without calls to <see cref="Pump"/> no events would ever be placed on the queue.
    /// Often the need for calls to <see cref="Pump"/> is hidden from the user since <see cref="Poll(out SdlEvent)"/> and <see cref="Wait(out SdlEvent, TimeSpan?)"/>
    /// implicitly call <see cref="Pump"/>. However, if you are not polling or waiting for events(e.g.you are filtering them),
    /// then you must call <see cref="Pump"/> to force an event queue update.
    /// </remarks>
    public static void Pump() => SDL_PumpEvents();

    /// <summary>
    /// Add an event to the event queue.
    /// </summary>
    /// <param name="e">The event to push to the event queue.</param>
    /// <returns>True on success, false if the event was filtered or on failure; call <see cref="SDL.GetError()"/> for more information.
    /// A common reason for error is the event queue being full.
    /// </returns>
    public static bool Push(ref SdlEvent e) => SDL_PushEvent(ref e) != 0;

    /// <summary>
    /// Add events to the back of the event queue.
    /// </summary>
    /// <param name="events">Added events to the event queue.</param>
    /// <returns>The number of events actually added or <see langword="null"/> on failure; call <see cref="SDL.GetError()"/>.</returns>
    public static int? Push(Span<SdlEvent> events)
    {
        int added = SDL_PeepEvents(events, events.Length, EventAction.Add, EventType.None, EventType.LastEvent);

        return added == -1 ? null : added;
    }

    /// <summary>
    /// Retrieve events from the event queue by removing them.
    /// </summary>
    /// <param name="events">Destination buffer for the retrieved events.</param>
    /// <param name="minType">The low end of event type to be cleared, inclusive.</param>
    /// <param name="maxType">the high end of event type to be cleared, inclusive. If <see langword="null"/> then minType is used.</param>
    /// <returns>The number of events actually stored or <see langword="null"/> on failure; call <see cref="SDL.GetError()"/>.</returns>
    /// <remarks>
    /// You may have to call <see cref="Pump"/> before calling this function. Otherwise, the events may not be ready to be filtered.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minType"/> is greater than <paramref name="maxType"/>.</exception>
    public static int? Retrieve(Span<SdlEvent> events, EventType minType, EventType? maxType = null)
    {
        ThrowIfGreaterThan(minType > maxType, nameof(minType));

        int retrievedEvents = SDL_PeepEvents(events, events.Length, EventAction.Get, minType, maxType ?? minType);

        return retrievedEvents == -1 ? null : retrievedEvents;
    }

    /// <summary>
    /// Wait until the specified timeout (in milliseconds) for the next available event.
    /// </summary>
    /// <param name="e">The next filled event from the queue.</param>
    /// <param name="timeSpan">The maximum time (milliseconds) to wait for the next available event.</param>
    /// <returns>True if this got an event or false if the timeout elapsed without any events available.</returns>
    /// <remarks>
    /// The timeout is not guaranteed, the actual wait time could be longer due to system scheduling.
    /// </remarks>
    public static bool Wait(out SdlEvent e, TimeSpan? timeSpan = null)
    {
        if (timeSpan is null || timeSpan == Timeout.InfiniteTimeSpan)
            return SDL_WaitEvent(out e) != 0;

        return SDL_WaitEventTimeout(out e, (int)timeSpan.Value.TotalMilliseconds) != 0;
    }

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

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_EventEnabled(EventType type);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_FlushEvent(EventType type);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_FlushEvents(EventType minType, EventType maxType);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_HasEvent(EventType type);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_HasEvents(EventType minType, EventType maxType);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int SDL_PeepEvents(Span<SdlEvent> events, int count, EventAction action, EventType minType, EventType maxType);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_PollEvent(out SdlEvent e);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_PumpEvents();

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_PushEvent(ref SdlEvent e);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial void SDL_SetEventEnabled(EventType type, [MarshalAs(UnmanagedType.Bool)] bool enabled);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_WaitEvent(out SdlEvent e);

    [LibraryImport(SDL.NativeLibrary)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial byte SDL_WaitEventTimeout(out SdlEvent e, int timeout);
}
