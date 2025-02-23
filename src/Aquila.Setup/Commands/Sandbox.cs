// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Processes;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Aquila.Setup.Commands;

internal sealed class Sandbox : Command<Sandbox.Settings>
{
    public const string Name = "sandbox";
    public const string Description = "Build and run the sandbox";

    private readonly DirectoryInfo _sandboxPath = new(Path.Combine(Directory.GetCurrentDirectory(), "src/sandbox"));

    public override int Execute(CommandContext context, Settings settings)
    {
        ProcessContext ctx = new()
        {
            SourcePath = _sandboxPath,
            Configuration = settings.Configuration,
            Silent = settings.Silent
        };

        ProcessHandler processes = new CMakeConfigure();

        processes.Next(new CMakeBuild())
                 .Next(new RunSandbox(_sandboxPath.FullName, settings.Configuration));

        ProcessResult result = processes.Run(ctx);

        if (!result.IsSuccess)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{result.Error}[/]");
            return 1;
        }

        AnsiConsole.MarkupLine("[green]Sandbox built successfully![/]");
        return 0;
    }

    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-c|--configuration")]
        [Description("Build configuration to use")]
        [DefaultValue("release")]
        public required string Configuration { get; init; }

        [CommandOption("-s|--silent")]
        [Description("Indicating whether to suppress the output of the installation")]
        [DefaultValue(true)]
        public bool Silent { get; init; }
    }

    private sealed class RunSandbox(string projectPath, string configuration) : ProcessHandler(GetExecutable(projectPath, configuration))
    {
        public override ProcessResult Run(ProcessContext context)
        {
            if (Execute($"Building {context.SourcePath.Name}...", context.Silent, workingDirectory: context.SourcePath.FullName, wait: false))
                return ProcessResult.Success();

            return ProcessResult.Fail($"Failed to run {context.SourcePath.Name}");
        }

        private static string GetExecutable(string projectPath, string configuration)
        {
            string executablePath = Path.Combine(projectPath, "build", configuration);

            string executable = OperatingSystem.IsWindows() ? "sandbox.exe" : "sandbox";

            return Path.Combine(executablePath, executable);
        }
    }
}
