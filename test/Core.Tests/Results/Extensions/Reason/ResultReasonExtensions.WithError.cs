using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests
{
    [Fact]
    public void WithError_Inline_CreatesErrorWithGivenCode()
    {
        var result = Result.Success()
                           .WithError("E_TEST", "boom");

        var error = result.Reasons.OfType<IError>().FirstOrDefault();
        Assert.NotNull(error);
        Assert.Equal("E_TEST", error.Code);
    }

    [Fact]
    public void WithError_Inline_CopiesMetadata_WhenEnabled()
    {
        var result = Result.Success()
                           .WithError("E_TEST", "boom", metadata: new Dictionary<string, object?> { { "k", 1 } });

        Assert.True(result.Metadata.ContainsKey("k"));
        Assert.Equal(1, result.Metadata["k"]);
    }

    [Fact]
    public void WithError_Inline_DoesNotCopyMetadata_WhenDisabled()
    {
        var result = Result.Success()
                           .WithError("E_TEST", "boom", metadata: new Dictionary<string, object?> { { "k", 1 } }, copyMetadata: false);

        Assert.False(result.Metadata.ContainsKey("k"));
    }

    [Fact]
    public void WithError_Instance_AttachesErrorReason()
    {
        var err = new NotFoundError("User", "42");
        var result = Result.Success()
                           .WithError(err);

        var error = result.Reasons.OfType<IError>().FirstOrDefault();
        Assert.NotNull(error);
        Assert.IsType<NotFoundError>(error);
    }

    [Fact]
    public void WithError_Instance_CopiesMetadata_WhenEnabled()
    {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err);

        Assert.True(result.Metadata.ContainsKey("shard"));
        Assert.Equal(3, result.Metadata["shard"]);
    }

    [Fact]
    public void WithError_Instance_DoesNotCopyMetadata_WhenDisabled()
    {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err, false);

        Assert.False(result.Metadata.ContainsKey("shard"));
    }

    [Fact]
    public void WithError_Instance_ReasonRetainsMetadata_WhenCopyDisabled()
    {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err, false);

        var stored = result.Reasons.OfType<IError>().FirstOrDefault();
        Assert.NotNull(stored);
        Assert.IsType<NotFoundError>(stored);
        var notFoundError = (NotFoundError)stored;
        Assert.True(notFoundError.Metadata.ContainsKey("shard"));
        Assert.Equal(3, notFoundError.Metadata["shard"]);
    }

}
