// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Components.Menu;

/// <summary>
/// Represents the state of a menu/menu item.
/// </summary>
[Flags]
public enum MenuItemState : uint
{
    /// <summary>
    /// Specifies the menu item is grayed.
    /// </summary>
    Grayed = 0x00000001,

    /// <summary>
    /// Specifies the menu item is disabled.
    /// </summary>
    Disabled = 0x00000002,

    /// <summary>
    /// Specifies the menu item is selected.
    /// </summary>
    Checked = 0x00000008,

    /// <summary>
    /// Specifies the menu item opens a drop-down menu or submenu.
    /// </summary>
    Popup = 0x00000010,

    /// <summary>
    /// Places the item on a new line.
    /// </summary>
    MenuBreak = 0x00000040,

    /// <summary>
    /// Draws a horizontal dividing line.
    /// </summary>
    /// <remarks>
    /// It only applies to a drop-down menu, submenu or shortcut menu.
    /// </remarks>
    Separator = 0x00000800,
}
