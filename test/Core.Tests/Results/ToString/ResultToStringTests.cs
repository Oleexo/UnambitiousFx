using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;
using Xunit;

namespace UnambitiousFx.Core.Tests.Results.ToStringFormatting;

public sealed class ResultToStringTests {
    [Fact]
    public void NonGenericSuccess_NoMetadata() {
        var r = Result.Success();
        var s = r.ToString();
        Assert.StartsWith("Success reasons=0", s);
        Assert.DoesNotContain("meta=", s);
    }

    [Fact]
    public void NonGenericSuccess_WithMetadata_LimitsToTwo() {
        var r = Result.Success()
            .WithMetadata("env", "prod")
            .WithMetadata("trace", "abc")
            .WithMetadata("ignored", 123);
        var s = r.ToString();
        Assert.Contains("meta=env:prod,trace:abc", s);
        Assert.DoesNotContain("ignored:123", s);
    }

    [Fact]
    public void NonGenericFailure_WithDomainErrorAndMetadata() {
        var r = Result.Failure(new Exception("boom"))
            .WithError(new ConflictError("conflict"))
            .WithMetadata("userId", 42)
            .WithMetadata("req", "123")
            .WithMetadata("extra", true);
        var s = r.ToString();
        Assert.Contains("Failure(ConflictError: conflict)", s);
        Assert.Contains("code=CONFLICT", s);
        Assert.Contains("meta=userId:42,req:123", s);
        Assert.DoesNotContain("extra:true", s);
    }

    [Fact]
    public void GenericSuccessArity2_WithMetadata() {
        var r = Result.Success(1, 2)
            .WithMetadata("a", 1)
            .WithMetadata("b", 2)
            .WithMetadata("c", 3);
        var s = r.ToString();
        Assert.Contains("Success<int, int>(1, 2)", s);
        Assert.Contains("meta=a:1,b:2", s);
        Assert.DoesNotContain("c:3", s);
    }

    [Fact]
    public void GenericSuccessArity2_NoMetadata_NoMetaPart() {
        var r = Result.Success(10, 20);
        var s = r.ToString();
        Assert.DoesNotContain("meta=", s);
    }

    [Fact]
    public void GenericFailureArity1_WithValidationError() {
        var r = Result.Failure<int>(new Exception("raw"))
            .WithError(new ValidationError(new List<string>{"Field A is required"}));
        var s = r.ToString();
        Assert.Contains("Failure<int>(ValidationError: Field A is required)", s);
        Assert.Contains("code=VALIDATION", s);
    }

    [Fact]
    public void GenericFailureArity3_NoDomainError_NoCode() {
        var r = Result.Failure<int,int,int>(new InvalidOperationException("bad"))
            .WithMetadata("k", "v")
            .WithMetadata("z", 9);
        var s = r.ToString();
        Assert.Contains("Failure<int, int, int>(InvalidOperationException: bad)", s);
        Assert.DoesNotContain("code=", s);
        Assert.Contains("meta=k:v,z:9", s);
    }

    [Fact]
    public void NonGenericFailure_NoDomainError_ShowsExceptionTypeNoCode() {
        var r = Result.Failure(new InvalidOperationException("broken"))
            .WithMetadata("a", 1)
            .WithMetadata("b", 2)
            .WithMetadata("c", 3);
        var s = r.ToString();
        Assert.StartsWith("Failure(InvalidOperationException: broken)", s);
        Assert.DoesNotContain("code=", s);
        Assert.Contains("meta=a:1,b:2", s);
        Assert.DoesNotContain("c:3", s);
    }

    [Fact]
    public void Success_MetadataNullValue_RendersNullLiteral() {
        var r = Result.Success(123)
            .WithMetadata("key1", null)
            .WithMetadata("key2", "value")
            .WithMetadata("ignored", 9);
        var s = r.ToString();
        Assert.Contains("meta=key1:null,key2:value", s);
        Assert.DoesNotContain("ignored:9", s);
    }
}
