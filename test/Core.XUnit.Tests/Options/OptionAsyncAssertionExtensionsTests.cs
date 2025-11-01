using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.XUnit.Options;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionAsyncAssertionExtensionsTests {
    [Fact]
    public async Task Task_Option_ShouldBeSome() {
        await Task.FromResult(Option.Some(10))
                  .ShouldBeSome(out var v);
        Assert.Equal(10, v);
    }

    [Fact]
    public async Task Task_Option_ShouldBeNone() {
        await Task.FromResult(Option<int>.None())
                  .ShouldBeNone();
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeSome() {
        await new ValueTask<Option<int>>(Option.Some(77)).ShouldBeSome(out var v);
        Assert.Equal(77, v);
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeNone() {
        await new ValueTask<Option<int>>(Option<int>.None()).ShouldBeNone();
    }
}
