using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.TryGet;

[TestSubject(typeof(Result<>))]
public sealed class ResultExtensionsTests
{
    [Fact]
    public void TryGet_Arity1_Success()
    {
        var r = Result.Success(123);
        var ok = r.TryGet(out var value);
        Assert.True(ok);
        Assert.Equal(123, value);
    }

    [Fact]
    public void TryGet_Arity1_Failure()
    {
        var r = Result.Failure<int>(new Exception("boom"));
        var ok = r.TryGet(out var value);
        Assert.False(ok);
        Assert.Equal(0, value);
    }

    [Fact]
    public void TryGet_Arity2_Success()
    {
        var r = Result.Success(1, 2);
        var ok = r.TryGet(out var value1, out var value2);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
    }

    [Fact]
    public void TryGet_Arity2_Failure()
    {
        var r = Result.Failure<int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
    }

    [Fact]
    public void TryGet_Arity3_Success()
    {
        var r = Result.Success(1, 2, 3);
        var ok = r.TryGet(out var value1, out var value2, out var value3);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
    }

    [Fact]
    public void TryGet_Arity3_Failure()
    {
        var r = Result.Failure<int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
    }

    [Fact]
    public void TryGet_Arity4_Success()
    {
        var r = Result.Success(1, 2, 3, 4);
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
    }

    [Fact]
    public void TryGet_Arity4_Failure()
    {
        var r = Result.Failure<int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
        Assert.Equal(default(int), value4);
    }

    [Fact]
    public void TryGet_Arity5_Success()
    {
        var r = Result.Success(1, 2, 3, 4, 5);
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
    }

    [Fact]
    public void TryGet_Arity5_Failure()
    {
        var r = Result.Failure<int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
        Assert.Equal(default(int), value4);
        Assert.Equal(default(int), value5);
    }

    [Fact]
    public void TryGet_Arity6_Success()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
    }

    [Fact]
    public void TryGet_Arity6_Failure()
    {
        var r = Result.Failure<int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
        Assert.Equal(default(int), value4);
        Assert.Equal(default(int), value5);
        Assert.Equal(default(int), value6);
    }

    [Fact]
    public void TryGet_Arity7_Success()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7);
        Assert.True(ok);
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
        Assert.Equal(4, value4);
        Assert.Equal(5, value5);
        Assert.Equal(6, value6);
        Assert.Equal(7, value7);
    }

    [Fact]
    public void TryGet_Arity7_Failure()
    {
        var r = Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
        Assert.Equal(default(int), value4);
        Assert.Equal(default(int), value5);
        Assert.Equal(default(int), value6);
        Assert.Equal(default(int), value7);
    }

    [Fact]
    public void TryGet_Arity8_Success()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8);
        Assert.True(ok);
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
    public void TryGet_Arity8_Failure()
    {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException());
        var ok = r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8);
        Assert.False(ok);
        Assert.Equal(default(int), value1);
        Assert.Equal(default(int), value2);
        Assert.Equal(default(int), value3);
        Assert.Equal(default(int), value4);
        Assert.Equal(default(int), value5);
        Assert.Equal(default(int), value6);
        Assert.Equal(default(int), value7);
        Assert.Equal(default(int), value8);
    }
}
