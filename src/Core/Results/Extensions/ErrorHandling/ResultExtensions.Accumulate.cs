namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    private static Result Accumulate(Result                     original,
                                     Func<Exception, Exception> mapError) {
        original.Ok(out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1> Accumulate<T1>(Result<T1>                 original,
                                             Func<Exception, Exception> mapError)
        where T1 : notnull {
        original.Ok(out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2> Accumulate<T1, T2>(Result<T1, T2>             original,
                                                     Func<Exception, Exception> mapError)
        where T1 : notnull
        where T2 : notnull {
        original.Ok(out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3> Accumulate<T1, T2, T3>(Result<T1, T2, T3>         original,
                                                             Func<Exception, Exception> mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        original.Ok(out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3, T4> Accumulate<T1, T2, T3, T4>(Result<T1, T2, T3, T4>     original,
                                                                     Func<Exception, Exception> mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        original.Ok(out _, out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5> Accumulate<T1, T2, T3, T4, T5>(Result<T1, T2, T3, T4, T5> original,
                                                                             Func<Exception, Exception> mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        original.Ok(out _, out _, out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6> Accumulate<T1, T2, T3, T4, T5, T6>(Result<T1, T2, T3, T4, T5, T6> original,
                                                                                     Func<Exception, Exception>     mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        original.Ok(out _, out _, out _, out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6, T7> Accumulate<T1, T2, T3, T4, T5, T6, T7>(Result<T1, T2, T3, T4, T5, T6, T7> original,
                                                                                             Func<Exception, Exception>         mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        original.Ok(out _, out _, out _, out _, out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6, T7>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }

    private static Result<T1, T2, T3, T4, T5, T6, T7, T8> Accumulate<T1, T2, T3, T4, T5, T6, T7, T8>(Result<T1, T2, T3, T4, T5, T6, T7, T8> original,
                                                                                                     Func<Exception, Exception>             mapError)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        original.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var existingError);
        var newEx  = mapError(existingError!);
        var mapped = Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(newEx);
        foreach (var r in original.Reasons) {
            mapped.AddReason(r);
        }

        foreach (var kv in original.Metadata) {
            mapped.AddMetadata(kv.Key, kv.Value);
        }

        return mapped;
    }
}
