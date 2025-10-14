using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.XUnit.Eithers;

namespace UnambitiousFx.Core.XUnit.Tests.Eithers;

public sealed class EitherAssertionExtensionsTests {
    [Fact]
    public void ShouldBeLeft_ExtractsLeftValue() {
        var either = Either<int, string>.FromLeft(10);
        either.ShouldBeLeft(out var left);
        Assert.Equal(10, left);
    }

    [Fact]
    public void ShouldBeRight_ExtractsRightValue() {
        var either = Either<int, string>.FromRight("ok");
        either.ShouldBeRight(out var right);
        Assert.Equal("ok", right);
    }

    [Fact]
    public void ShouldBeLeft_AssertionAction() {
        Either<int, string> either = 42; // implicit left
        either.ShouldBeLeft(v => Assert.True(v > 10));
    }

    [Fact]
    public void ShouldBeRight_AssertionAction() {
        Either<int, string> either = "value"; // implicit right
        either.ShouldBeRight(v => Assert.StartsWith("val", v));
    }
}
