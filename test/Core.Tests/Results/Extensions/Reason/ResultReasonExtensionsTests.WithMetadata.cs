using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests
{
    [Fact]
    public void WithMetadata_Dictionary_CopiesEntry()
    {
        var meta = new Dictionary<string, object?> { { "a", 1 } };
        var result = Result.Success()
                           .WithMetadata(meta);

        Assert.Equal(1, result.Metadata["a"]);
    }

    [Fact]
    public void WithMetadata_KeyValue_AddsEntry()
    {
        var result = Result.Success()
                           .WithMetadata("traceId", "abc");

        Assert.Equal("abc", result.Metadata["traceId"]);
    }

    [Fact]
    public void WithMetadata_KeyValue_LastWriteWins()
    {
        var result = Result.Success()
                           .WithMetadata("k", 1)
                           .WithMetadata("k", 2);

        Assert.Equal(2, result.Metadata["k"]);
    }

    [Fact]
    public void WithMetadata_Params_AttachesFirst()
    {
        var result = Result.Success()
                           .WithMetadata(("a", 1), ("b", "x"));

        Assert.Equal(1, result.Metadata["a"]);
    }

    [Fact]
    public void WithMetadata_Params_AttachesSecond()
    {
        var result = Result.Success()
                           .WithMetadata(("a", 1), ("b", "x"));

        Assert.Equal("x", result.Metadata["b"]);
    }
}
