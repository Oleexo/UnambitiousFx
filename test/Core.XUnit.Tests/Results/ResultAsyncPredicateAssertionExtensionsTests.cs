using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.XUnit.Tests.Results;

public sealed class ResultAsyncPredicateAssertionExtensionsTests {
    [Fact]
    public async Task Task_ShouldBeSuccessWhereAsync_Arity1() {
        await Task.FromResult(Result.Success(42))
                  .ShouldBeSuccessWhereAsync(v => v > 40);
    }

    [Fact]
    public async Task Task_ShouldBeFailureWhereAsync_Arity1() {
        await Task.FromResult(Result.Failure<int>(new Exception("boom")))
                  .ShouldBeFailureWhereAsync(ex => ex.Message == "boom");
    }

    [Fact]
    public async Task ValueTask_ShouldBeSuccessWhereAsync_Arity2() {
        await new ValueTask<Result<int,int>>(Result.Success(1,2))
            .ShouldBeSuccessWhereAsync(t => t.Item1 + t.Item2 == 3);
    }

    [Fact]
    public async Task ValueTask_ShouldBeFailureWhereAsync_Arity2() {
        await new ValueTask<Result<int,int>>(Result.Failure<int,int>(new InvalidOperationException("x")))
            .ShouldBeFailureWhereAsync(ex => ex is InvalidOperationException);
    }
}

