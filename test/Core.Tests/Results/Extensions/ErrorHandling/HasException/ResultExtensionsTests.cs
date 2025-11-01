using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.HasException;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void HasException_Arity0_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity0_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity0_Success_ReturnsFalse() {
        var r = Result.Success();
        var has = r.HasException<InvalidOperationException>();
        Assert.False(has);
    }

    // Arity 1
    [Fact]
    public void HasException_Arity1_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity1_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity1_Success_ReturnsFalse() {
        var r = Result.Success(123);
        var has = r.HasException<InvalidOperationException, int>();
        Assert.False(has);
    }

    // Arity 2
    [Fact]
    public void HasException_Arity2_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity2_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity2_Success_ReturnsFalse() {
        var r = Result.Success(1, 2);
        var has = r.HasException<InvalidOperationException, int, int>();
        Assert.False(has);
    }

    // Arity 3
    [Fact]
    public void HasException_Arity3_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity3_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity3_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3);
        var has = r.HasException<InvalidOperationException, int, int, int>();
        Assert.False(has);
    }

    // Arity 4
    [Fact]
    public void HasException_Arity4_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity4_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity4_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3, 4);
        var has = r.HasException<InvalidOperationException, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 5
    [Fact]
    public void HasException_Arity5_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity5_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity5_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var has = r.HasException<InvalidOperationException, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 6
    [Fact]
    public void HasException_Arity6_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity6_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity6_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 7
    [Fact]
    public void HasException_Arity7_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity7_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity7_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    // Arity 8
    [Fact]
    public void HasException_Arity8_PrimaryExceptionMatch_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException("boom"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int, int>();
        Assert.True(has);
    }

    [Fact]
    public void HasException_Arity8_PrimaryExceptionMismatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new ArgumentException("bad"));
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int, int>();
        Assert.False(has);
    }

    [Fact]
    public void HasException_Arity8_Success_ReturnsFalse() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var has = r.HasException<InvalidOperationException, int, int, int, int, int, int, int, int>();
        Assert.False(has);
    }
}
