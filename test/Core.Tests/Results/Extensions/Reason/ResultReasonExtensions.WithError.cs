using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests {
    [Fact]
    public void WithError_Inline_CreatesErrorWithGivenCode() {
        var result = Result.Success()
                           .WithError("E_TEST", "boom");

        Assert.Equal("E_TEST", ((IError)result.Reasons[0]).Code);
    }

    [Fact]
    public void WithError_Inline_CopiesMetadata_WhenEnabled() {
        var result = Result.Success()
                           .WithError("E_TEST", "boom", metadata: new Dictionary<string, object?> { { "k", 1 } });

        Assert.Equal(1, result.Metadata["k"]);
    }

    [Fact]
    public void WithError_Inline_DoesNotCopyMetadata_WhenDisabled() {
        var result = Result.Success()
                           .WithError("E_TEST", "boom", metadata: new Dictionary<string, object?> { { "k", 1 } }, copyMetadata: false);

        Assert.False(result.Metadata.ContainsKey("k"));
    }
    
    [Fact]
    public void WithError_Instance_AttachesErrorReason() {
        var err = new NotFoundError("User", "42");
        var result = Result.Success()
                           .WithError(err);

        Assert.IsAssignableFrom<IError>(result.Reasons[0]);
    }

    [Fact]
    public void WithError_Instance_CopiesMetadata_WhenEnabled() {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err);

        Assert.Equal(3, result.Metadata["shard"]);
    }

    [Fact]
    public void WithError_Instance_DoesNotCopyMetadata_WhenDisabled() {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err, false);

        Assert.False(result.Metadata.ContainsKey("shard"));
    }

    [Fact]
    public void WithError_Instance_ReasonRetainsMetadata_WhenCopyDisabled() {
        var err = new NotFoundError("User", "42", new Dictionary<string, object?> { { "shard", 3 } });
        var result = Result.Success()
                           .WithError(err, false);

        var stored = (IError)result.Reasons[0];
        Assert.Equal(3, stored.Metadata["shard"]);
    }

}
