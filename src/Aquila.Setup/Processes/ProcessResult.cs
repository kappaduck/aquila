// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;

namespace Aquila.Setup.Processes;

internal sealed class ProcessResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    public string? Error { get; }

    private ProcessResult(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Error = message;
    }

    public static ProcessResult Success() => new(isSuccess: true, message: null);

    public static ProcessResult Fail(string message) => new(isSuccess: false, message);
}
