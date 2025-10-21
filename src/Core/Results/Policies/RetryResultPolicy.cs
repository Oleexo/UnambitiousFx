// access Result/BaseResult

namespace UnambitiousFx.Core.Results.Policies;

/// <summary>
///     Retry execution policy for Result returning operations.
///     Retries on exceptions and/or faulted results based on provided predicates.
/// </summary>
internal sealed class RetryResultPolicy : IResultPolicy {
    private readonly Func<int, TimeSpan>     _delayProvider;
    private readonly Func<Exception, bool>?  _exceptionPredicate;
    private readonly int                     _maxAttempts;
    private readonly Func<BaseResult, bool>? _resultPredicate;

    public RetryResultPolicy(int                     maxAttempts,
                             Func<Exception, bool>?  exceptionPredicate,
                             Func<BaseResult, bool>? resultPredicate,
                             Func<int, TimeSpan>?    delayProvider) {
        if (maxAttempts <= 0) {
            throw new ArgumentOutOfRangeException(nameof(maxAttempts));
        }

        _maxAttempts        = maxAttempts;
        _exceptionPredicate = exceptionPredicate;
        _resultPredicate    = resultPredicate;
        _delayProvider      = delayProvider ?? (_ => TimeSpan.Zero);
    }

    public async ValueTask<Result> ExecuteAsync(Func<ValueTask<Result>> action,
                                                CancellationToken       cancellationToken = default) {
        if (action is null) {
            throw new ArgumentNullException(nameof(action));
        }

        Exception? lastException = null;
        Result?    lastResult    = null;
        for (var attempt = 1; attempt <= _maxAttempts; attempt++) {
            cancellationToken.ThrowIfCancellationRequested();
            try {
                var result = await action();
                if (result.IsSuccess) {
                    return result.WithMetadata("attempts", attempt);
                }

                lastResult = result;
                if (_resultPredicate is not null &&
                    !_resultPredicate(result)) {
                    return result.WithMetadata("attempts", attempt);
                }
            }
            catch (Exception ex) {
                lastException = ex;
                if (_exceptionPredicate is not null &&
                    !_exceptionPredicate(ex)) {
                    return Result.Failure(ex)
                                 .WithMetadata("attempts", attempt);
                }
            }

            if (attempt < _maxAttempts) {
                var delay = _delayProvider(attempt);
                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }

        if (lastResult is not null) {
            return lastResult.WithMetadata("attempts", _maxAttempts);
        }

        if (lastException is not null) {
            return Result.Failure(lastException)
                         .WithMetadata("attempts", _maxAttempts);
        }

        // Should not happen: no result and no exception
        return Result.Failure(new Exception("Retry policy produced no result."))
                     .WithMetadata("attempts", _maxAttempts);
    }

    public async ValueTask<Result<T>> ExecuteAsync<T>(Func<ValueTask<Result<T>>> action,
                                                      CancellationToken          cancellationToken = default)
        where T : notnull {
        if (action is null) {
            throw new ArgumentNullException(nameof(action));
        }

        Exception? lastException = null;
        Result<T>? lastResult    = null;
        for (var attempt = 1; attempt <= _maxAttempts; attempt++) {
            cancellationToken.ThrowIfCancellationRequested();
            try {
                var result = await action();
                if (result.IsSuccess) {
                    return result.WithMetadata("attempts", attempt);
                }

                lastResult = result;
                if (_resultPredicate is not null &&
                    !_resultPredicate(result)) {
                    return result.WithMetadata("attempts", attempt);
                }
            }
            catch (Exception ex) {
                lastException = ex;
                if (_exceptionPredicate is not null &&
                    !_exceptionPredicate(ex)) {
                    return Result.Failure<T>(ex)
                                 .WithMetadata("attempts", attempt);
                }
            }

            if (attempt < _maxAttempts) {
                var delay = _delayProvider(attempt);
                if (delay > TimeSpan.Zero) {
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }

        if (lastResult is not null) {
            return lastResult.WithMetadata("attempts", _maxAttempts);
        }

        if (lastException is not null) {
            return Result.Failure<T>(lastException)
                         .WithMetadata("attempts", _maxAttempts);
        }

        return Result.Failure<T>(new Exception("Retry policy produced no result."))
                     .WithMetadata("attempts", _maxAttempts);
    }
}

public static partial class ResultPolicies {
    // make partial to extend in timeout file
    /// <summary>Create a retry policy.</summary>
    /// <param name="maxAttempts">Maximum attempts (>=1).</param>
    /// <param name="exceptionFilter">If provided, only exceptions matching this predicate are retried.</param>
    /// <param name="resultFilter">If provided, only faulted results matching this predicate are retried.</param>
    /// <param name="delayProvider">Delay function taking attempt number (1-based) returning delay before next attempt.</param>
    public static IResultPolicy Retry(int                     maxAttempts,
                                      Func<Exception, bool>?  exceptionFilter = null,
                                      Func<BaseResult, bool>? resultFilter    = null,
                                      Func<int, TimeSpan>?    delayProvider   = null) {
        return new RetryResultPolicy(maxAttempts, exceptionFilter, resultFilter, delayProvider);
    }
}
