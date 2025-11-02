using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.XUnit.Options;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionAsyncPredicateAssertionExtensionsTests {
    [Fact]
    public async Task Task_ShouldBeSomeWhereAsync() {
        await Task.FromResult(Maybe<int>.Some(10))
                  .ShouldBeSomeWhereAsync(v => v == 10);
    }

    [Fact]
    public async Task Task_ShouldBeNoneWhereAsync() {
        await Task.FromResult(Maybe<int>.None())
                  .ShouldBeNoneWhereAsync(() => true);
    }

    [Fact]
    public async Task ValueTask_ShouldBeSomeWhereAsync() {
        await new ValueTask<Maybe<int>>(Maybe<int>.Some(5))
           .ShouldBeSomeWhereAsync(v => v > 3);
    }

    [Fact]
    public async Task ValueTask_ShouldBeNoneWhereAsync() {
        await new ValueTask<Maybe<int>>(Maybe<int>.None())
           .ShouldBeNoneWhereAsync(() => true);
    }
}
