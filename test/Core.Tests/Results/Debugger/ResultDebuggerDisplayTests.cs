using System.Reflection;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Debugger;

public sealed class ResultDebuggerDisplayTests {
    private static string InvokeDebuggerDisplay(object result) {
        var m = typeof(BaseResult).GetMethod("BuildDebuggerDisplay", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(m);
        var str = m!.Invoke(result, null) as string;
        Assert.NotNull(str);
        return str!;
    }

    [Fact]
    public void Success_NoMetadata_DebuggerDisplayMinimal() {
        var r = Result.Success();
        var dbg = InvokeDebuggerDisplay(r);
        Assert.Equal("Success reasons=0", dbg);
    }

    [Fact]
    public void Success_WithMetadata_ShowsFirstTwo() {
        var r = Result.Success(1)
            .WithMetadata("a", 1)
            .WithMetadata("b", 2)
            .WithMetadata("c", 3);
        var dbg = InvokeDebuggerDisplay(r);
        Assert.StartsWith("Success reasons=0 meta=a:1,b:2", dbg);
        Assert.DoesNotContain("c:3", dbg);
    }

    [Fact]
    public void Failure_WithDomainError_ShowsCodeAndMessage() {
        var r = Result.Failure(new Exception("raw"))
            .WithError(new NotFoundError("User", "42"))
            .WithMetadata("trace", "abc")
            .WithMetadata("env", "prod")
            .WithMetadata("ignored", 1);
        var dbg = InvokeDebuggerDisplay(r);
        Assert.Contains("Failure(Resource 'User' with id '42' was not found.) code=NOT_FOUND", dbg);
        Assert.Contains("meta=trace:abc,env:prod", dbg);
        Assert.DoesNotContain("ignored:1", dbg);
    }

    [Fact]
    public void Failure_NoDomainError_NoCode() {
        var r = Result.Failure<int>(new InvalidOperationException("oops"))
            .WithMetadata("k1", "v1")
            .WithMetadata("k2", "v2")
            .WithMetadata("k3", "v3");
        var dbg = InvokeDebuggerDisplay(r);
        Assert.Contains("Failure(oops) reasons=0", dbg);
        Assert.DoesNotContain("code=", dbg);
        Assert.Contains("meta=k1:v1,k2:v2", dbg);
        Assert.DoesNotContain("k3:v3", dbg);
    }
}

