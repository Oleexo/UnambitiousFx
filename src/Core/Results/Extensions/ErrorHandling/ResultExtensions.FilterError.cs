using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    private static TRes FilterErrorCore<TRes>(
        BaseResult original,
        Func<IError, bool> predicate,
        Func<TRes> successFactory,
        Func<IError, TRes> failureFactory)
        where TRes : BaseResult
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (original.IsSuccess)
        {
            return (TRes)original;
        }

        var successes = original.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = original.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0)
        {
            var success = successFactory();
            if (successes.Count != 0)
            {
                success.WithReasons(successes);
            }

            if (original.Metadata.Count != 0)
            {
                success.WithMetadata(original.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = failureFactory(firstErr);
        foreach (var reason in errorsKept.Skip(1))
        {
            failure.WithReason(reason);
        }

        if (successes.Count != 0)
        {
            failure.WithReasons(successes);
        }

        if (original.Metadata.Count != 0)
        {
            failure.WithMetadata(original.Metadata);
        }

        return failure;
    }
    public static Result FilterError(this Result result,
                                     Func<IError, bool> predicate) {
        return FilterErrorCore(result,
                               predicate,
                               Result.Success,
                               Result.Failure);
    }

    public static Result<T1> FilterError<T1>(this Result<T1> result,
                                             Func<IError, bool> predicate)
        where T1 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!), // cannot recover original value
                               Result.Failure<T1>);
    }

    public static Result<T1, T2> FilterError<T1, T2>(this Result<T1, T2> result,
                                                     Func<IError, bool>  predicate)
        where T1 : notnull
        where T2 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!),
                               Result.Failure<T1, T2>);
    }

    public static Result<T1, T2, T3> FilterError<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                             Func<IError, bool>      predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!),
                               Result.Failure<T1, T2, T3>);
    }

    public static Result<T1, T2, T3, T4> FilterError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                     Func<IError, bool>          predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!),
                               Result.Failure<T1, T2, T3, T4>);
    }

    public static Result<T1, T2, T3, T4, T5> FilterError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                             Func<IError, bool>              predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!),
                               Result.Failure<T1, T2, T3, T4, T5>);
    }

    public static Result<T1, T2, T3, T4, T5, T6> FilterError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                     Func<IError, bool>                  predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!),
                               Result.Failure<T1, T2, T3, T4, T5, T6>);
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7> FilterError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                             Func<IError, bool>                      predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!, default(T7)!),
                               Result.Failure<T1, T2, T3, T4, T5, T6, T7>);
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> FilterError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                     Func<IError, bool>                          predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        return FilterErrorCore(result,
                               predicate,
                               () => Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!, default(T7)!, default(T8)!),
                               Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>);
    }
}
