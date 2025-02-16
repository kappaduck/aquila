// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Runtime.InteropServices.Marshalling;

namespace KappaDuck.Aquila.Interop.SDL.Marshallers;

/// <summary>
/// Marshaller for C arrays that are owned by the caller.
/// </summary>
/// <remarks>
/// Caller responsible for freeing the memory. It uses the <see cref="SDLNative.Free{T}(T*)"/> method.
/// </remarks>
/// <typeparam name="T">The managed type.</typeparam>
/// <typeparam name="TUnmanaged">The unmanaged type.</typeparam>
[ContiguousCollectionMarshaller]
[CustomMarshaller(typeof(Span<>), MarshalMode.Default, typeof(CallerOwnedArrayMarshaller<,>))]
internal static unsafe class CallerOwnedArrayMarshaller<T, TUnmanaged> where TUnmanaged : unmanaged
{
    /// <summary>
    /// This method is not used because the marshaller is only used for unmanaged to managed conversions.
    /// </summary>
    /// <param name="managed">The managed span.</param>
    /// <param name="length">The length of the span.</param>
    /// <returns>The pointer to the unmanaged memory.</returns>
    /// <exception cref="NotSupportedException">Not supported.</exception>
    internal static TUnmanaged* AllocateContainerForUnmanagedElements(Span<T> managed, out int length)
        => throw new NotSupportedException();

    internal static Span<T> AllocateContainerForManagedElements(TUnmanaged* unmanaged, int length)
    {
        if (unmanaged is null)
            return default;

        return new T[length];
    }

    internal static ReadOnlySpan<T> GetManagedValuesSource(Span<T> managed) => managed;

    public static Span<TUnmanaged> GetUnmanagedValuesDestination(TUnmanaged* unmanaged, int length) => new(unmanaged, length);

    public static ReadOnlySpan<TUnmanaged> GetUnmanagedValuesSource(TUnmanaged* unmanaged, int length) => new(unmanaged, length);

    public static Span<T> GetManagedValuesDestination(Span<T> managed) => managed;

    public static void Free(TUnmanaged* unmanaged) => SDLNative.Free(unmanaged);
}
