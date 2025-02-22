// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Aquila.Setup.Commands;
using Spectre.Console.Cli;

CommandApp app = new();

app.Configure(c =>
{
    IBranchConfigurator install = c.AddBranch("install", ctx =>
    {
        ctx.AddCommand<SdlInstallation>(SdlInstallation.Name)
           .WithDescription(SdlInstallation.Description)
           .WithExample("install sdl -s -c debug")
           .WithExample("install sdl --with-image")
           .WithExample("install sdl --all")
           .WithExample("i sdl -s -c debug");
    });

    install.WithAlias("i");

    c.AddCommand<Sandbox>(Sandbox.Name)
        .WithDescription(Sandbox.Description)
        .WithExample("sandbox -sc debug");
});

int exitCode = await app.RunAsync(args).ConfigureAwait(false);
return exitCode;
