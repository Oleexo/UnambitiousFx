namespace UnambitiousFx.Core.Results.Types;

/// <summary>
///     Strategy controlling how failures are merged.
/// </summary>
public enum MergeFailureStrategy
{
    /// <summary>
    ///     Accumulate all results, collecting every failure (default – previous behavior).
    /// </summary>
    AccumulateAll,

    /// <summary>
    ///     Stop at first failure encountered; subsequent results are ignored.
    /// </summary>
    FirstFailure
}
