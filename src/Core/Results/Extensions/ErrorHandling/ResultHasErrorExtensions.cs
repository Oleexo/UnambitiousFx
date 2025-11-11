using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static class ResultHasErrorExtensions
{
    /// <summary>
    /// Determines whether the result contains an error of the specified type.
    /// </summary>
    /// <typeparam name="TError">The type of error to check for. Can be an error type or exception type.</typeparam>
    /// <param name="result">The result to check for errors.</param>
    /// <returns>true if the result contains an error of the specified type; otherwise, false.</returns>
    public static bool HasError<TError>(this Result result)
    {
        if (typeof(Exception).IsAssignableFrom(typeof(TError)))
        {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }

        return result.Reasons.OfType<TError>()
                     .Any();
    }
}
