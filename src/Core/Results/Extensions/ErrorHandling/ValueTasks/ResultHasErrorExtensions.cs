namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks;

public static class ResultHasErrorExtensions
{
    /// <summary>
    /// Asynchronously determines whether the result contains an error of the specified type.
    /// </summary>
    /// <typeparam name="TError">The type of error to check for. Can be an error type or exception type.</typeparam>
    /// <param name="awaitableResult">The awaitable result to check for errors.</param>
    /// <returns>A task with true if the result contains an error of the specified type; otherwise, false.</returns>
    public static async ValueTask<bool> HasErrorAsync<TError>(this ValueTask<Result> awaitableResult)
    {
        var result = await awaitableResult;
        return result.HasError<TError>();
    }

}