namespace UnambitiousFx.Core.Tests;

public sealed class ResultTests {
    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsTrue() {
        var result = Result<int>.Success(42);

        var b = result.Ok(out _, out _);

        Assert.True(b);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsTheValue() {
        var result = Result<int>.Success(42);

        if (result.Ok(out var value, out _)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Success branch should be reached when testing a success result");
        }
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsNullError() {
        var result = Result<int>.Success(42);

        if (result.Ok(out _, out var error)) {
            Assert.Null(error);
        }
        else {
            Assert.Fail("Success branch should be reached when testing a success result");
        }
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingOk_ThenReturnsTheError() {
        var result = Result<int>.Failure(new Error("error"));

        if (!result.Ok(out _, out var error)) {
            Assert.Equal("error", error.Message);
        }
        else {
            Assert.Fail("Failure branch should be reached when testing a failure result");
        }
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingOk_ThenReturnsFalse() {
        var result = Result<int>.Failure(new Error("error"));

        var b = result.Ok(out _, out _);

        Assert.False(b);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingIfSuccess_ThenCallsTheAction() {
        var result = Result<int>.Success(42);

        var called = false;
        result.IfSuccess(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingIfSuccess_ThenDoesNotCallTheAction() {
        var result = Result<int>.Failure(new Error("error"));

        var called = false;
        result.IfSuccess(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingIfFailure_ThenDoesNotCallTheAction() {
        var result = Result<int>.Success(42);

        var called = false;
        result.IfFailure(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingIfFailure_ThenCallsTheAction() {
        var result = Result<int>.Failure(new Error("error"));

        var called = false;
        result.IfFailure(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingMatch_ThenCallsTheSuccessAction() {
        var result = Result<int>.Success(42);

        var called = false;
        result.Match(_ => called = true, _ => called = false);

        Assert.True(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingMatch_ThenCallsTheFailureAction() {
        var result = Result<int>.Failure(new Error("error"));

        var called = false;
        result.Match(_ => called = false, _ => called = true);

        Assert.True(called);
    }
}
