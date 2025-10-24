using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.TryGet;

[TestSubject(typeof(ResultTryGetExtensions))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void TryGet_Arity1_Success() {
        var r  = Result.Success(123);
        var ok = r.TryGet(out var value);
        Assert.True(ok);
        Assert.Equal(123, value);
    }

    [Fact]
    public void TryGet_Arity1_Failure() {
        var r  = Result.Failure<int>(new Exception("boom"));
        var ok = r.TryGet(out var value);
        Assert.False(ok);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryGet_Arity2_Success() {
        var r  = Result.Success(1, 2);
        var ok = r.TryGet(out (int a, int b) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2), tuple);
    }

    [Fact]
    public void TryGet_Arity2_Failure() {
        var r  = Result.Failure<int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity3_Success() {
        var r  = Result.Success(1, 2, 3);
        var ok = r.TryGet(out (int a, int b, int c) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3), tuple);
    }

    [Fact]
    public void TryGet_Arity3_Failure() {
        var r  = Result.Failure<int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity4_Success() {
        var r  = Result.Success(1, 2, 3, 4);
        var ok = r.TryGet(out (int a, int b, int c, int d) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3, 4), tuple);
    }

    [Fact]
    public void TryGet_Arity4_Failure() {
        var r  = Result.Failure<int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c, int d) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity5_Success() {
        var r  = Result.Success(1, 2, 3, 4, 5);
        var ok = r.TryGet(out (int a, int b, int c, int d, int e) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3, 4, 5), tuple);
    }

    [Fact]
    public void TryGet_Arity5_Failure() {
        var r  = Result.Failure<int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c, int d, int e) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity6_Success() {
        var r  = Result.Success(1, 2, 3, 4, 5, 6);
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3, 4, 5, 6), tuple);
    }

    [Fact]
    public void TryGet_Arity6_Failure() {
        var r  = Result.Failure<int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity7_Success() {
        var r  = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f, int g) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), tuple);
    }

    [Fact]
    public void TryGet_Arity7_Failure() {
        var r  = Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f, int g) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }

    [Fact]
    public void TryGet_Arity8_Success() {
        var r  = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f, int g, int h) tuple);
        Assert.True(ok);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), tuple);
    }

    [Fact]
    public void TryGet_Arity8_Failure() {
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out (int a, int b, int c, int d, int e, int f, int g, int h) tuple);
        Assert.False(ok);
        Assert.Equal(default, tuple);
    }
}
