using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Bind.Tasks;

[TestSubject(typeof(ResultBindExtensions))]
public sealed class ToArity6BindAsyncTaskTests {
    [Fact]
    public async Task BindAsync_0_To_6_Success() {
        var r = await Result.Success()
                             .BindAsync(() => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_0_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure(ex)
                            .BindAsync(() => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_1_To_6_Success() {
        var r = await Result.Success(1)
                             .BindAsync(_ => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_1_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int>(ex)
                            .BindAsync(_ => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_2_To_6_Success() {
        var r = await Result.Success(1, 2)
                             .BindAsync((_, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_2_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int>(ex)
                            .BindAsync((_, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_3_To_6_Success() {
        var r = await Result.Success(1, 2, 3)
                             .BindAsync((_, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_3_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int>(ex)
                            .BindAsync((_, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_4_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4)
                             .BindAsync((_, _, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_4_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int>(ex)
                            .BindAsync((_, _, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_5_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5)
                             .BindAsync((_, _, _, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_5_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_6_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_7_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6, 7)
                             .BindAsync((_, _, _, _, _, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_7_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_8_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                             .BindAsync((_, _, _, _, _, _, _, _) => Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task BindAsync_8_To_6_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _, _, _) => {
                                called = true;
                                return Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                            });
        r.ShouldBeFailure(out var e);
        Assert.Same(ex, e);
        Assert.False(called);
    }
}
