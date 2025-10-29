using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.XUnit.Tests.Results;

public sealed class ResultAsyncAssertionExtensionsTests {
    [Fact]
    public async Task Task_NonGeneric_ShouldBeSuccess() {
        await Task.FromResult(Result.Success())
                  .ShouldBeSuccess();
    }

    [Fact]
    public async Task Task_Generic_ShouldBeSuccess() {
        await Task.FromResult(Result.Success(123))
                  .ShouldBeSuccess(out var v);
        Assert.Equal(123, v);
    }

    [Fact]
    public async Task Task_Generic_ShouldBeFailure() {
        await Task.FromResult(Result.Failure<int>(new Exception("boom")))
                  .ShouldBeFailure(out var errors);
        var firstError = errors.First();
        Assert.Equal("boom", firstError.Message);
    }

    [Fact]
    public async Task ValueTask_Generic_ShouldBeSuccess() {
        await new ValueTask<Result<int>>(Result.Success(42))
           .ShouldBeSuccess(out var v);
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task ValueTask_Generic_ShouldBeFailure() {
        await new ValueTask<Result<int>>(Result.Failure<int>(new Exception("err")))
           .ShouldBeFailure(out var errors);
        var firstError = errors.First();
        Assert.Equal("err", firstError.Message);
    }
}
