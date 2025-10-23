using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.HasError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void HasError_Arity0_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure(new NotFound("x")).WithError(new Conflict("c1"));
        var has = r.HasError<NotFound>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity0_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure(new Conflict("c1"));
        var has = r.HasError<NotFound>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity0_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity0_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException>();
        Assert.False(has);
    }

    // Arity 1
    [Fact]
    public void HasError_Arity1_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int>(new NotFound("id")).WithError(new Conflict("c"));
        var has = r.HasError<NotFound, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity1_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int>(new Conflict("c1"));
        var has = r.HasError<NotFound, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity1_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity1_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int>();
        Assert.False(has);
    }

    // Arity 2
    [Fact]
    public void HasError_Arity2_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int>(new NotFound("id2"));
        var has = r.HasError<NotFound, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity2_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int>(new Conflict("c2"));
        var has = r.HasError<NotFound, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity2_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity2_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int>();
        Assert.False(has);
    }

    // Arity 3
    [Fact]
    public void HasError_Arity3_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int>(new NotFound("id3"));
        var has = r.HasError<NotFound, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity3_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int>(new Conflict("c3"));
        var has = r.HasError<NotFound, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity3_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity3_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int>();
        Assert.False(has);
    }

    // Arity 4
    [Fact]
    public void HasError_Arity4_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int>(new NotFound("id4"));
        var has = r.HasError<NotFound, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity4_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int>(new Conflict("c4"));
        var has = r.HasError<NotFound, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity4_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity4_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 5
    [Fact]
    public void HasError_Arity5_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int>(new NotFound("id5"));
        var has = r.HasError<NotFound, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity5_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int>(new Conflict("c5"));
        var has = r.HasError<NotFound, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity5_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity5_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 6
    [Fact]
    public void HasError_Arity6_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int>(new NotFound("id6"));
        var has = r.HasError<NotFound, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity6_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int>(new Conflict("c6"));
        var has = r.HasError<NotFound, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity6_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity6_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 7
    [Fact]
    public void HasError_Arity7_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new NotFound("id7"));
        var has = r.HasError<NotFound, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity7_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Conflict("c7"));
        var has = r.HasError<NotFound, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity7_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity7_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 8
    [Fact]
    public void HasError_Arity8_WithSpecificError_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new NotFound("id8"));
        var has = r.HasError<NotFound, int, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity8_NoMatchingError_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Conflict("c8"));
        var has = r.HasError<NotFound, int, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasError_Arity8_WithPrimaryExceptionType_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasError_Arity8_PrimaryExceptionDifferentType_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasError<InvalidOperationException, int, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    private sealed record Conflict(string Msg) : ErrorBase("CONFLICT", Msg);
    private sealed record NotFound(string Id) : ErrorBase("NOT_FOUND", $"Missing {Id}");
}
