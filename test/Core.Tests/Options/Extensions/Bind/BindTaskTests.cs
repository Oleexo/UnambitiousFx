using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.Options.Tasks;

namespace UnambitiousFx.Core.Tests.Options.Extensions.Bind;

public sealed class BindTaskTests {
    private static Task<Option<T>> TaskOption<T>(Option<T> option) where T : notnull {
        return Task.FromResult(option);
    }

    [Fact]
    public async Task BindAsync_SomeToSome_ReturnsSome() {
        var option = Option.Some(10);
        var result = await TaskOption(option).BindAsync(value => Option.Some(value.ToString()));

        Assert.True(result.IsSome);
        Assert.Equal("10", result.Match(v => v, () => {
            Assert.Fail("Should not be called");
            return null;
        }));
    }

    [Fact]
    public async Task BindAsync_SomeToNone_ReturnsNone() {
        var option = Option.Some(10);
        var result = await TaskOption(option).BindAsync(_ => Option.None<string>());

        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task BindAsync_None_ReturnsNone() {
        var option = Option.None<int>();
        var result = await TaskOption(option).BindAsync(value => Option.Some(value.ToString()));

        Assert.True(result.IsNone);
    }
}
