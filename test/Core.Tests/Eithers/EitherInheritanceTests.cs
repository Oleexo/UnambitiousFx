using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.Eithers;

public sealed class EitherInheritanceTests {
    [Fact]
    public void Either_ShouldBeAssignable_ToOneOf() {
        // Arrange
        var either = Either<string, int>.FromRight(123);

        // Act
        OneOf<string, int> oneOf = either; // upcast

        // Assert
        Assert.True(oneOf.IsSecond);
        Assert.False(oneOf.IsFirst);
        Assert.Equal("Right: 123", oneOf.Match(first => $"Left: {first}", second => $"Right: {second}"));
    }
}
