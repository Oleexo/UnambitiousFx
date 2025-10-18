using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.FilterError;

public class ResultFilterErrorTests {
    private sealed record Conflict(string Msg) : ErrorBase("CONFLICT", Msg);
    private sealed record NotFound(string Id) : ErrorBase("NOT_FOUND", $"Missing {Id}");

    [Fact]
    public void FilterError_NonGeneric_DropsSpecifiedErrorType() {
        var r = Result.Failure(new Conflict("c1"))
                      .WithError(new NotFound("x"));
        var filtered = r.FilterError(e => e is NotFound); // keep only NotFound
        Assert.True(filtered.IsFaulted);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
        Assert.Contains(filtered.Reasons, rr => rr is NotFound);
    }

    [Fact]
    public void FilterError_NonGeneric_RemoveAll_YieldsSuccess() {
        var r = Result.Failure(new Conflict("c1"));
        var filtered = r.FilterError(_ => false); // remove all
        Assert.True(filtered.IsSuccess);
    }

    [Fact]
    public void FilterError_Generic_SingleArity_KeepOne() {
        var r = Result.Failure<int>(new NotFound("id"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError<int>(e => e.Code == "NOT_FOUND");
        Assert.True(filtered.IsFaulted);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }
    
    [Fact]
    public void FilterError_NullPredicate_Throws() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.FilterError(null!));
    }
}

