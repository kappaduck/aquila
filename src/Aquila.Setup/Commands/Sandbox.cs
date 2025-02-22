// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Extensions;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;

namespace Aquila.Setup.Commands;

internal sealed class Sandbox : Command<Sandbox.Settings>
{
    private const string BuildPath = "build";

    private readonly DirectoryInfo _sandboxPath;

    public const string Name = "sandbox";

    public const string Description = "Build and run the sandbox";

    public Sandbox()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        _sandboxPath = new DirectoryInfo(Path.Combine(currentDirectory, "src/sandbox"));
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        Status status = AnsiConsole.Status()
            .Spinner(Spinner.Known.BouncingBar)
            .SpinnerStyle(Style.Parse("cyan"));

        status.Start("Building sandbox...", ctx => Build(ctx, settings));

        using Process process = Process.Start(new ProcessStartInfo { FileName = GetExecutable() })!;

        return 0;

        string GetExecutable()
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            Path.Combine(_sandboxPath.FullName, BuildPath, settings.Configuration);

            if (OperatingSystem.IsWindows())
                return Path.Combine(_sandboxPath.FullName, BuildPath, settings.Configuration, "sandbox.exe");

            return Path.Combine(_sandboxPath.FullName, BuildPath, settings.Configuration, "sandbox");
        }
    }

    private void Build(StatusContext context, Settings settings)
    {
        using (Process setup = new())
        {
            setup.Cmake($"-S . -B {BuildPath}", _sandboxPath.FullName, settings.Silent);
        }

        context.Status("Building sandbox...");

        using Process build = new();
        build.Cmake($"--build {BuildPath} --config {settings.Configuration}", _sandboxPath.FullName, settings.Silent);
    }

    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-c|--configuration")]
        [Description("Build configuration to use")]
        [DefaultValue("release")]
        public required string Configuration { get; init; }

        [CommandOption("-s|--silent")]
        [Description("Indicating whether to suppress the output of the installation")]
        public bool Silent { get; init; }
    }
}
