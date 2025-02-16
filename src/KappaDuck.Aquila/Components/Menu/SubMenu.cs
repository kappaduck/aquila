// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila.Exceptions;
using KappaDuck.Aquila.Interop.Win32;
using KappaDuck.Aquila.Interop.Win32.Handles;
using System.Collections;
using System.Runtime.Versioning;

namespace KappaDuck.Aquila.Components.Menu;

/// <summary>
/// Represents a sub menu.
/// </summary>
/// <param name="label">The label of the sub menu.</param>
[SupportedOSPlatform("windows")]
public sealed class SubMenu(string label) : IEnumerable<MenuItem>, IDisposable
{
    private readonly List<MenuItem> _items = [];
    private MenuHandle? _handle;

    /// <summary>
    /// Gets the menu item at the specified index.
    /// </summary>
    /// <param name="index">The index of the menu item to get.</param>
    /// <returns>The menu item at the specified index.</returns>
    public MenuItem this[int index] => _items[index];

    /// <summary>
    /// Gets or sets the label of the sub menu.
    /// </summary>
    public string Label { get; set; } = label;

    /// <summary>
    /// Adds a menu item to the sub menu.
    /// </summary>
    /// <param name="item">The menu item to add.</param>
    public void Add(MenuItem item)
    {
        _items.Add(item);
        item.SetParent(this);
    }

    /// <summary>
    /// Adds a range of menu items to the sub menu.
    /// </summary>
    /// <param name="items">The menu items to add.</param>
    public void AddRange(ReadOnlySpan<MenuItem> items)
    {
        _items.AddRange(items);

        foreach (MenuItem item in items)
            item.SetParent(this);
    }

    /// <summary>
    /// Adds a range of menu items to the sub menu.
    /// </summary>
    /// <param name="items">The menu items to add.</param>
    public void AddRange(IEnumerable<MenuItem> items)
    {
        _items.AddRange(items);

        foreach (MenuItem item in items)
            item.SetParent(this);
    }

    /// <inheritdoc/>
    public void Dispose() => _handle?.Dispose();

    /// <inheritdoc/>
    public IEnumerator<MenuItem> GetEnumerator() => _items.GetEnumerator();

    internal void SetHandle(MenuHandle handle)
    {
        _handle = handle;

        foreach (MenuItem item in _items)
        {
            if (!Win32Native.AppendMenuW(handle, item.State, item.Id, item.Label))
                Win32Exception.Throw("Failed to append the menu item.");
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
