using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static Result MapErrors(this Result                               result,
                                   Func<IReadOnlyList<Exception>, Exception> map) {
        if (result.TryGet(out _)) {
            return result;
        }

        result.TryGet(out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    private static List<Exception> CollectErrors(BaseResult r,
                                                 Exception? primary) {
        var list = new List<Exception>();
        if (primary != null) {
            list.Add(primary);
        }
        else if (!r.TryGet(out var e)) {
            list.Add(e);
        }

        foreach (var re in r.Reasons.OfType<IError>()) {
            if (re.Exception != null &&
                !list.Contains(re.Exception)) {
                list.Add(re.Exception);
            }
            else if (re.Exception == null) {
                list.Add(new Exception(re.Message));
            }
        }

        return list;
    }

    private static void CopyReasonsAndMetadataReplacingPrimary(BaseResult from,
                                                               BaseResult to,
                                                               Exception? oldPrimary,
                                                               Exception  newPrimary) {
        var replaced = false;
        foreach (var rs in from.Reasons) {
            if (!replaced                 &&
                rs is ExceptionalError ex &&
                oldPrimary != null        &&
                ReferenceEquals(ex.Exception, oldPrimary)) {
                to.AddReason(new ExceptionalError(newPrimary));
                replaced = true;
                continue;
            }

            to.AddReason(rs);
        }

        foreach (var kv in from.Metadata) {
            to.AddMetadata(kv.Key, kv.Value);
        }

        if (!replaced) {
            /* if there was no exceptional primary before, do not alter reason count by adding now */
        }
    }

    public static Result<TValue1> MapErrors<TValue1>(this Result<TValue1>                      result,
                                                     Func<IReadOnlyList<Exception>, Exception> map)
        where TValue1 : notnull {
        if (result.TryGet(out _, out _)) {
            return result;
        }

        result.TryGet(out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2> MapErrors<TValue1, TValue2>(this Result<TValue1, TValue2>             result,
                                                                       Func<IReadOnlyList<Exception>, Exception> map)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (result.TryGet(out _, out _)) {
            return result;
        }

        result.TryGet(out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3> MapErrors<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>    result,
                                                                                         Func<IReadOnlyList<Exception>, Exception> map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> MapErrors<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                           Func<IReadOnlyList<Exception>, Exception>       map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3, TValue4>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> MapErrors<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Func<IReadOnlyList<Exception>, Exception>                map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> MapErrors<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Func<IReadOnlyList<Exception>, Exception>                         map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> MapErrors<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<IReadOnlyList<Exception>, Exception>                                  map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out _, out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> MapErrors<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<IReadOnlyList<Exception>, Exception>                                           map)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (result.IsSuccess) {
            return result;
        }

        result.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var primary);
        var errs       = CollectErrors(result, primary);
        var newPrimary = map(errs);
        var failure    = new FailureResult<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(newPrimary, false);
        CopyReasonsAndMetadataReplacingPrimary(result, failure, primary, newPrimary);
        return failure;
    }
}
