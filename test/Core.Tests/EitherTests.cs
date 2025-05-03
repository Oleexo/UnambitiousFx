using JetBrains.Annotations;

namespace UnambitiousFx.Core.Tests;

[TestSubject(typeof(Either<,>))]
public sealed class EitherTests {
    [Fact]
    public void FromLeft_ShouldCreateLeftEither() {
        // Arrange
        var leftValue = "LeftValue";

        // Act
        var either = Either<string, int>.FromLeft(leftValue);

        // Assert
        Assert.True(either.IsLeft);
        Assert.False(either.IsRight);
    }

    [Fact]
    public void FromRight_ShouldCreateRightEither() {
        // Arrange
        var rightValue = 42;

        // Act
        var either = Either<string, int>.FromRight(rightValue);

        // Assert
        Assert.False(either.IsLeft);
        Assert.True(either.IsRight);
    }

    [Fact]
    public void Bind_ShouldApplyLeftFunc_WhenIsLeft() {
        // Arrange
        var                               leftEither = Either<string, int>.FromLeft("LeftValue");
        Func<string, Either<string, int>> leftFunc   = left => Either<string, int>.FromLeft(left.ToUpper());
        Func<int, Either<string, int>>    rightFunc  = right => Either<string, int>.FromRight(right + 1);

        // Act
        var result = leftEither.Bind(leftFunc, rightFunc);

        // Assert
        Assert.True(result.IsLeft);
        Assert.False(result.IsRight);
        Assert.Equal("LEFTVALUE", result.Match(left => left, right => ""));
    }

    [Fact]
    public void Bind_ShouldApplyRightFunc_WhenIsRight() {
        // Arrange
        var                               rightEither = Either<string, int>.FromRight(42);
        Func<string, Either<string, int>> leftFunc    = left => Either<string, int>.FromLeft(left.ToUpper());
        Func<int, Either<string, int>>    rightFunc   = right => Either<string, int>.FromRight(right + 1);

        // Act
        var result = rightEither.Bind(leftFunc, rightFunc);

        // Assert
        Assert.False(result.IsLeft);
        Assert.True(result.IsRight);
        Assert.Equal(43, result.Match(left => 0, right => right));
    }

    [Fact]
    public void Match_ShouldReturnLeftValue_WhenIsLeft() {
        // Arrange
        var                  either    = Either<string, int>.FromLeft("LeftValue");
        Func<string, string> leftFunc  = left => $"Left: {left}";
        Func<int, string>    rightFunc = right => $"Right: {right}";

        // Act
        var result = either.Match(leftFunc, rightFunc);

        // Assert
        Assert.Equal("Left: LeftValue", result);
    }

    [Fact]
    public void Match_ShouldReturnRightValue_WhenIsRight() {
        // Arrange
        var                  either    = Either<string, int>.FromRight(42);
        Func<string, string> leftFunc  = left => $"Left: {left}";
        Func<int, string>    rightFunc = right => $"Right: {right}";

        // Act
        var result = either.Match(leftFunc, rightFunc);

        // Assert
        Assert.Equal("Right: 42", result);
    }

    [Fact]
    public void Match_WithActions_ShouldExecuteLeftAction_WhenIsLeft() {
        // Arrange
        var     either      = Either<string, int>.FromLeft("LeftValue");
        string? leftResult  = null;
        int?    rightResult = null;

        void LeftAction(string left) {
            leftResult = left;
        }

        void RightAction(int right) {
            rightResult = right;
        }

        // Act
        either.Match(LeftAction, RightAction);

        // Assert
        Assert.Equal("LeftValue", leftResult);
        Assert.Null(rightResult);
    }

    [Fact]
    public void Match_WithActions_ShouldExecuteRightAction_WhenIsRight() {
        // Arrange
        var     either      = Either<string, int>.FromRight(42);
        string? leftResult  = null;
        int?    rightResult = null;

        void LeftAction(string left) {
            leftResult = left;
        }

        void RightAction(int right) {
            rightResult = right;
        }

        // Act
        either.Match(LeftAction, RightAction);

        // Assert
        Assert.Null(leftResult);
        Assert.Equal(42, rightResult);
    }

    [Fact]
    public void Left_ShouldReturnTrueAndLeftValue_WhenIsLeft() {
        // Arrange
        var either = Either<string, int>.FromLeft("LeftValue");

        // Act
        if (either.Left(out var left, out _)) {
            // Assert
            Assert.Equal("LeftValue", left);
        }
        else {
            // Assert
            Assert.Fail("Expected value should be present in either but was not found");
        }
    }

    [Fact]
    public void Left_ShouldReturnFalseAndNull_WhenIsRight() {
        // Arrange
        var either = Either<string, int>.FromRight(42);

        // Act
        var result = either.Left(out var left, out var right);

        // Assert
        Assert.False(result);
        Assert.Null(left);
        Assert.Equal(42, right);
    }

    [Fact]
    public void Right_ShouldReturnTrueAndRightValue_WhenIsRight() {
        // Arrange
        var either = Either<string, int>.FromRight(42);

        // Act
        var result = either.Right(out var left, out var right);

        // Assert
        Assert.True(result);
        Assert.Null(left);
        Assert.Equal(42, right);
    }

    [Fact]
    public void Right_ShouldReturnFalseAndNull_WhenIsLeft() {
        // Arrange
        var either = Either<string, int>.FromLeft("LeftValue");

        // Act
        if (!either.Right(out var left, out var right)) {
            // Assert
            Assert.Equal("LeftValue", left);
        }
        else {
            // Assert
            Assert.Fail("Expected value should be present in either but was not found");
        }
    }

    [Fact]
    public void ImplicitConversion_ShouldConvertLeft_ToEither() {
        // Arrange
        var leftValue = "LeftValue";

        // Act
        Either<string, int> either = leftValue;

        // Assert
        Assert.True(either.IsLeft);
        Assert.False(either.IsRight);
        Assert.Equal("LeftValue", either.Match(left => left, right => ""));
    }

    [Fact]
    public void ImplicitConversion_ShouldConvertRight_ToEither() {
        // Arrange
        var rightValue = 42;

        // Act
        Either<string, int> either = rightValue;

        // Assert
        Assert.False(either.IsLeft);
        Assert.True(either.IsRight);
        Assert.Equal(42, either.Match(left => 0, right => right));
    }
}
