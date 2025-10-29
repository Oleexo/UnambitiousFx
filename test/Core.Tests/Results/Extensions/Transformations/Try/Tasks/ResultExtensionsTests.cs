using UnambitiousFx.Core.Results.Reasons;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Try.Tasks;

[TestSubject(typeof(ResultTryExtensions))]
public class ResultExtensionsTests {
    #region Arity 1

    [Fact]
    public async Task TryAsync_Arity1_Result_Success_MapsValue() {
        var r = Result.Success(1);

        var mapped = await r.TryAsync(async x => {
            await Task.Yield();
            return x + 1;
        });

        if (mapped.TryGet(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity1_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int>(ex);

        var mapped = await r.TryAsync(async x => {
            await Task.Yield();
            return x + 1;
        });

        if (!mapped.TryGet(out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity1_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async x => {
            called = true;
            await Task.Yield();
            return x + 1;
        });

        _ = mapped; // silence unused
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity1_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom");
        var r      = Result.Success(123);

        var mapped = await r.TryAsync<int, int>(async _ => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity1_ValueTask_Success_MapsValue() {
        var rTask = Task.FromResult(Result.Success(1));

        var mapped = await rTask.TryAsync(async x => {
            await Task.Yield();
            return x + 1;
        });

        if (mapped.TryGet(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity1_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int>(ex));

        var mapped = await rTask.TryAsync(async x => {
            await Task.Yield();
            return x + 1;
        });

        if (!mapped.TryGet(out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity1_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async x => {
            called = true;
            await Task.Yield();
            return x + 1;
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity1_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom");
        var rTask  = Task.FromResult(Result.Success(123));

        var mapped = await rTask.TryAsync<int, int>(async _ => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task TryAsync_Arity2_Result_Success_MapsValues() {
        var r = Result.Success(1, 2);

        var mapped = await r.TryAsync(async (a,
                                             b) => {
            await Task.Yield();
            return (a + 10, b * 2);
        });

        if (mapped.TryGet(out var v1, out var v2, out _)) {
            Assert.Equal((11, 4), (v1, v2));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity2_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b) => {
            await Task.Yield();
            return (a + 10, b * 2);
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity2_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b) => {
            called = true;
            await Task.Yield();
            return (a + 10, b * 2);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity2_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom2");
        var r      = Result.Success(1, 2);

        var mapped = await r.TryAsync<int, int, int, int>(async (a,
                                                                 b) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity2_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2));

        var mapped = await rTask.TryAsync(async (a,
                                                 b) => {
            await Task.Yield();
            return (a + 10, b * 2);
        });

        if (mapped.TryGet(out var v1, out var v2, out _)) {
            Assert.Equal((11, 4), (v1, v2));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity2_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b) => {
            await Task.Yield();
            return (a + 10, b * 2);
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity2_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b) => {
            called = true;
            await Task.Yield();
            return (a + 10, b * 2);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity2_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom2");
        var rTask  = Task.FromResult(Result.Success(1, 2));

        var mapped = await rTask.TryAsync<int, int, int, int>(async (a,
                                                                     b) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task TryAsync_Arity3_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c) => {
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal((2, 4, 6), (v1, v2, v3));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity3_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c) => {
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity3_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity3_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom3");
        var r      = Result.Success(1, 2, 3);

        var mapped = await r.TryAsync<int, int, int, int, int, int>(async (a,
                                                                           b,
                                                                           c) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity3_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c) => {
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal((2, 4, 6), (v1, v2, v3));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity3_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c) => {
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity3_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 2, c + 3);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity3_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom3");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int>(async (a,
                                                                               b,
                                                                               c) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task TryAsync_Arity4_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3, 4);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal((2, 3, 4, 5), (v1, v2, v3, v4));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity4_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity4_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity4_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom4");
        var r      = Result.Success(1, 2, 3, 4);

        var mapped = await r.TryAsync<int, int, int, int, int, int, int, int>(async (a,
                                                                                     b,
                                                                                     c,
                                                                                     d) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity4_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal((2, 3, 4, 5), (v1, v2, v3, v4));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity4_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity4_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity4_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom4");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3, 4));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int, int, int>(async (a,
                                                                                         b,
                                                                                         c,
                                                                                         d) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task TryAsync_Arity5_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal((2, 3, 4, 5, 6), (v1, v2, v3, v4, v5));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity5_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity5_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity5_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom5");
        var r      = Result.Success(1, 2, 3, 4, 5);

        var mapped = await r.TryAsync<int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                               b,
                                                                                               c,
                                                                                               d,
                                                                                               e) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity5_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal((2, 3, 4, 5, 6), (v1, v2, v3, v4, v5));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity5_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity5_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity5_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom5");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3, 4, 5));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                   b,
                                                                                                   c,
                                                                                                   d,
                                                                                                   e) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task TryAsync_Arity6_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7), (v1, v2, v3, v4, v5, v6));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity6_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity6_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity6_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom6");
        var r      = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = await r.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                         b,
                                                                                                         c,
                                                                                                         d,
                                                                                                         e,
                                                                                                         f) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity6_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7), (v1, v2, v3, v4, v5, v6));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity6_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity6_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity6_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom6");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                             b,
                                                                                                             c,
                                                                                                             d,
                                                                                                             e,
                                                                                                             f) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task TryAsync_Arity7_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7, 8), (v1, v2, v3, v4, v5, v6, v7));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity7_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity7_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity7_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom7");
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = await r.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                                   b,
                                                                                                                   c,
                                                                                                                   d,
                                                                                                                   e,
                                                                                                                   f,
                                                                                                                   g) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity7_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7, 8), (v1, v2, v3, v4, v5, v6, v7));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity7_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity7_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity7_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom7");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                                       b,
                                                                                                                       c,
                                                                                                                       d,
                                                                                                                       e,
                                                                                                                       f,
                                                                                                                       g) => {
            await Task.Yield();
            throw thrown;
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task TryAsync_Arity8_Result_Success_MapsValues() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g,
                                             h) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7, 8, 9), (v1, v2, v3, v4, v5, v6, v7, v8));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity8_Result_SourceFailure_ErrorPropagated() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g,
                                             h) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity8_Result_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = await r.TryAsync(async (a,
                                             b,
                                             c,
                                             d,
                                             e,
                                             f,
                                             g,
                                             h) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity8_Result_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom8");
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = await r.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                                                                                                                          b,
                                                                                                                          c,
                                                                                                                          d,
                                                                                                                          e,
                                                                                                                          f,
                                                                                                                          g,
                                                                                                                          h) => {
                                                                                                                          await Task.Yield();
                                                                                                                          throw thrown;
                                                                                                                      });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity8_ValueTask_Success_MapsValues() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g,
                                                 h) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal((2, 3, 4, 5, 6, 7, 8, 9), (v1, v2, v3, v4, v5, v6, v7, v8));
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task TryAsync_Arity8_ValueTask_SourceFailure_ErrorPropagated() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g,
                                                 h) => {
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task TryAsync_Arity8_ValueTask_SourceFailure_FuncNotInvoked() {
        var ex     = new Exception("boom");
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));
        var called = false;

        var mapped = await rTask.TryAsync(async (a,
                                                 b,
                                                 c,
                                                 d,
                                                 e,
                                                 f,
                                                 g,
                                                 h) => {
            called = true;
            await Task.Yield();
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        _ = mapped;
        Assert.False(called);
    }

    [Fact]
    public async Task TryAsync_Arity8_ValueTask_FuncThrows_ErrorCaptured() {
        var thrown = new InvalidOperationException("kaboom8");
        var rTask  = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));

        var mapped = await rTask.TryAsync<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(async (a,
                             b,
                             c,
                             d,
                             e,
                             f,
                             g,
                             h) => {
                             await Task.Yield();
                             throw thrown;
                         });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err?.FirstOrDefault()?.Exception);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion
}
