// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Spectre.Console;
using System.Diagnostics;

namespace Aquila.Setup.Processes;

internal abstract class ProcessHandler(string command)
{
    private readonly Status _status = AnsiConsole.Status()
        .Spinner(Spinner.Known.BouncingBar)
        .SpinnerStyle(Style.Parse("cyan"));

    private ProcessHandler? _nextHandler;

    public ProcessHandler Next(ProcessHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual ProcessResult Run(ProcessContext context)
    {
        if (_nextHandler is not null)
            return _nextHandler.Run(context);

        return ProcessResult.Success();
    }

    protected bool Execute(string message, bool silent, string arguments = "", string workingDirectory = "", bool wait = true)
    {
        if (silent)
            return _status.Start(message, _ => SilentExecute(arguments, workingDirectory, wait));

        return Execute(arguments, workingDirectory, wait);
    }

    private bool Execute(string arguments, string workingDirectory, bool wait)
    {
        using Process process = new();

        process.StartInfo = CommandSettings(arguments, workingDirectory, silent: false);
        process.Start();

        if (!wait)
        {
            process.Dispose();
            return true;
        }

        process.WaitForExit();

        return process.ExitCode == 0;
    }

    private bool SilentExecute(string arguments, string workingDirectory, bool wait)
    {
        using Process process = new();

        process.StartInfo = CommandSettings(arguments, workingDirectory, silent: true);
        process.Start();

        if (!wait)
        {
            process.Dispose();
            return true;
        }

        while (!process.StandardOutput.EndOfStream)
            process.StandardOutput.ReadLine();

        return process.ExitCode == 0;
    }

    private ProcessStartInfo CommandSettings(string arguments, string workingDirectory, bool silent)
    {
        ProcessStartInfo info = new()
        {
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
        };

        if (silent)
        {
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.UseShellExecute = false;
        }

        return info;
    }
}
