using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using TasksExt = UnambitiousFx.Core.Results.Tasks.ResultExtensions;
using ValueTasksExt = UnambitiousFx.Core.Results.ValueTasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultTaskExtensionsTests
{
    [Fact]
    public async Task FromTask_NonGeneric_Success_ReturnsSuccess()
    {
        var task = Task.CompletedTask;

        var result = await TasksExt.FromTask(task);

        Assert.True(result.Ok(out _));
    }

    [Fact]
    public async Task FromTask_NonGeneric_Failure_ReturnsFailure()
    {
        var ex = new InvalidOperationException("boom");
        var task = Task.FromException(ex);

        var result = await TasksExt.FromTask(task);

        Assert.False(result.Ok(out var error));
        Assert.Same(ex, error);
    }

    [Fact]
    public async Task FromTask_Generic_Success_WrapsValue()
    {
        var task = Task.FromResult(42);

        var result = await TasksExt.FromTask(task);

        Assert.True(result.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public async Task FromTask_Generic_Exception_WrappedAsFailure()
    {
        var ex = new Exception("oops");
        var task = Task.FromException<int>(ex);

        var result = await TasksExt.FromTask(task);

        Assert.False(result.Ok(out int _, out var error));
        Assert.Same(ex, error);
    }

    [Fact]
    public async Task FromTask_MultiArity_Tuple_Success_WrapsValues()
    {
        var task = Task.FromResult((1, "a"));

        var result = await TasksExt.FromTask(task);

        Assert.True(result.Ok(out (int, string) values, out _));
        Assert.Equal(1, values.Item1);
        Assert.Equal("a", values.Item2);
    }

    [Fact]
    public async Task ToTask_Generic_Success_ReturnsSameResult()
    {
        var r = Result.Success(99);

        var t = TasksExt.ToTask(r);
        var result = await t;

        Assert.True(result.Ok(out var value, out _));
        Assert.Equal(99, value);
    }

    [Fact]
    public async Task ToTask_Generic_Failure_ReturnsSameError()
    {
        var ex = new Exception("fail");
        var r = Result.Failure<int>(ex);

        var t = TasksExt.ToTask(r);
        var result = await t;

        Assert.False(result.Ok(out int _, out var error));
        Assert.Same(ex, error);
    }

    [Fact]
    public async Task ToTask_NonGeneric_Success_ReturnsSame()
    {
        var r = Result.Success();

        var t = TasksExt.ToTask(r);
        var result = await t;

        Assert.True(result.Ok(out _));
    }

    // ValueTask counterparts
    [Fact]
    public async Task FromTask_ValueTask_Generic_Success_WrapsValue()
    {
        var task = new ValueTask<int>(21);

        var result = await ValueTasksExt.FromTask(task);

        Assert.True(result.Ok(out var value, out _));
        Assert.Equal(21, value);
    }

    [Fact]
    public async Task ToTask_ValueTask_Generic_Success_ReturnsSame()
    {
        var r = Result.Success(7);

        var vt     = ValueTasksExt.ToTask(r);
        var result = await vt;

        Assert.True(result.Ok(out var value, out _));
        Assert.Equal(7, value);
    }
}
