using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Ensure;

public class ResultEnsureNotEmptyTests {
    [Fact]
    public void EnsureNotEmpty_String_Empty_Fails() {
        var r = Result.Success("")
                      .EnsureNotEmpty("Must not be empty", "name");
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

    private sealed record User(string?      Email,
                               List<string> Roles);
}
