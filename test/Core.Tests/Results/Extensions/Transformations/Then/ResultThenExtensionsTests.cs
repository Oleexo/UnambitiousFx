using UnambitiousFx.Core.Results.Reasons;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Then;

[TestSubject(typeof(ResultThenExtensions))]
public sealed class ResultThenExtensionsTests {
    #region Arity 1

    [Fact]
    public void Then_Arity1_Success_ReturnsMapped() {
        var r = Result.Success(1);

        var mapped = r.Then(x => Result.Success(x + 1));

        if (mapped.TryGet(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity1_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int>(ex);
        var called = false;

        var mapped = r.Then(x => {
            called = true;
            return Result.Success(x + 1);
        });

        if (!mapped.TryGet(out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 2

    [Fact]
    public void Then_Arity2_Success_ReturnsMapped() {
        var r = Result.Success(1, "test");

        var mapped = r.Then((x,
                             y) => Result.Success(x + 1, y + "42"));

        if (mapped.TryGet(out var v1, out var v2, out _)) {
            Assert.Equal(2,        v1);
            Assert.Equal("test42", v2);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity2_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y) => {
            called = true;
            return Result.Success(x + 1, y + "42");
        });

        if (!mapped.TryGet(out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 3

    [Fact]
    public void Then_Arity3_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true);

        var mapped = r.Then((x,
                             y,
                             z) => Result.Success(x + 1, y + "42", !z));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out _)) {
            Assert.Equal(2,     v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity3_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z);
        });

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 4

    [Fact]
    public void Then_Arity4_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5);

        var mapped = r.Then((x,
                             y,
                             z,
                             w) => Result.Success(x + 1, y + "42", !z, w + 0.5));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out _)) {
            Assert.Equal(2,     v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0, v4);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity4_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z,
                             w) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z, w + 0.5);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 5

    [Fact]
    public void Then_Arity5_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x');

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1)));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out _)) {
            Assert.Equal(2,     v1);
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
    public void Then_Arity5_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1));
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 6

    [Fact]
    public void Then_Arity6_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L);

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out _)) {
            Assert.Equal(2,     v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0,  v4);
            Assert.Equal('y',  v5);
            Assert.Equal(101L, v6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity6_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 7

    [Fact]
    public void Then_Arity7_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m);

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l,
                             d) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out _)) {
            Assert.Equal(2,     v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0,  v4);
            Assert.Equal('y',  v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity7_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long, decimal>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l,
                             d) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion

    #region Arity 8

    [Fact]
    public void Then_Arity8_Success_ReturnsMapped() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 100L, 1.5m, 1.25f);

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l,
                             d,
                             f) => Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f));

        if (mapped.TryGet(out var v1, out var v2, out var v3, out var v4, out var v5, out var v6, out var v7, out var v8, out _)) {
            Assert.Equal(2,     v1);
            Assert.Equal("a42", v2);
            Assert.False(v3);
            Assert.Equal(3.0,  v4);
            Assert.Equal('y',  v5);
            Assert.Equal(101L, v6);
            Assert.Equal(2.0m, v7);
            Assert.Equal(2.0f, v8);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Then_Arity8_Failure_DoesNotInvokeThen_PropagatesError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, string, bool, double, char, long, decimal, float>(ex);
        var called = false;

        var mapped = r.Then((x,
                             y,
                             z,
                             w,
                             c,
                             l,
                             d,
                             f) => {
            called = true;
            return Result.Success(x + 1, y + "42", !z, w + 0.5, (char)(c + 1), l + 1, d + 0.5m, f + 0.75f);
        });

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Equal(ex, firstError.Exception);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    #endregion
}
