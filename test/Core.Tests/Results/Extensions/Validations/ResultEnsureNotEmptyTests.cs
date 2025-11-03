using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations;

[TestSubject(typeof(ResultEnsureNotEmptyExtensions))]
public sealed class ResultEnsureNotEmptyTests {
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

    [Fact]
    public void EnsureNotEmpty_String_FailureResult_DoNothing() {
        var rInitial = Result.Failure<string>("failed");
        var r        = rInitial.EnsureNotEmpty();
        Assert.Same(rInitial, r);
    }
    
    [Fact]
    public void EnsureNotEmpty_Collection_FailureResult_DoNothing() {
        var rInitial = Result.Failure<List<int>>("failed");
        var r        = rInitial.EnsureNotEmpty<List<int>, int>();
        Assert.Same(rInitial, r);
    }
}
