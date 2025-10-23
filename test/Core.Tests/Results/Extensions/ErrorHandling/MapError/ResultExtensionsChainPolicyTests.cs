using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Types;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.MapError;

[TestSubject(typeof(Result))]
public sealed class ResultExtensionsChainPolicyTests {
    // Arity 0
    [Fact]
    public void MapError_Arity0_Accumulate_Success_NoChange() {
        var r      = Result.Success();
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        Assert.True(mapped.Ok(out _));
        Assert.Equal(0, called);
    }

    [Fact]
    public void MapError_Arity0_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count); // new ExceptionalError + prior reasons
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity0_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons); // only new ExceptionalError remains
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 1
    [Fact]
    public void MapError_Arity1_Accumulate_Success_NoChange() {
        var r      = Result.Success(1);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var value, out _)) {
            Assert.Equal(1, value);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity1_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity1_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 2
    [Fact]
    public void MapError_Arity2_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out _)) {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(0, called);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity2_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _, out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity2_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 3
    [Fact]
    public void MapError_Arity3_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out _)) {
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
    public void MapError_Arity3_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity3_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 4
    [Fact]
    public void MapError_Arity4_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out var d, out _)) {
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
    public void MapError_Arity4_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity4_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 5
    [Fact]
    public void MapError_Arity5_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out var d, out var e, out _)) {
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
    public void MapError_Arity5_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity5_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 6
    [Fact]
    public void MapError_Arity6_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out _)) {
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
    public void MapError_Arity6_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity6_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 7
    [Fact]
    public void MapError_Arity7_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out _)) {
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
    public void MapError_Arity7_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity7_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 8
    [Fact]
    public void MapError_Arity8_Accumulate_Success_NoChange() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        var mapped = r.MapError(_ => {
            called++;
            return new Exception("wrapped");
        }, MapErrorChainPolicy.Accumulate);

        if (mapped.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h, out _)) {
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
    public void MapError_Arity8_Accumulate_Failure_PreservesReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");
        var origReasonCount = initial.Reasons.Count;

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.Accumulate);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Equal(origReasonCount + 1, mapped.Reasons.Count);
            Assert.True(mapped.Metadata.ContainsKey("k"));
            Assert.Equal("v", mapped.Metadata["k"]);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public void MapError_Arity8_ShortCircuit_Failure_DropsReasonsAndMetadata() {
        var ex      = new Exception("boom");
        var initial = Result.Failure<int, int, int, int, int, int, int, int>(ex)
                           .WithError("E1", "first")
                           .WithMetadata("k", "v");

        var mapped = initial.MapError(e => new InvalidOperationException("wrapped", e), MapErrorChainPolicy.ShortCircuit);

        if (!mapped.Ok(out _,out _,out _,out _,out _,out _,out _,out _, out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
            Assert.Single(mapped.Reasons);
            Assert.Empty(mapped.Metadata);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
