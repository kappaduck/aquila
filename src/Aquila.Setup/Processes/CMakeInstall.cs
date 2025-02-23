// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal sealed class CMakeInstall(string installPath) : ProcessHandler("cmake")
{
    public override ProcessResult Run(ProcessContext context)
    {
        string arguments = CMake.Install(installPath, context.Configuration);

        if (Execute($"Installing {context.SourcePath.Name}...", context.Silent, arguments, context.SourcePath.FullName))
            return base.Run(context);

        return ProcessResult.Fail($"Failed to installing {context.SourcePath.Name}");
    }
}
