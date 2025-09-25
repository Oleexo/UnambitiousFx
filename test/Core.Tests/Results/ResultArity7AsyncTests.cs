using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(Result<,,,,,,>))]
public sealed class ResultArity7AsyncTests {
    [Fact]
    public async Task Task_Success_Ok_ReturnsValues() {
        var r = await Task.FromResult(Result.Success(1, "a", true, 2.5, 'x', 99L, (byte)7));

        if (r.Ok(out var value)) {
            Assert.Equal((1, "a", true, 2.5, 'x', 99L, (byte)7), value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task Task_Failure_Ok_ReturnsErrorMessage() {
        var r = await Task.FromResult(Result.Failure<int, string, bool, double, char, long, byte>(new Exception("boom")));

        if (!r.Ok(out var _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ValueTask_Success_Ok_ReturnsValues() {
        var r = await ValueTask.FromResult(Result.Success(1, "a", true, 2.5, 'x', 99L, (byte)7));

        if (r.Ok(out var value)) {
            Assert.Equal((1, "a", true, 2.5, 'x', 99L, (byte)7), value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ValueTask_Failure_Ok_ReturnsErrorMessage() {
        var r = await ValueTask.FromResult(Result.Failure<int, string, bool, double, char, long, byte>(new Exception("boom")));

        if (!r.Ok(out var _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}

