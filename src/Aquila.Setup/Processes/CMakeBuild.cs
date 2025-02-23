// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal sealed class CMakeBuild() : ProcessHandler("cmake")
{
    public override ProcessResult Run(ProcessContext context)
    {
        if (Execute($"Building {context.SourcePath.Name}...", context.Silent, CMake.Build(context.Configuration), context.SourcePath.FullName))
            return base.Run(context);

        return ProcessResult.Fail($"Failed to building {context.SourcePath.Name}");
    }
}
