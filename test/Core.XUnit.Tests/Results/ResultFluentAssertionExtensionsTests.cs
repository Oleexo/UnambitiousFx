using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Fluent;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.XUnit.Tests.Results;

public sealed class ResultFluentAssertionExtensionsTests {
    [Fact]
    public void NonGenericResult_EnsureSuccess_Chaining() {
        Result.Success()
              .EnsureSuccess()
              .And(_ => Assert.True(true))
              .Map(_ => 123)
              .And(i => Assert.Equal(123, i));
    }

    [Fact]
    public void GenericResult_EnsureSuccess_Chaining() {
        Result.Success(42)
              .EnsureSuccess()
              .And(v => Assert.Equal(42, v))
              .Map(v => v + 1)
              .And(v => Assert.Equal(43, v));
    }

    [Fact]
    public void GenericResult_ToResult_Chaining() {
        var r2 = Result.Success(10)
                       .EnsureSuccess()
                       .ToResult(v => v * 2);
        r2.ShouldBeSuccess(out var doubled);
        Assert.Equal(20, doubled);
    }

    [Fact]
    public void GenericResult_EnsureFailure_Chaining() {
        Result.Failure<int>(new Exception("boom"))
              .EnsureFailure()
              .And(e => Assert.Equal("boom", e.Message))
              .AndMessage("boom");
    }

    [Fact]
    public void MultiArityResult_EnsureSuccess_Deconstruct() {
        var assertion = Result.Success(1, "a")
                              .EnsureSuccess();
        assertion.Deconstruct(out var tuple);
        Assert.Equal((1, "a"), tuple);
    }

    [Fact]
    public void MultiArityResult_EnsureFailure() {
        Result.Failure<int, string>(new Exception("err"))
              .EnsureFailure()
              .AndMessage("err");
    }

    [Fact]
    public async Task Async_Task_Generic_EnsureSuccess() {
        var assertion = await Task.FromResult(Result.Success(5))
                                  .EnsureSuccess();
        assertion.And(v => Assert.Equal(5, v));
    }

    [Fact]
    public async Task Async_Task_Generic_EnsureFailure() {
        var failure = await Task.FromResult(Result.Failure<int>(new Exception("x")))
                                .EnsureFailure();
        failure.AndMessage("x");
    }

    [Fact]
    public async Task Async_ValueTask_Generic_EnsureSuccess() {
        var assertion = await new ValueTask<Result<int>>(Result.Success(9)).EnsureSuccess();
        assertion.And(v => Assert.Equal(9, v));
    }
}
