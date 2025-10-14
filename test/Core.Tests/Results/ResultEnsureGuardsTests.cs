using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultEnsureGuardsTests {
    private sealed record User(string? Email, List<string> Roles);

    [Fact]
    public void EnsureNotNull_WhenInnerNull_FailsWithValidationError() {
        var r = Result.Success(new User(null, new List<string> { "admin" }))
                       .EnsureNotNull(u => u.Email, "Email required", field: "email");
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("email: Email required")));
    }

    [Fact]
    public void EnsureNotNull_PassesThrough_WhenInnerNotNull() {
        var r = Result.Success(new User("a@b.c", new List<string>()))
                       .EnsureNotNull(u => u.Email, "Email required");
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public void EnsureNotNull_DoesNotOverrideExistingFailure() {
        var failure = Result.Failure<User>(new ValidationError(new[] { "orig" }));
        var guarded = failure.EnsureNotNull(u => u.Email, "Email required");
        Assert.Same(failure, guarded); // same instance semantics preserved
        Assert.Contains(guarded.Reasons, rr => rr is ValidationError ve && ve.Failures.Contains("orig"));
    }

    [Fact]
    public void EnsureNotEmpty_String_Empty_Fails() {
        var r = Result.Success("")
                       .EnsureNotEmpty("Must not be empty", field: "name");
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("name: Must not be empty")));
    }

    [Fact]
    public void EnsureNotEmpty_String_NonEmpty_Succeeds() {
        var r = Result.Success("value")
                       .EnsureNotEmpty();
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public void EnsureNotEmpty_Collection_Empty_Fails() {
        var r = Result.Success(new List<int>())
                       .EnsureNotEmpty<List<int>, int>(field: "items");
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Any(f => f.Contains("items: Collection must not be empty.")));
    }

    [Fact]
    public void EnsureNotEmpty_Collection_NonEmpty_Succeeds() {
        var r = Result.Success(new List<int> { 1 })
                       .EnsureNotEmpty<List<int>, int>();
        Assert.True(r.IsSuccess);
    }
}

