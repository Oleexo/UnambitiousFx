using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultMapExtensionsTests {
    // Arity 1
    [Fact]
    public void Map_Arity1_Success_ReturnsMapped() {
        var r = Result.Success(1);

        var mapped = r.Map(x => x + 1);

        if (mapped.Ok(out var v, out _)) {
            Assert.Equal(2, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity1_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int>(ex);
        var called = false;

        var mapped = r.Map(x => { called = true; return x + 1; });

        if (!mapped.Ok(out var _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 2
    [Fact]
    public void Map_Arity2_Success_ReturnsMapped() {
        var r = Result.Success(1, 2);

        var mapped = r.Map((a, b) => (a + 10, b * 2));

        if (mapped.Ok(out var values, out _)) {
            var (x, y) = values;
            Assert.Equal(11, x);
            Assert.Equal(4, y);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity2_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b) => { called = true; return (a + 10, b * 2); });

        if (!mapped.Ok(out (int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 3
    [Fact]
    public void Map_Arity3_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3);

        var mapped = r.Map((a, b, c) => (a + 1, b + 1, c + 1));

        if (mapped.Ok(out var values, out _)) {
            var (x, y, z) = values;
            Assert.Equal(2, x);
            Assert.Equal(3, y);
            Assert.Equal(4, z);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity3_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c) => { called = true; return (a + 1, b + 1, c + 1); });

        if (!mapped.Ok(out (int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 4
    [Fact]
    public void Map_Arity4_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4);

        var mapped = r.Map((a, b, c, d) => (a + 1, b + 1, c + 1, d + 1));

        if (mapped.Ok(out var values, out _)) {
            var (w, x, y, z) = values;
            Assert.Equal(2, w);
            Assert.Equal(3, x);
            Assert.Equal(4, y);
            Assert.Equal(5, z);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity4_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c, d) => { called = true; return (a + 1, b + 1, c + 1, d + 1); });

        if (!mapped.Ok(out (int, int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 5
    [Fact]
    public void Map_Arity5_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var mapped = r.Map((a, b, c, d, e) => (a + 1, b + 1, c + 1, d + 1, e + 1));

        if (mapped.Ok(out var values, out _)) {
            var (v1, v2, v3, v4, v5) = values;
            Assert.Equal(2, v1);
            Assert.Equal(3, v2);
            Assert.Equal(4, v3);
            Assert.Equal(5, v4);
            Assert.Equal(6, v5);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity5_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c, d, e) => { called = true; return (a + 1, b + 1, c + 1, d + 1, e + 1); });

        if (!mapped.Ok(out (int, int, int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 6
    [Fact]
    public void Map_Arity6_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = r.Map((a, b, c, d, e, f) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));

        if (mapped.Ok(out var values, out _)) {
            var (v1, v2, v3, v4, v5, v6) = values;
            Assert.Equal(2, v1);
            Assert.Equal(3, v2);
            Assert.Equal(4, v3);
            Assert.Equal(5, v4);
            Assert.Equal(6, v5);
            Assert.Equal(7, v6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity6_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c, d, e, f) => { called = true; return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1); });

        if (!mapped.Ok(out (int, int, int, int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 7
    [Fact]
    public void Map_Arity7_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = r.Map((a, b, c, d, e, f, g) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));

        if (mapped.Ok(out var values, out _)) {
            var (v1, v2, v3, v4, v5, v6, v7) = values;
            Assert.Equal(2, v1);
            Assert.Equal(3, v2);
            Assert.Equal(4, v3);
            Assert.Equal(5, v4);
            Assert.Equal(6, v5);
            Assert.Equal(7, v6);
            Assert.Equal(8, v7);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity7_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c, d, e, f, g) => { called = true; return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1); });

        if (!mapped.Ok(out (int, int, int, int, int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 8
    [Fact]
    public void Map_Arity8_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = r.Map((a, b, c, d, e, f, g, h) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));

        if (mapped.Ok(out var values, out _)) {
            var (v1, v2, v3, v4, v5, v6, v7, v8) = values;
            Assert.Equal(2, v1);
            Assert.Equal(3, v2);
            Assert.Equal(4, v3);
            Assert.Equal(5, v4);
            Assert.Equal(6, v5);
            Assert.Equal(7, v6);
            Assert.Equal(8, v7);
            Assert.Equal(9, v8);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Map_Arity8_Failure_DoesNotInvokeMap_PropagatesError() {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = false;

        var mapped = r.Map((a, b, c, d, e, f, g, h) => { called = true; return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1); });

        if (!mapped.Ok(out (int, int, int, int, int, int, int, int) _, out var err)) {
            Assert.Equal(ex, err);
            Assert.False(called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
