using System.Diagnostics;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Policies;
// corrected namespace

internal sealed class TimeoutResultPolicy : IResultPolicy
{
    private readonly TimeSpan _timeout;

    public TimeoutResultPolicy(TimeSpan timeout)
    {
        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        _timeout = timeout;
    }

    public async ValueTask<Result> ExecuteAsync(Func<ValueTask<Result>> action,
                                                CancellationToken cancellationToken = default)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var sw = Stopwatch.StartNew();
        var opTask = action();
        var delayTask = Task.Delay(_timeout, cancellationToken);
        var completed = await Task.WhenAny(opTask.AsTask(), delayTask);
        sw.Stop();
        if (completed == opTask.AsTask())
        {
            return (await opTask).WithMetadata("elapsedMs", sw.ElapsedMilliseconds);
        }

        return Result.Failure(new TimeoutError(_timeout, sw.Elapsed))
                     .WithMetadata("timeoutMs", _timeout.TotalMilliseconds)
                     .WithMetadata("elapsedMs", sw.ElapsedMilliseconds);
    }

    public async ValueTask<Result<T>> ExecuteAsync<T>(Func<ValueTask<Result<T>>> action,
                                                      CancellationToken cancellationToken = default)
        where T : notnull
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        var sw = Stopwatch.StartNew();
        var opTask = action();
        var delayTask = Task.Delay(_timeout, cancellationToken);
        var completed = await Task.WhenAny(opTask.AsTask(), delayTask);
        sw.Stop();
        if (completed == opTask.AsTask())
        {
            return (await opTask).WithMetadata("elapsedMs", sw.ElapsedMilliseconds);
        }

        return Result.Failure<T>(new TimeoutError(_timeout, sw.Elapsed))
                     .WithMetadata("timeoutMs", _timeout.TotalMilliseconds)
                     .WithMetadata("elapsedMs", sw.ElapsedMilliseconds);
    }
}

public static partial class ResultPolicies
{
    public static IResultPolicy Timeout(TimeSpan timeout)
    {
        return new TimeoutResultPolicy(timeout);
    }
}
