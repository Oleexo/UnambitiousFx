using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.Maybe.Tasks;

namespace UnambitiousFx.Core.Tests.Maybe.Extensions.Bind;

public sealed class BindTaskTests
{
    private static Task<Maybe<T>> TaskOption<T>(Maybe<T> maybe) where T : notnull
    {
        return Task.FromResult(maybe);
    }

    [Fact]
    public async Task BindAsync_SomeToSome_ReturnsSome()
    {
        var option = Core.Maybe.Maybe.Some(10);
        var result = await TaskOption(option).BindAsync(value => Core.Maybe.Maybe.Some(value.ToString()));

        Assert.True(result.IsSome);
        Assert.Equal("10", result.Match(v => v, () =>
        {
            Assert.Fail("Should not be called");
            return null;
        }));
    }

    [Fact]
    public async Task BindAsync_SomeToNone_ReturnsNone()
    {
        var option = Core.Maybe.Maybe.Some(10);
        var result = await TaskOption(option).BindAsync(_ => Core.Maybe.Maybe.None<string>());

        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task BindAsync_None_ReturnsNone()
    {
        var option = Core.Maybe.Maybe.None<int>();
        var result = await TaskOption(option).BindAsync(value => Core.Maybe.Maybe.Some(value.ToString()));

        Assert.True(result.IsNone);
    }
}
