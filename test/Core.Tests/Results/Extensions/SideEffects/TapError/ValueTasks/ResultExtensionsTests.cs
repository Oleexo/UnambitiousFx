using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SideEffects.TapError.ValueTasks;

[TestSubject(typeof(ResultTapErrorExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task TapErrorAsync_Arity1_Success_DoesNotInvoke() {
        var r = Result.Success(1);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity1_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity1_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task TapErrorAsync_Arity2_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity2_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity2_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task TapErrorAsync_Arity3_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity3_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity3_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task TapErrorAsync_Arity4_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3, 4);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity4_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity4_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task TapErrorAsync_Arity5_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity5_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity5_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task TapErrorAsync_Arity6_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity6_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity6_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task TapErrorAsync_Arity7_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity7_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity7_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task TapErrorAsync_Arity8_Success_DoesNotInvoke() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity8_Failure_InvokesOnce() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = 0;
        await r.TapErrorAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapErrorAsync_Arity8_Failure_CapturesSameException() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        Exception? captured = null;
        await r.TapErrorAsync(e => { captured = e; return new ValueTask(); });
        Assert.Same(ex, captured);
    }
    #endregion
}
