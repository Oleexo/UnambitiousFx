using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.TapError;

[TestSubject(typeof(Result))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void TapError_Arity0_Success_DoesNotInvoke() {
        var r      = Result.Success();
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity0_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity1_Success_DoesNotInvoke() {
        var r      = Result.Success(1);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity1_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity2_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity2_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity3_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity3_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity4_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity4_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity5_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity5_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity6_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity6_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int, int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity7_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity7_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int, int, int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapError_Arity8_Success_DoesNotInvoke() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity8_Failure_InvokesOnce() {
        var        ex       = new Exception("boom");
        var        r        = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var        called   = 0;
        Exception? captured = null;

        r.TapError(e => {
            called++;
            captured = e;
        });

        Assert.Equal(1,  called);
        Assert.Equal(ex, captured);
    }
}
