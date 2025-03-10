// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Components.Menu;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Geometry;
using KappaDuck.Aquila.Graphics.Pixels;
using KappaDuck.Aquila.Graphics.Rendering;
using KappaDuck.Aquila.Interop.SDL;
using KappaDuck.Aquila.Interop.SDL.Handles;
using KappaDuck.Aquila.Interop.Win32;
using KappaDuck.Aquila.Interop.Win32.Extensions;
using KappaDuck.Aquila.Video.Displays;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Video.Windows;

/// <summary>
/// Represents the base window.
/// </summary>
/// <remarks>
/// It is recommended to use <see cref="RenderWindow"/> for 2D rendering.
/// </remarks>
public abstract class BaseWindow : IDisposable
{
    private const string Win32PropertyName = "SDL.window.win32.hwnd";

    private bool _disposed;
    private WindowState _state;
    private Vector2i _position;
    private int _width;
    private int _height;
    private string _title = string.Empty;
    private Action<BaseWindow, MenuItem>? _onMenuItemClick;
    private bool _hasWindowsMessage;
    private WindowHandle _handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseWindow"/> class.
    /// </summary>
    protected BaseWindow()
    {
        _handle = WindowHandle.Zero;
        Opacity = 1.0f;

        Handle = new(_handle);
    }

    /// <summary>
    /// Initializes and create the window.
    /// </summary>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The initial state of the window.</param>
    /// <exception cref="SDLException">An error occurred while creating the window.</exception>
    protected BaseWindow(string title, int width, int height, WindowState state = WindowState.None)
    {
        _handle = CreateWindow(title, width, height, state);
        IsOpen = true;

        Handle = new(_handle);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is always on top.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window always on top.</exception>
    public bool AlwaysOnTop
    {
        get => (_state & WindowState.AlwaysOnTop) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.AlwaysOnTop) : (_state & ~WindowState.AlwaysOnTop);

            if (!SDLNative.SDL_SetWindowAlwaysOnTop(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets the aspect ratio of the window's client area.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Minimum or maximum is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window aspect ratio.</exception>
    public (float Minimum, float Maximum) AspectRatio
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Minimum);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Maximum);

            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowAspectRatio(_handle, value.Minimum, value.Maximum))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is borderless.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window borderless.</exception>
    public bool Borderless
    {
        get => (_state & WindowState.Borderless) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.Borderless) : (_state & ~WindowState.Borderless);

