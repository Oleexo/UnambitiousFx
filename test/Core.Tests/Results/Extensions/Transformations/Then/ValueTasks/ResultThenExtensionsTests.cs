using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Then.ValueTasks;

[TestSubject(typeof(ResultThenExtensions))]
public class ResultThenExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task ThenAsync_Arity1_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1);

        var mapped = await r.ThenAsync(x => new ValueTask<Result<int>>(Result.Success(x + 1)));

        if (mapped.Ok(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity1_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int>(ex);
        var called = false;

        var mapped = await r.ThenAsync(x => {
            called = true;
            return new ValueTask<Result<int>>(Result.Success(x + 1));
        });

        if (!mapped.Ok(out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity1_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(1));

        var mapped = await awaitable.ThenAsync(x => Result.Success(x + 1));

        if (mapped.Ok(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity1_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(1));

        var mapped = await awaitable.ThenAsync(x => new ValueTask<Result<int>>(Result.Success(x + 1)));

        if (mapped.Ok(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    #region Arity 2
      [Fact]
    public async Task ThenAsync_Arity2_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "test");

        var mapped = await r.ThenAsync((x, y) => new ValueTask<Result<int, string>>(Result.Success(x + 1, y + "42")));

        if (mapped.Ok(out var v1, out var v2, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("test42", v2);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity2_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y) => {
            called = true;
            return new ValueTask<Result<int, string>>(Result.Success(x + 1, y + "42"));
        });

        if (!mapped.Ok(out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity2_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string>>(Result.Success(1, "test"));

        var mapped = await awaitable.ThenAsync((x, y) => Result.Success(x + 1, y + "42"));

        if (mapped.Ok(out var v1, out var v2, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("test42", v2);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity2_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string>>(Result.Success(1, "test"));

        var mapped = await awaitable.ThenAsync((x, y) => new ValueTask<Result<int, string>>(Result.Success(x + 1, y + "42")));

        if (mapped.Ok(out var v1, out var v2, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("test42", v2);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    
    #region Arity 3
      [Fact]
    public async Task ThenAsync_Arity3_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true);

        var mapped = await r.ThenAsync((x, y, z) => new ValueTask<Result<int, string, bool>>(Result.Success(x + 1, y + "42", !z)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity3_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z) => {
            called = true;
            return new ValueTask<Result<int, string, bool>>(Result.Success(x + 1, y + "42", !z));
        });

        if (!mapped.Ok(out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity3_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool>>(Result.Success(1, "a", true));

        var mapped = await awaitable.ThenAsync((x, y, z) => Result.Success(x + 1, y + "42", !z));

        if (mapped.Ok(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity3_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool>>(Result.Success(1, "a", true));

        var mapped = await awaitable.ThenAsync((x, y, z) => new ValueTask<Result<int, string, bool>>(Result.Success(x + 1, y + "42", !z)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task ThenAsync_Arity4_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5);

        var mapped = await r.ThenAsync((x, y, z, w) => new ValueTask<Result<int, string, bool, double>>(Result.Success(x + 1, y + "42", !z, w + 0.5)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity4_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z, w) => {
            called = true;
            return new ValueTask<Result<int, string, bool, double>>(Result.Success(x + 1, y + "42", !z, w + 0.5));
        });

        if (!mapped.Ok(out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity4_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double>>(Result.Success(1, "a", true, 2.5));

        var mapped = await awaitable.ThenAsync((x, y, z, w) => Result.Success(x + 1, y + "42", !z, w + 0.5));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity4_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double>>(Result.Success(1, "a", true, 2.5));

        var mapped = await awaitable.ThenAsync((x, y, z, w) => new ValueTask<Result<int, string, bool, double>>(Result.Success(x + 1, y + "42", !z, w + 0.5)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    
    #region Arity 5
      [Fact]
    public async Task ThenAsync_Arity5_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x');

        var mapped = await r.ThenAsync((x, y, z, w, c) => new ValueTask<Result<int, string, bool, double, char>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1))));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity5_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z, w, c) => {
            called = true;
            return new ValueTask<Result<int, string, bool, double, char>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1)));
        });

        if (!mapped.Ok(out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity5_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char>>(Result.Success(1, "a", true, 2.5, 'x'));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity5_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char>>(Result.Success(1, "a", true, 2.5, 'x'));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c) => new ValueTask<Result<int, string, bool, double, char>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1))));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    
    #region Arity 6
     [Fact]
    public async Task ThenAsync_Arity6_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L);

        var mapped = await r.ThenAsync((x, y, z, w, c, l) => new ValueTask<Result<int, string, bool, double, char, long>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity6_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z, w, c, l) => {
            called = true;
            return new ValueTask<Result<int, string, bool, double, char, long>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1));
        });

        if (!mapped.Ok(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity6_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long>>(Result.Success(1, "a", true, 2.5, 'x', 100L));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity6_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long>>(Result.Success(1, "a", true, 2.5, 'x', 100L));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l) => new ValueTask<Result<int, string, bool, double, char, long>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    
    #region Arity 7
     [Fact]
    public async Task ThenAsync_Arity7_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m);

        var mapped = await r.ThenAsync((x, y, z, w, c, l, d) => new ValueTask<Result<int, string, bool, double, char, long, decimal>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity7_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long, decimal>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z, w, c, l, d) => {
            called = true;
            return new ValueTask<Result<int, string, bool, double, char, long, decimal>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m));
        });

        if (!mapped.Ok(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity7_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long, decimal>>(Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l, d) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity7_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long, decimal>>(Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l, d) => new ValueTask<Result<int, string, bool, double, char, long, decimal>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
    
    #region Arity 8
     [Fact]
    public async Task ThenAsync_Arity8_FromResult_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m, 1.25f);

        var mapped = await r.ThenAsync((x, y, z, w, c, l, d, f) => new ValueTask<Result<int, string, bool, double, char, long, decimal, float>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
            Assert.Equal(2.0f, v8);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity8_FromResult_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long, decimal, float>(ex);
        var called = false;

        var mapped = await r.ThenAsync((x, y, z, w, c, l, d, f) => {
            called = true;
            return new ValueTask<Result<int, string, bool, double, char, long, decimal, float>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f));
        });

        if (!mapped.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity8_FromValueTaskAwaitable_WithSyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long, decimal, float>>(Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m, 1.25f));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l, d, f) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
            Assert.Equal(2.0f, v8);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ThenAsync_Arity8_FromValueTaskAwaitable_WithAsyncThen_Success_ReturnsMapped() {
        var awaitable = new ValueTask<Result<int, string, bool, double, char, long, decimal, float>>(Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m, 1.25f));

        var mapped = await awaitable.ThenAsync((x, y, z, w, c, l, d, f) => new ValueTask<Result<int, string, bool, double, char, long, decimal, float>>(Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f)));

        if (mapped.Ok(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal(2, v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
            Assert.Equal('y', v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
            Assert.Equal(2.0f, v8);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    #endregion
}
