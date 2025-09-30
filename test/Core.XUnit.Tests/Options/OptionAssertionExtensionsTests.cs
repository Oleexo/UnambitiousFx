using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.XUnit.Options;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionAssertionExtensionsTests {
    [Fact]
    public void ShouldBeSome_ExtractsValue() {
        var opt = Option<int>.Some(42);
        opt.ShouldBeSome(out var v);
        Assert.Equal(42, v);
    }

    [Fact]
    public void ShouldBeSome_WithAssertionAction() {
        var opt = Option<int>.Some(100);
        opt.ShouldBeSome(x => Assert.True(x > 50));
    }

    [Fact]
    public void ShouldBeNone_DoesNotThrow() {
        var opt = Option<string>.None();
        opt.ShouldBeNone();
    }
}