            if (!SDLNative.SDL_SetWindowBordered(_handle, !value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the size of the window's borders (decorations) around the client area.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open or borderless, it will return (0, 0, 0, 0).</para>
    /// <para>
    /// It is possible that failed to get the border size because the window has not yet been decorated by the display server
    /// or the information is not supported.
    /// </para>
    /// </remarks>
    public (int Top, int Left, int Bottom, int Right) BordersSize
    {
        get
        {
            if (!IsOpen || Borderless)
                return default;

            SDLNative.SDL_GetWindowBordersSize(_handle, out int top, out int left, out int bottom, out int right);
            return (top, left, bottom, right);
        }
    }

    /// <summary>
    /// Gets the display associated with the window.
    /// </summary>
    /// <remarks>
    /// If the window is not open, it will return the primary display.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the window display.</exception>
    public Display Display
    {
        get
        {
            if (!IsOpen)
                return Display.GetPrimaryDisplay();

            uint displayId = SDLNative.SDL_GetDisplayForWindow(_handle);

            SDLException.ThrowIfZero(displayId);

            return Display.GetDisplay(displayId);
        }
    }

    /// <summary>
    /// Gets the content display scale relative to the window's pixel size.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open, it will return 0.0f.</para>
    /// <para>
    /// This is a combination of the window pixel density and the display content scale,
    /// and is the expected scale for displaying content in this window.
    /// For example, if a 3840x2160 window had a display scale of 2.0,
    /// the user expects the content to take twice as many pixels and be the same physical size
    /// as if it were being displayed in a 1920x1080 window with a display scale of 1.0.
    /// </para>
    /// <para>Conceptually this value corresponds to the scale display setting, and is updated when that setting is changed, or the window moves to a display with a different scale setting.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the window display scale.</exception>
    public float DisplayScale
    {
        get
        {
            if (!IsOpen)
                return 0.0f;

            float scale = SDLNative.SDL_GetWindowDisplayScale(_handle);

            SDLException.ThrowIfZero(scale);

            return scale;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is focusable.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window focusable.</exception>
    public bool Focusable
    {
        get => (_state & ~WindowState.NotFocusable) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state & ~WindowState.NotFocusable) : (_state | WindowState.NotFocusable);

            if (!SDLNative.SDL_SetWindowFocusable(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is fullscreen.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while settings the window fullscreen.</exception>
    public bool Fullscreen
    {
        get => (_state & WindowState.Fullscreen) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.Fullscreen) : (_state & ~WindowState.Fullscreen);

            if (!SDLNative.SDL_SetWindowFullscreen(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets the fullscreen display mode to use when
    /// the window is in <see cref="WindowState.Fullscreen"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Setting to <see langword="null"/> will use borderless fullscreen desktop mode,
    /// or one of the fullscreen modes from <see cref="Display.GetFullScreenModes"/> to set an exclusive fullscreen mode.
    /// </para>
    /// <para>
    /// If the window is currently in <see cref="WindowState.Fullscreen"/> state, this request is asynchronous on some windowing
    /// systems and the new mode dimensions may not be applied immediately. If an immediate change is needed, call <see cref="Sync"/> to block
    /// until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the new mode takes effect, an <see cref="EventType.WindowResized"/> and/or
    /// an <see cref="EventType.WindowPixelSizeChanged"/> event will be emitted with the new mode dimensions.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window fullscreen mode.</exception>
    public DisplayMode? FullscreenMode
    {
        get;
        set
        {
            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SetWindowFullscreenMode(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the handle of the window.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You can use this handle to interact with the window using platform-specific APIs.
    /// </para>
    /// <para>
    /// You do not need to dispose of this handle as it is owned by the window.
    /// </para>
    /// </remarks>
    public WindowHandle Handle { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window has keyboard focus.
    /// </summary>
    public bool HasKeyboardFocus => (_state & WindowState.InputFocus) != WindowState.None;

    /// <summary>
    /// Gets a value indicating whether the window has mouse focus.
    /// </summary>
    public bool HasMouseFocus => (_state & WindowState.MouseFocus) != WindowState.None;

    /// <summary>
    /// Gets or sets the height of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the height if the window is in <see cref="WindowState.Fullscreen"/> or <see cref="WindowState.Maximized"/> state will be ignored.</para>
    /// <para>To change the exclusive fullscreen mode dimensions, use <see cref="FullscreenMode"/>.</para>
    /// <para>It will be restricted by <see cref="MinimumSize"/> and <see cref="MaximumSize"/>.</para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window size changes, an <see cref="EventType.WindowResized"/> event will be emitted with the new dimensions.
    /// Note that the new dimensions may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Height is less than or equal to 0.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window height.</exception>
    public int Height
    {
        get => _height;
        set
        {
            if (Fullscreen || Maximized)
                return;

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0);

            _height = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowSize(_handle, _width, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the height of the window in pixels.
    /// </summary>
    public int HeightInPixel { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window is hidden.
    /// </summary>
    public bool Hidden => (_state & WindowState.Hidden) != WindowState.None;

    /// <summary>
    /// Gets the window's identifier.
    /// </summary>
    /// <remarks>
    /// The identifier is what <see cref="WindowEvent"/> references.
    /// </remarks>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the window uses high pixel density.
    /// </summary>
    public bool HighPixelDensity => (_state & WindowState.HighPixelDensity) != WindowState.None;

    /// <summary>
    /// Gets a value indicating whether the window is open.
    /// </summary>
    public bool IsOpen
    {
        get => !_handle.IsInvalid && field;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether the screen keyboard is visible.
    /// </summary>
    public bool IsScreenKeyboardVisible
    {
        get
        {
            if (!IsOpen)
                return false;

            return SDLNative.SDL_ScreenKeyboardShown(_handle);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window has grabbed keyboard input.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </para>
    /// <para>
    /// Keyboard grab enables capture of system keyboard shortcuts like Alt+Tab or the Meta/Super key.
    /// Important to note that not all system keyboard shortcuts can be captured by applications (one example is Ctrl+Alt+Del on Windows).
    /// </para>
    /// <para>
    /// This is primarily intended for specialized applications such as VNC clients or VM frontends. Normal games should not use keyboard grab.
    /// </para>
    /// <para>
    /// When keyboard is enabled, SDL will continue to handle Alt+Tab when
    /// the window is fullscreen to ensure the user is not trapped in your application.
    /// </para>
    /// <para>If the caller enables a grab while another window is currently grabbed, the other window loses its grab in favor of the caller's window.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window keyboard grab.</exception>
    public bool KeyboardGrabbed
    {
        get => (_state & WindowState.KeyboardGrabbed) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.KeyboardGrabbed) : (_state & ~WindowState.KeyboardGrabbed);

            if (!SDLNative.SDL_SetWindowKeyboardGrab(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is maximized.
    /// </summary>
    public bool Maximized => (_state & WindowState.Maximized) != WindowState.None;

    /// <summary>
    /// Gets or sets the horizontal menu bar at the top of the window.
    /// </summary>
    /// <remarks>
    /// Only available on Windows.
    /// </remarks>
    /// <exception cref="PlatformNotSupportedException">Menu bar is only available on Windows.</exception>
    public MenuBar? MenuBar
    {
        get;
        set
        {
            if (!OperatingSystem.IsWindows())
                throw new PlatformNotSupportedException("Menu bar is only available on Windows.");

            if (!IsOpen)
            {
                field = value;
                return;
            }

            nint win32Handle = GetWin32Handle();

            if (value is null)
            {
                field?.Dispose();
                MenuBar.Detach(win32Handle);

                _onMenuItemClick = null;

                return;
            }

            field = value;
            field.Attach(win32Handle);

            if (!_hasWindowsMessage)
            {
                HookWindowsMessage();
                _hasWindowsMessage = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum size of the window's client area.
    /// </summary>
    /// <remarks>
    /// <para>Setting to (0, 0) removes the maximum size limit.</para>
    /// <para>It will influence the window's size when resizing or using <see cref="Maximize"/>.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window maximum size.</exception>
    public (int Width, int Height) MaximumSize
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowMaximumSize(_handle, value.Width, value.Height))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window is minimized.
    /// </summary>
    public bool Minimized => (_state & WindowState.Minimized) != WindowState.None;

    /// <summary>
    /// Gets or sets the minimum size of the window's client area.
    /// </summary>
    /// <remarks>
    /// <para>Setting to (0, 0) removes the maximum size limit.</para>
    /// <para>It will influence the window's size when resizing.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width or height is negative.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window minimum size.</exception>
    public (int Width, int Height) MinimumSize
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value.Width);
            ArgumentOutOfRangeException.ThrowIfNegative(value.Height);

            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowMinimumSize(_handle, value.Width, value.Height))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window has captured mouse input.
    /// </summary>
    /// <remarks>
    /// It is not related to <see cref="MouseGrabbed"/> (<see cref="WindowState.MouseGrabbed"/>).
    /// </remarks>
    public bool MouseCaptured => (_state & WindowState.MouseCapture) != WindowState.None;

    /// <summary>
    /// Gets or sets the confined area of the mouse in the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting to <see langword="null"/> or an empty <see cref="RectInt"/> removes the confined area.</para>
    /// <para>This will not grab the cursor, it only defines the area a cursor is restricted to when the window has mouse focus.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window mouse clip.</exception>
    public RectInt? MouseClip
    {
        get;
        set
        {
            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SetWindowMouseRect(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window has mouse input grabbed.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window mouse grab.</exception>
    public bool MouseGrabbed
    {
        get => (_state & WindowState.MouseGrabbed) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.MouseGrabbed) : (_state & ~WindowState.MouseGrabbed);

            if (!SDLNative.SDL_SetWindowMouseGrab(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the window has relative mouse mode enabled.
    /// </summary>
    public bool MouseRelativeMode => (_state & WindowState.MouseRelativeMode) != WindowState.None;

    /// <summary>
    /// Gets a value indicating whether the window is occluded.
    /// </summary>
    public bool Occluded => (_state & WindowState.Occluded) != WindowState.None;

    /// <summary>
    /// Event that occurs when a menu item is clicked from the window's menu bar.
    /// </summary>
    public event Action<BaseWindow, MenuItem>? OnMenuItemClick
    {
        add
        {
            if (_onMenuItemClick is not null)
                _onMenuItemClick = null;

            _onMenuItemClick = value;
        }

        remove
        {
            if (_onMenuItemClick is null)
                return;

            _onMenuItemClick = value;
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the window.
    /// </summary>
    /// <remarks>
    /// <para>The default value is 1.0f.</para>
    /// <para>The opacity value should be in the range 0.0f - 1.0f.</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Opacity is negative or greater than 1.0f.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window opacity.</exception>
    public float Opacity
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1.0f);

            field = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowOpacity(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the pixel density of the window.
    /// </summary>
    /// <remarks>
    /// <para>If the window is not open, it will return 0.0f.</para>
    /// <para>
    /// This is a ratio of pixel size to window size. For example, if the window is 1920x1080 and it has a
    /// high density back buffer of 3840x2160 pixels, it would have a pixel density of 2.0.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the window pixel density.</exception>
    public float PixelDensity
    {
        get
        {
            if (!IsOpen)
                return 0.0f;

            float density = SDLNative.SDL_GetWindowPixelDensity(_handle);

            SDLException.ThrowIfZero(density);

            return density;
        }
    }

    /// <summary>
    /// Gets the pixel format associated with the window.
    /// </summary>
    /// <exception cref="SDLException">An error occurred while getting the window pixel format.</exception>
    public PixelFormat PixelFormat
    {
        get
        {
            if (!IsOpen)
                return PixelFormat.Unknown;

            PixelFormat format = SDLNative.SDL_GetWindowPixelFormat(_handle);

            SDLException.ThrowIf(format == PixelFormat.Unknown);

            return format;
        }
    }

    /// <summary>
    /// Gets or sets the position of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the position if the window is in <see cref="WindowState.Fullscreen"/> or <see cref="WindowState.Maximized"/> state will be ignored.</para>
    /// <para>
    /// This can be used to reposition fullscreen desktop windows onto a different display,
    /// however, as exclusive fullscreen windows are locked to a specific display, they can only be repositioned via <see cref="FullscreenMode"/>.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window position changes, an <see cref="EventType.WindowMoved"/> event will be emitted with the new coordinates.
    /// Note that the new coordinates may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// <para>This is the current position of the window as last reported by the windowing system.</para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window position.</exception>
    public Vector2i Position
    {
        get => _position;
        set
        {
            if (Fullscreen || Maximized)
                return;

            _position = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowPosition(_handle, value.X, value.Y))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the window is resizable.
    /// </summary>
    /// <remarks>
    /// If you set this property during the creation of an empty <see cref="BaseWindow"/> then using <see cref="Create(string, int, int, WindowState)"/>, it will be ignored.
    /// Use <see cref="WindowState"/> parameter instead.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while setting the window resizable.</exception>
    public bool Resizable
    {
        get => (_state & WindowState.Resizable) != WindowState.None;
        set
        {
            if (!IsOpen)
                return;

            _state = value ? (_state | WindowState.Resizable) : (_state & ~WindowState.Resizable);

            if (!SDLNative.SDL_SetWindowResizable(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the safe area for the window.
    /// </summary>
    /// <remarks>
    /// Some devices have portions of the screen which are partially obscured or not interactive,
    /// possibly due to on-screen controls, curved edges, camera notches, TV over scan, etc.
    /// This provides the area of the window which is safe to have interactable content.
    /// You should continue rendering into the rest of the window,
    /// but it should not contain visually important or interactable content.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while getting the window safe area.</exception>
    public RectInt SafeArea
    {
        get
        {
            if (!IsOpen)
                return default;

            if (!SDLNative.SDL_GetWindowSafeArea(_handle, out RectInt area))
                SDLException.Throw();

            return area;
        }
    }

    /// <summary>
    /// Gets or sets the title of the window.
    /// </summary>
    /// <exception cref="SDLException">An error occurred while setting the window title.</exception>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowTitle(_handle, value))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets or sets the width of the window.
    /// </summary>
    /// <remarks>
    /// <para>Setting the width if the window is in <see cref="WindowState.Fullscreen"/> or <see cref="WindowState.Maximized"/> state will be ignored.</para>
    /// <para>To change the exclusive fullscreen mode dimensions, use <see cref="FullscreenMode"/>.</para>
    /// <para>It will be restricted by <see cref="MinimumSize"/> and <see cref="MaximumSize"/>.</para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window size changes, an <see cref="EventType.WindowResized"/> event will be emitted with the new dimensions.
    /// Note that the new dimensions may not be the same as those requested, as the windowing system may impose its own constraints.
    /// (e.g constraining the size of the content area to remain within the usable desktop bounds). Additionally,
    /// as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Width is less than or equal to 0.</exception>
    /// <exception cref="SDLException">An error occurred while setting the window width.</exception>
    public int Width
    {
        get => _width;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0);

            _width = value;

            if (!IsOpen)
                return;

            if (!SDLNative.SDL_SetWindowSize(_handle, value, _height))
                SDLException.Throw();
        }
    }

    /// <summary>
    /// Gets the width of the window in pixels.
    /// </summary>
    public int WidthInPixel { get; private set; }

    /// <summary>
    /// Closes the window.
    /// </summary>
    /// <remarks>
    /// If the window is already closed or not created, It does nothing.
    /// </remarks>
    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;
    }

    /// <summary>
    /// Creates the window.
    /// </summary>
    /// <remarks>
    /// If the window is already open, it does nothing.
    /// </remarks>
    /// <param name="title">The title of the window.</param>
    /// <param name="width">The width of the window.</param>
    /// <param name="height">The height of the window.</param>
    /// <param name="state">The initial state of the window.</param>
    /// <exception cref="SDLException">An error occurred while creating the window.</exception>
    public void Create(string title, int width, int height, WindowState state = WindowState.None)
    {
        if (IsOpen)
            return;

        _handle = CreateWindow(title, width, height, state);
        Handle = new(_handle);

        SDLNative.SDL_SetWindowAspectRatio(_handle, AspectRatio.Minimum, AspectRatio.Maximum);
        SDLNative.SetWindowFullscreenMode(_handle, FullscreenMode);
        SDLNative.SDL_SetWindowMaximumSize(_handle, MaximumSize.Width, MaximumSize.Height);
        SDLNative.SDL_SetWindowMinimumSize(_handle, MinimumSize.Width, MinimumSize.Height);
        SDLNative.SetWindowMouseRect(_handle, MouseClip);
        SDLNative.SDL_SetWindowOpacity(_handle, Opacity);

        IsOpen = true;

        if (OperatingSystem.IsWindows())
            MenuBar?.Attach(GetWin32Handle());
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Request the window to demand attention from the user.
    /// </summary>
    /// <param name="state">The state of the flash.</param>
    /// <exception cref="SDLException">An error occurred while flashing the window.</exception>
    public void Flash(FlashState state)
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_FlashWindow(_handle, state))
            SDLException.Throw();
    }

    /// <summary>
    /// Hides the window. It can be shown again with <see cref="Show"/>.
    /// </summary>
    /// <exception cref="SDLException">An error occurred while hiding the window.</exception>
    public void Hide()
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_HideWindow(_handle))
            SDLException.Throw();

        _state |= WindowState.Hidden;
    }

    /// <summary>
    /// Request that the window be made as large as possible.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Non-resizable windows can't be maximized. The window must have the <see cref="WindowState.Resizable"/> state set.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowMaximized"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// <para>
    /// When maximizing a window, whether the constraints set via <see cref="MaximumSize"/> are honored depends on the policy of the window manager.
    /// Win32 enforce the constraints when maximizing, while X11 and Wayland window managers may vary.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while maximizing the window.</exception>
    public void Maximize()
    {
        if (!IsOpen || Maximized || !Resizable)
            return;

        if (!SDLNative.SDL_MaximizeWindow(_handle))
            SDLException.Throw();

        _state &= ~WindowState.Minimized;
        _state |= WindowState.Maximized;
    }

    /// <summary>
    /// Request that the window be minimized to an iconic representation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the window is in <see cref="WindowState.Fullscreen"/> state, it will has no direct effect.
    /// It may alter the state the window is restored to when leaving fullscreen.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowMinimized"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while minimizing the window.</exception>
    public void Minimize()
    {
        if (!IsOpen || Minimized)
            return;

        if (!SDLNative.SDL_MinimizeWindow(_handle))
            SDLException.Throw();

        _state &= ~WindowState.Maximized;
        _state |= WindowState.Minimized;
    }

    /// <summary>
    /// Polls for currently pending events.
    /// </summary>
    /// <remarks>
    /// <para>Some events are processed internally by the window.</para>
    /// Will return <see langword="false"/> and empty <see cref="SDLEvent"/> if the window is not open.
    /// </remarks>
    /// <param name="e">The next filled event from the queue.</param>
    /// <returns><see langword="true"/> if this got an event or <see langword="false"/> if there are none available.</returns>
    public bool Poll(out SDLEvent e)
    {
        if (!IsOpen)
        {
            e = default;
            return false;
        }

        bool hasEvent = Event.Poll(out e);

        if (!IsOpen || e.Window.Id != Id)
            return hasEvent;

        if (e.Type == EventType.WindowExposed)
            _state &= ~WindowState.Occluded;

        if (e.Type == EventType.WindowOccluded)
            _state |= WindowState.Occluded;

        if (e.Type == EventType.WindowResized)
        {
            _width = e.Window.Data1;
            _height = e.Window.Data2;
        }

        if (e.Type == EventType.WindowPixelSizeChanged)
        {
            WidthInPixel = e.Window.Data1;
            HeightInPixel = e.Window.Data2;
        }

        if (e.Type == EventType.WindowMoved)
        {
            _position.X = e.Window.Data1;
            _position.Y = e.Window.Data2;
        }

        if (e.Type == EventType.MouseEnter)
            _state |= WindowState.MouseFocus;

        if (e.Type == EventType.MouseLeave)
            _state &= ~WindowState.MouseFocus;

        if (e.Type == EventType.FocusGained)
            _state |= WindowState.InputFocus;

        if (e.Type == EventType.FocusLost)
            _state &= ~WindowState.InputFocus;

        if (e.Type == EventType.WindowRestored)
            _state &= ~(WindowState.Minimized | WindowState.Maximized);

        return hasEvent;
    }

    /// <summary>
    /// Request that the window be raised above other windows and gain the input focus.
    /// </summary>
    /// <remarks>
    /// The result of this request is subject to desktop window manager policy, particularly if raising
    /// the requested window would result in stealing focus from another application.
    /// If the window is successfully raised and gains input focus,
    /// an <see cref="EventType.FocusGained"/> event will be emitted,
    /// and the window will have <see cref="WindowState.InputFocus"/> state set.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while raising the window.</exception>
    public void Raise()
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_RaiseWindow(_handle))
            SDLException.Throw();

        _state |= WindowState.InputFocus;
    }

    /// <summary>
    /// Request that the size and position of a minimized or maximized window be restored.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the window is in <see cref="WindowState.Fullscreen"/> state, it will has no direct effect.
    /// It may alter the state the window is restored to when leaving fullscreen.
    /// </para>
    /// <para>
    /// On some windowing systems this request is asynchronous and the new window state may not have been applied immediately.
    /// If an immediate change is required, call <see cref="Sync"/> to block until the changes have taken effect.
    /// </para>
    /// <para>
    /// When the window state changes, an <see cref="EventType.WindowRestored"/> event will be emitted.
    /// Note that, as this is just a request, the windowing system can deny the state change.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while restoring the window.</exception>
    public void Restore()
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_RestoreWindow(_handle))
            SDLException.Throw();
    }

    /// <summary>
    /// Show the window.
    /// </summary>
    /// <remarks>
    /// It's only the way to show a window that has been hidden
    /// with <see cref="Hide"/> or using <see cref="WindowState.Hidden"/> state.
    /// </remarks>
    /// <exception cref="SDLException">An error occurred while showing the window.</exception>
    public void Show()
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_ShowWindow(_handle))
            SDLException.Throw();

        _state &= ~WindowState.Hidden;
    }

    /// <summary>
    /// Block until any pending window state is finalized.
    /// </summary>
    /// <remarks>
    /// <para>On windowing systems where changes are immediate, this does nothing.</para>
    /// <para>
    /// On asynchronous windowing systems, this acts as a synchronization barrier for pending window state.
    /// It will attempt to wait until any pending window state has been applied and is guaranteed to return within finite time.
    /// Note that for how long it can potentially block depends on the underlying window system,
    /// as window state changes may involve somewhat lengthy animations that must complete before the window is in its final requested state.
    /// </para>
    /// </remarks>
    /// <exception cref="SDLException">Failed to sync the window.</exception>
    public void Sync()
    {
        if (!IsOpen)
            return;

        if (!SDLNative.SDL_SyncWindow(_handle))
            SDLException.Throw();
    }

    /// <summary>
    /// Move the mouse cursor to the given position withing the window.
    /// </summary>
    /// <remarks>
    /// <para>It generates a <see cref="EventType.MouseMotion"/> event if relative mode is not enabled.</para>
    /// <para>It will not move the mouse when used over Microsoft Remote Desktop.</para>
    /// </remarks>
    /// <param name="x">The x-coordinate within the window.</param>
    /// <param name="y">The y-coordinate within the window.</param>
    public void WarpMouse(float x, float y) => SDLNative.SDL_WarpMouseInWindow(_handle, x, y);

    /// <summary>
    /// Move the mouse cursor to the given position withing the window.
    /// </summary>
    /// <remarks>
    /// <para>It generates a <see cref="EventType.MouseMotion"/> event if relative mode is not enabled.</para>
    /// <para>It will not move the mouse when used over Microsoft Remote Desktop.</para>
    /// </remarks>
    /// <param name="position">The position within the window.</param>
    public void WarpMouse(Vector2 position) => WarpMouse(position.X, position.Y);

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="BaseWindow"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (OperatingSystem.IsWindows())
                MenuBar?.Dispose();

            _handle.Dispose();
        }

        _disposed = true;
    }

    private WindowHandle CreateWindow(string title, int width, int height, WindowState state)
    {
        WindowHandle handle = SDLNative.SDL_CreateWindow(title, width, height, state);

        SDLException.ThrowIf(handle.IsInvalid);

        Id = SDLNative.SDL_GetWindowID(handle);
        SDLException.ThrowIfZero(Id);

        _state = state;

        _title = title;
        _width = width;
        _height = height;

        if (_position.IsZero)
        {
            SDLNative.SDL_GetWindowPosition(handle, out int x, out int y);
            _position = new Vector2i(x, y);
        }
        else
        {
            SDLNative.SDL_SetWindowPosition(handle, _position.X, _position.Y);
        }

        return handle;
    }

    private nint GetWin32Handle()
    {
        uint propertiesId = SDLNative.SDL_GetWindowProperties(_handle);
        return SDLNative.SDL_GetPointerProperty(propertiesId, Win32PropertyName, nint.Zero);
    }

    [SupportedOSPlatform("windows")]
    private void HookWindowsMessage()
    {
        if (!OperatingSystem.IsWindows())
            return;

        SDLNative.SDL_SetWindowsMessageHook((_, msg) =>
        {
            if (IsItemClicked(msg))
            {
                MenuItem item = MenuBar!.FindMenuItem(msg.WParam.Lower16bits());

                _onMenuItemClick?.Invoke(this, item);
                return false;
            }

            return true;
        });

        static bool IsItemClicked(in MSG msg)
        {
            const int wmCommand = 0x0111;
            const int menu = 0;

            return msg.Message == wmCommand && msg.WParam.Upper16bits() == menu;
        }
    }
}
