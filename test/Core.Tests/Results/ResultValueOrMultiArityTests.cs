using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultValueOrMultiArityTests
{
    [Fact]
    public void ValueOr_Arity2_Success_ReturnsOriginal()
    {
        var r = Result.Success(1, 2);
        var value = r.ValueOr((9, 9));
        Assert.Equal((1, 2), value);
    }

    [Fact]
    public void ValueOr_Arity2_Failure_ReturnsFallback()
    {
        var r = Result.Failure<int, int>(new Exception("boom"));
        var value = r.ValueOr((9, 9));
        Assert.Equal((9, 9), value);
    }

    [Fact]
    public void ValueOr_Arity3_Factory_NotInvoked_OnSuccess()
    {
        var r = Result.Success(3, 4, 5);
        var called = false;
        (int,int,int) Fallback() { called = true; return (7,7,7); }
        var value = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal((3,4,5), value);
    }

    [Fact]
    public void ValueOr_Arity3_Factory_Invoked_OnFailure()
    {
        var r = Result.Failure<int,int,int>(new InvalidOperationException());
        var called = false;
        (int,int,int) Fallback() { called = true; return (1,2,3); }
        var value = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal((1,2,3), value);
    }

    [Fact]
    public async Task ValueOr_Task_Arity2_Failure_ReturnsFallback()
    {
        Task<Result<int,int>> task = Task.FromResult(Result.Failure<int,int>(new Exception()));
        var value = await task.ValueOr((5, 6));
        Assert.Equal((5,6), value);
    }

    [Fact]
    public async Task ValueOr_ValueTask_Arity3_Factory_Failure()
    {
        ValueTask<Result<int,int,int>> vt = new(Result.Failure<int,int,int>(new Exception()));
        var value = await vt.ValueOr(() => (8,9,10));
        Assert.Equal((8,9,10), value);
    }
}

