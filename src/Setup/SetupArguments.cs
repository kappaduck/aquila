// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Cake.Common;
using Cake.Core;
using Cake.Frosting;

namespace Setup;

/// <summary>
/// Setup arguments coming from the command line.
/// </summary>
/// <param name="context">The cake context.</param>
public sealed class SetupArguments(ICakeContext context) : FrostingContext(context)
{
    /// <summary>
    /// Gets the specific SDL branch/tag to build and install.
    /// </summary>
    public string Branch { get; } = GetArgument(context, "branch", "b", "release-3.2.0");

    /// <summary>
    /// Gets the configuration to build SDL.
    /// </summary>
    public string SdlConfiguration { get; } = GetArgument(context, "configuration", "c", "release");

    /// <summary>
    /// Gets a value indicating whether to force the reinstallation of SDL.
    /// </summary>
    public bool Force { get; } = HasArgument(context, "force", "f");

    /// <summary>
    /// Gets a value indicating whether to suppress the output of the installation.
    /// </summary>
    public bool Silent { get; } = HasArgument(context, "silent", "s");

    private static T GetArgument<T>(ICakeContext context, string name, string alias, T defaultValue)
    {
        return context.HasArgument(name)
            ? context.Argument<T>(name)
            : context.Argument(alias, defaultValue);
    }

    private static bool HasArgument(ICakeContext context, string name, string alias)
        => context.HasArgument(name) || context.HasArgument(alias);
}
