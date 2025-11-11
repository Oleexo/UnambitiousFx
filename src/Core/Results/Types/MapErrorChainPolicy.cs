namespace UnambitiousFx.Core.Results.Types;

/// <summary>
///     Policy controlling how successive MapError operations behave in a chain.
/// </summary>
public enum MapErrorChainPolicy
{
    /// <summary>
    ///     Short-circuit (default): each MapError replaces the previous failure result (current built-in behavior).
    ///     Reasons / metadata are not implicitly copied forward unless higher-level helpers do so.
    /// </summary>
    ShortCircuit = 0,

    /// <summary>
    ///     Accumulate: each MapError wraps/transforms the current exception and preserves prior reasons + metadata.
    ///     This allows building an exception/reason chain for richer diagnostics.
    /// </summary>
    Accumulate = 1
}
