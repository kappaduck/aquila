// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Processes;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace Aquila.Setup.Commands;

internal sealed class SDLInstall : Command<SDLInstall.Settings>
{
    public const string Name = "sdl";
    public const string Description = "Setup/Install SDL and his extensions";

    private readonly string _workingDirectoryPath;
    private readonly string _installPath;
    private readonly Table _table;

    public SDLInstall()
    {
        _workingDirectoryPath = Directory.GetCurrentDirectory();

        _installPath = Path.Combine(_workingDirectoryPath, "SDL3");
        _table = CreateInstalledLibrariesTable();
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (Directory.Exists(_installPath) && !settings.Force)
        {
            AnsiConsole.MarkupLine("[yellow]SDL3 is already installed. Use --force/-f to reinstall.[/]");
            return 0;
        }

        if (!TryInstallSDL(settings))
            return 1;

        if (settings.WithImage.IsSet && !TryInstallSDLImage(settings))
            return 1;

        AnsiConsole.Write(_table);

        return 0;
    }

    private bool TryInstallSDL(Settings settings)
    {
        Uri repository = new("https://github.com/libsdl-org/SDL");

        ProcessContext context = new()
        {
            Configuration = settings.Configuration,
            SourcePath = new DirectoryInfo(Path.Combine(_workingDirectoryPath, repository.Segments[^1])),
            Silent = settings.Silent
        };

        ProcessHandler processes = new GitClone(repository, $"release-{settings.Version}");

        processes.Next(new CMakeConfigure())
                 .Next(new CMakeBuild())
                 .Next(new CMakeInstall(_installPath));

        ProcessResult result = processes.Run(context);

        Cleanup(context.SourcePath);

        if (!result.IsSuccess)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{result.Error}[/]");
            return false;
        }

        _table.AddRow("SDL3", settings.Version);
        return true;
    }

    private bool TryInstallSDLImage(Settings settings)
    {
        Uri repository = new("https://github.com/libsdl-org/SDL_image");

        ProcessContext context = new()
        {
            Configuration = settings.Configuration,
            SourcePath = new DirectoryInfo(Path.Combine(_workingDirectoryPath, repository.Segments[^1])),
            Silent = settings.Silent
        };

        ProcessHandler processes = new GitClone(repository, $"release-{settings.WithImage.Value}");

        processes.Next(new CMakeConfigure($"-DSDL3_DIR={Path.Combine(_installPath, "cmake")} -DSDLIMAGE_VENDORED=OFF"))
                 .Next(new CMakeBuild())
                 .Next(new CMakeInstall(_installPath));

        ProcessResult result = processes.Run(context);

        Cleanup(context.SourcePath);

        if (!result.IsSuccess)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]{result.Error}[/]");
            return false;
        }

        _table.AddRow(repository.Segments[^1], settings.WithImage.Value);
        return true;
    }

    private static void Cleanup(DirectoryInfo repositoryPath)
    {
        if (Directory.Exists(repositoryPath.FullName))
        {
            AnsiConsole.MarkupLine($"[gray]Cleaning up {repositoryPath.Name} repository...[/]");

            foreach (FileInfo file in repositoryPath.GetFiles("*", SearchOption.AllDirectories))
                file.Attributes = FileAttributes.Normal;

            repositoryPath.Delete(recursive: true);
        }
    }

    private static Table CreateInstalledLibrariesTable()
    {
        Table table = new();

        table.Title(new TableTitle("Installed SDL Libraries", new Style(foreground: Color.Green)));
        table.Border(TableBorder.Ascii);
        table.AddColumn("Library", c => c.Alignment = Justify.Center);
        table.AddColumn("Version", c => c.Alignment = Justify.Center);
        table.Width(25);

        return table;
    }

    internal sealed class Settings : CommandSettings
    {
        [CommandOption("-v|--version")]
        [Description("SDL3 version to install")]
        [DefaultValue("3.2.0")]
        public required string Version { get; init; }

        [CommandOption("-c|--configuration")]
        [Description("Build configuration to use")]
        [DefaultValue("release")]
        public required string Configuration { get; init; }

        [CommandOption("-f|--force")]
        [Description("Indicating whether to force an installation")]
        public bool Force { get; init; }

        [CommandOption("-s|--silent")]
        [Description("Indicating whether to suppress the output")]
        public bool Silent { get; init; }

        [CommandOption("--with-image [version]")]
        [Description("Install SDL_image with an optional version")]
        [DefaultValue("3.2.0")]
        public required FlagValue<string> WithImage { get; init; }
    }
}
