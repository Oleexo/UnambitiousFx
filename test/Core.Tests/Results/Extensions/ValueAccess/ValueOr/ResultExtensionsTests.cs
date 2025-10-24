using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ValueOr;

[TestSubject(typeof(ResultValueOrExtensions))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void ValueOr_Arity1_Success_ReturnsOriginal() {
        var r = Result.Success(5);
        var v = r.ValueOr(10);
        Assert.Equal(5, v);
    }

    [Fact]
    public void ValueOr_Arity1_Failure_ReturnsFallback() {
        var r = Result.Failure<int>(new Exception("boom"));
        var v = r.ValueOr(10);
        Assert.Equal(10, v);
    }

    [Fact]
    public void ValueOr_Arity1_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(3);
        var called = false;

        int Fallback() {
            called = true;
            return 99;
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(3, v);
    }

    [Fact]
    public void ValueOr_Arity1_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int>(new InvalidOperationException());
        var called = false;

        int Fallback() {
            called = true;
            return 77;
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(77, v);
    }

    [Fact]
    public void ValueOr_Arity2_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2);
        var v = r.ValueOr(42, 24);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
    }

    [Fact]
    public void ValueOr_Arity2_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
    }

    [Fact]
    public void ValueOr_Arity2_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2);
        var called = false;

        (int, int) Fallback() {
            called = true;
            return (42, 24);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
    }

    [Fact]
    public void ValueOr_Arity2_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int>(new InvalidOperationException());
        var called = false;

        (int, int) Fallback() {
            called = true;
            return (42, 24);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(42, v.Item1);
        Assert.Equal(24, v.Item2);
    }

    [Fact]
    public void ValueOr_Arity3_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3);
        var v = r.ValueOr(10, 20, 30);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
    }

    [Fact]
    public void ValueOr_Arity3_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
    }

    [Fact]
    public void ValueOr_Arity3_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3);
        var called = false;

        (int, int, int) Fallback() {
            called = true;
            return (10, 20, 30);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
    }

    [Fact]
    public void ValueOr_Arity3_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int) Fallback() {
            called = true;
            return (10, 20, 30);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
    }

    [Fact]
    public void ValueOr_Arity4_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3, 4);
        var v = r.ValueOr(10, 20, 30, 40);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
    }

    [Fact]
    public void ValueOr_Arity4_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30, 40);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
    }

    [Fact]
    public void ValueOr_Arity4_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = false;

        (int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
    }

    [Fact]
    public void ValueOr_Arity4_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
    }

    [Fact]
    public void ValueOr_Arity5_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var v = r.ValueOr(10, 20, 30, 40, 50);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
    }

    [Fact]
    public void ValueOr_Arity5_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30, 40, 50);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
    }

    [Fact]
    public void ValueOr_Arity5_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = false;

        (int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
    }

    [Fact]
    public void ValueOr_Arity5_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
    }

    [Fact]
    public void ValueOr_Arity6_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var v = r.ValueOr(10, 20, 30, 40, 50, 60);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
    }

    [Fact]
    public void ValueOr_Arity6_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30, 40, 50, 60);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
    }

    [Fact]
    public void ValueOr_Arity6_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = false;

        (int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
    }

    [Fact]
    public void ValueOr_Arity6_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int, int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
    }

    [Fact]
    public void ValueOr_Arity7_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var v = r.ValueOr(10, 20, 30, 40, 50, 60, 70);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
    }

    [Fact]
    public void ValueOr_Arity7_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30, 40, 50, 60, 70);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
        Assert.Equal(70, v.Item7);
    }

    [Fact]
    public void ValueOr_Arity7_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = false;

        (int, int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60, 70);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
    }

    [Fact]
    public void ValueOr_Arity7_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60, 70);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
        Assert.Equal(70, v.Item7);
    }

    [Fact]
    public void ValueOr_Arity8_Success_ReturnsOriginal() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var v = r.ValueOr(10, 20, 30, 40, 50, 60, 70, 80);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
        Assert.Equal(8, v.Item8);
    }

    [Fact]
    public void ValueOr_Arity8_Failure_ReturnsFallback() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var v = r.ValueOr(10, 20, 30, 40, 50, 60, 70, 80);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
        Assert.Equal(70, v.Item7);
        Assert.Equal(80, v.Item8);
    }

    [Fact]
    public void ValueOr_Arity8_Factory_NotInvoked_OnSuccess() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = false;

        (int, int, int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60, 70, 80);
        }

        var v = r.ValueOr(Fallback);
        Assert.False(called);
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
        Assert.Equal(8, v.Item8);
    }

    [Fact]
    public void ValueOr_Arity8_Factory_Invoked_OnFailure() {
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException());
        var called = false;

        (int, int, int, int, int, int, int, int) Fallback() {
            called = true;
            return (10, 20, 30, 40, 50, 60, 70, 80);
        }

        var v = r.ValueOr(Fallback);
        Assert.True(called);
        Assert.Equal(10, v.Item1);
        Assert.Equal(20, v.Item2);
        Assert.Equal(30, v.Item3);
        Assert.Equal(40, v.Item4);
        Assert.Equal(50, v.Item5);
        Assert.Equal(60, v.Item6);
        Assert.Equal(70, v.Item7);
        Assert.Equal(80, v.Item8);
    }
}
