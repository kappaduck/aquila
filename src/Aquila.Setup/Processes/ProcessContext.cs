// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Aquila.Setup.Processes;

internal sealed class ProcessContext
{
    public required DirectoryInfo SourcePath { get; init; }

    public bool Silent { get; init; }

    public required string Configuration { get; init; }
}
