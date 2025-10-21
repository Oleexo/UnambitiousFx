using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests {
    [Fact]
    public void WithErrors_AddsAllErrors() {
        var e1 = new ConflictError("conflict");
        var e2 = new UnauthorizedError();
        var result = Result.Success()
                           .WithErrors(new IError[] { e1, e2 });

        Assert.Equal(2, result.Reasons.Count);
    }

    [Fact]
    public void WithErrors_CopiesMetadata_WhenEnabled() {
        var e1 = new ConflictError("c", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithErrors(new IError[] { e1 });

        Assert.Equal(1, result.Metadata["k"]);
    }

    [Fact]
    public void WithErrors_DoesNotCopyMetadata_WhenDisabled() {
        var e1 = new ConflictError("c", new Dictionary<string, object?> { { "k", 1 } });
        var result = Result.Success()
                           .WithErrors(new IError[] { e1 }, false);

        Assert.False(result.Metadata.ContainsKey("k"));
    }
}
