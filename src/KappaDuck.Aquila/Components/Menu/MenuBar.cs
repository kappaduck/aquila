// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.Win32;
using KappaDuck.Aquila.Interop.Win32.Handles;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Components.Menu;

/// <summary>
/// Represents a menu bar.
/// </summary>
/// <remarks>
/// Only available on Windows.
/// </remarks>
[SupportedOSPlatform("windows")]
public sealed class MenuBar : IDisposable
{
    private readonly MenuHandle _handle;
    private readonly List<SubMenu> _menus = [];

    /// <summary>
    /// Creates an empty menu bar.
    /// </summary>
    public MenuBar() => _handle = Win32Native.CreateMenu();

    /// <summary>
    /// Creates a menu bar with the specified sub menus.
    /// </summary>
    /// <param name="menus">The sub menus to add to the menu bar.</param>
    public MenuBar(ReadOnlySpan<SubMenu> menus)
    {
        _handle = Win32Native.CreateMenu();
        AddRange(menus);
    }

    /// <summary>
    /// Gets the sub menu at the specified index.
    /// </summary>
    /// <param name="index">The index of the sub menu to get.</param>
    /// <returns>The sub menu at the specified index.</returns>
    public SubMenu this[int index] => _menus[index];

    /// <summary>
    /// Adds a sub menu to the menu bar.
    /// </summary>
    /// <param name="menu">The sub menu to add.</param>
    public void Add(SubMenu menu)
    {
        MenuHandle handle = Win32Native.CreatePopupMenu();

        menu.SetHandle(handle);
        _menus.Add(menu);

        if (!Win32Native.AppendMenuW(_handle, MenuItemState.Popup, handle, menu.Label))
            Win32Exception.Throw("Failed to append menu.");
    }

    /// <summary>
    /// Adds a sub menu to the menu bar.
    /// </summary>
    /// <param name="label">The label of the sub menu.</param>
    public void Add(string label) => Add(new SubMenu(label));

    /// <summary>
    /// Adds a range of sub menus to the menu bar.
    /// </summary>
    /// <param name="menus">The sub menus to add.</param>
    public void AddRange(ReadOnlySpan<SubMenu> menus)
    {
        foreach (SubMenu menu in menus)
        {
            MenuHandle handle = Win32Native.CreatePopupMenu();
            menu.SetHandle(handle);

            if (!Win32Native.AppendMenuW(_handle, MenuItemState.Popup, handle, menu.Label))
                Win32Exception.Throw("Failed to append menu.");
        }

        _menus.AddRange(menus);
    }

    /// <summary>
    /// Adds a range of sub menus to the menu bar.
    /// </summary>
    /// <param name="menus">The sub menus to add.</param>
    public void AddRange(IEnumerable<SubMenu> menus) => AddRange(menus.ToArray());

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (SubMenu menu in _menus)
            menu.Dispose();

        _handle.Dispose();
    }

    /// <summary>
    /// Finds a menu item by its Id.
    /// </summary>
    /// <param name="id">The Id of the menu item to find.</param>
    /// <returns>The menu item with the specified Id, or <see langword="null"/> if not found.</returns>
    public MenuItem FindMenuItem(ushort id)
    {
        foreach (SubMenu menu in _menus)
        {
            foreach (MenuItem item in menu)
            {
                if (item.Id == id)
                    return item;
            }
        }

        return default;
    }

    internal void Attach(nint window)
    {
        if (!Win32Native.SetMenu(window, _handle))
            Win32Exception.Throw("Failed to attach to the windows.");
    }

    internal static void Detach(nint window)
    {
        if (!Win32Native.SetMenu(window, MenuHandle.Zero))
            Win32Exception.Throw("Failed to detach from the windows.");

        if (!Win32Native.DrawMenuBar(window))
            Win32Exception.Throw("Failed to redraw the menu bar.");
    }
}
