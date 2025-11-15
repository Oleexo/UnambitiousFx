using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.XUnit.Maybe;

namespace UnambitiousFx.Core.XUnit.Tests.Maybe;

public sealed class OptionAssertionExtensionsTests
{
    [Fact]
    public void ShouldBeSome_ExtractsValue()
    {
        var opt = Maybe<int>.Some(42);
        opt.ShouldBeSome(out var v);
        Assert.Equal(42, v);
    }

    [Fact]
    public void ShouldBeSome_WithAssertionAction()
    {
        var opt = Maybe<int>.Some(100);
        opt.ShouldBeSome(x => Assert.True(x > 50));
    }

    [Fact]
    public void ShouldBeNone_DoesNotThrow()
    {
        var opt = Maybe<string>.None();
        opt.ShouldBeNone();
    }
}
