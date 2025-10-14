using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorShaping;

public sealed class PrependAppendErrorExtensionsTests {
    // Arity 0
    [Fact]
    public void PrependError_Arity0_Success_NoChange() {
        var r      = Result.Success();
        var shaped = r.PrependError("CTX: ");
        Assert.True(shaped.Ok(out _));
    }

    [Fact]
    public void PrependError_Arity0_Failure_PrependsAndKeepsInner() {
        var original = new Exception("boom");
        var r        = Result.Failure(original);
        var shaped   = r.PrependError("CTX: ");
        Assert.False(shaped.Ok(out var err));
        Assert.NotNull(err);
        Assert.Equal("CTX: boom", err.Message);
        Assert.Equal(original,    err.InnerException);
    }

    [Fact]
    public void AppendError_Arity0_Failure_AppendsAndKeepsInner() {
        var original = new Exception("boom");
        var r        = Result.Failure(original);
        var shaped   = r.AppendError(" [extra]");
        Assert.False(shaped.Ok(out var err));
        Assert.Equal("boom [extra]", err.Message);
        Assert.Equal(original,       err.InnerException);
    }

    [Fact]
    public void PrependError_Arity0_EmptyPrefix_NoOp() {
        var original = new Exception("boom");
        var r        = Result.Failure(original);
        var shaped   = r.PrependError("");
        Assert.False(shaped.Ok(out var err));
        Assert.Same(original, err); // same instance because MapError not called
    }

    // Arity 1
    [Fact]
    public void AppendError_Arity1_Success_NoChange() {
        var r      = Result.Success(42);
        var shaped = r.AppendError(" ::CTX");
        Assert.True(shaped.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public void AppendError_Arity1_Failure_Appends() {
        var ex     = new InvalidOperationException("bad");
        var r      = Result.Failure<int>(ex);
        var shaped = r.AppendError("!");
        Assert.False(shaped.Ok(out var _, out var err));
        Assert.Equal("bad!", err.Message);
        Assert.Equal(ex,     err.InnerException);
    }

    // Arity 2
    [Fact]
    public void PrependError_Arity2_Success_NoChange() {
        var r      = Result.Success(1, 2);
        var shaped = r.PrependError("X: ");
        Assert.True(shaped.Ok(out (int, int) values, out _));
        Assert.Equal((1, 2), values);
    }

    [Fact]
    public void PrependError_Arity2_Failure_Prepends() {
        var ex     = new Exception("fail");
        var r      = Result.Failure<int, int>(ex);
        var shaped = r.PrependError("X: ");
        Assert.False(shaped.Ok(out (int, int) _, out var err));
        Assert.Equal("X: fail", err.Message);
        Assert.Equal(ex,        err.InnerException);
    }

    [Fact]
    public void AppendError_Arity2_Failure_Appends() {
        var ex     = new Exception("fail");
        var r      = Result.Failure<int, int>(ex);
        var shaped = r.AppendError(" :: tail");
        Assert.False(shaped.Ok(out (int, int) _, out var err));
        Assert.Equal("fail :: tail", err.Message);
        Assert.Equal(ex,             err.InnerException);
    }

    // Arity 3
    [Fact]
    public void PrependError_Arity3_Failure() {
        var ex     = new Exception("oops");
        var r      = Result.Failure<int, int, int>(ex);
        var shaped = r.PrependError("CTX3: ");
        Assert.False(shaped.Ok(out (int, int, int) _, out var err));
        Assert.Equal("CTX3: oops", err.Message);
        Assert.Equal(ex,           err.InnerException);
    }

    [Fact]
    public void AppendError_Arity3_Success_NoChange() {
        var r      = Result.Success(1, 2, 3);
        var shaped = r.AppendError(" trailing");
        Assert.True(shaped.Ok(out (int, int, int) values, out _));
        Assert.Equal((1, 2, 3), values);
    }

    // Arity 8 (max) - only compile + transformation check
    [Fact]
    public void PrependError_Arity8_Failure() {
        var ex     = new Exception("deep");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var shaped = r.PrependError("Layer: ");
        Assert.False(shaped.Ok(out (int, int, int, int, int, int, int, int) _, out var err));
        Assert.Equal("Layer: deep", err.Message);
    }

    [Fact]
    public void AppendError_Arity8_Failure() {
        var ex     = new Exception("deep");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var shaped = r.AppendError(" tail");
        Assert.False(shaped.Ok(out (int, int, int, int, int, int, int, int) _, out var err));
        Assert.Equal("deep tail", err.Message);
    }

    [Fact]
    public void PrependError_PreservesReasonsAndMetadata() {
        var original = Result.Failure(new Exception("err"))
                             .WithError(new ConflictError("conflict"))
                             .WithMetadata("traceId", "abc123");
        var originalReasonCount = original.Reasons.Count;
        var shaped              = original.PrependError("CTX: ");
        Assert.False(shaped.Ok(out _));
        Assert.Equal(originalReasonCount, shaped.Reasons.Count);
        Assert.True(shaped.Metadata.ContainsKey("traceId"));
        Assert.Contains(shaped.Reasons, r => r is ConflictError);
    }

    [Fact]
    public void WithContext_DelegatesToPrependError() {
        var ex     = new Exception("boom");
        var r      = Result.Failure(ex);
        var shaped = r.WithContext("API: ");
        Assert.False(shaped.Ok(out var err));
        Assert.StartsWith("API: ", err.Message);
        Assert.Equal(ex, err.InnerException);
    }
}
