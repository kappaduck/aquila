// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace KappaDuck.Aquila.Components.Menu;

/// <summary>
/// Represents a menu item.
/// </summary>
/// <param name="id">The identifier of the menu item.</param>
/// <param name="label">The label of the menu item.</param>
/// <param name="state">The state of the menu item.</param>
public struct MenuItem(ushort id, string label, MenuItemState state = 0)
{
    /// <summary>
    /// Gets the identifier of the menu item.
    /// </summary>
    public readonly ushort Id { get; } = id;

    /// <summary>
    /// Gets the label of the menu item.
    /// </summary>
    public readonly string Label { get; } = label;

    /// <summary>
    /// Gets or sets the state of the menu item.
    /// </summary>
    public MenuItemState State { readonly get; set; } = state;

    /// <summary>
    /// Gets the parent sub menu of the menu item.
    /// </summary>
    public SubMenu Parent { get; private set; } = default!;

    /// <summary>
    /// Gets a separator menu item.
    /// </summary>
    public static MenuItem Separator { get; } = new(0, string.Empty, MenuItemState.Separator);

    internal void SetParent(SubMenu parent) => Parent = parent;
}
