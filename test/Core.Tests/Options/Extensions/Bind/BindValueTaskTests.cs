using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.Options.ValueTasks;

namespace UnambitiousFx.Core.Tests.Options.Extensions.Bind;

public sealed class BindValueTaskTests {
    private static ValueTask<Option<T>> TaskOption<T>(Option<T> option)
        where T : notnull {
        return new ValueTask<Option<T>>(option);
    }

    [Fact]
    public async Task BindAsync_SomeToSome_ReturnsSome() {
        var option = Option.Some(10);
        var result = await TaskOption(option)
                        .BindAsync(value => Option.Some(value.ToString()));

        Assert.True(result.IsSome);
        Assert.Equal("10", result.Match(v => v, () => {
            Assert.Fail("Should not be called");
            return null;
        }));
    }

    [Fact]
    public async Task BindAsync_SomeToNone_ReturnsNone() {
        var option = Option.Some(10);
        var result = await TaskOption(option)
                        .BindAsync(_ => Option.None<string>());

        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task BindAsync_None_ReturnsNone() {
        var option = Option.None<int>();
        var result = await TaskOption(option)
                        .BindAsync(value => Option.Some(value.ToString()));

        Assert.True(result.IsNone);
    }
}
