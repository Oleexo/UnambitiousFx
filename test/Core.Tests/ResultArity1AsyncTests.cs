using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests;

[TestSubject(typeof(Result<>))]
public sealed class ResultArity1AsyncTests {
    [Fact]
    public async Task Task_Success_Ok_ReturnsValue() {
        var r = await Task.FromResult(Result.Success(42));

        if (r.Ok(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task Task_Failure_Ok_ReturnsErrorMessage() {
        var r = await Task.FromResult(Result.Failure<int>(new Exception("boom")));

        if (!r.Ok(out var _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }

    [Fact]
    public async Task ValueTask_Success_Ok_ReturnsValue() {
        var r = await ValueTask.FromResult(Result.Success(42));

        if (r.Ok(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public async Task ValueTask_Failure_Ok_ReturnsErrorMessage() {
        var r = await ValueTask.FromResult(Result.Failure<int>(new Exception("boom")));

        if (!r.Ok(out var _, out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
