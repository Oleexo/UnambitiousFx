using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultValueOrTests {
    [Fact]
    public void ValueOr_Success_ReturnsOriginal() {
        var r = Result.Success(5);
        var v = r.ValueOr(10);
        Assert.Equal(5, v);
    }

    [Fact]
    public void ValueOr_Failure_ReturnsFallback() {
        var r = Result.Failure<int>(new Exception("boom"));
        var v = r.ValueOr(10);
        Assert.Equal(10, v);
    }

    [Fact]
    public void ValueOr_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(3);
        var called = false;

        int Fallback() {
            called = true;
            return 99;
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(3, v);
    }

    [Fact]
    public void ValueOr_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int>(new InvalidOperationException());
        var called = false;

        int Fallback() {
            called = true;
            return 77;
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(77, v);
    }

    [Fact]
    public async Task ValueOr_Task_Failure() {
        var task = Task.FromResult(Result.Failure<int>(new Exception()));
        var v    = await task.ValueOr(42);
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task ValueOr_Task_Success() {
        var task = Task.FromResult(Result.Success(11));
        var v    = await task.ValueOr(99);
        Assert.Equal(11, v);
    }

    [Fact]
    public async Task ValueOr_ValueTask_Factory() {
        ValueTask<Result<int>> vt = new(Result.Failure<int>(new Exception()));
        var                    v  = await vt.ValueOr(() => 123);
        Assert.Equal(123, v);
    }
}
