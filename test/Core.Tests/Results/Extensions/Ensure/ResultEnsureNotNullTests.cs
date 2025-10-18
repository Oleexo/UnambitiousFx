using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Ensure;

public class ResultEnsureNotNullTests {
    private sealed record User(string?      Email,
                               List<string> Roles);

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
        Assert.Contains(guarded.Reasons, rr => rr is ValidationError ve && ve.Failures.Contains("orig"));
    }
}
