using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.Maybe.ValueTasks;

namespace UnambitiousFx.Core.Tests.Options.Extensions.Bind;

public sealed class BindValueTaskTests {
    private static ValueTask<Maybe<T>> TaskOption<T>(Maybe<T> maybe)
        where T : notnull {
        return new ValueTask<Maybe<T>>(maybe);
    }

    [Fact]
    public async Task BindAsync_SomeToSome_ReturnsSome() {
        var option = Maybe.Maybe.Some(10);
        var result = await TaskOption(option)
                        .BindAsync(value => Maybe.Maybe.Some(value.ToString()));

        Assert.True(result.IsSome);
        Assert.Equal("10", result.Match(v => v, () => {
            Assert.Fail("Should not be called");
            return null;
        }));
    }

    [Fact]
    public async Task BindAsync_SomeToNone_ReturnsNone() {
        var option = Maybe.Maybe.Some(10);
        var result = await TaskOption(option)
                        .BindAsync(_ => Maybe.Maybe.None<string>());

        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task BindAsync_None_ReturnsNone() {
        var option = Maybe.Maybe.None<int>();
        var result = await TaskOption(option)
                        .BindAsync(value => Maybe.Maybe.Some(value.ToString()));

        Assert.True(result.IsNone);
    }
}
