using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations.Ensure;

[TestSubject(typeof(Result<>))]
public sealed class ResultEnsureTests {
    // Arity 1
    [Fact]
    public void Ensure_Arity1_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(5);

        var ensured = r.Ensure(x => x == 5, _ => new Exception("bad"));

        if (ensured.Ok(out var v, out _)) {
            Assert.Equal(5, v);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity1_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(5);

        var ensured = r.Ensure(x => x != 5, _ => new Exception("bad"));

        if (!ensured.Ok(out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity1_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int>(ex);
        var called = 0;

        var ensured = r.Ensure(_ => {
            called++;
            return true;
        }, _ => new Exception("bad"));

        if (!ensured.Ok(out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 2
    [Fact]
    public void Ensure_Arity2_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(2, 3);

        var ensured = r.Ensure((a,
                                b) => a + b == 5, (_,
                                                   _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out _)) {
            Assert.Equal(2, a);
            Assert.Equal(3, b);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity2_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(2, 3);

        var ensured = r.Ensure((a,
                                b) => a + b == 6, (_,
                                                   _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity2_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _) => {
            called++;
            return true;
        }, (_,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 3
    [Fact]
    public void Ensure_Arity3_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 2, 3);

        var ensured = r.Ensure((a,
                                b,
                                c) => a + b + c == 6, (_,
                                                       _,
                                                       _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity3_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 2, 3);

        var ensured = r.Ensure((a,
                                b,
                                c) => a + b + c == 5, (_,
                                                       _,
                                                       _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity3_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 4
    [Fact]
    public void Ensure_Arity4_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d) => a + b + c + d == 4, (_,
                                                           _,
                                                           _,
                                                           _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out var d, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(1, b);
            Assert.Equal(1, c);
            Assert.Equal(1, d);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity4_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d) => a + b + c + d == 5, (_,
                                                           _,
                                                           _,
                                                           _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity4_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 5
    [Fact]
    public void Ensure_Arity5_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e) => a + b + c + d + e == 5, (_,
                                                               _,
                                                               _,
                                                               _,
                                                               _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out var d, out var e, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(1, b);
            Assert.Equal(1, c);
            Assert.Equal(1, d);
            Assert.Equal(1, e);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity5_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e) => a + b + c + d + e == 4, (_,
                                                               _,
                                                               _,
                                                               _,
                                                               _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity5_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 6
    [Fact]
    public void Ensure_Arity6_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f) => a + b + c + d + e + f == 6, (_,
                                                                   _,
                                                                   _,
                                                                   _,
                                                                   _,
                                                                   _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(1, b);
            Assert.Equal(1, c);
            Assert.Equal(1, d);
            Assert.Equal(1, e);
            Assert.Equal(1, f);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity6_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f) => a + b + c + d + e + f == 5, (_,
                                                                   _,
                                                                   _,
                                                                   _,
                                                                   _,
                                                                   _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity6_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _,
                                _,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _,
            _,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 7
    [Fact]
    public void Ensure_Arity7_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f,
                                g) => a + b + c + d + e + f + g == 7, (_,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(1, b);
            Assert.Equal(1, c);
            Assert.Equal(1, d);
            Assert.Equal(1, e);
            Assert.Equal(1, f);
            Assert.Equal(1, g);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity7_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f,
                                g) => a + b + c + d + e + f + g == 6, (_,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _,
                                                                       _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity7_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _,
                                _,
                                _,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _,
            _,
            _,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 8
    [Fact]
    public void Ensure_Arity8_Success_PredicateTrue_KeepsSuccess() {
        var r = Result.Success(1, 1, 1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f,
                                g,
                                h) => a + b + c + d + e + f + g + h == 8, (_,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _) => new Exception("bad"));

        if (ensured.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(1, b);
            Assert.Equal(1, c);
            Assert.Equal(1, d);
            Assert.Equal(1, e);
            Assert.Equal(1, f);
            Assert.Equal(1, g);
            Assert.Equal(1, h);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Ensure_Arity8_Success_PredicateFalse_ReturnsFailure() {
        var r = Result.Success(1, 1, 1, 1, 1, 1, 1, 1);

        var ensured = r.Ensure((a,
                                b,
                                c,
                                d,
                                e,
                                f,
                                g,
                                h) => a + b + c + d + e + f + g + h == 7, (_,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _,
                                                                           _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal("bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void Ensure_Arity8_Failure_SkipsPredicate_KeepsFailure() {
        var ex     = new Exception("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = 0;

        var ensured = r.Ensure((_,
                                _,
                                _,
                                _,
                                _,
                                _,
                                _,
                                _) => {
            called++;
            return true;
        }, (_,
            _,
            _,
            _,
            _,
            _,
            _,
            _) => new Exception("bad"));

        if (!ensured.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err)) {
            Assert.Equal(ex, err);
            Assert.Equal(0,  called);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
