using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

/// <summary>
///     Error-centric utilities: MatchError (functional pattern match on a specific error reason) and
///     FilterError (retain only errors satisfying a predicate, potentially converting a failure to success).
/// </summary>
public static partial class ResultExtensions {
    // -------------------- MatchError (non generic) --------------------
    public static TOut MatchError<TError, TOut>(this Result        result,
                                                Func<TError, TOut> onMatch,
                                                Func<TOut>         onElse)
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    // Generic arities MatchError
    public static TOut MatchError<TError, TValue1, TOut>(this Result<TValue1> result,
                                                         Func<TError, TOut>   onMatch,
                                                         Func<TOut>           onElse)
        where TValue1 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TOut>(this Result<TValue1, TValue2> result,
                                                                  Func<TError, TOut>            onMatch,
                                                                  Func<TOut>                    onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TOut>(this Result<TValue1, TValue2, TValue3> result,
                                                                           Func<TError, TOut>                     onMatch,
                                                                           Func<TOut>                             onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TOut>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                    Func<TError, TOut>                              onMatch,
                                                                                    Func<TOut>                                      onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                             Func<TError, TOut>                                       onMatch,
                                                                                             Func<TOut>                                               onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                      Func<TError, TOut>                                                onMatch,
                                                                                                      Func<TOut>                                                        onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<TError, TOut>                                                         onMatch,
        Func<TOut>                                                                 onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<TError, TOut>                                                                  onMatch,
        Func<TOut>                                                                          onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TError : class, IError {
        if (onMatch == null) {
            throw new ArgumentNullException(nameof(onMatch));
        }

        if (onElse == null) {
            throw new ArgumentNullException(nameof(onElse));
        }

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    // -------------------- FilterError (retain matching error reasons) --------------------
    public static Result FilterError(this Result        result,
                                     Func<IError, bool> predicate) {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result; // nothing to filter
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            // All errors filtered out -> success (preserve success reasons + metadata)
            var success = Result.Success();
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        // Build failure from first kept error (domain-first) fallback to its exception
        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure(firstErr);
        // Remove duplicate original firstErr addition made by Failure(IError) (already included). Add remaining kept errors + preserved successes
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    // Generic arities FilterError
    public static Result<T1> FilterError<T1>(this Result<T1>    result,
                                             Func<IError, bool> predicate)
        where T1 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!); // impossible path unless user forced failure; we cannot recover value so treat as failure->success with default
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    public static Result<T1, T2> FilterError<T1, T2>(this Result<T1, T2> result,
                                                     Func<IError, bool>  predicate)
        where T1 : notnull
        where T2 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    public static Result<T1, T2, T3> FilterError<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                             Func<IError, bool>      predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    public static Result<T1, T2, T3, T4> FilterError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                     Func<IError, bool>          predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3, T4>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    public static Result<T1, T2, T3, T4, T5> FilterError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                             Func<IError, bool>              predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3, T4, T5>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    public static Result<T1, T2, T3, T4, T5, T6> FilterError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                     Func<IError, bool>                  predicate)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3, T4, T5, T6>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
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
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!, default(T7)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3, T4, T5, T6, T7>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
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
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (result.IsSuccess) {
            return result;
        }

        var successes = result.Reasons.Where(r => r is not IError)
                              .ToList();
        var errorsKept = result.Reasons.OfType<IError>()
                               .Where(predicate)
                               .Cast<IReason>()
                               .ToList();
        if (errorsKept.Count == 0) {
            var success = Result.Success(default(T1)!, default(T2)!, default(T3)!, default(T4)!, default(T5)!, default(T6)!, default(T7)!, default(T8)!);
            if (successes.Count != 0) {
                success.WithReasons(successes);
            }

            if (result.Metadata.Count != 0) {
                success.WithMetadata(result.Metadata);
            }

            return success;
        }

        var firstErr = (IError)errorsKept[0];
        var failure  = Result.Failure<T1, T2, T3, T4, T5, T6, T7, T8>(firstErr);
        foreach (var reason in errorsKept.Skip(1)) {
            failure.WithReason(reason);
        }

        if (successes.Count != 0) {
            failure.WithReasons(successes);
        }

        if (result.Metadata.Count != 0) {
            failure.WithMetadata(result.Metadata);
        }

        return failure;
    }

    // Additional generic arities would mimic the pattern; omitted to reduce repetition in this initial utility set.
}
