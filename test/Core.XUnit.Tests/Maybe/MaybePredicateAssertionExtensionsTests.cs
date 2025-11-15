using UnambitiousFx.Core.Maybe;
using UnambitiousFx.Core.XUnit.Maybe;

namespace UnambitiousFx.Core.XUnit.Tests.Maybe;

public sealed class MaybePredicateAssertionExtensionsTests
{
    [Fact]
    public void ShouldBeSomeWhere_PredicateTrue()
    {
        Maybe<int>.Some(50)
                  .ShouldBeSomeWhere(v => v >= 50);
    }

    [Fact]
    public void ShouldBeNoneWhere_PredicateTrue()
    {
        Maybe<int>.None()
                  .ShouldBeNoneWhere(() => true);
    }

    [Fact]
    public void CustomMessage_Some()
    {
        Maybe<int>.Some(7)
                  .ShouldBeSome(out var v, "Expected Some");
        Assert.Equal(7, v);
    }

    [Fact]
    public void CustomMessage_None()
    {
        Maybe<int>.None()
                  .ShouldBeNone("Expected None");
    }
}
