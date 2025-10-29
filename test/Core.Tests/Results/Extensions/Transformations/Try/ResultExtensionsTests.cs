using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Try;

[TestSubject(typeof(ResultTryExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1

    [Fact]
    public void Try_Arity1_Success_ReturnsMapped() {
        var r = Result.Success(1);

        var mapped = r.Try(x => x + 1);

        if (mapped.TryGet(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity1_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int>(ex);
        var called = false;

        var mapped = r.Try(x => {
            called = true;
            return x + 1;
        });

        if (!mapped.TryGet(out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity1_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom");
        var r      = Result.Success(123);

        var mapped = r.Try<int, int>(x => throw thrown);

        if (!mapped.TryGet(out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 2

    [Fact]
    public void Try_Arity2_Success_ReturnsMapped() {
        var r = Result.Success(1, 2);

        var mapped = r.Try((a,
                            b) => (a + 10, b * 2));

        if (mapped.TryGet(out var value1, out var value2, out _)) {
            Assert.Equal(11, value1);
            Assert.Equal(4,  value2);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity2_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b) => {
            called = true;
            return (a + 10, b * 2);
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity2_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom2");
        var r      = Result.Success(1, 2);

        var mapped = r.Try<int, int, int, int>((a,
                                                b) => throw thrown);

        if (!mapped.TryGet(out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 3

    [Fact]
    public void Try_Arity3_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3);

        var mapped = r.Try((a,
                            b,
                            c) => (a + 1, b + 1, c + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity3_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c) => {
            called = true;
            return (a + 1, b + 1, c + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity3_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom3");
        var r      = Result.Success(1, 2, 3);

        var mapped = r.Try<int, int, int, int, int, int>((a,
                                                          b,
                                                          c) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 4

    [Fact]
    public void Try_Arity4_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4);

        var mapped = r.Try((a,
                            b,
                            c,
                            d) => (a + 1, b + 1, c + 1, d + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out var value4, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
            Assert.Equal(5, value4);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity4_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c,
                            d) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity4_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom4");
        var r      = Result.Success(1, 2, 3, 4);

        var mapped = r.Try<int, int, int, int, int, int, int, int>((a,
                                                                    b,
                                                                    c,
                                                                    d) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 5

    [Fact]
    public void Try_Arity5_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e) => (a + 1, b + 1, c + 1, d + 1, e + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
            Assert.Equal(5, value4);
            Assert.Equal(6, value5);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity5_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity5_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom5");
        var r      = Result.Success(1, 2, 3, 4, 5);

        var mapped = r.Try<int, int, int, int, int, int, int, int, int, int>((a,
                                                                              b,
                                                                              c,
                                                                              d,
                                                                              e) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 6

    [Fact]
    public void Try_Arity6_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
            Assert.Equal(5, value4);
            Assert.Equal(6, value5);
            Assert.Equal(7, value6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity6_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity6_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom6");
        var r      = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = r.Try<int, int, int, int, int, int, int, int, int, int, int, int>((a,
                                                                                        b,
                                                                                        c,
                                                                                        d,
                                                                                        e,
                                                                                        f) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 7

    [Fact]
    public void Try_Arity7_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f,
                            g) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
            Assert.Equal(5, value4);
            Assert.Equal(6, value5);
            Assert.Equal(7, value6);
            Assert.Equal(8, value7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity7_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f,
                            g) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity7_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom7");
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = r.Try<int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a,
                                                                                                  b,
                                                                                                  c,
                                                                                                  d,
                                                                                                  e,
                                                                                                  f,
                                                                                                  g) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 8

    [Fact]
    public void Try_Arity8_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f,
                            g,
                            h) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));

        if (mapped.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8, out _)) {
            Assert.Equal(2, value1);
            Assert.Equal(3, value2);
            Assert.Equal(4, value3);
            Assert.Equal(5, value4);
            Assert.Equal(6, value5);
            Assert.Equal(7, value6);
            Assert.Equal(8, value7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Try_Arity8_Failure_DoesNotInvokeFunc_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Try((a,
                            b,
                            c,
                            d,
                            e,
                            f,
                            g,
                            h) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Try_Arity8_Throws_CapturedAsFailure() {
        var thrown = new InvalidOperationException("kaboom8");
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = r.Try<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>((a,
                                                                                                            b,
                                                                                                            c,
                                                                                                            d,
                                                                                                            e,
                                                                                                            f,
                                                                                                            g,
                                                                                                            h) => throw thrown);

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Same(thrown, err);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion
}
