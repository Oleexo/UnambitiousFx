using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.FilterError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void FilterError_Arity0_KeepSpecificErrorType_RetainsMatch() {
        var r = Result.Failure(new Conflict("c1"))
                      .WithError(new NotFound("x"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Contains(filtered.Reasons, rr => rr is NotFound);
    }

    [Fact]
    public void FilterError_Arity0_KeepSpecificErrorType_RemovesOthers() {
        var r = Result.Failure(new Conflict("c1"))
                      .WithError(new NotFound("x"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity0_RemoveAll_YieldsSuccess() {
        var r = Result.Failure(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    [Fact]
    public void FilterError_Arity0_NullPredicate_Throws() {
        var r = Result.Failure(new Conflict("c1"));
        Assert.Throws<ArgumentNullException>(() => r.FilterError(null!));
    }

    // Arity 1
    [Fact]
    public void FilterError_Arity1_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int>(new NotFound("id"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e.Code == "NOT_FOUND");
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity1_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int>(new NotFound("id"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e.Code == "NOT_FOUND");
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity1_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    [Fact]
    public void FilterError_Arity1_ResultSuccess_DoNothing() {
        var r = Result.Success(1);
        var filtered = r.FilterError(_ => false);
        Assert.Same(r, filtered);   
    }

    // Arity 2
    [Fact]
    public void FilterError_Arity2_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int>(new NotFound("id2"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity2_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int>(new NotFound("id2"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity2_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 3
    [Fact]
    public void FilterError_Arity3_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int>(new NotFound("id3"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity3_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int>(new NotFound("id3"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity3_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 4
    [Fact]
    public void FilterError_Arity4_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int, int>(new NotFound("id4"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity4_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int, int>(new NotFound("id4"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity4_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 5
    [Fact]
    public void FilterError_Arity5_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int, int, int>(new NotFound("id5"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity5_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int, int, int>(new NotFound("id5"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity5_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 6
    [Fact]
    public void FilterError_Arity6_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int, int, int, int>(new NotFound("id6"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity6_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int, int, int, int>(new NotFound("id6"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity6_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int, int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 7
    [Fact]
    public void FilterError_Arity7_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new NotFound("id7"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity7_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new NotFound("id7"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity7_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    // Arity 8
    [Fact]
    public void FilterError_Arity8_KeepByPredicate_RetainsMatch() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new NotFound("id8"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.Single(filtered.Reasons.OfType<NotFound>());
    }

    [Fact]
    public void FilterError_Arity8_KeepByPredicate_RemovesOthers() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new NotFound("id8"))
                      .WithError(new Conflict("c"));
        var filtered = r.FilterError(e => e is NotFound);
        Assert.DoesNotContain(filtered.Reasons, rr => rr is Conflict);
    }

    [Fact]
    public void FilterError_Arity8_RemoveAll_YieldsSuccess() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new Conflict("c1"));
        var filtered = r.FilterError(_ => false);
        Assert.True(filtered.IsSuccess);
    }

    [Fact]
    public void FilterError_Arity0_RemoveAll_PreservesSuccessReasonsAndMetadata() {
        var original = Result.Failure(new Conflict("c1"))
                             .WithSuccess("ok")
                             .WithMetadata("key", 123);

        var filtered = original.FilterError(_ => false);

        Assert.True(filtered.IsSuccess);
        Assert.Single(filtered.Reasons.OfType<ISuccess>());
        Assert.Empty(filtered.Reasons.OfType<IError>());
        Assert.True(filtered.Metadata.ContainsKey("key"));
        Assert.Equal(123, filtered.Metadata["key"]);
    }

    [Fact]
    public void FilterError_Arity0_KeepSomeErrors_AppendsRemainingErrors_AddsSuccessesAndMetadata() {
        var original = Result.Failure(new Conflict("first"))
                             .WithError(new Conflict("second"))
                             .WithError(new NotFound("x"))
                             .WithSuccess("ok", new Dictionary<string, object?> { ["m"] = 42 });

        var filtered = original.FilterError(e => e is Conflict);

        Assert.False(filtered.IsSuccess);

        var errors = filtered.Reasons.OfType<IError>().ToList();
        Assert.Equal(2, errors.Count);
        Assert.All(errors, e => Assert.IsType<Conflict>(e));
        Assert.DoesNotContain(filtered.Reasons, r => r is NotFound);

        Assert.Single(filtered.Reasons.OfType<ISuccess>());

        Assert.True(filtered.Metadata.ContainsKey("m"));
        Assert.Equal(42, filtered.Metadata["m"]);
    }

    private sealed record Conflict(string Msg) : ErrorBase("CONFLICT", Msg);

    private sealed record NotFound(string Id) : ErrorBase("NOT_FOUND", $"Missing {Id}");
}
