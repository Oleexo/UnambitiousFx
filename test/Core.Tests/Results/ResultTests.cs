using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(Result<>))]
public sealed class ResultTests {
    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsTrue() {
        var result = Result.Success(42);

        var b = result.Ok(out int _, out _);

        Assert.True(b);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsTheValue() {
        var result = Result.Success(42);

        if (result.Ok(out var value, out _)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Success branch should be reached when testing a success result");
        }
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingOk_ThenReturnsNullError() {
        var result = Result.Success(42);

        if (result.Ok(out int _, out var error)) {
            Assert.Null(error);
        }
        else {
            Assert.Fail("Success branch should be reached when testing a success result");
        }
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingOk_ThenReturnsTheError() {
        var result = Result.Failure(new Exception("error"));

        if (!result.Ok(out var error)) {
            Assert.Equal("error", error.Message);
        }
        else {
            Assert.Fail("Failure branch should be reached when testing a failure result");
        }
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingOk_ThenReturnsFalse() {
        var result = Result.Failure(new Exception("error"));

        var b = result.Ok(out _);

        Assert.False(b);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingIfSuccess_ThenCallsTheAction() {
        var result = Result.Success(42);

        var called = false;
        result.IfSuccess(() => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingIfSuccess_ThenDoesNotCallTheAction() {
        var result = Result.Failure(new Exception("error"));

        var called = false;
        result.IfSuccess(() => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingIfFailure_ThenDoesNotCallTheAction() {
        var result = Result.Success(42);

        var called = false;
        result.IfFailure(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingIfFailure_ThenCallsTheAction() {
        var result = Result.Failure(new Exception("error"));

        var called = false;
        result.IfFailure(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenASuccessResult_WhenCallingMatch_ThenCallsTheSuccessAction() {
        var result = Result.Success(42);

        var called = false;
        result.Match(() => called = true, _ => called = false);

        Assert.True(called);
    }

    [Fact]
    public void GivenAFailureResult_WhenCallingMatch_ThenCallsTheFailureAction() {
        var result = Result.Failure(new Exception("error"));

        var called = false;
        result.Match(() => called = false, _ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenMultipleFunctions_WhenChained_ThenCallsTheLastFunction() {
        var result = GetUser("toto")
                    .Bind(user => GetLatestOrder(user))
                    .Bind(tuple => GetShippingInfo(tuple));

        if (result.Ok(out var value, out _)) {
            Assert.Equal("Hello 42 from fx", value);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }

        return;

        Result<string> GetShippingInfo((string order, int user) tuple) {
            return Result.Success($"Hello {tuple.user} from {tuple.order}");
        }

        Result<(string, int)> GetLatestOrder(int user) {
            return user == 42
                       ? Result.Success(("fx", 42))
                       : Result.Success(("fx", 24));
        }

        // Example of chaining operations that might fail
        Result<int> GetUser(string userId) {
            return userId == "toto"
                       ? Result.Success(42)
                       : Result.Success(24);
        }
    }

    [Fact]
    public void GivenResultWithArityOf1_ShouldBeAbleToBindToArityOf2() {
        var result = Result.Success(42);

        var result2 = result.Bind(x => Result.Success(x, x + 1));

        if (result2.Ok(out var values, out _)) {
            var (a, b) = values;
            Assert.Equal(42, a);
            Assert.Equal(43, b);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }

    [Fact]
    public void GivenResultWithArityOf3_ShouldBeAbleToBindToArityOf2() {
        var result = Result.Success(24, 42, 1337);

        var result2 = result.Bind((x,
                                   y,
                                   z) => Result.Success(x + y, y + z));

        if (result2.Ok(out var values, out _)) {
            var (a, b) = values;
            Assert.Equal(66,   a);
            Assert.Equal(1379, b);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }

    [Fact]
    public async Task GivenResultWithArityOf1_ShouldBeAbleToBindToArityOf2Async() {
        var result = Result.Success(42);

        var result2 = await Core.Results.Tasks.ResultExtensions.BindAsync(result, x => Task.FromResult(Result.Success(x, x + 1)));

        if (result2.Ok(out var values, out _)) {
            var (a, b) = values;
            Assert.Equal(42, a);
            Assert.Equal(43, b);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }

    [Fact]
    public async Task GivenResultWithArityOf3_ShouldBeAbleToBindToArityOf2Async() {
        var result = Result.Success(24, 42, 1337);

        var result2 = await Core.Results.Tasks.ResultExtensions.BindAsync(result, (x,
                                                                              y,
                                                                              z) => Task.FromResult(Result.Success(x + y, y + z)));

        if (result2.Ok(out var values, out _)) {
            var (a, b) = values;
            Assert.Equal(66,   a);
            Assert.Equal(1379, b);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }
    }
}
