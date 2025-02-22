// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Extensions;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics;

namespace Aquila.Setup.Commands;

internal sealed class SdlInstallation : Command<SdlInstallation.Settings>
{
    public const string Name = "sdl";
    public const string Description = "Setup/Install SDL";

    private const string InstallPath = "SDL3";
    private const string BinaryPath = "build";
    private const string RepositoryPath = "SDL";

    private readonly DirectoryInfo _installPath;
    private readonly DirectoryInfo _repositoryPath;

    public SdlInstallation()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        _installPath = new DirectoryInfo(Path.Combine(currentDirectory, InstallPath));
        _repositoryPath = new DirectoryInfo(Path.Combine(currentDirectory, RepositoryPath));
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (_installPath.Exists && (!settings.Force ?? false))
        {
            AnsiConsole.MarkupLine("[yellow]SDL3 is already installed. Use --force/-f to reinstall.[/]");
            return 0;
        }

        AnsiConsole.MarkupLineInterpolated($"[bold]SDL3-{settings.Version} | {settings.Configuration}[/]");

        Status status = AnsiConsole.Status()
                .Spinner(Spinner.Known.BouncingBar)
                .SpinnerStyle(Style.Parse("cyan"));

        status.Start("Cloning SDL repository...", _ => Git($"clone --depth 1 --quiet --branch release-{settings.Version} https://github.com/libsdl-org/SDL {_repositoryPath}"));
        status.Start("Prepare SDL to building...", ctx => InstallSDL(ctx, settings));

        AnsiConsole.MarkupLine("[green]SDL3 built successfully![/]");

        Cleanup();

        return 0;
    }

    private static void Git(string arguments)
    {
        using Process process = Process.Start(new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
        })!;

        process.WaitForExit();
    }

    private void InstallSDL(StatusContext context, Settings settings)
    {
        using (Process setup = new())
        {
            setup.Cmake($"-S . -B {BinaryPath}", _repositoryPath.FullName, settings.Silent);
        }

        context.Status("Building SDL...");
        using (Process build = new())
        {
            build.Cmake($"--build {BinaryPath} --config {settings.Configuration}", _repositoryPath.FullName, settings.Silent);
        }

        context.Status("Installing SDL...");

        using Process install = new();
        install.Cmake($"--install {BinaryPath} --config {settings.Configuration} --prefix {_installPath}", _repositoryPath.FullName, settings.Silent);
    }

    private void Cleanup()
    {
        if (_repositoryPath.Exists)
        {
            AnsiConsole.MarkupLine("[gray]Cleaning up SDL repository...[/]");

            foreach (FileInfo file in _repositoryPath.GetFiles("*", SearchOption.AllDirectories))
                file.Attributes = FileAttributes.Normal;

            _repositoryPath.Delete(recursive: true);
        }
    }

    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-v|--version")]
        [Description("version to install")]
        [DefaultValue("3.2.0")]
        public required string Version { get; init; }

        [CommandOption("-c|--configuration")]
        [Description("Build configuration to use")]
        [DefaultValue("release")]
        public required string Configuration { get; init; }

        [CommandOption("-f|--force")]
        [Description("Indicating whether to force an installation")]
        public bool? Force { get; init; }

        [CommandOption("-s|--silent")]
        [Description("Indicating whether to suppress the output of the installation")]
        public bool Silent { get; init; }

        [CommandOption("--with-image [version]")]
        [Description("Indicating whether to install SDL_image with an optional version")]
        public required FlagValue<string?> WithImage { get; init; }
    }
}
