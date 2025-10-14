using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Arity;

[TestSubject(typeof(Result<,,,,,,,>))]
public sealed class ResultArity8Tests {
    [Fact]
    public void Success_Ok_ReturnsValues() {
        var r = Result.Success(1, "a", true, 2.5, 'x', 99L, (byte)7, 123m);

        if (r.Ok(out var value)) {
            Assert.Equal((1, "a", true, 2.5, 'x', 99L, (byte)7, 123m), value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Failure_Ok_ReturnsErrorMessage() {
        var r = Result.Failure<int, string, bool, double, char, long, byte, decimal>(new Exception("boom"));

        if (!r.Ok(out _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
