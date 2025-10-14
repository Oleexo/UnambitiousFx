namespace UnambitiousFx.Core.Results.Policies;

/// <summary>
/// Abstraction for an execution policy that wraps an operation producing a Result.
/// Policies can add behaviors like retries, timeout, etc.
/// </summary>
public interface IResultPolicy {
    /// <summary>Execute a non-generic result returning operation.</summary>
    ValueTask<Result> ExecuteAsync(Func<ValueTask<Result>> action, CancellationToken cancellationToken = default);

    /// <summary>Execute a generic Result&lt;T&gt; returning operation.</summary>
    ValueTask<Result<T>> ExecuteAsync<T>(Func<ValueTask<Result<T>>> action, CancellationToken cancellationToken = default) where T : notnull;
}

public static class ResultPolicyExtensions {
    /// <summary>Convenience overload for sync operations.</summary>
    public static ValueTask<Result> ExecuteAsync(this IResultPolicy policy, Func<Result> action, CancellationToken ct = default) {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (action is null) throw new ArgumentNullException(nameof(action));
        return policy.ExecuteAsync(() => new ValueTask<Result>(action()), ct);
    }

    /// <summary>Convenience overload for sync operations producing Result&lt;T&gt;.</summary>
    public static ValueTask<Result<T>> ExecuteAsync<T>(this IResultPolicy policy, Func<Result<T>> action, CancellationToken ct = default) where T : notnull {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (action is null) throw new ArgumentNullException(nameof(action));
        return policy.ExecuteAsync(() => new ValueTask<Result<T>>(action()), ct);
    }
}
