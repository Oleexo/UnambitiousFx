using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SideEffects.Tap.ValueTasks;

[TestSubject(typeof(ResultTapExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task TapAsync_Arity1_Success_InvokesOnce() {
        var r = Result.Success(42);
        var called = 0;
        await r.TapAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity1_Failure_DoesNotInvoke() {
        var r = Result.Failure<int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync(_ => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task TapAsync_Arity2_Success_InvokesOnce() {
        var r = Result.Success(1, 2);
        var called = 0;
        await r.TapAsync((_, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity2_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task TapAsync_Arity3_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3);
        var called = 0;
        await r.TapAsync((_, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity3_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task TapAsync_Arity4_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3, 4);
        var called = 0;
        await r.TapAsync((_, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity4_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task TapAsync_Arity5_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var called = 0;
        await r.TapAsync((_, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity5_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task TapAsync_Arity6_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity6_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task TapAsync_Arity7_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity7_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task TapAsync_Arity8_Success_InvokesOnce() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(1, called);
    }

    [Fact]
    public async Task TapAsync_Arity8_Failure_DoesNotInvoke() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;
        await r.TapAsync((_, _, _, _, _, _, _, _) => { called++; return new ValueTask(); });
        Assert.Equal(0, called);
    }
    #endregion
}
