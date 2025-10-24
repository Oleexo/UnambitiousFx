using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.Match.ValueTasks;

[TestSubject(typeof(ResultMatchExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task MatchAsync_Arity1_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(5));
        var v = await awaitable.MatchAsync(
            success: x => new ValueTask<int>(x),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(5, v);
    }

    [Fact]
    public async Task MatchAsync_Arity1_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(5));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: _ => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity1_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new InvalidOperationException("boom")));
        var v = await awaitable.MatchAsync(
            success: _ => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity1_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new InvalidOperationException("boom")));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: _ => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task MatchAsync_Arity2_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var v = await awaitable.MatchAsync(
            success: (a, b) => new ValueTask<int>(a + b),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(3, v);
    }

    [Fact]
    public async Task MatchAsync_Arity2_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity2_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity2_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task MatchAsync_Arity3_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var v = await awaitable.MatchAsync(
            success: (a, b, c) => new ValueTask<int>(a + b + c),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(6, v);
    }

    [Fact]
    public async Task MatchAsync_Arity3_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity3_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity3_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task MatchAsync_Arity4_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var v = await awaitable.MatchAsync(
            success: (a, b, c, d) => new ValueTask<int>(a + b + c + d),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(10, v);
    }

    [Fact]
    public async Task MatchAsync_Arity4_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity4_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity4_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task MatchAsync_Arity5_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var v = await awaitable.MatchAsync(
            success: (a, b, c, d, e) => new ValueTask<int>(a + b + c + d + e),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(15, v);
    }

    [Fact]
    public async Task MatchAsync_Arity5_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity5_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity5_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task MatchAsync_Arity6_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var v = await awaitable.MatchAsync(
            success: (a, b, c, d, e, f) => new ValueTask<int>(a + b + c + d + e + f),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(21, v);
    }

    [Fact]
    public async Task MatchAsync_Arity6_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity6_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity6_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task MatchAsync_Arity7_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var v = await awaitable.MatchAsync(
            success: (a, b, c, d, e, f, g) => new ValueTask<int>(a + b + c + d + e + f + g),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(28, v);
    }

    [Fact]
    public async Task MatchAsync_Arity7_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity7_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity7_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task MatchAsync_Arity8_Success_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var v = await awaitable.MatchAsync(
            success: (a, b, c, d, e, f, g, h) => new ValueTask<int>(a + b + c + d + e + f + g + h),
            failure: _ => new ValueTask<int>(-1));
        Assert.Equal(36, v);
    }

    [Fact]
    public async Task MatchAsync_Arity8_Success_DoesNotCallFailure() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => { called = true; return new ValueTask<int>(-1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_Arity8_Failure_ReturnsExpected() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException()));
        var v = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _, _) => new ValueTask<int>(0),
            failure: _ => new ValueTask<int>(999));
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task MatchAsync_Arity8_Failure_DoesNotCallSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException()));
        var called = false;
        _ = await awaitable.MatchAsync(
            success: (_, _, _, _, _, _, _, _) => { called = true; return new ValueTask<int>(0); },
            failure: _ => new ValueTask<int>(999));
        Assert.False(called);
    }
    #endregion
}
