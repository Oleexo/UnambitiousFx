using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Bind;

public sealed class ToArity6BindTests {
    [Fact]
    public void Bind_0_To_6_Success() {
        var r = Result.Success()
                      .Bind(() => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_0_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure(ex)
                        .Bind(() => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_1_To_6_Success() {
        var r = Result.Success(1)
                      .Bind(_ => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_1_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int>(ex)
                        .Bind(_ => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_2_To_6_Success() {
        var r = Result.Success(1, 2)
                      .Bind((_,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_2_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int>(ex)
                        .Bind((_,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_3_To_6_Success() {
        var r = Result.Success(1, 2, 3)
                      .Bind((_,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_3_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int>(ex)
                        .Bind((_,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_4_To_6_Success() {
        var r = Result.Success(1, 2, 3, 4)
                      .Bind((_,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_4_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_6_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_5_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_6_To_6_Success() {
        var r = Result.Success(1, 2, 3, 4, 5, 6)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_6_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_7_To_6_Success() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_7_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_8_To_6_Success() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public void Bind_8_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }
}
