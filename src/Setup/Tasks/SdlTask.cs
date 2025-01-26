// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Cake.CMake;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Setup.Tasks;

/// <summary>
/// Setup task to download and install SDL.
/// </summary>
[TaskName("sdl")]
[TaskDescription("Download and install SDL")]
public sealed class SdlTask : FrostingTask<SetupArguments>
{
    private const string InstallPath = "SDL3";
    private const string BinaryPath = "build";

    private readonly DirectoryPath _repositoryPath = new("SDL");

    /// <inheritdoc/>
    public override void Finally(SetupArguments context)
    {
        if (!context.DirectoryExists(_repositoryPath))
            return;

        context.Information("Cleaning up SDL repository...");
        context.DeleteDirectory("SDL", new DeleteDirectorySettings { Recursive = true, Force = true });
    }

    /// <inheritdoc/>
    public override void OnError(Exception exception, SetupArguments context)
    {
        context.Error("An error occurred while installing SDL3.");
        context.Error(exception.Message);
    }

    /// <inheritdoc/>
    public override void Run(SetupArguments context)
    {
        if (context.DirectoryExists(InstallPath) && !context.Force)
        {
            context.Information("SDL3 is already installed. Use --force/-f to reinstall.");
            return;
        }

        context.Information($"SDL3-{GetVersion(context.Branch)} | {context.SdlConfiguration}");

        context.Information("Cloning SDL repository...");
        context.StartProcess("git", new ProcessSettings
        {
            Arguments = $"clone --depth 1 {QuietArgument(context.Silent)} --branch {context.Branch} https://github.com/libsdl-org/SDL {_repositoryPath}",
        });

        context.Information("Building SDL...");
        context.CMake(_repositoryPath, new CMakeSettings
        {
            ArgumentCustomization = args => args.Append($"-S . -B {BinaryPath}"),
            SetupProcessSettings = settings => settings.RedirectStandardOutput = context.Silent
        });

        DirectoryPath binaryPath = _repositoryPath.Combine(BinaryPath);

        context.CMakeBuild(new CMakeBuildSettings
        {
            BinaryPath = binaryPath,
            Configuration = context.SdlConfiguration,
            SetupProcessSettings = settings => settings.RedirectStandardOutput = context.Silent
        });

        context.Information("Installing SDL3...");
        context.StartProcess("cmake", new ProcessSettings
        {
            Arguments = $"--install {binaryPath} --config {context.SdlConfiguration} --prefix {InstallPath}",
            RedirectStandardOutput = context.Silent
        });

        context.Information("SDL3 installed successfully!");

        static string GetVersion(string tag)
        {
            int index = tag.IndexOf('-') + 1;
            return tag[index..];
        }

        static string QuietArgument(bool silent)
            => silent ? "--quiet" : string.Empty;
    }
}
