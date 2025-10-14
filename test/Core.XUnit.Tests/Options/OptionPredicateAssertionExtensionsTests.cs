using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.XUnit.Options;

namespace UnambitiousFx.Core.XUnit.Tests.Options;

public sealed class OptionPredicateAssertionExtensionsTests {
    [Fact]
    public void ShouldBeSomeWhere_PredicateTrue() {
        Option<int>.Some(50)
                   .ShouldBeSomeWhere(v => v >= 50);
    }

    [Fact]
    public void ShouldBeNoneWhere_PredicateTrue() {
        Option<int>.None()
                   .ShouldBeNoneWhere(() => true);
    }

    [Fact]
    public void CustomMessage_Some() {
        Option<int>.Some(7)
                   .ShouldBeSome(out var v, "Expected Some");
        Assert.Equal(7, v);
    }

    [Fact]
    public void CustomMessage_None() {
        Option<int>.None()
                   .ShouldBeNone("Expected None");
    }
}
