// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal static class CMake
{
    private const string BinaryPath = "build";

    internal static string Configure(string? arguments = null)
    {
        const string configure = $"-S . -B {BinaryPath}";

        return !string.IsNullOrEmpty(arguments)
            ? $"{configure} {arguments}"
            : configure;
    }

    internal static string Build(string configuration)
        => $"--build {BinaryPath} --config {configuration}";

    internal static string Install(string installPath, string configuration)
        => $"--install {BinaryPath} --config {configuration} --prefix {installPath}";
}
