// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Cake.CMake;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;
using System.Diagnostics;

namespace Setup.Tasks;

/// <summary>
/// Build and run sandbox project.
/// </summary>
[TaskName("sandbox")]
public sealed class SandboxTask : FrostingTask<SetupArguments>
{
    private const string BuildPath = "build";

    private readonly DirectoryPath _sandboxPath = new("src/sandbox");

    /// <inheritdoc/>
    public override void OnError(Exception exception, SetupArguments context)
    {
        context.Error("An error occurred while building sandbox.");
        context.Error(exception.Message);
    }

    /// <inheritdoc/>
    public override void Run(SetupArguments context)
    {
        context.Information("Building sandbox...");
        context.CMake(_sandboxPath, new CMakeSettings
        {
            ArgumentCustomization = args => args.Append($"-S . -B {BuildPath}"),
            SetupProcessSettings = settings => settings.RedirectStandardOutput = context.Silent
        });

        DirectoryPath binaryPath = _sandboxPath.Combine(BuildPath);

        context.CMakeBuild(new CMakeBuildSettings
        {
            BinaryPath = binaryPath,
            Configuration = context.BuildConfiguration,
            SetupProcessSettings = settings => settings.RedirectStandardOutput = context.Silent,
            CleanFirst = context.Force
        });

        if (context.NoRun)
            return;

        context.Information("Running sandbox...");
        using Process? process = Process.Start(new ProcessStartInfo
        {
            UseShellExecute = true,
            CreateNoWindow = false,
            FileName = GetExecutable(context, binaryPath)
        });

        static string GetExecutable(SetupArguments context, DirectoryPath binaryPath)
        {
            DirectoryPath path = context.Environment.WorkingDirectory.Combine(binaryPath).Combine(context.BuildConfiguration);

            return context.Environment.Platform.Family switch
            {
                PlatformFamily.Windows => path.CombineWithFilePath("sandbox.exe").FullPath,
                PlatformFamily.Linux => path.CombineWithFilePath("sandbox").FullPath,
                _ => throw new PlatformNotSupportedException()
            };
        }
    }
}
