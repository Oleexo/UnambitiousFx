using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests;

[TestSubject(typeof(Result<>))]
public sealed class ResultArity1Tests {
    [Fact]
    public void Success_Ok_ReturnsValue() {
        var r = Result.Success(42);

        if (r.Ok(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Failure_Ok_ReturnsErrorMessage() {
        var r = Result.Failure<int>(new Exception("boom"));

        if (!r.Ok(out var _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
