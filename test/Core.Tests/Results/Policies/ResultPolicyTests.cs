using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Policies;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Policies;

public sealed class ResultPolicyTests
{
    [Fact]
    public async Task Retry_Succeeds_FirstAttempt()
    {
        var policy = ResultPolicies.Retry(3);
        var r = await policy.ExecuteAsync(() => Result.Success());
        Assert.True(r.IsSuccess);
        Assert.Equal(1, r.Metadata["attempts"]);
    }

    [Fact]
    public async Task Retry_Retries_OnFailure_ResultPredicate()
    {
        var attempts = 0;
        var policy = ResultPolicies.Retry(5, resultFilter: _ => true);
        var r = await policy.ExecuteAsync(() =>
        {
            attempts++;
            if (attempts < 3) return Result.Failure(new Exception("boom"));
            return Result.Success();
        });
        Assert.True(r.IsSuccess);
        Assert.Equal(3, r.Metadata["attempts"]);
    }

    [Fact]
    public async Task Retry_StopsEarly_WhenPredicateBlocks()
    {
        var attempts = 0;
        var policy = ResultPolicies.Retry(5, resultFilter: _ => false); // never retry failures
        var r = await policy.ExecuteAsync(() => { attempts++; return Result.Failure(new Exception("fail")); });
        Assert.True(r.IsFaulted);
        Assert.Equal(1, r.Metadata["attempts"]);
        Assert.Equal(1, attempts);
    }

    [Fact]
    public async Task Retry_Generic_SucceedsAfterRetries()
    {
        var attempts = 0;
        var policy = ResultPolicies.Retry(4, resultFilter: _ => true);
        var r = await policy.ExecuteAsync(() =>
        {
            attempts++;
            if (attempts < 4) return Result.Failure<int>(new Exception("no"));
            return Result.Success(42);
        });
        Assert.True(r.IsSuccess);
        Assert.Equal(42, r.Match(v => v, _ => -1));
        Assert.Equal(4, r.Metadata["attempts"]);
    }

    [Fact]
    public async Task Retry_StopsOnUnhandledException()
    {
        var attempts = 0;
        var policy = ResultPolicies.Retry(5, exceptionFilter: ex => ex is InvalidOperationException);
        var r = await policy.ExecuteAsync(() =>
        {
            attempts++;
            throw new ArgumentException("boom");
        });
        Assert.True(r.IsFaulted);
        Assert.Equal(1, r.Metadata["attempts"]);
        Assert.Equal(1, attempts);
    }

    [Fact]
    public async Task Timeout_Fails_WhenExceeded()
    {
        var policy = ResultPolicies.Timeout(TimeSpan.FromMilliseconds(50));
        var r = await policy.ExecuteAsync(async () =>
        {
            await Task.Delay(200);
            return Result.Success();
        });
        Assert.True(r.IsFaulted);
        var err = r.FindError(e => e is TimeoutError);
        Assert.NotNull(err);
    }

    [Fact]
    public async Task Timeout_Succeeds_WhenWithinLimit()
    {
        var policy = ResultPolicies.Timeout(TimeSpan.FromMilliseconds(200));
        var r = await policy.ExecuteAsync(async () =>
        {
            await Task.Delay(50);
            return Result.Success();
        });
        Assert.True(r.IsSuccess);
        Assert.True(r.Metadata.ContainsKey("elapsedMs"));
    }

    [Fact]
    public async Task Timeout_Generic_Works()
    {
        var policy = ResultPolicies.Timeout(TimeSpan.FromMilliseconds(200));
        var r = await policy.ExecuteAsync(async () =>
        {
            await Task.Delay(20);
            return Result.Success(123);
        });
        Assert.True(r.IsSuccess);
        Assert.Equal(123, r.Match(v => v, _ => -1));
    }
}
