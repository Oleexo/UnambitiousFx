using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.XUnit.Eithers;

namespace UnambitiousFx.Core.XUnit.Tests.Eithers;

public sealed class EitherAsyncPredicateAssertionExtensionsTests {
    [Fact]
    public async Task Task_ShouldBeLeftWhereAsync() {
        await Task.FromResult(Either<int,string>.FromLeft(3))
                  .ShouldBeLeftWhereAsync(v => v == 3);
    }

    [Fact]
    public async Task Task_ShouldBeRightWhereAsync() {
        await Task.FromResult(Either<int,string>.FromRight("abc"))
                  .ShouldBeRightWhereAsync(s => s.Length == 3);
    }

    [Fact]
    public async Task ValueTask_ShouldBeLeftWhereAsync() {
        await new ValueTask<Either<int,string>>(Either<int,string>.FromLeft(5))
            .ShouldBeLeftWhereAsync(v => v > 1);
    }

    [Fact]
    public async Task ValueTask_ShouldBeRightWhereAsync() {
        await new ValueTask<Either<int,string>>(Either<int,string>.FromRight("ok"))
            .ShouldBeRightWhereAsync(s => s == "ok");
    }
}

