using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.XUnit.Fluent;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionFluentAssertionExtensionsTests {
    [Fact]
    public void EnsureSome_Chaining() {
        Option<int>.Some(10)
                    .EnsureSome()
                    .And(v => Assert.Equal(10, v))
                    .Map(v => v + 5)
                    .And(v => Assert.Equal(15, v));
    }

    [Fact]
    public void EnsureNone_Chaining() {
        Option<string>.None()
                       .EnsureNone()
                       .And(() => Assert.True(true));
    }

    [Fact]
    public async Task Async_Task_EnsureSome() {
        var assertion = await Task.FromResult(Option<int>.Some(7)).EnsureSome();
        assertion.And(v => Assert.Equal(7, v));
    }

    [Fact]
    public async Task Async_ValueTask_EnsureNone() {
        var assertion = await new ValueTask<Option<int>>(Option<int>.None()).EnsureNone();
        assertion.And(() => Assert.True(true));
    }
}
