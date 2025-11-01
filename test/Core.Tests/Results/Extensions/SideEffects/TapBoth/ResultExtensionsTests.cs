using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.SideEffects;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SideEffects.TapBoth;

[TestSubject(typeof(ResultTapBothExtensions))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void TapBoth_Arity0_Success_InvokesOnce() {
        var r      = Result.Success();
        var called = 0;

        r.TapBoth(() => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity0_Failure_InvokesOnce() {
        var r      = Result.Failure("boom");
        var called = 0;

        r.TapBoth(() => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity1_Success_InvokesOnce() {
        var r      = Result.Success(42);
        var called = 0;

        r.TapBoth(_ => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity1_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int>(new Exception("boom"));
        var called = 0;

        r.TapBoth(_ => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }
    
    [Fact]
    public void TapBoth_Arity2_Success_InvokesOnce() {
        var r      = Result.Success(42, "test");
        var called = 0;

        r.TapBoth((_, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity2_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, string>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }
    
    [Fact]
    public void TapBoth_Arity3_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3);
        var called = 0;

        r.TapBoth((_, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity3_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity4_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = 0;

        r.TapBoth((_, _, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity4_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity5_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        r.TapBoth((_, _, _, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity5_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity6_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        r.TapBoth((_, _, _, _, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity6_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _, _, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity7_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        r.TapBoth((_, _, _, _, _, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity7_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _, _, _, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity8_Success_InvokesOnce() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        r.TapBoth((_, _, _, _, _, _, _, _) => called++, _ => Assert.Fail("Should not be called"));

        Assert.Equal(1, called);
    }

    [Fact]
    public void TapBoth_Arity8_Failure_DoesNotInvoke() {
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var called = 0;

        r.TapBoth((_, _, _, _, _, _, _, _) => Assert.Fail("Should not be called"), _ => called++);

        Assert.Equal(1, called);
    }
}
