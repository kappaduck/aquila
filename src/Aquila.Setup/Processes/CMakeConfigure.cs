// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal sealed class CMakeConfigure(string? arguments = null) : ProcessHandler("cmake")
{
    public override ProcessResult Run(ProcessContext context)
    {
        if (Execute($"Configuring {context.SourcePath.Name}...", context.Silent, CMake.Configure(arguments), context.SourcePath.FullName))
            return base.Run(context);

        return ProcessResult.Fail($"Failed to configuring {context.SourcePath.Name}");
    }
}
