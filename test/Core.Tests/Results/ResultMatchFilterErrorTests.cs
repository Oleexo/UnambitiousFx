using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultMatchFilterErrorTests {
    private sealed record Conflict(string Msg) : ErrorBase("CONFLICT", Msg);
    private sealed record NotFound(string Id) : ErrorBase("NOT_FOUND", $"Missing {Id}");

    [Fact]
    public void MatchError_NonGeneric_FindsSpecificError() {
        var r = Result.Failure(new Conflict("c1"))
                      .WithError(new NotFound("42"));
        var outcome = r.MatchError<NotFound, string>(nf => nf.Message, () => "none");
        Assert.Contains("Missing 42", outcome);
    }

    [Fact]
    public void MatchError_NonGeneric_NoMatch_UsesElse() {
        var r = Result.Failure(new Conflict("c1"));
        var outcome = r.MatchError<NotFound, string>(nf => nf.Message, () => "fallback");
        Assert.Equal("fallback", outcome);
    }

    [Fact]
    public void MatchError_Generic_SingleArity() {
        var r = Result.Failure<int>(new NotFound("99"));
        var matched = r.MatchError<NotFound, int, bool>(_ => true, () => false);
        Assert.True(matched);
    }

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
    public void MatchError_NullDelegates_Throw() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(null!, () => "x"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(_ => "x", null!));
    }

    [Fact]
    public void FilterError_NullPredicate_Throws() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.FilterError(null!));
    }
}

