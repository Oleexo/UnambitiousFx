namespace UnambitiousFx.Core.Results;

/// <summary>
/// Extensions enabling MapError chain policies (short-circuit or accumulate) without changing the existing MapError contract.
/// </summary>
public static class ResultExtensionsMapErrorChainPolicy {
    // Arity 0
    public static Result MapError(this Result result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 1
    public static Result<T1> MapError<T1>(this Result<T1> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 2
    public static Result<T1, T2> MapError<T1, T2>(this Result<T1, T2> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 3
    public static Result<T1, T2, T3> MapError<T1, T2, T3>(this Result<T1, T2, T3> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 4
    public static Result<T1, T2, T3, T4> MapError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 5
    public static Result<T1, T2, T3, T4, T5> MapError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 6
    public static Result<T1, T2, T3, T4, T5, T6> MapError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 7
    public static Result<T1, T2, T3, T4, T5, T6, T7> MapError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull where T7 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    // Arity 8
    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> MapError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result, Func<Exception, Exception> mapError, MapErrorChainPolicy policy) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull where T7 : notnull where T8 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (mapError == null) throw new ArgumentNullException(nameof(mapError));
        if (result.IsSuccess) return result; // no-op
        return policy switch {
            MapErrorChainPolicy.ShortCircuit => result.MapError(mapError),
            MapErrorChainPolicy.Accumulate => Accumulate(result, mapError),
            _ => throw new ArgumentOutOfRangeException(nameof(policy), policy, null)
        };
    }

    private static Result Accumulate(Result original, Func<Exception, Exception> mapError) {
        original.Ok(out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure(newEx);
        // Copy prior reasons + metadata (accumulation semantics)
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1> Accumulate<T1>(Result<T1> original, Func<Exception, Exception> mapError) where T1 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2> Accumulate<T1, T2>(Result<T1, T2> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3> Accumulate<T1, T2, T3>(Result<T1, T2, T3> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3, T4> Accumulate<T1, T2, T3, T4>(Result<T1, T2, T3, T4> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5> Accumulate<T1, T2, T3, T4, T5>(Result<T1, T2, T3, T4, T5> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6> Accumulate<T1, T2, T3, T4, T5, T6>(Result<T1, T2, T3, T4, T5, T6> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6, T7> Accumulate<T1, T2, T3, T4, T5, T6, T7>(Result<T1, T2, T3, T4, T5, T6, T7> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull where T7 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6, T7>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6, T7, T8> Accumulate<T1, T2, T3, T4, T5, T6, T7, T8>(Result<T1, T2, T3, T4, T5, T6, T7, T8> original, Func<Exception, Exception> mapError) where T1 : notnull where T2 : notnull where T3 : notnull where T4 : notnull where T5 : notnull where T6 : notnull where T7 : notnull where T8 : notnull {
        original.Ok(out _, out var existingError);
        var newEx = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(newEx);
        foreach (var r in original.Reasons) mapped.AddReason(r);
        foreach (var kv in original.Metadata) mapped.AddMetadata(kv.Key, kv.Value);
        return mapped;
    }
}
