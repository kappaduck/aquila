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

    config.AddCommand<SDLInstall>(SDLInstall.Name)
          .WithDescription(SDLInstall.Description)
          .WithExample("sdl -s -c debug")
          .WithExample("sdl --silent --configuration debug")
          .WithExample("sdl -s -c debug -v 3.2.4 --with-image 3.2.0");

    config.AddCommand<Sandbox>(Sandbox.Name)
          .WithDescription(Sandbox.Description)
          .WithExample("sandbox -c debug")
          .WithExample("sandbox --silent false --configuration debug");
});

int exitCode = await app.RunAsync(args).ConfigureAwait(false);
return exitCode;
