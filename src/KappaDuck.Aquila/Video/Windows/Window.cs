// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Handles;
using KappaDuck.Aquila.Video.Displays;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace KappaDuck.Aquila.Video.Windows;

/// <summary>
/// Represents a basic window.
/// </summary>
public partial class Window : IDisposable
{
    private WindowHandle _handle;

    private string _title = string.Empty;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Create(string, int, int, WindowState)"/> to create the window.
    /// </remarks>
    public Window() => _handle = new WindowHandle();

    /// <summary>
    /// Initializes a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <remarks>
    /// It will show the window. If you want to prepare the window before showing it, use <see cref="WindowState.Hidden"/> to hide it.
    /// Then use <see cref="Show"/> to show it.
    /// </remarks>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The state of the window.</param>
    /// <exception cref="SDLException">Failed to create the window.</exception>
    public Window(string title, int width, int height, WindowState state = WindowState.None)
        => _handle = CreateWindow(title, width, height, state);

    /// <summary>
    /// Gets or sets a value indicating whether the window is always on top.
    /// </summary>
    public bool AlwaysOnTop
    {
        get => (State & WindowState.AlwaysOnTop) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State | WindowState.AlwaysOnTop) : (State & ~WindowState.AlwaysOnTop);

            NativeMethods.SDL_SetWindowAlwaysOnTop(_handle, value);
        }
    }

    /// <summary>
    /// Gets or sets the aspect ratio of the window.
    /// </summary>
    /// <remarks>
    /// The aspect ratio is the ratio of width divided by height, e.g. 2560x1600 would be 1.6.
    /// Larger aspect ratios are wider and smaller aspect ratios are narrower.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">The minimum or maximum aspect ratio is less than 0.0.</exception>
    public (float Min, float Max) AspectRatio
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Min);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Max);

            field = value;

            if (_handle.IsInvalid)
                return;

            NativeMethods.SDL_SetWindowAspectRatio(_handle, value.Min, value.Max);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is borderless.
    /// </summary>
    public bool Borderless
    {
        get => (State & WindowState.Borderless) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State | WindowState.Borderless) : (State & ~WindowState.Borderless);

            NativeMethods.SDL_SetWindowBordered(_handle, !value);
        }
    }

    /// <summary>
    /// Gets the size of the window borders.
    /// </summary>
    /// <remarks>
    /// If the window is <see cref="WindowState.Borderless"/> or the window is not created. The borders size will be 0.
    /// </remarks>
    public (int Top, int Left, int Bottom, int Right) BordersSize
    {
        get
        {
            if (_handle.IsInvalid)
                return (0, 0, 0, 0);

            NativeMethods.SDL_GetWindowBordersSize(_handle, out int top, out int left, out int bottom, out int right);

            return (top, left, bottom, right);
        }
    }

    /// <summary>
    /// Gets the display the window is on.
    /// </summary>
    /// <exception cref="SDLException">Failed to get the display for the window.</exception>
    public Display Display
    {
        get
        {
            uint displayId = NativeMethods.SDL_GetDisplayForWindow(_handle);

            return Display.GetDisplay(displayId);
        }
    }

    /// <summary>
    /// Gets the content display scale relative to a window's pixel size.
    /// </summary>
    /// <remarks>
    /// This is a combination of the window pixel density and the display content scale, and is the expected scale for displaying content in this window.
    /// For example, if a 3840x2160 window had a display scale of 2.0, the user expects the content to take twice as many pixels and
    /// be the same physical size as if it were being displayed in a 1920x1080 window with a display scale of 1.0.
    /// Conceptually this value corresponds to the scale display setting, and is updated when that setting is changed,
    /// or the window moves to a display with a different scale setting.
    /// </remarks>
    public float DisplayScale
    {
        get
        {
            if (_handle.IsInvalid)
                return 1.0f;

            return NativeMethods.SDL_GetWindowDisplayScale(_handle);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window have input focus.
    /// </summary>
    public bool Focusable
    {
        get => (State & ~WindowState.NotFocusable) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State & ~WindowState.NotFocusable) : (State | WindowState.NotFocusable);

            NativeMethods.SDL_SetWindowFocusable(_handle, value);
        }
    }

    /// <summary>
    /// Gets the fullscreen mode when using <see cref="WindowState.Fullscreen"/>
    /// otherwise <see langword="null"/> which use borderless fullscreen desktop mode.
    /// </summary>
    public DisplayMode? FullscreenMode
    {
        get
        {
            if (_handle.IsInvalid)
                return default;

            unsafe
            {
                DisplayMode* mode = NativeMethods.SDL_GetWindowFullscreenMode(_handle);

                return mode is null ? null : *mode;
            }
        }
    }

    /// <summary>
    /// Gets the unique ID of the window.
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window has grabbed keyboard input.
    /// </summary>
    public bool IsKeyboardGrabbed
    {
        get => (State & WindowState.KeyboardGrabbed) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State | WindowState.KeyboardGrabbed) : (State & ~WindowState.KeyboardGrabbed);

            NativeMethods.SDL_SetWindowKeyboardGrab(_handle, value);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window has grabbed mouse input.
    /// </summary>
    public bool IsMouseGrabbed
    {
        get => (State & WindowState.MouseGrabbed) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State | WindowState.MouseGrabbed) : (State & ~WindowState.MouseGrabbed);

            NativeMethods.SDL_SetWindowMouseGrab(_handle, value);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is open.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Gets or sets the opacity of the window.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">The opacity is less than 0.0 or greater than 1.0.</exception>
    public float Opacity
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1.0f);

            field = value;

            if (_handle.IsInvalid)
                return;

            NativeMethods.SDL_SetWindowOpacity(_handle, value);
        }
    }

    /// <summary>
    /// Gets the state of the window.
    /// </summary>
    public WindowState State { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the window is resizable.
    /// </summary>
    public bool Resizable
    {
        get => (State & WindowState.Resizable) != WindowState.None;
        set
        {
            if (_handle.IsInvalid)
                return;

            State = value ? (State | WindowState.Resizable) : (State & ~WindowState.Resizable);

            NativeMethods.SDL_SetWindowResizable(_handle, value);
        }
    }

    /// <summary>
    /// Gets or sets the title of the window.
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            if (_handle.IsInvalid)
                return;

            NativeMethods.SDL_SetWindowTitle(_handle, value);
        }
    }

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <remarks>
    /// It will release the resources used by the window. Use <see cref="Create"/> to recreate the window.
    /// </remarks>
    public void Close() => Dispose();

    /// <summary>
    /// Creates the window.
    /// </summary>
    /// <remarks>
    /// It will set other properties for the window such as <see cref="AspectRatio"/>, <see cref="Opacity"/>, etc..
    /// </remarks>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The state of the window.</param>
    public void Create(string title, int width, int height, WindowState state = WindowState.None)
    {
        if (!_handle.IsInvalid)
            return;

        _handle = CreateWindow(title, width, height, state);

        NativeMethods.SDL_SetWindowAspectRatio(_handle, AspectRatio.Min, AspectRatio.Max);
        NativeMethods.SDL_SetWindowOpacity(_handle, Opacity);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Request the window to demand attention from the user.
    /// </summary>
    /// <param name="state">The flash state.</param>
    public void Flash(FlashState state)
    {
        if (NativeMethods.SDL_FlashWindow(_handle, state) == 0)
            SDLException.Throw();
    }

    /// <summary>
    /// Hides the window.
    /// </summary>
    /// <exception cref="SDLException">Failed to hide the window.</exception>
    public void Hide()
    {
        State |= WindowState.Hidden;

        if (NativeMethods.SDL_HideWindow(_handle) == 0)
            SDLException.Throw();
    }

    /// <summary>
    /// Request that the size and position of a minimized or maximized window be restored.
    /// </summary>
    /// <remarks>
    /// Restore will not be called if the window is <see cref="WindowState.Fullscreen"/>.
    /// </remarks>
    public void Restore()
    {
        if ((State & WindowState.Fullscreen) != WindowState.None)
            return;

        if (NativeMethods.SDL_RestoreWindow(_handle) == 0)
            SDLException.Throw();
    }

    /// <summary>
    /// Shows the window.
    /// </summary>
    /// <exception cref="SDLException">Failed to show the window.</exception>
    public void Show()
    {
        State &= ~WindowState.Hidden;

        if (NativeMethods.SDL_ShowWindow(_handle) == 0)
            SDLException.Throw();
    }

    /// <summary>
    /// Block until any pending window state is finalized.
    /// </summary>
    /// <remarks>
    /// On asynchronous windowing systems, this acts as a synchronization barrier for pending window state.
    /// It will attempt to wait until any pending window state has been applied and is guaranteed to return within finite time.
    /// Note that for how long it can potentially block depends on the underlying window system,
    /// as window state changes may involve somewhat lengthy animations that must complete before the window is in its final requested state.
    /// On windowing systems where changes are immediate, this does nothing.
    /// </remarks>
    public void Sync()
    {
        if (NativeMethods.SDL_SyncWindow(_handle) == 0)
            SDLException.Throw();
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Window"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _handle.Dispose();

        _disposed = true;
        IsOpen = false;
    }

    private WindowHandle CreateWindow(string title, int width, int height, WindowState state)
    {
        WindowHandle handle = NativeMethods.SDL_CreateWindow(title, width, height, state);

        if (handle.IsInvalid)
            SDLException.Throw();

        Id = NativeMethods.SDL_GetWindowID(handle);

        if (Id == 0)
            SDLException.Throw();

        _title = title;

        State = state;
        IsOpen = true;

        return handle;
    }

    private static partial class NativeMethods
    {
        [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial WindowHandle SDL_CreateWindow(string title, int width, int height, WindowState flags);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_FlashWindow(WindowHandle window, FlashState state);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial uint SDL_GetDisplayForWindow(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_GetWindowBordersSize(WindowHandle window, out int top, out int left, out int bottom, out int right);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowAspectRatio(WindowHandle window, float min, float max);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial float SDL_GetWindowDisplayScale(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial DisplayMode* SDL_GetWindowFullscreenMode(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static unsafe partial byte SDL_SetWindowFullscreenMode(WindowHandle window, ref DisplayMode mode);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_SyncWindow(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial uint SDL_GetWindowID(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowOpacity(WindowHandle window, float opacity);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowResizable(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool resizable);

        [LibraryImport(SDL.NativeLibrary, StringMarshalling = StringMarshalling.Utf8)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowTitle(WindowHandle window, string title);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_HideWindow(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_ShowWindow(WindowHandle window);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_SetWindowMouseGrab(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool grabbed);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_SetWindowKeyboardGrab(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool grabbed);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowFocusable(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool focusable);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowBordered(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool bordered);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial void SDL_SetWindowAlwaysOnTop(WindowHandle window, [MarshalAs(UnmanagedType.Bool)] bool onTop);

        [LibraryImport(SDL.NativeLibrary)]
        [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
        internal static partial byte SDL_RestoreWindow(WindowHandle window);
    }
}
