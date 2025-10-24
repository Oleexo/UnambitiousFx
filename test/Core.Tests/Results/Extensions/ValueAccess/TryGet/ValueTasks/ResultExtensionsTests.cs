using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.TryGet.ValueTasks;

[TestSubject(typeof(ResultTryGetExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1

    [Fact]
    public async Task TryGetAsync_Arity1_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(123));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity1_Success_ReturnsValue() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(123));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(123, t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity1_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new InvalidOperationException("boom")));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity1_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new InvalidOperationException("boom")));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(0, t.value);
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task TryGetAsync_Arity2_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity2_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity2_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity2_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int)), t.value);
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task TryGetAsync_Arity3_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity3_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity3_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity3_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int)), t.value);
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task TryGetAsync_Arity4_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity4_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3, 4), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity4_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity4_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int, int)), t.value);
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task TryGetAsync_Arity5_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity5_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3, 4, 5), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity5_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity5_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int, int, int)), t.value);
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task TryGetAsync_Arity6_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity6_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity6_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity6_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int, int, int, int)), t.value);
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task TryGetAsync_Arity7_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity7_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity7_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity7_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int, int, int, int, int)), t.value);
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task TryGetAsync_Arity8_Success_OkTrue() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var t         = await awaitable.TryGetAsync();
        Assert.True(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity8_Success_ReturnsTuple() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), t.value);
    }

    [Fact]
    public async Task TryGetAsync_Arity8_Failure_OkFalse() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.False(t.ok);
    }

    [Fact]
    public async Task TryGetAsync_Arity8_Failure_ReturnsDefault() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new InvalidOperationException()));
        var t         = await awaitable.TryGetAsync();
        Assert.Equal(default((int, int, int, int, int, int, int, int)), t.value);
    }

    #endregion
}
