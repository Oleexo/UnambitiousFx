using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static class ResultHasExceptionExtensions {
    /// <summary>
    /// Determines whether the result contains an error with an exception of the specified type.
    /// </summary>
    /// <typeparam name="TException">The type of exception to check for.</typeparam>
    /// <param name="result">The result to check for exceptions.</param>
    /// <returns>true if the result contains an error with an exception of the specified type; otherwise, false.</returns>
    public static bool HasException<TException>(this Result result)
        where TException : Exception {
        return !result.IsSuccess &&
               result.Reasons.OfType<IError>()
                     .Any(x => x.Exception is TException);
    }
}
