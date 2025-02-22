// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics;

namespace Aquila.Setup.Extensions;

internal static class ProcessExtensions
{
    internal static void Cmake(this Process process, string arguments, string workingDirectory, bool silent)
    {
        process.StartInfo = CmakeSettings(arguments, workingDirectory, silent);
        process.Start();

        if (silent)
        {
            while (!process.StandardOutput.EndOfStream)
            {
                process.StandardOutput.ReadLine();
            }
        }

        process.WaitForExit();
    }

    private static ProcessStartInfo CmakeSettings(string arguments, string workingDirectory, bool silent) => new()
    {
        FileName = "cmake",
        Arguments = arguments,
        WorkingDirectory = workingDirectory,
        RedirectStandardOutput = silent,
        UseShellExecute = false,
        CreateNoWindow = true
    };
}
