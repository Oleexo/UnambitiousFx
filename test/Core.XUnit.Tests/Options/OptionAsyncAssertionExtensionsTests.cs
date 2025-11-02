using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.XUnit.Options;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionAsyncAssertionExtensionsTests {
    [Fact]
    public async Task Task_Option_ShouldBeSome() {
        await Task.FromResult(Maybe.Maybe.Some(10))
                  .ShouldBeSome(out var v);
        Assert.Equal(10, v);
    }

    [Fact]
    public async Task Task_Option_ShouldBeNone() {
        await Task.FromResult(Maybe<int>.None())
                  .ShouldBeNone();
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeSome() {
        await new ValueTask<Maybe<int>>(Maybe.Maybe.Some(77)).ShouldBeSome(out var v);
        Assert.Equal(77, v);
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeNone() {
        await new ValueTask<Maybe<int>>(Maybe<int>.None()).ShouldBeNone();
    }
}
