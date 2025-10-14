using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.XUnit.Eithers;

namespace UnambitiousFx.Core.XUnit.Tests.Eithers;

public sealed class EitherAsyncAssertionExtensionsTests {
    [Fact]
    public async Task Task_Either_ShouldBeLeft() {
        await Task.FromResult(Either<int, string>.FromLeft(5))
                  .ShouldBeLeft(out var left);
        Assert.Equal(5, left);
    }

    [Fact]
    public async Task Task_Either_ShouldBeRight() {
        await Task.FromResult(Either<int, string>.FromRight("r"))
                  .ShouldBeRight(out var right);
        Assert.Equal("r", right);
    }

    [Fact]
    public async Task ValueTask_Either_ShouldBeLeft() {
        await new ValueTask<Either<int, string>>(Either<int, string>.FromLeft(9))
           .ShouldBeLeft(out var left);
        Assert.Equal(9, left);
    }

    [Fact]
    public async Task ValueTask_Either_ShouldBeRight() {
        await new ValueTask<Either<int, string>>(Either<int, string>.FromRight("x"))
           .ShouldBeRight(out var right);
        Assert.Equal("x", right);
    }
}
