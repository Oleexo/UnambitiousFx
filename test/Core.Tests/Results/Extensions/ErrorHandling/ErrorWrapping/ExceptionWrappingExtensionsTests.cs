using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.ErrorWrapping;

public sealed class ExceptionWrappingExtensionsTests {
    [Fact]
    public void Wrap_Exception_ToExceptionalError() {
        var ex = new InvalidOperationException("boom");

        var error = ex.Wrap();

        Assert.Equal("EXCEPTION", error.Code);
        Assert.Equal("boom",      error.Message);
        Assert.Equal(ex,          error.Exception);
        Assert.True(error.Metadata.ContainsKey("exceptionType"));
    }

    [Fact]
    public void Wrap_WithMessageOverride_UsesOverride() {
        var ex    = new InvalidOperationException("boom");
        var error = ex.Wrap("override message");
        Assert.Equal("override message", error.Message);
    }

    [Fact]
    public void AsError_Alias_Works() {
        var ex    = new ArgumentNullException("param");
        var error = ex.AsError();
        Assert.Equal("EXCEPTION", error.Code);
    }

    [Fact]
    public void Wrap_CanBeAttachedToResult() {
        var ex     = new Exception("broken");
        var result = Result.Failure(ex.Wrap());
        Assert.False(result.Ok(out _));
        Assert.Single(result.Reasons);
        var exceptional = Assert.IsType<ExceptionalError>(result.Reasons[0]);
        Assert.Equal(ex, exceptional.Exception);
    }

    [Fact]
    public void WrapAndPrepend_PrefixesOriginalMessage() {
        var ex    = new InvalidOperationException("boom");
        var error = ex.WrapAndPrepend("CTX: ");
        Assert.Equal("CTX: boom", error.Message);
        Assert.Equal(ex,          error.Exception);
    }

    [Fact]
    public void WrapAndPrepend_WithOverride_UsesOverrideAfterPrefix() {
        var ex    = new InvalidOperationException("boom");
        var error = ex.WrapAndPrepend("CTX: ", "override");
        Assert.Equal("CTX: override", error.Message);
    }

    [Fact]
    public void WrapAndPrepend_WithMetadata_CopiesMetadata() {
        var ex    = new Exception("uh");
        var meta  = new Dictionary<string, object?> { { "traceId", "abc" } };
        var error = ex.WrapAndPrepend("API/", extra: meta);
        Assert.True(error.Metadata.ContainsKey("traceId"));
        Assert.Equal("abc", error.Metadata["traceId"]);
    }
}
