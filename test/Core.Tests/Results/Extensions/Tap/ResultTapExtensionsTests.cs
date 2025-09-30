using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Tap;

[TestSubject(typeof(Result))]
public sealed class ResultTapExtensionsTests
{
    // Arity 0
    [Fact]
    public void Tap_Arity0_Success_InvokesOnce()
    {
        var r = Result.Success();
        var called = 0;

        r.Tap(() => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity0_Failure_DoesNotInvoke()
    {
        var r = Result.Failure("boom");
        var called = 0;

        r.Tap(() => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity0_Success_DoesNotInvoke()
    {
        var r = Result.Success();
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity0_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 1
    [Fact]
    public void Tap_Arity1_Success_InvokesOnce()
    {
        var r = Result.Success(42);
        var called = 0;

        r.Tap(_ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity1_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int>(new Exception("boom"));
        var called = 0;

        r.Tap(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity1_Success_DoesNotInvoke()
    {
        var r = Result.Success(1);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity1_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 2
    [Fact]
    public void Tap_Arity2_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2);
        var called = 0;

        r.Tap((_, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity2_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity2_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity2_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 3
    [Fact]
    public void Tap_Arity3_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3);
        var called = 0;

        r.Tap((_, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity3_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity3_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity3_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 4
    [Fact]
    public void Tap_Arity4_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3, 4);
        var called = 0;

        r.Tap((_, _, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity4_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity4_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3, 4);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity4_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 5
    [Fact]
    public void Tap_Arity5_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        r.Tap((_, _, _, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity5_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity5_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity5_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 6
    [Fact]
    public void Tap_Arity6_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        r.Tap((_, _, _, _, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity6_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _, _, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity6_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity6_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 7
    [Fact]
    public void Tap_Arity7_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        r.Tap((_, _, _, _, _, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity7_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _, _, _, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity7_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity7_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }

    // Arity 8
    [Fact]
    public void Tap_Arity8_Success_InvokesOnce()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        r.Tap((_, _, _, _, _, _, _, _) => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void Tap_Arity8_Failure_DoesNotInvoke()
    {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.Tap((_, _, _, _, _, _, _, _) => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity8_Success_DoesNotInvoke()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        r.TapError(_ => called++);

        Assert.Equal(0, called);
    }

    [Fact]
    public void TapError_Arity8_Failure_InvokesOnce()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var called = 0;
        Exception? captured = null;

        r.TapError(e => { called++; captured = e; });

        Assert.Equal(1, called);
        Assert.Equal(ex, captured);
    }
}
