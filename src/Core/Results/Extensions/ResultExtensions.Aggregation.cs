using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Types;

namespace UnambitiousFx.Core.Results.Extensions;

/// <summary>
///     Aggregation and analytical helpers for collections of Result instances.
/// </summary>
public static partial class ResultExtensions {
    public static IEnumerable<IError> Errors(this Result result) {
        return result.Errors;
    }

    /// <summary>
    ///     Enumerates all error reasons (IError) across a set of results.
    /// </summary>
    public static IEnumerable<IError> AllErrors(this IEnumerable<BaseResult> results) {
        return results.SelectMany(r => r.Errors);
    }

    /// <summary>
    ///     Groups all error reasons across results by their error code (case-insensitive).
    /// </summary>
    public static IReadOnlyDictionary<string, IReadOnlyList<IError>> GroupByErrorCode(this IEnumerable<BaseResult> results) {
        return results.AllErrors()
                      .GroupBy(e => e.Code, StringComparer.OrdinalIgnoreCase)
                      .ToDictionary(g => g.Key, g => (IReadOnlyList<IError>)g.ToList(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Provides a compact summary of error counts per code across results.
    /// </summary>
    public static IReadOnlyDictionary<string, int> SummarizeErrors(this IEnumerable<BaseResult> results) {
        return results.AllErrors()
                      .GroupBy(e => e.Code, StringComparer.OrdinalIgnoreCase)
                      .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     Merge multiple Result instances preserving success reasons, error reasons and metadata.
    ///     If any result is a failure, the merged result is a failure. The primary exception is:
    ///     - the single failure's primary exception when only one failure exists;
    ///     - an AggregateException of all primary exceptions when multiple failures exist.
    ///     Uses <see cref="MergeFailureStrategy.AccumulateAll" />.
    /// </summary>
    public static Result Merge(this IEnumerable<Result> results) {
        return results.Merge(MergeFailureStrategy.AccumulateAll);
    }

    /// <summary>
    ///     Merge with explicit failure strategy.
    ///     Metadata collision policy: last write wins (case-insensitive keys) in encounter order.
    /// </summary>
    public static Result Merge(this IEnumerable<Result> results,
                               MergeFailureStrategy     strategy) {
        var list = results as IList<Result> ?? results.ToList();
        if (list.Count == 0) {
            return Result.Success();
        }

        if (strategy == MergeFailureStrategy.FirstFailure) {
            var successReasons = new List<IReason>();
            var metadata       = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in list) {
                foreach (var kv in r.Metadata) {
                    metadata[kv.Key] = kv.Value; // last write wins
                }

                if (!r.TryGet(out var errors)) {
                    // Create failure without auto ExceptionalError so original domain error ordering preserved
                    var failure = new FailureResult(errors);
                    if (successReasons.Count != 0) {
                        failure.WithReasons(successReasons);
                    }

                    if (metadata.Count != 0) {
                        failure.WithMetadata(metadata);
                    }

                    return failure;
                }

                // accumulate reasons from successes
                if (r.Reasons.Count != 0) {
                    successReasons.AddRange(r.Reasons.Where(rr => rr is not IError));
                }
            }

            // all success
            var successResult = Result.Success();
            if (successReasons.Count != 0) {
                successResult.WithReasons(successReasons);
            }

            if (metadata.Count != 0) {
                successResult.WithMetadata(metadata);
            }

            return successResult;
        }

        // AccumulateAll path (previous implementation)
        var successReasonsAll = new List<IReason>();
        var errorReasonsAll   = new List<IReason>();
        var metadataAll       = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        var exceptions        = new List<Exception>();

        foreach (var r in list) {
            foreach (var kv in r.Metadata) {
                metadataAll[kv.Key] = kv.Value; // last write wins
            }

            foreach (var reason in r.Reasons) {
                if (reason is IError err) {
                    if (err is not ExceptionalError) {
                        errorReasonsAll.Add(err);
                    }
                }
                else {
                    successReasonsAll.Add(reason);
                }
            }

            if (!r.TryGet(out var errors)) {
                exceptions.Add(ExtractPrimaryException(r, errors));
            }
        }

        if (exceptions.Count == 0) {
            var mergedSuccess = Result.Success();
            if (successReasonsAll.Count != 0) {
                mergedSuccess.WithReasons(successReasonsAll);
            }

            if (metadataAll.Count != 0) {
                mergedSuccess.WithMetadata(metadataAll);
            }

            return mergedSuccess;
        }

        var primaryAcc = exceptions.Count == 1
                             ? exceptions[0]
                             : new AggregateException(exceptions);
        var mergedFailureAcc = Result.Failure(primaryAcc);
        if (successReasonsAll.Count != 0) {
            mergedFailureAcc.WithReasons(successReasonsAll);
        }

        if (errorReasonsAll.Count != 0) {
            mergedFailureAcc.WithReasons(errorReasonsAll);
        }

        if (metadataAll.Count != 0) {
            mergedFailureAcc.WithMetadata(metadataAll);
        }

        return mergedFailureAcc;
    }

    private static Exception ExtractPrimaryException(BaseResult r, IEnumerable<IError> errors) {
        // Prefer ExceptionalError if present (preserves original primary when created via Result.Failure(Exception))
        var exceptional = r.Reasons.OfType<ExceptionalError>().FirstOrDefault();
        if (exceptional is not null) {
            return exceptional.Exception ?? new Exception(exceptional.Message);
        }

        // Then prefer first domain error with an exception attached
        var withEx = errors.FirstOrDefault(e => e.Exception is not null);
        if (withEx is not null) {
            return withEx.Exception ?? new Exception(withEx.Message);
        }

        // Otherwise synthesize from the first domain error message if any
        var any = errors.FirstOrDefault();
        if (any is not null) {
            return new Exception(any.Message);
        }

        // Fallback safety: shouldn't normally happen because caller checks failure
        return new Exception("Unknown error");
    }

    public static Result FirstFailureOrSuccess(this IEnumerable<Result> results) {
        foreach (var r in results) {
            if (!r.TryGet(out _)) {
                return r; // return original failure instance
            }
        }

        return Result.Success();
    }
}
