using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations;
using UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations.Ensure.ValueTasks;

[TestSubject(typeof(ResultEnsureNotEmptyExtensions))]
public sealed class ResultEnsureNotEmptyTests {
    [Fact]
    public async Task EnsureNotEmptyAsync_String_Empty_Fails_WithFieldAndCustomMessage() {
        var awaitable = ValueTask.FromResult(Result.Success(""));

        var r = await awaitable.EnsureNotEmptyAsync("Must not be empty", field: "name");

        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("name: Must not be empty")));
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_String_Empty_Fails_WithDefaultMessage() {
        var awaitable = ValueTask.FromResult(Result.Success(""));

        var r = await awaitable.EnsureNotEmptyAsync();

        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f == "Value must not be empty."));
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_String_NonEmpty_Succeeds() {
        var awaitable = ValueTask.FromResult(Result.Success("value"));

        var r = await awaitable.EnsureNotEmptyAsync();

        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_String_FailureResult_DoNothing() {
        var initial = Result.Failure<string>("failed");

        var r = await ValueTask.FromResult(initial).EnsureNotEmptyAsync();

        Assert.Same(initial, r);
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_Collection_Empty_Fails_WithField_AndDefaultMessage() {
        var awaitable = ValueTask.FromResult(Result.Success(new List<int>()));

        var r = await awaitable.EnsureNotEmptyAsync<List<int>, int>(field: "items");

        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("items: Collection must not be empty.")));
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_Collection_NonEmpty_Succeeds() {
        var awaitable = ValueTask.FromResult(Result.Success(new List<int> { 1 }));

        var r = await awaitable.EnsureNotEmptyAsync<List<int>, int>();

        Assert.True(r.IsSuccess);
    }

    [Fact]
    public async Task EnsureNotEmptyAsync_Collection_FailureResult_DoNothing() {
        var initial = Result.Failure<List<int>>("failed");

        var r = await ValueTask.FromResult(initial).EnsureNotEmptyAsync<List<int>, int>();

        Assert.Same(initial, r);
    }
}
