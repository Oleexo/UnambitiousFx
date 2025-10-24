using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ToNullable.Tasks;

[TestSubject(typeof(ResultToNullableExtensions))]
public sealed class ResultToNullableExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task ToNullableAsync_Arity1_Success_ReturnsValue() {
        var awaitable = Task.FromResult(Result.Success(5));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(5, n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity1_Failure_ReturnsDefault() {
        var awaitable = Task.FromResult(Result.Failure<int>(new Exception("boom")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(0, n);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task ToNullableAsync_Arity2_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(2, 3));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((2, 3), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity2_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int>(new InvalidOperationException("fail")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int)?)null, n);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task ToNullableAsync_Arity3_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((1, 2, 3), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity3_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int>(new Exception("boom3")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int)?)null, n);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task ToNullableAsync_Arity4_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((1, 2, 3, 4), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity4_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int>(new Exception("boom4")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int)?)null, n);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task ToNullableAsync_Arity5_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int, int)?)(1, 2, 3, 4, 5), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity5_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int>(new Exception("boom5")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int, int)?)null, n);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task ToNullableAsync_Arity6_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity6_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int>(new Exception("boom6")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int, int, int)?)null, n);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task ToNullableAsync_Arity7_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity7_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom7")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int, int, int, int)?)null, n);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task ToNullableAsync_Arity8_Success_ReturnsTuple() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), n);
    }

    [Fact]
    public async Task ToNullableAsync_Arity8_Failure_ReturnsNull() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom8")));
        var n = await awaitable.ToNullableAsync();
        Assert.Equal(((int, int, int, int, int, int, int, int)?)null, n);
    }
    #endregion
}