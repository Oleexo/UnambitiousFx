using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests
{
    [Fact]
    public void WithSuccess_Instance_AttachesProvidedInstance()
    {
        var success = new SuccessReason("ok", new Dictionary<string, object?>());
        var result = Result.Success()
                           .WithSuccess(success);

        Assert.Same(success, result.Reasons.ElementAt(0));
    }

    [Fact]
    public void WithSuccess_Instance_CopiesMetadata_WhenEnabled()
    {
        var success = new SuccessReason("ok", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithSuccess(success);

        Assert.True(result.Metadata.ContainsKey("k"));
        Assert.Equal(1, result.Metadata["k"]);
    }

    [Fact]
    public void WithSuccess_Instance_DoesNotCopyMetadata_WhenDisabled()
    {
        var success = new SuccessReason("ok", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithSuccess(success, false);

        Assert.False(result.Metadata.ContainsKey("k"));
    }

    [Fact]
    public void WithSuccess_AttachesSuccessReason()
    {
        var result = Result.Success()
                           .WithSuccess("cache hit");

        var reason = result.Reasons.FirstOrDefault();
        Assert.NotNull(reason);
        Assert.IsType<SuccessReason>(reason);
    }

    [Fact]
    public void WithSuccess_PreservesMessage()
    {
        var result = Result.Success()
                           .WithSuccess("cache");

        var reason = result.Reasons.FirstOrDefault();
        Assert.NotNull(reason);
        Assert.IsType<SuccessReason>(reason);
        Assert.Equal("cache", ((SuccessReason)reason).Message);
    }

    [Fact]
    public void WithSuccess_CopiesMetadata_WhenEnabled()
    {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta);

        Assert.True(result.Metadata.ContainsKey("source"));
        Assert.Equal("cache", result.Metadata["source"]);
    }

    [Fact]
    public void WithSuccess_DoesNotCopyMetadata_WhenDisabled()
    {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta, false);

        Assert.False(result.Metadata.ContainsKey("source"));
    }

    [Fact]
    public void WithSuccess_ReasonRetainsMetadata_WhenCopyDisabled()
    {
        var meta = new Dictionary<string, object?> { { "source", "cache" } };
        var result = Result.Success()
                           .WithSuccess("cache", meta, false);

        var sr = result.Reasons.FirstOrDefault();
        Assert.NotNull(sr);
        Assert.IsType<SuccessReason>(sr);
        var successReason = (SuccessReason)sr;
        Assert.True(successReason.Metadata.ContainsKey("source"));
        Assert.Equal("cache", successReason.Metadata["source"]);
    }

}
