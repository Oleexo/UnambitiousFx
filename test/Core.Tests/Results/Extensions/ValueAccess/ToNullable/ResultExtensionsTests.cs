using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ToNullable;

[TestSubject(typeof(ResultToNullableExtensions))]
public sealed class ResultToNullableExtensionsTests {
    // Arity 1
    [Fact]
    public void ToNullable_Arity1_Success_ReturnsValue() {
        var r = Result.Success(5);
        var n = r.ToNullable();
        Assert.Equal((int?)5, n);
    }

    [Fact]
    public void ToNullable_Arity1_Failure_ReturnsDefault() {
        var r = Result.Failure<int>(new Exception("boom"));
        var n = r.ToNullable();
        Assert.Equal(0, n);
    }

    // Arity 2
    [Fact]
    public void ToNullable_Arity2_Success_ReturnsTuple() {
        var r = Result.Success(2, 3);
        var n = r.ToNullable();
        Assert.Equal((2, 3), n);
    }

    [Fact]
    public void ToNullable_Arity2_Failure_ReturnsNull() {
        var r = Result.Failure<int, int>(new InvalidOperationException("fail"));
        var n = r.ToNullable();
        Assert.Equal(((int, int)?)null, n);
    }

    // Arity 3
    [Fact]
    public void ToNullable_Arity3_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3);
        var n = r.ToNullable();
        Assert.Equal((1, 2, 3), n);
    }

    [Fact]
    public void ToNullable_Arity3_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int>(new Exception("boom3"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int)?)null, n);
    }

    // Arity 4
    [Fact]
    public void ToNullable_Arity4_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3, 4);
        var n = r.ToNullable();
        Assert.Equal((1, 2, 3, 4), n);
    }

    [Fact]
    public void ToNullable_Arity4_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int, int>(new Exception("boom4"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int)?)null, n);
    }

    // Arity 5
    [Fact]
    public void ToNullable_Arity5_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int, int)?)(1, 2, 3, 4, 5), n);
    }

    [Fact]
    public void ToNullable_Arity5_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int>(new Exception("boom5"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int, int)?)null, n);
    }

    // Arity 6
    [Fact]
    public void ToNullable_Arity6_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var n = r.ToNullable();
        Assert.Equal((1, 2, 3, 4, 5, 6), n);
    }

    [Fact]
    public void ToNullable_Arity6_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int>(new Exception("boom6"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int, int, int)?)null, n);
    }

    // Arity 7
    [Fact]
    public void ToNullable_Arity7_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var n = r.ToNullable();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), n);
    }

    [Fact]
    public void ToNullable_Arity7_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom7"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int, int, int, int)?)null, n);
    }

    // Arity 8
    [Fact]
    public void ToNullable_Arity8_Success_ReturnsTuple() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var n = r.ToNullable();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), n);
    }

    [Fact]
    public void ToNullable_Arity8_Failure_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom8"));
        var n = r.ToNullable();
        Assert.Equal(((int, int, int, int, int, int, int, int)?)null, n);
    }
}
