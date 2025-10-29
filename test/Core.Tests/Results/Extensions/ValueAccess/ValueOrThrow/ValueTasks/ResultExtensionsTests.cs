using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ValueOrThrow.ValueTasks;

[TestSubject(typeof(ResultValueOrThrowExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task ValueOrThrowAsync_Arity1_Success_ReturnsValue() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(42));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity1_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity1_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity1_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(5));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task ValueOrThrowAsync_Arity2_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity2_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity2_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity2_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task ValueOrThrowAsync_Arity3_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity3_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity3_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity3_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task ValueOrThrowAsync_Arity4_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3, 4), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity4_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity4_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity4_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task ValueOrThrowAsync_Arity5_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3, 4, 5), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity5_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity5_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity5_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task ValueOrThrowAsync_Arity6_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity6_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity6_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity6_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task ValueOrThrowAsync_Arity7_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity7_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity7_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity7_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task ValueOrThrowAsync_Arity8_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var v = await awaitable.ValueOrThrowAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity8_Failure_ThrowsOriginal() {
        var ex = new InvalidOperationException("boom");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(ex));
        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(async () => await awaitable.ValueOrThrowAsync());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity8_Failure_FactoryTransformsType() {
        var ex = new ArgumentException("bad");
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(ex));
        var custom = await Assert.ThrowsAsync<ApplicationException>(async () => await awaitable.ValueOrThrowAsync(errors => new ApplicationException("wrapped", errors.ToException())));
        Assert.IsType<ApplicationException>(custom);
    }

    [Fact]
    public async Task ValueOrThrowAsync_Arity8_Success_FactoryNotInvoked() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var called = false;
        var _ = await awaitable.ValueOrThrowAsync(_ => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
    #endregion
}
