using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ValueOr.Tasks;

[TestSubject(typeof(ResultValueOrExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1
    [Fact]
    public async Task ValueOrAsync_Arity1_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(5));
        var v = await awaitable.ValueOrAsync(999);
        Assert.Equal(5, v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity1_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(999);
        Assert.Equal(999, v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity1_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(5));
        var invoked = false;
        int Factory() { invoked = true; return -1; }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity1_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int>(new Exception("boom")));
        var invoked = false;
        int Factory() { invoked = true; return -1; }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 2
    [Fact]
    public async Task ValueOrAsync_Arity2_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2));
        var v = await awaitable.ValueOrAsync(9, 9);
        Assert.Equal((1, 2), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity2_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9);
        Assert.Equal((9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity2_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2));
        var invoked = false;
        (int, int) Factory() { invoked = true; return (-1, -2); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity2_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int>(new Exception("boom")));
        var invoked = false;
        (int, int) Factory() { invoked = true; return (-1, -2); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 3
    [Fact]
    public async Task ValueOrAsync_Arity3_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3));
        var v = await awaitable.ValueOrAsync(9, 9, 9);
        Assert.Equal((1, 2, 3), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity3_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9);
        Assert.Equal((9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity3_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3));
        var invoked = false;
        (int, int, int) Factory() { invoked = true; return (-1, -2, -3); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity3_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int) Factory() { invoked = true; return (-1, -2, -3); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 4
    [Fact]
    public async Task ValueOrAsync_Arity4_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9);
        Assert.Equal((1, 2, 3, 4), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity4_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9);
        Assert.Equal((9, 9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity4_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4));
        var invoked = false;
        (int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity4_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 5
    [Fact]
    public async Task ValueOrAsync_Arity5_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9);
        Assert.Equal((1, 2, 3, 4, 5), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity5_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9);
        Assert.Equal((9, 9, 9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity5_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5));
        var invoked = false;
        (int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity5_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 6
    [Fact]
    public async Task ValueOrAsync_Arity6_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity6_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9);
        Assert.Equal((9, 9, 9, 9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity6_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));
        var invoked = false;
        (int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity6_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 7
    [Fact]
    public async Task ValueOrAsync_Arity7_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9, 9);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity7_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9, 9);
        Assert.Equal((9, 9, 9, 9, 9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity7_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var invoked = false;
        (int, int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6, -7); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity7_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6, -7); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion

    #region Arity 8
    [Fact]
    public async Task ValueOrAsync_Arity8_Success_ReturnsOriginal() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9, 9, 9);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity8_Failure_ReturnsFallback() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom")));
        var v = await awaitable.ValueOrAsync(9, 9, 9, 9, 9, 9, 9, 9);
        Assert.Equal((9, 9, 9, 9, 9, 9, 9, 9), v);
    }

    [Fact]
    public async Task ValueOrAsync_Arity8_Factory_NotInvoked_OnSuccess() {
        var awaitable = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var invoked = false;
        (int, int, int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6, -7, -8); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.False(invoked);
    }

    [Fact]
    public async Task ValueOrAsync_Arity8_Factory_Invoked_OnFailure() {
        var awaitable = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom")));
        var invoked = false;
        (int, int, int, int, int, int, int, int) Factory() { invoked = true; return (-1, -2, -3, -4, -5, -6, -7, -8); }
        _ = await awaitable.ValueOrAsync(Factory);
        Assert.True(invoked);
    }
    #endregion
}
