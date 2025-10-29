using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Types;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Aggregation;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void AllErrors_MixedResults_EnumeratesAllDomainErrors() {
        var r1 = Result.Success()
                       .WithSuccess("ok1");
        var r2 = Result.Failure(new ValidationError(new List<string> { "fail-a" }));
        var r3 = Result.Failure(new NotFoundError("User", "42"));
        var r4 = Result.Success()
                       .WithSuccess("ok2");

        var all = new[] { r1, r2, r3, r4 }.AllErrors()
                                          .ToList();

        Assert.Equal(2, all.Count);
        Assert.Contains(all, e => e.Code == "VALIDATION");
        Assert.Contains(all, e => e.Code == "NOT_FOUND");
    }

    [Fact]
    public void GroupByErrorCode_GroupsCorrectly() {
        var r1 = Result.Failure(new ValidationError(new List<string> { "x" }));
        var r2 = Result.Failure(new ValidationError(new List<string> { "y" }));
        var r3 = Result.Failure(new NotFoundError("Order", "abc"));

        var groups = new[] { r1, r2, r3 }.GroupByErrorCode();

        Assert.Equal(2, groups.Count);
        Assert.True(groups.ContainsKey("VALIDATION"));
        Assert.Equal(2, groups["VALIDATION"].Count);
        Assert.True(groups.ContainsKey("NOT_FOUND"));
        Assert.Single(groups["NOT_FOUND"]);
    }

    [Fact]
    public void SummarizeErrors_ReturnsCounts() {
        var r1 = Result.Failure(new ValidationError(new List<string> { "a" }));
        var r2 = Result.Failure(new ValidationError(new List<string> { "b" }));
        var r3 = Result.Failure(new NotFoundError("Item", "7"));

        var summary = new[] { r1, r2, r3 }.SummarizeErrors();

        Assert.Equal(2, summary.Count);
        Assert.Equal(2, summary["VALIDATION"]);
        Assert.Equal(1, summary["NOT_FOUND"]);
    }

    [Fact]
    public void Merge_AllSuccess_PreservesSuccessReasonsAndMetadata() {
        var r1 = Result.Success()
                       .WithSuccess("s1")
                       .WithMetadata("a", 1);
        var r2 = Result.Success()
                       .WithSuccess("s2")
                       .WithMetadata("b", 2);

        var merged = new[] { r1, r2 }.Merge();

        Assert.True(merged.TryGet(out _));
        Assert.Equal(2, merged.Reasons.OfType<ISuccess>()
                              .Count());
        Assert.Equal(2, merged.Metadata.Count); // a + b
        Assert.Equal(1, merged.Metadata["a"]);
        Assert.Equal(2, merged.Metadata["b"]);
    }

    [Fact]
    public void Merge_MultipleFailures_AggregatesExceptionsAndReasons() {
        var v1 = new ValidationError(new List<string> { "x" });
        var v2 = new NotFoundError("User", "1");
        var f1 = Result.Failure(v1);
        var f2 = Result.Failure(v2);

        var merged = new[] { f1, f2 }.Merge();

        Assert.False(merged.TryGet(out var primary));
        var agg = Assert.IsType<AggregateException>(primary);
        // two primary exceptions captured
        Assert.Equal(2, agg.InnerExceptions.Count);
        var errorCodes = merged.Errors()
                               .Select(e => e.Code)
                               .ToList();
        Assert.Contains("VALIDATION", errorCodes);
        Assert.Contains("NOT_FOUND",  errorCodes);
    }

    [Fact]
    public void Merge_FirstFailure_StopsEarlyAndDoesNotAccumulateLaterMetadataOrReasons() {
        var first = Result.Success()
                          .WithSuccess("s1")
                          .WithMetadata("a", 1);
        var failing = Result.Failure(new ValidationError(new List<string> { "boom" }))
                            .WithMetadata("b", 2);
        var after = Result.Success()
                          .WithSuccess("s2")
                          .WithMetadata("c", 3); // should be ignored

        var merged = new[] { first, failing, after }.Merge(MergeFailureStrategy.FirstFailure);

        Assert.False(merged.TryGet(out var primary));
        Assert.IsType<ValidationError>(merged.Errors()
                                             .First());
        Assert.True(merged.Metadata.ContainsKey("a"));
        Assert.True(merged.Metadata.ContainsKey("b"));
        Assert.False(merged.Metadata.ContainsKey("c")); // ignored
        // success reason from after should not appear
        Assert.DoesNotContain(merged.Reasons.OfType<ISuccess>(), s => s.Message == "s2");
    }

    [Fact]
    public void Merge_AccumulateAll_CollectsAllMetadataLastWriteWins() {
        var r1 = Result.Success()
                       .WithMetadata("k", 1);
        var r2 = Result.Success()
                       .WithMetadata("k", 2); // overwrites
        var r3 = Result.Success()
                       .WithMetadata("k", 3); // final

        var merged = new[] { r1, r2, r3 }.Merge(MergeFailureStrategy.AccumulateAll);

        Assert.True(merged.TryGet(out _));
        Assert.Equal(3, merged.Metadata["k"]);
    }

    [Fact]
    public void FirstFailureOrSuccess_AllSuccess_ReturnsSuccess() {
        var r1 = Result.Success()
                       .WithSuccess("a");
        var r2 = Result.Success()
                       .WithSuccess("b");
        var ff = new[] { r1, r2 }.FirstFailureOrSuccess();
        Assert.True(ff.TryGet(out _));
    }

    [Fact]
    public void FirstFailureOrSuccess_Empty_ReturnsSuccess() {
        var ff = Array.Empty<Result>()
                      .FirstFailureOrSuccess();
        Assert.True(ff.TryGet(out _));
    }

    [Fact]
    public void FirstFailureOrSuccess_ReturnsOriginalFailureWithoutAggregation() {
        var success = Result.Success()
                            .WithSuccess("s1")
                            .WithMetadata("k", "v");
        var failure = Result.Failure(new ValidationError(new List<string> { "boom" }))
                            .WithMetadata("f", "x");
        var after = Result.Success()
                          .WithSuccess("s2")
                          .WithMetadata("ignored", "y");
        var ff = new[] { success, failure, after }.FirstFailureOrSuccess();
        Assert.Same(failure, ff); // original instance
        // ensure we did not add success reasons from before or after (only original failure reasons remain)
        Assert.DoesNotContain(ff.Reasons.OfType<ISuccess>(), s => s.Message == "s1" || s.Message == "s2");
        // metadata from prior success should not leak
        Assert.False(ff.Metadata.ContainsKey("k"));
        Assert.True(ff.Metadata.ContainsKey("f"));
        Assert.False(ff.Metadata.ContainsKey("ignored"));
    }
}
