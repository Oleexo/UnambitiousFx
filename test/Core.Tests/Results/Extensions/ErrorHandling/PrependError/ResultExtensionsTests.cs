using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.PrependError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void PrependError_Arity0_Success_NoChange_IsSuccess() {
        var r = Result.Success();

        var prepended = r.PrependError("[P]");

        Assert.True(prepended.IsSuccess);
    }

    [Fact]
    public void PrependError_Arity0_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("boom");
        var r  = Result.Failure(ex);

        var prepended = r.PrependError("PRE: ");

        if (!prepended.Ok(out var err)) {
            Assert.Equal("PRE: boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity0_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure(new Exception("err"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 1
    [Fact]
    public void PrependError_Arity1_Success_NoChange_PreservesValue() {
        var r = Result.Success(123);

        var prepended = r.PrependError("[x]");

        prepended.Ok(out var value, out _);
        Assert.Equal(123, value);
    }

    [Fact]
    public void PrependError_Arity1_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("bad");
        var r  = Result.Failure<int>(ex);

        var prepended = r.PrependError("?? ");

        if (!prepended.Ok(out _, out var err)) {
            Assert.Equal("?? bad", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity1_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int>(new Exception("e1"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 2
    [Fact]
    public void PrependError_Arity2_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);       
    }

    [Fact]
    public void PrependError_Arity2_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("ouch");
        var r  = Result.Failure<int, int>(ex);

        var prepended = r.PrependError("+ ");

        if (!prepended.Ok(out _,out _, out var err)) {
            Assert.Equal("+ ouch", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity2_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int>(new Exception("eee"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 3
    [Fact]
    public void PrependError_Arity3_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
    }

    [Fact]
    public void PrependError_Arity3_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("ouch3");
        var r  = Result.Failure<int, int, int>(ex);

        var prepended = r.PrependError("# ");

        if (!prepended.Ok(out _,out _,out _, out var err)) {
            Assert.Equal("# ouch3", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity3_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int>(new Exception("e3"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 4
    [Fact]
    public void PrependError_Arity4_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out var value4, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
    }

    [Fact]
    public void PrependError_Arity4_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("err4");
        var r  = Result.Failure<int, int, int, int>(ex);

        var prepended = r.PrependError(">> ");

        if (!prepended.Ok(out _,out _,out _,out _, out var err)) {
            Assert.Equal(">> err4", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity4_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int>(new Exception("e4"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 5
    [Fact]
    public void PrependError_Arity5_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
    }

    [Fact]
    public void PrependError_Arity5_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("boom5");
        var r  = Result.Failure<int, int, int, int, int>(ex);

        var prepended = r.PrependError("[p] ");

        if (!prepended.Ok(out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("[p] boom5", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity5_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int>(new Exception("e5"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 6
    [Fact]
    public void PrependError_Arity6_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
    }

    [Fact]
    public void PrependError_Arity6_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("e6");
        var r  = Result.Failure<int, int, int, int, int, int>(ex);

        var prepended = r.PrependError("~ ");

        if (!prepended.Ok(out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("~ e6", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity6_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int>(new Exception("e6x"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 7
    [Fact]
    public void PrependError_Arity7_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
        Assert.Equal(7, value7);
    }

    [Fact]
    public void PrependError_Arity7_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("stop7");
        var r  = Result.Failure<int, int, int, int, int, int, int>(ex);

        var prepended = r.PrependError("!! ");

        if (!prepended.Ok(out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("!! stop7", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity7_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int, int>(new Exception("e7"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }

    // Arity 8
    [Fact]
    public void PrependError_Arity8_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var prepended = r.PrependError("[t]");

        prepended.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
        Assert.Equal(7, value7);
        Assert.Equal(8, value8);
    }

    [Fact]
    public void PrependError_Arity8_Failure_PrependsPrefix_UpdatesMessage() {
        var ex = new Exception("x8");
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var prepended = r.PrependError("@ ");

        if (!prepended.Ok(out _,out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("@ x8", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void PrependError_Arity8_EmptyPrefix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("e8"));
        var r2 = r.PrependError("");

        Assert.True(object.ReferenceEquals(r, r2));
    }
}
