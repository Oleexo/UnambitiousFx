using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

internal static class Helper {
     /// <summary>
    /// Collects all errors from the result, including the primary exception.
    /// </summary>
    /// <param name="r">The result to collect errors from.</param>
    /// <param name="primary">The primary exception, if any.</param>
    /// <returns>A list of all exceptions found in the result.</returns>
    public static List<Exception> CollectErrors(BaseResult r, Exception? primary) {
        var list = new List<Exception>();
        if (primary != null) {
            list.Add(primary);
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
    
    /// <summary>
    /// Finds the primary exception in the result, preferring ExceptionalError when present.
    /// </summary>
    /// <param name="r">The result to find the primary exception in.</param>
    /// <returns>The primary exception if found, otherwise null.</returns>
    public static Exception? FindPrimaryException(BaseResult r) {
        // Prefer ExceptionalError when present (original primary from Result.Failure(Exception))
        var exceptional = r.Reasons.OfType<ExceptionalError>().FirstOrDefault();
        if (exceptional is not null) {
            return exceptional.Exception;
        }
        // Otherwise use the first domain error that carries an exception, if any
        var withEx = r.Reasons.OfType<IError>().FirstOrDefault(e => e.Exception is not null);
        return withEx?.Exception;
    }
    
    /// <summary>
    /// Copies reasons and metadata from one result to another, replacing the primary exception.
    /// </summary>
    /// <param name="from">The source result to copy from.</param>
    /// <param name="to">The target result to copy to.</param>
    /// <param name="oldPrimary">The old primary exception to replace.</param>
    /// <param name="newPrimary">The new primary exception to use as replacement.</param>
    public static void CopyReasonsAndMetadataReplacingPrimary(BaseResult from, BaseResult to, Exception? oldPrimary, Exception newPrimary) {
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
}
