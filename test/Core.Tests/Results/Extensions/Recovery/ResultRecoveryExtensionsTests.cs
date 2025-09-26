using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using TasksExt = UnambitiousFx.Core.Results.Tasks.ResultExtensions;
using ValueTasksExt = UnambitiousFx.Core.Results.ValueTasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultRecoveryExtensionsTests
{
    [Fact]
    public void Recover_Success_DoesNotInvoke_ReturnsSameValue()
    {
        var r = Result.Success(5);
        var called = false;

        var recovered = r.Recover(_ => { called = true; return 0; });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(5, value);
        Assert.False(called);
    }

    [Fact]
    public void Recover_Failure_UsesFallbackValue()
    {
        var ex = new InvalidOperationException("boom");
        var r = Result.Failure<int>(ex);
        var passedSameError = false;

        var recovered = r.Recover(err => { passedSameError = ReferenceEquals(err, ex); return 42; });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(42, value);
        Assert.True(passedSameError);
    }

    [Fact]
    public void Recover_WithConstantFallback_WhenFailure_UsesConstant()
    {
        var ex = new Exception("oops");
        var r = Result.Failure<int>(ex);

        var recovered = r.Recover(99);

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(99, value);
    }

    [Fact]
    public void RecoverWith_Success_DoesNotInvoke_ReturnsSame()
    {
        var r = Result.Success(7);
        var called = false;

        var recovered = r.RecoverWith(_ => { called = true; return Result.Success(0); });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(7, value);
        Assert.False(called);
    }

    [Fact]
    public void RecoverWith_Failure_UsesAlternateResult()
    {
        var ex = new Exception("nope");
        var r = Result.Failure<int>(ex);

        var recovered = r.RecoverWith(_ => Result.Success(123));

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(123, value);
    }

    // Async (Task)
    [Fact]
    public async Task RecoverAsync_Task_Success_DoesNotInvoke_ReturnsSame()
    {
        var r = Result.Success(10);
        var called = false;

        var recovered = await TasksExt.RecoverAsync(r, _ => { called = true; return Task.FromResult(0); });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(10, value);
        Assert.False(called);
    }

    [Fact]
    public async Task RecoverAsync_Task_Failure_UsesFallback()
    {
        var ex = new Exception("tfail");
        var r = Result.Failure<int>(ex);
        var observed = false;

        var recovered = await TasksExt.RecoverAsync(r, err => { observed = ReferenceEquals(err, ex); return Task.FromResult(77); });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(77, value);
        Assert.True(observed);
    }

    [Fact]
    public async Task RecoverWithAsync_Task_Failure_UsesAlternateResult()
    {
        var ex = new Exception("wf");
        var r = Result.Failure<int>(ex);

        var recovered = await TasksExt.RecoverWithAsync(r, _ => Task.FromResult(Result.Success(55)));

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(55, value);
    }

    // Async (ValueTask)
    [Fact]
    public async Task RecoverAsync_ValueTask_Success_DoesNotInvoke_ReturnsSame()
    {
        var r = Result.Success(10);
        var called = false;

        var recovered = await ValueTasksExt.RecoverAsync(r, _ => { called = true; return new ValueTask<int>(0); });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(10, value);
        Assert.False(called);
    }

    [Fact]
    public async Task RecoverAsync_ValueTask_Failure_UsesFallback()
    {
        var ex = new Exception("vtfail");
        var r = Result.Failure<int>(ex);
        var observed = false;

        var recovered = await ValueTasksExt.RecoverAsync(r, err => { observed = ReferenceEquals(err, ex); return new ValueTask<int>(88); });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(88, value);
        Assert.True(observed);
    }

    [Fact]
    public async Task RecoverWithAsync_ValueTask_Failure_UsesAlternateResult()
    {
        var ex = new Exception("wv");
        var r = Result.Failure<int>(ex);

        var recovered = await ValueTasksExt.RecoverWithAsync(r, _ => new ValueTask<Result<int>>(Result.Success(66)));

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(66, value);
    }
}
