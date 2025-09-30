using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultValueOrThrowTests
{
    [Fact]
    public void ValueOrThrow_Single_Success_ReturnsValue()
    {
        var r = Result.Success(42);
        var v = r.ValueOrThrow();
        Assert.Equal(42, v);
    }

    [Fact]
    public void ValueOrThrow_Single_Failure_ThrowsOriginal()
    {
        var ex = new InvalidOperationException("boom");
        var r = Result.Failure<int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Single_Failure_FactoryTransforms()
    {
        var ex = new ArgumentException("bad");
        var r = Result.Failure<int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Single_Success_FactoryNotInvoked()
    {
        var r = Result.Success(5);
        var called = false;
        int _ = r.ValueOrThrow(e => { called = true; return new Exception(); });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_MultiArity_Success_ReturnsTuple()
    {
        var r = Result.Success(1, 2, 3);
        var tuple = r.ValueOrThrow();
        Assert.Equal((1,2,3), tuple);
    }

    [Fact]
    public void ValueOrThrow_MultiArity_Failure_Throws()
    {
        var ex = new Exception("multi");
        var r = Result.Failure<int,int,int>(ex);
        var thrown = Assert.Throws<Exception>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrow_Task_Success()
    {
        Task<Result<int>> task = Task.FromResult(Result.Success(9));
        var v = await task.ValueOrThrow();
        Assert.Equal(9, v);
    }

    [Fact]
    public async Task ValueOrThrow_Task_Failure()
    {
        var ex = new Exception("task");
        Task<Result<int>> task = Task.FromResult(Result.Failure<int>(ex));
        var thrown = await Assert.ThrowsAsync<Exception>(async () => await task.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrow_Task_Factory_Transforms()
    {
        var ex = new InvalidOperationException("taskfail");
        Task<Result<int>> task = Task.FromResult(Result.Failure<int>(ex));
        var thrown = await Assert.ThrowsAsync<ApplicationException>(async () => await task.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<InvalidOperationException>(thrown.InnerException);
    }

    [Fact]
    public async Task ValueOrThrow_ValueTask_Success()
    {
        ValueTask<Result<int>> vt = new(Result.Success(17));
        var v = await vt.ValueOrThrow();
        Assert.Equal(17, v);
    }

    [Fact]
    public async Task ValueOrThrow_ValueTask_Failure()
    {
        var ex = new Exception("vt");
        ValueTask<Result<int>> vt = new(Result.Failure<int>(ex));
        var thrown = await Assert.ThrowsAsync<Exception>(async () => await vt.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrow_ValueTask_Factory_Transforms()
    {
        var ex = new Exception("vt2");
        ValueTask<Result<int>> vt = new(Result.Failure<int>(ex));
        var thrown = await Assert.ThrowsAsync<ApplicationException>(async () => await vt.ValueOrThrow(e => new ApplicationException("wrap", e)));
        Assert.Same(ex, thrown.InnerException);
    }
}

