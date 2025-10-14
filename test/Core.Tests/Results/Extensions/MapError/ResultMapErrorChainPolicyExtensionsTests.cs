using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.MapError;

public sealed class ResultMapErrorChainPolicyExtensionsTests {
    [Fact]
    public void MapError_Accumulate_Arity0_ReasonsAccumulateAndExceptionChainMaintained() {
        var root = new Exception("root");
        var r = Result.Failure(root)
                      .WithError(new ConflictError("conflict"));

        Assert.Equal(2, r.Reasons.Count);

        var firstWrap = r.MapError(e => new InvalidOperationException("wrap1", e), MapErrorChainPolicy.Accumulate);
        Assert.False(firstWrap.Ok(out _));
        Assert.Equal(3, firstWrap.Reasons.Count);
        firstWrap.Ok(out var err1);
        Assert.IsType<InvalidOperationException>(err1);
        Assert.Equal(root, err1.InnerException);

        var secondWrap = firstWrap.MapError(e => new ApplicationException("wrap2", e), MapErrorChainPolicy.Accumulate);
        Assert.False(secondWrap.Ok(out _));
        Assert.Equal(4, secondWrap.Reasons.Count);
        secondWrap.Ok(out var err2);
        Assert.IsType<ApplicationException>(err2);
        Assert.IsType<InvalidOperationException>(err2.InnerException);
        Assert.Equal(root, err2.InnerException.InnerException);
    }

    [Fact]
    public void MapError_ShortCircuit_Arity0_DoesNotAccumulateReasons() {
        var root = new Exception("root");
        var r = Result.Failure(root)
                      .WithError(new ConflictError("conflict"));
        Assert.Equal(2, r.Reasons.Count);

        var mapped = r.MapError(e => new InvalidOperationException("wrap1", e), MapErrorChainPolicy.ShortCircuit);
        Assert.False(mapped.Ok(out _));
        Assert.Single(mapped.Reasons); // only new ExceptionalError for wrap1
        mapped.Ok(out var err);
        Assert.IsType<InvalidOperationException>(err);
        Assert.Equal(root, err.InnerException);
    }

    [Fact]
    public void MapError_Accumulate_Arity1_PreservesAndBuildsChain() {
        var root = new Exception("root");
        var r = Result.Failure<int>(root)
                      .WithError(new ConflictError("conflict"));
        Assert.Equal(2, r.Reasons.Count);

        var firstWrap = r.MapError(e => new InvalidOperationException("wrap1", e), MapErrorChainPolicy.Accumulate);
        Assert.Equal(3, firstWrap.Reasons.Count);
        firstWrap.Ok(out _, out var err1);
        Assert.IsType<InvalidOperationException>(err1);
        Assert.Equal(root, err1.InnerException);

        var secondWrap = firstWrap.MapError(e => new ApplicationException("wrap2", e), MapErrorChainPolicy.Accumulate);
        Assert.Equal(4, secondWrap.Reasons.Count);
        secondWrap.Ok(out _, out var err2);
        Assert.IsType<ApplicationException>(err2);
        Assert.IsType<InvalidOperationException>(err2.InnerException);
        Assert.Equal(root, err2.InnerException.InnerException);
    }
}
