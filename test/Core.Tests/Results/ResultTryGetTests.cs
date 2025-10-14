using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultTryGetTests {
    [Fact]
    public void TryGet_Single_Success() {
        var r  = Result.Success(123);
        var ok = r.TryGet(out var value);
        Assert.True(ok);
        Assert.Equal(123, value);
    }

    [Fact]
    public void TryGet_Single_Failure() {
        var r  = Result.Failure<int>(new Exception("boom"));
        var ok = r.TryGet(out var value);
        Assert.False(ok);
        Assert.Equal(default, value);
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
        var r  = Result.Success(3, 4, 5);
        var ok = r.TryGet(out (int a, int b, int c) tuple);
        Assert.True(ok);
        Assert.Equal((3, 4, 5), tuple);
    }
}
