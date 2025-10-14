using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.XUnit.Eithers;

namespace UnambitiousFx.Core.XUnit.Tests.Eithers;

public sealed class EitherPredicateAssertionExtensionsTests {
    [Fact]
    public void ShouldBeLeftWhere_PredicateTrue() {
        Either<int, string>.FromLeft(10)
                           .ShouldBeLeftWhere(l => l == 10);
    }

    [Fact]
    public void ShouldBeRightWhere_PredicateTrue() {
        Either<int, string>.FromRight("abc")
                           .ShouldBeRightWhere(r => r.StartsWith("a"));
    }

    [Fact]
    public void CustomMessage_Left() {
        Either<int, string>.FromLeft(5)
                           .ShouldBeLeft(out var v, "Expected Left");
        Assert.Equal(5, v);
    }

    [Fact]
    public void CustomMessage_Right() {
        Either<int, string>.FromRight("z")
                           .ShouldBeRight(out var v, "Expected Right");
        Assert.Equal("z", v);
    }
}
