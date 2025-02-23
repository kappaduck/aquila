// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal sealed class GitClone(Uri repository, string version) : ProcessHandler("git")
{
    public override ProcessResult Run(ProcessContext context)
    {
        string arguments = $"clone --depth 1 --branch {version} {repository} {context.SourcePath} {(context.Silent ? "--quiet" : string.Empty)}";

        if (Execute($"Cloning {context.SourcePath.Name}...", context.Silent, arguments))
            return base.Run(context);

        return ProcessResult.Fail($"Failed to clone {context.SourcePath.Name}");
    }
}
