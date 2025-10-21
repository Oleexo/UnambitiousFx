using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Bind;

public sealed class FromArity5BindTests {
    [Fact]
    public void Bind_5_To_0_Success() {
        Result.Success(1, 2, 3, 4, 5)
              .Bind((_,
                     _,
                     _,
                     _,
                     _) => Result.Success())
              .ShouldBeSuccess();
    }

    [Fact]
    public void Bind_5_To_0_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;

        Result.Failure<int, int, int, int, int>(ex)
              .Bind((_,
                     _,
                     _,
                     _,
                     _) => {
                   called = true;
                   return Result.Success();
               })
              .ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_1_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1));
        Assert.True(r.Ok(out var v));
        Assert.Equal(1, v);
    }

    [Fact]
    public void Bind_5_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_2_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2), v);
    }

    [Fact]
    public void Bind_5_To_2_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_3_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3), v);
    }

    [Fact]
    public void Bind_5_To_3_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_4_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4), v);
    }

    [Fact]
    public void Bind_5_To_4_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_5_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5), v);
    }

    [Fact]
    public void Bind_5_To_5_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5);
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
    public void Bind_5_To_7_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6, 7));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public void Bind_5_To_7_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6, 7);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public void Bind_5_To_8_Success() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Bind((_,
                             _,
                             _,
                             _,
                             _) => Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        Assert.True(r.Ok(out var v));
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public void Bind_5_To_8_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var res = Result.Failure<int, int, int, int, int>(ex)
                        .Bind((_,
                               _,
                               _,
                               _,
                               _) => {
                             called = true;
                             return Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
                         });
        res.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }
}
