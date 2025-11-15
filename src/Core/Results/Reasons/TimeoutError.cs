namespace UnambitiousFx.Core.Results.Reasons;

/// <summary>Represents an operation exceeding the configured execution timeout.</summary>
public sealed record TimeoutError(TimeSpan ConfiguredTimeout,
                                  TimeSpan Elapsed)
    : ErrorBase("TIMEOUT", $"Operation exceeded timeout of {ConfiguredTimeout.TotalMilliseconds}ms after {Elapsed.TotalMilliseconds}ms.")
{
}
