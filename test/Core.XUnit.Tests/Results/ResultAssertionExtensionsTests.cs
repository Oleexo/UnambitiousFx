using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.XUnit.Tests.Results;

public sealed class ResultAssertionExtensionsTests {
    [Fact]
    public void NonGenericResult_ShouldBeSuccess_DoesNotThrow() {
        var r = Result.Success();
        r.ShouldBeSuccess();
    }

    [Fact]
    public void NonGenericResult_ShouldBeFailureWithMessage_AssertsMessage() {
        var r = Result.Failure("boom");
        r.ShouldBeFailureWithMessage("boom");
    }

    [Fact]
    public void GenericResult_ShouldBeSuccess_ExtractsValue() {
        var r = Result.Success(42);
        r.ShouldBeSuccess(out var value);
        Assert.Equal(42, value);
    }

    [Fact]
    public void GenericResult_ShouldBeFailureWithMessage() {
        var r = Result.Failure<int>(new Exception("err"));
        r.ShouldBeFailureWithMessage("err");
    }

    [Fact]
    public void MultiArityResult_ShouldBeSuccess_ExtractsTuple() {
        var r = Result.Success(1, "a", true);
        r.ShouldBeSuccess(out var tuple);
        Assert.Equal((1, "a", true), tuple);
    }

    [Fact]
    public void MultiArityResult_ShouldBeFailure() {
        var r = Result.Failure<int, string, bool>(new Exception("multi"));
        r.ShouldBeFailure(out var ex);
        Assert.Equal("multi", ex.Message);
    }

    [Fact]
    public void MultiArityResult_ShouldBeSuccess_WithAction() {
        var r = Result.Success(1, 2, 3, 4);
        r.ShouldBeSuccess((a,
                           b,
                           c,
                           d) => {
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
        });
    }
}
