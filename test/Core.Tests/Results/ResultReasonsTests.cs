// filepath: /Users/maxime.charlesn2f.com/dev/oleexo/UnambitiousFx/test/Core.Tests/Results/ResultReasonsTests.cs

using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultReasonsTests {
    [Fact]
    public void Failure_IError_IsFailure() {
        var r = Result.Failure(new NotFoundError("User", "42"));
        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Failure_IError_HasSingleReason() {
        var r = Result.Failure(new NotFoundError("User", "42"));
        Assert.Single(r.Reasons);
    }

    [Fact]
    public void Failure_IError_ReasonCodeMatches() {
        var r = Result.Failure(new NotFoundError("User", "42"));
        Assert.Equal("NOT_FOUND", ((IError)r.Reasons[0]).Code);
    }

    [Fact]
    public void Failure_IError_MetadataCopied() {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var r   = Result.Failure(err);
        Assert.Equal(3, r.Metadata["shard"]);
    }

    [Fact]
    public void WithMetadata_AddsKey() {
        var r = Result.Success()
                      .WithMetadata("traceId", "abc");
        Assert.Contains("traceId", r.Metadata.Keys);
    }

    [Fact]
    public void Success_WithReason_IncrementsReasons() {
        var r = Result.Success()
                      .WithReason(new SuccessReason("cache", new Dictionary<string, object?>()));
        Assert.Single(r.Reasons);
    }

    [Fact]
    public void NotFoundError_Code() {
        var e = new NotFoundError("Order", "A1");
        Assert.Equal("NOT_FOUND", e.Code);
    }

    [Fact]
    public void NotFoundError_MessageContainsResource() {
        var e = new NotFoundError("Order", "A1");
        Assert.Contains("Order", e.Message);
    }

    [Fact]
    public void ValidationError_ConcatsFailures() {
        var e = new ValidationError(new[] { "a", "b" });
        Assert.Contains("a; b", e.Message);
    }

    [Fact]
    public void ExceptionalError_UsesExceptionMessage() {
        var e = new ExceptionalError(new InvalidOperationException("boom"));
        Assert.Equal("boom", e.Message);
    }

    [Fact]
    public void GenericFailure_DomainReasonAttached() {
        var r = Result.Failure<int>(new ConflictError("conflict"));
        Assert.Single(r.Reasons);
    }

    [Fact]
    public void Metadata_LastWriteWins() {
        var r = Result.Success()
                      .WithMetadata("k", 1)
                      .WithMetadata("k", 2);
        Assert.Equal(2, r.Metadata["k"]);
    }

    [Fact]
    public void HasError_Extension_DetectsReason() {
        var r = Result.Failure<int>(new UnauthorizedError());
        Assert.True(r.HasError<UnauthorizedError, int>());
    }
}
