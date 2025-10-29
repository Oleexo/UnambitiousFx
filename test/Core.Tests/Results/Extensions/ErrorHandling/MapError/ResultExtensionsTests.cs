using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.MapError;

[TestSubject(typeof(Result))]
public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void MapError_Arity0_Success_NoChange() {
        var r      = Result.Success();
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        Assert.True(mapped.TryGet(out _));
        Assert.Equal(0, called);
    }

    [Fact]
    public void MapError_Arity0_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 1
    [Fact]
    public void MapError_Arity1_Success_NoChange() {
        var r      = Result.Success(1);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var value, out _)) {
            Assert.Equal(1, value);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity1_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 2
    [Fact]
    public void MapError_Arity2_Success_NoChange() {
        var r      = Result.Success(1, 2);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity2_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 3
    [Fact]
    public void MapError_Arity3_Success_NoChange() {
        var r      = Result.Success(1, 2, 3);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity3_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 4
    [Fact]
    public void MapError_Arity4_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out var d, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity4_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 5
    [Fact]
    public void MapError_Arity5_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out var d, out var e, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity5_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 6
    [Fact]
    public void MapError_Arity6_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out var d, out var e, out var f, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity6_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 7
    [Fact]
    public void MapError_Arity7_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(7, g);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity7_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 8
    [Fact]
    public void MapError_Arity8_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        });

        if (mapped.TryGet(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(7, g);
            Assert.Equal(8, h);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity8_Failure_TransformsError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
