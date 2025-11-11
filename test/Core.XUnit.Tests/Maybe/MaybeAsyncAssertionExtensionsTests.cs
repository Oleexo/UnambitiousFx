using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.XUnit.Maybe;

namespace UnambitiousFx.Core.XUnit.Tests.Maybe;

public sealed class MaybeAsyncAssertionExtensionsTests
{
    [Fact]
    public async Task Task_Option_ShouldBeSome()
    {
        await Task.FromResult(Maybe<int>.Some(10))
            .ShouldBeSome(out var v);
        Assert.Equal(10, v);
    }

    [Fact]
    public async Task Task_Option_ShouldBeNone()
    {
        await Task.FromResult(Maybe<int>.None())
            .ShouldBeNone();
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeSome()
    {
        await new ValueTask<Maybe<int>>(Maybe<int>.Some(77)).ShouldBeSome(out var v);
        Assert.Equal(77, v);
    }

    [Fact]
    public async Task ValueTask_Option_ShouldBeNone()
    {
        await new ValueTask<Maybe<int>>(Maybe<int>.None()).ShouldBeNone();
    }
}
