using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.MatchError;

public sealed class ResultMatchErrorTests {
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
        var r       = Result.Failure(new Conflict("c1"));
        var outcome = r.MatchError<NotFound, string>(nf => nf.Message, () => "fallback");
        Assert.Equal("fallback", outcome);
    }

    [Fact]
    public void MatchError_Generic_SingleArity() {
        var r       = Result.Failure<int>(new NotFound("99"));
        var matched = r.MatchError<NotFound, int, bool>(_ => true, () => false);
        Assert.True(matched);
    }

    [Fact]
    public void MatchError_NullDelegates_Throw() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(null!,    () => "x"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(_ => "x", null!));
    }
}
