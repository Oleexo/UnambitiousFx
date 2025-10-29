using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Bind.ValueTasks;

[TestSubject(typeof(ResultBindExtensions))]
public sealed class ToArity1BindAsyncValueTaskTests {
    [Fact]
    public async Task BindAsync_0_To_1_Success() {
        var r = await Result.Success()
                             .BindAsync(() => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_0_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure(ex)
                            .BindAsync(() => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_1_To_1_Success() {
        var r = await Result.Success(1)
                             .BindAsync(_ => ValueTask.FromResult(Result.Success(10)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(10, v);
    }

    [Fact]
    public async Task BindAsync_1_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int>(ex)
                            .BindAsync(_ => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_2_To_1_Success() {
        var r = await Result.Success(1, 2)
                             .BindAsync((_, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_2_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int>(ex)
                            .BindAsync((_, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_3_To_1_Success() {
        var r = await Result.Success(1, 2, 3)
                             .BindAsync((_, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_3_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int>(ex)
                            .BindAsync((_, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_4_To_1_Success() {
        var r = await Result.Success(1, 2, 3, 4)
                             .BindAsync((_, _, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_4_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int>(ex)
                            .BindAsync((_, _, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_5_To_1_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5)
                             .BindAsync((_, _, _, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_5_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_1_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_6_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_7_To_1_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6, 7)
                             .BindAsync((_, _, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_7_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_8_To_1_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                             .BindAsync((_, _, _, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal(1, v);
    }

    [Fact]
    public async Task BindAsync_8_To_1_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _, _, _) => {
                                called = true;
                                return ValueTask.FromResult(Result.Success(1));
                            });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }
}
