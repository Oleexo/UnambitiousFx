using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests {
    [Fact]
    public void WithSuccess_Instance_AttachesProvidedInstance() {
        var success = new SuccessReason("ok", new Dictionary<string, object?>());
        var result = Result.Success()
                           .WithSuccess(success);

        Assert.Same(success, result.Reasons[0]);
    }

    [Fact]
    public void WithSuccess_Instance_CopiesMetadata_WhenEnabled() {
        var success = new SuccessReason("ok", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithSuccess(success);

        Assert.Equal(1, result.Metadata["k"]);
    }

    [Fact]
    public void WithSuccess_Instance_DoesNotCopyMetadata_WhenDisabled() {
        var success = new SuccessReason("ok", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithSuccess(success, false);

        Assert.False(result.Metadata.ContainsKey("k"));
    }
    
    [Fact]
    public void WithSuccess_AttachesSuccessReason() {
        var result = Result.Success()
                           .WithSuccess("cache hit");

        Assert.IsType<SuccessReason>(result.Reasons[0]);
    }

    [Fact]
    public void WithSuccess_PreservesMessage() {
        var result = Result.Success()
                           .WithSuccess("cache");

        Assert.Equal("cache", ((SuccessReason)result.Reasons[0]).Message);
    }

    [Fact]
    public void WithSuccess_CopiesMetadata_WhenEnabled() {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta);

        Assert.Equal("cache", result.Metadata["source"]);
    }

    [Fact]
    public void WithSuccess_DoesNotCopyMetadata_WhenDisabled() {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta, false);

        Assert.False(result.Metadata.ContainsKey("source"));
    }

    [Fact]
    public void WithSuccess_ReasonRetainsMetadata_WhenCopyDisabled() {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta, false);

        var sr = (SuccessReason)result.Reasons[0];
        Assert.Equal("cache", sr.Metadata["source"]);
    }

}
