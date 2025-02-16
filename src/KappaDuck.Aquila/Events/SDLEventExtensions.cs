// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Inputs;

namespace KappaDuck.Aquila.Events;

/// <summary>
/// Provides extension methods for <see cref="SDLEvent"/>.
/// </summary>
public static class SDLEventExtensions
{
    /// <summary>
    /// Determines whether the event is a key down event with the specified scancode.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="code">The scancode to compare.</param>
    /// <returns><see langword="true"/> if the event is a key down event with the specified scancode; otherwise, <see langword="false"/>.</returns>
    public static bool IsKeyDown(this SDLEvent e, Keyboard.Scancode code)
        => e.Type is EventType.KeyDown && e.Keyboard.Code == code;

    /// <summary>
    /// Determines whether the event is a key down event with the specified keycode.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="key">The keycode to compare.</param>
    /// <returns><see langword="true"/> if the event is a key down event with the specified keycode; otherwise, <see langword="false"/>.</returns>
    public static bool IsKeyDown(this SDLEvent e, Keyboard.Keycode key)
        => e.Type is EventType.KeyDown && e.Keyboard.Key == key;

    /// <summary>
    /// Determines whether the event is a key up event with the specified scancode.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="code">The scancode to compare.</param>
    /// <returns><see langword="true"/> if the event is a key up event with the specified scancode; otherwise, <see langword="false"/>.</returns>
    public static bool IsKeyUp(this SDLEvent e, Keyboard.Scancode code)
        => e.Type is EventType.KeyUp && e.Keyboard.Code == code;

    /// <summary>
    /// Determines whether the event is a key up event with the specified keycode.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="key">The keycode to compare.</param>
    /// <returns><see langword="true"/> if the event is a key up event with the specified keycode; otherwise, <see langword="false"/>.</returns>
    public static bool IsKeyUp(this SDLEvent e, Keyboard.Keycode key)
        => e.Type is EventType.KeyUp && e.Keyboard.Key == key;

    /// <summary>
    /// Determines whether the event is a mouse button down event with the specified button.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="button">The button to compare.</param>
    /// <returns><see langword="true"/> if the event is a mouse button down event with the specified button; otherwise, <see langword="false"/>.</returns>
    public static bool IsMouseButtonDown(this SDLEvent e, Mouse.Button button)
        => e.Type is EventType.MouseButtonDown && e.Mouse.Button == button;

    /// <summary>
    /// Determines whether the event is a mouse button up event with the specified button.
    /// </summary>
    /// <param name="e">The SDL event.</param>
    /// <param name="button">The button to compare.</param>
    /// <returns><see langword="true"/> if the event is a mouse button up event with the specified button; otherwise, <see langword="false"/>.</returns>
    public static bool IsMouseButtonUp(this SDLEvent e, Mouse.Button button)
        => e.Type is EventType.MouseButtonUp && e.Mouse.Button == button;

    /// <summary>
    /// Requests to quit the application.
    /// </summary>
    /// <remarks>
    /// It checks if is a <see cref="EventType.Quit"/> event or the specified key is pressed.
    /// By default, it checks if the <see cref="Keyboard.Scancode.Escape"/> key is pressed.
    /// </remarks>
    /// <param name="e">The SDL event.</param>
    /// <param name="code">The scancode of the key to compare.</param>
    /// <returns><see langword="true"/> if is a <see cref="EventType.Quit"/> event or the escape key is pressed; otherwise, <see langword="false"/>.</returns>
    public static bool RequestQuit(this SDLEvent e, Keyboard.Scancode code = Keyboard.Scancode.Escape)
        => e.Type is EventType.Quit || IsKeyDown(e, code);

    /// <summary>
    /// Requests to quit the application for the specified window.
    /// </summary>
    /// <remarks>
    /// It checks if is a <see cref="EventType.WindowCloseRequested"/> event for the specified window.
    /// It is not same as <see cref="RequestQuit(SDLEvent, Keyboard.Scancode)"/> which is the last event in the event loop before quitting the application.
    /// </remarks>
    /// <param name="e">The SDL event.</param>
    /// <param name="windowId">The ID of the window to compare.</param>
    /// <returns><see langword="true"/> if is a <see cref="EventType.WindowCloseRequested"/> event for the specified window; otherwise, <see langword="false"/>.</returns>
    public static bool RequestQuit(this SDLEvent e, uint windowId)
        => e.Type is EventType.WindowCloseRequested && e.Window.Id == windowId;
}
