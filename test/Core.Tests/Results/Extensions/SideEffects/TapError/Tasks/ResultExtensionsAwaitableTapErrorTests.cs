using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SideEffects.TapError.Tasks;

public sealed class ResultExtensionsAwaitableTapErrorTests {
    #region Arity 1

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity1_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(42));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity1_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity2_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity2_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity3_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity3_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity4_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity4_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity5_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity5_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity6_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity6_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity7_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity7_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity8_Success_DoesNotInvoke() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_ValueTask_Arity8_Failure_InvokesOnce() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom")));
        var called    = 0;
        await awaitable.TapErrorAsync(_ => {
            called++;
            return Task.CompletedTask;
        });
        Assert.Equal(1, called);
    }

    #endregion
}
