using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations;
using UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations.Ensure.ValueTasks;

[TestSubject(typeof(ResultEnsureNotNullExtensions))]
public sealed class ResultEnsureNotNullTests {
    private sealed class User {
        public string? Name { get; init; }
    }

    [Fact]
    public async Task EnsureNotNullAsync_NullInner_Fails_WithFieldPrefixedMessage() {
        var user = new User { Name = null };
        var awaitable = ValueTask.FromResult(Result.Success(user));

        var r = await awaitable.EnsureNotNullAsync(u => u.Name, "is required", field: "Name");

        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("Name: is required")));
    }

    [Fact]
    public async Task EnsureNotNullAsync_NullInner_Fails_WithoutField_UsesMessage() {
        var user = new User { Name = null };
        var awaitable = ValueTask.FromResult(Result.Success(user));

        var r = await awaitable.EnsureNotNullAsync(u => u.Name, "value must exist");

        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f == "value must exist"));
    }

    [Fact]
    public async Task EnsureNotNullAsync_NonNull_PassesThrough() {
        var user = new User { Name = "Jane" };
        var awaitable = ValueTask.FromResult(Result.Success(user));

        var r = await awaitable.EnsureNotNullAsync(u => u.Name, "is required");

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out var u));
        Assert.Equal("Jane", u.Name);
    }

    [Fact]
    public async Task EnsureNotNullAsync_FailureInput_PassThrough() {
        var initial = Result.Failure<User>("boom");

        var r = await ValueTask.FromResult(initial).EnsureNotNullAsync(u => u.Name, "is required");

        Assert.Same(initial, r);
    }
}
