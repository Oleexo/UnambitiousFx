using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Bind.ValueTasks;

[TestSubject(typeof(ResultBindExtensions))]
public sealed class FromArity6BindAsyncValueTaskTests {
    [Fact]
    public async Task BindAsync_6_To_0_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success()));
        r.ShouldBeSuccess();
    }

    [Fact]
    public async Task BindAsync_6_To_0_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;

        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success());
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
    public async Task BindAsync_6_To_2_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2), v);
    }

    [Fact]
    public async Task BindAsync_6_To_2_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_3_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3), v);
    }

    [Fact]
    public async Task BindAsync_6_To_3_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2, 3));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_4_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3, 4)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4), v);
    }

    [Fact]
    public async Task BindAsync_6_To_4_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2, 3, 4));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_5_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5), v);
    }

    [Fact]
    public async Task BindAsync_6_To_5_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_6_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)));
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
                                 return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_7_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task BindAsync_6_To_7_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }

    [Fact]
    public async Task BindAsync_6_To_8_Success() {
        var r = await Result.Success(1, 2, 3, 4, 5, 6)
                             .BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8)));
        r.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task BindAsync_6_To_8_Failure_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var r = await Result.Failure<int, int, int, int, int, int>(ex)
                            .BindAsync((_, _, _, _, _, _) => {
                                 called = true;
                                 return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
                             });
        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
    }
    // 6 -> 0 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_0_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess();
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_0_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success();
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 0 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_0_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success()), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess();
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_0_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success());
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 1 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_1_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal(10, v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_1_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 1 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_1_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal(10, v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_1_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 2 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_2_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_2_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 2 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_2_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_2_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 3 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_3_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_3_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 3 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_3_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_3_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 4 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_4_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30, 40), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_4_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3, 4);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 4 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_4_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30, 40)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_4_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3, 4));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 5 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_5_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30, 40, 50), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_5_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3, 4, 5);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 5 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_5_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30, 40, 50)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_5_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 6 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_6_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30, 40, 50, 60), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_6_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3, 4, 5, 6);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 6 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_6_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30, 40, 50, 60)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_6_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 7 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_7_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30, 40, 50, 60, 70), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60, 70), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_7_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3, 4, 5, 6, 7);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 7 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_7_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30, 40, 50, 60, 70)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60, 70), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_7_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 8 (sync bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_8_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => Result.Success(10, 20, 30, 40, 50, 60, 70, 80), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60, 70, 80), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_8_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }

    // 6 -> 8 (async bind)
    [Fact]
    public async Task BindAsync_Awaitable6_To_8_AsyncBind_Success_NoCopy() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6)
                                             .WithSuccess("s1")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => ValueTask.FromResult(Result.Success(10, 20, 30, 40, 50, 60, 70, 80)), copyReasonsAndMetadata: false);

        r.ShouldBeSuccess(out var v);
        Assert.Equal((10, 20, 30, 40, 50, 60, 70, 80), v);
        Assert.Empty(r.Reasons);
        Assert.Empty(r.Metadata);
    }

    [Fact]
    public async Task BindAsync_Awaitable6_To_8_AsyncBind_Failure_Copy_DoesNotInvoke() {
        var ex     = new Exception("err");
        var called = false;
        var awaitable = ValueTask.FromResult(Result.Failure<int, int, int, int, int, int>(ex)
                                             .WithError("E", "msg")
                                             .WithMetadata("m1", 1));

        var r = await awaitable.BindAsync((_, _, _, _, _, _) => {
            called = true;
            return ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        }, copyReasonsAndMetadata: true);

        r.ShouldBeFailure(out var e);
        { var firstError = e?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
        Assert.False(called);
        Assert.NotEmpty(r.Reasons);
        Assert.NotEmpty(r.Metadata);
    }
}
