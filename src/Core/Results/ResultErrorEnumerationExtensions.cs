using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

public static class ResultErrorEnumerationExtensions {
    /// <summary>
    ///     Returns all attached error reasons (IError) in order.
    /// </summary>
    public static IEnumerable<IError> Errors(this BaseResult result) {
        return result.Reasons.OfType<IError>();
    }

    /// <summary>
    ///     Returns the primary error reason: first non-ExceptionalError if present, otherwise the first (which may be the
    ///     ExceptionalError wrapper).
    /// </summary>
    public static IError? PrimaryError(this BaseResult result) {
        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(r => r is not ExceptionalError) ??
               result.Reasons.OfType<IError>()
                     .FirstOrDefault();
    }

    /// <summary>
    ///     Obtains the primary exception associated with the failure, if any.
    /// </summary>
    /// <param name="result">The instance of <see cref="BaseResult" /> from which the primary exception is retrieved.</param>
    /// <returns>
    ///     The primary exception if the result represents a failure; otherwise, null if the result is not faulted or no
    ///     primary exception can be determined.
    /// </returns>
    public static Exception? PrimaryException(this BaseResult result) {
        if (!result.IsFaulted) {
            return null;
        }

        if (result is IFailureResult fr) {
            return fr.PrimaryException;
        }

        return new Exception("Unknown failure");
    }
}
