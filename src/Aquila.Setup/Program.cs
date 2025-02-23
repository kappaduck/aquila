// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Commands;
using Spectre.Console.Cli;
using System.Reflection;

CommandApp app = new();

app.Configure(config =>
{
    Version version = Assembly.GetExecutingAssembly().GetName().Version!;

    config.SetApplicationName("quack")
          .SetApplicationVersion($"{version.Major}.{version.Minor}.{version.Revision}");

    config.AddBranch("install", AddInstallCommand).WithAlias("i");
    config.AddCommand<Sandbox>(Sandbox.Name)
          .WithDescription(Sandbox.Description)
          .WithExample("sandbox -c debug");
});

int exitCode = await app.RunAsync(args).ConfigureAwait(false);
return exitCode;

static void AddInstallCommand(IConfigurator<CommandSettings> configurator)
{
    configurator.SetDescription("Install libraries");

    configurator.AddCommand<SDLInstall>(SDLInstall.Name)
        .WithDescription(SDLInstall.Description)
        .WithExample("install sdl -s -c debug")
        .WithExample("install sdl --with-image")
        .WithExample("install sdl --all")
        .WithExample("i sdl -s -c debug");
}
