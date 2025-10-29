using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.AppendError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void AppendError_Arity0_Success_NoChange_IsSuccess() {
        var r = Result.Success();

        var appended = r.AppendError("_x");

        Assert.True(appended.IsSuccess);
    }

    [Fact]
    public void AppendError_Arity0_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("boom");
        var r  = Result.Failure(ex);

        var appended = r.AppendError("!");

        if (!appended.TryGet(out var err)) {
            Assert.Equal("boom!", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity0_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure(new Exception("err"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 1
    [Fact]
    public void AppendError_Arity1_Success_NoChange_PreservesValue() {
        var r = Result.Success(123);

        var appended = r.AppendError("_suffix");

        appended.TryGet(out var value, out _);
        Assert.Equal(123, value);
    }

    [Fact]
    public void AppendError_Arity1_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("bad");
        var r  = Result.Failure<int>(ex);

        var appended = r.AppendError("?");

        if (!appended.TryGet(out _, out var err)) {
            Assert.Equal("bad?", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity1_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int>(new Exception("e1"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 2
    [Fact]
    public void AppendError_Arity2_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
    }

    [Fact]
    public void AppendError_Arity2_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("ouch");
        var r  = Result.Failure<int, int>(ex);

        var appended = r.AppendError("+");

        if (!appended.TryGet(out _, out _, out var err)) {
            Assert.Equal("ouch+", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity2_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int>(new Exception("eee"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 3
    [Fact]
    public void AppendError_Arity3_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
    }

    [Fact]
    public void AppendError_Arity3_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("trip");
        var r  = Result.Failure<int, int, int>(ex);

        var appended = r.AppendError("*");

        if (!appended.TryGet(out _, out _, out _, out var err)) {
            Assert.Equal("trip*", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity3_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int>(new Exception("e3"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 4
    [Fact]
    public void AppendError_Arity4_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out var value4, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
    }

    [Fact]
    public void AppendError_Arity4_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("quad");
        var r  = Result.Failure<int, int, int, int>(ex);

        var appended = r.AppendError("#");

        if (!appended.TryGet(out _, out _,out _,out _, out var err)) {
            Assert.Equal("quad#", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity4_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int>(new Exception("e4"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 5
    [Fact]
    public void AppendError_Arity5_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5,  out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);       
    }

    [Fact]
    public void AppendError_Arity5_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("penta");
        var r  = Result.Failure<int, int, int, int, int>(ex);

        var appended = r.AppendError("@");

        if (!appended.TryGet(out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("penta@", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity5_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int>(new Exception("e5"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 6
    [Fact]
    public void AppendError_Arity6_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);       
    }

    [Fact]
    public void AppendError_Arity6_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("hexa");
        var r  = Result.Failure<int, int, int, int, int, int>(ex);

        var appended = r.AppendError("%");

        if (!appended.TryGet(out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("hexa%", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity6_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int>(new Exception("e6"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 7
    [Fact]
    public void AppendError_Arity7_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out _);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
        Assert.Equal(7, value7);
    }

    [Fact]
    public void AppendError_Arity7_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("hepta");
        var r  = Result.Failure<int, int, int, int, int, int, int>(ex);

        var appended = r.AppendError("=");

        if (!appended.TryGet(out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("hepta=", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity7_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int, int>(new Exception("e7"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }

    // Arity 8
    [Fact]
    public void AppendError_Arity8_Success_NoChange_PreservesTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var appended = r.AppendError("_t");

        appended.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8, out _);
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
    public void AppendError_Arity8_Failure_AppendsSuffix_UpdatesMessage() {
        var ex = new Exception("octa");
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var appended = r.AppendError("^");

        if (!appended.TryGet(out _,out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.Equal("octa^", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void AppendError_Arity8_EmptySuffix_NoChange_ReturnsOriginalInstance() {
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("e8"));
        var r2 = r.AppendError("");

        Assert.True(ReferenceEquals(r, r2));
    }
}
