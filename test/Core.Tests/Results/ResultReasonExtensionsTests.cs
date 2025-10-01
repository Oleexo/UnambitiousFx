using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultReasonExtensionsTests {
    [Fact]
    public void WithSuccess_AttachesSuccessReason() {
        var r = Result.Success().WithSuccess("cache hit");
        Assert.Single(r.Reasons);
        Assert.IsType<SuccessReason>(r.Reasons[0]);
    }

    [Fact]
    public void WithSuccess_CopiesMetadata_WhenEnabled() {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var r = Result.Success().WithSuccess("cache", meta, copyMetadata: true);
        Assert.Equal("cache", ((SuccessReason)r.Reasons[0]).Message);
        Assert.Equal("cache", r.Metadata["source"]);
    }

    [Fact]
    public void WithSuccess_DoesNotCopyMetadata_WhenDisabled() {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var r = Result.Success().WithSuccess("cache", meta, copyMetadata: false);
        Assert.False(r.Metadata.ContainsKey("source"));
        var sr = (SuccessReason)r.Reasons[0];
        Assert.Equal("cache", sr.Metadata["source"]);
    }

    [Fact]
    public void WithError_Inline_CreatesReasonAndCopiesMetadata() {
        var r = Result.Success().WithError("E_TEST", "boom", metadata: new Dictionary<string, object?> { { "k", 1 } });
        Assert.Single(r.Reasons);
        var err = Assert.IsAssignableFrom<IError>(r.Reasons[0]);
        Assert.Equal("E_TEST", err.Code);
        Assert.Equal(1, r.Metadata["k"]);
    }

    [Fact]
    public void WithError_ExistingInstance_NoMetadataCopyWhenDisabled() {
        var err = new NotFoundError("User","42", new Dictionary<string, object?> { { "shard", 3 } });
        var r = Result.Success().WithError(err, copyMetadata: false);
        Assert.Single(r.Reasons);
        Assert.False(r.Metadata.ContainsKey("shard"));
        var stored = (IError)r.Reasons[0];
        Assert.Equal(3, stored.Metadata["shard"]);
    }

    [Fact]
    public void WithErrors_AddsAllErrors() {
        var e1 = new ConflictError("conflict");
        var e2 = new UnauthorizedError();
        var r = Result.Success().WithErrors(new IError[] { e1, e2 });
        Assert.Collection(r.Reasons,
            _ => { },
            _ => { });
        Assert.All(r.Reasons, reason => Assert.IsAssignableFrom<IError>(reason));
    }

    [Fact]
    public void WithMetadata_ParamsTuple_AttachesAll() {
        var r = Result.Success().WithMetadata(("a",1),("b","x"));
        Assert.Equal(1, r.Metadata["a"]);
        Assert.Equal("x", r.Metadata["b"]);
    }

    [Fact]
    public void WithError_MultipleMetadataWrites_LastWins() {
        var r = Result.Success()
            .WithError("E1","m1", metadata: new Dictionary<string, object?> { { "k", 1 } })
            .WithError("E2","m2", metadata: new Dictionary<string, object?> { { "k", 2 } });
        Assert.Collection(r.Reasons,
            _ => { },
            _ => { });
        Assert.Equal(2, r.Metadata["k"]);
    }

    [Fact]
    public void WithSuccess_And_Error_Metadata_Aggregate() {
        var r = Result.Success()
            .WithSuccess("cache", new Dictionary<string, object?> { { "cache", true } })
            .WithError("E","boom", metadata: new Dictionary<string, object?> { { "attempt", 1 } });
        Assert.True((bool)r.Metadata["cache"]!);
        Assert.Equal(1, r.Metadata["attempt"]);
    }
}
