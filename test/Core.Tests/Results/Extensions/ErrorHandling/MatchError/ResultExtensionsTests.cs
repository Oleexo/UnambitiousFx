using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.MatchError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void MatchError_Arity0_Match_ReturnsOnMatch() {
        var r = Result.Failure(new Conflict("c1"))
                      .WithError(new NotFound("42"));

        var outcome = r.MatchError<NotFound, string>(nf => nf.Message, () => "none");

        Assert.Contains("Missing 42", outcome);
    }

    [Fact]
    public void MatchError_Arity0_NoMatch_ReturnsOnElse() {
        var r = Result.Failure(new Conflict("c1"));

        var outcome = r.MatchError<NotFound, string>(nf => nf.Message, () => "fallback");

        Assert.Equal("fallback", outcome);
    }

    [Fact]
    public void MatchError_Arity0_NullDelegates_Throw() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(null!,    () => "x"));
        Assert.Throws<ArgumentNullException>(() => r.MatchError<Conflict, string>(_ => "x", null!));
    }

    // Arity 1
    [Fact]
    public void MatchError_Arity1_Match_ReturnsOnMatch() {
        var r = Result.Failure<int>(new NotFound("99"));

        var matched = r.MatchError<NotFound, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity1_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 2
    [Fact]
    public void MatchError_Arity2_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int>(new NotFound("11"));

        var matched = r.MatchError<NotFound, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity2_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 3
    [Fact]
    public void MatchError_Arity3_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int>(new NotFound("13"));

        var matched = r.MatchError<NotFound, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity3_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 4
    [Fact]
    public void MatchError_Arity4_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int, int>(new NotFound("14"));

        var matched = r.MatchError<NotFound, int, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity4_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 5
    [Fact]
    public void MatchError_Arity5_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int, int, int>(new NotFound("15"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity5_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 6
    [Fact]
    public void MatchError_Arity6_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int, int, int, int>(new NotFound("16"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity6_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int, int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 7
    [Fact]
    public void MatchError_Arity7_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new NotFound("17"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity7_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    // Arity 8
    [Fact]
    public void MatchError_Arity8_Match_ReturnsOnMatch() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new NotFound("18"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.True(matched);
    }

    [Fact]
    public void MatchError_Arity8_NoMatch_ReturnsOnElse() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Conflict("c1"));

        var matched = r.MatchError<NotFound, int, int, int, int, int, int, int, int, bool>(_ => true, () => false);

        Assert.False(matched);
    }

    private sealed record Conflict(string Msg) : ErrorBase("CONFLICT", Msg);

    private sealed record NotFound(string Id) : ErrorBase("NOT_FOUND", $"Missing {Id}");
}
