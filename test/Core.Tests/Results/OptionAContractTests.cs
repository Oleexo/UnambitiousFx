using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public sealed class OptionAContractTests {
    [Fact]
    public void Failure_FromException_HasSingleExceptionalErrorReason() {
        var ex = new InvalidOperationException("boom");
        var r  = Result.Failure(ex);
        Assert.True(r.IsFaulted);
        Assert.Single(r.Reasons);
        var err = Assert.IsType<ExceptionalError>(r.Reasons[0]);
        Assert.Equal(ex, err.Exception);
        Assert.Equal(ex, r.PrimaryException());
    }

    [Fact]
    public void Failure_FromIError_DoesNotDuplicateExceptionalError() {
        var domain = new ConflictError("conflict");
        var r      = Result.Failure(domain);
        Assert.True(r.IsFaulted);
        Assert.Single(r.Reasons); // only provided domain error
        Assert.Same(domain, r.Reasons[0]);
        // PrimaryException should still surface an Exception (synthesized if domain.Exception is null)
        var primaryEx = r.PrimaryException();
        Assert.NotNull(primaryEx);
        Assert.Equal(domain.Message, primaryEx.Message);
    }

    [Fact]
    public void Failure_Generic_FromException_HasExceptionalError() {
        var ex = new Exception("x");
        var r  = Result.Failure<int>(ex);
        Assert.True(r.IsFaulted);
        Assert.Single(r.Reasons);
        Assert.IsType<ExceptionalError>(r.Reasons[0]);
        Assert.Equal(ex, r.PrimaryException());
    }

    [Fact]
    public void Failure_Generic_FromDomainError_NoDuplicate() {
        var domain = new NotFoundError("User", "42");
        var r      = Result.Failure<int>(domain);
        Assert.True(r.IsFaulted);
        Assert.Single(r.Reasons);
        Assert.Same(domain, r.Reasons[0]);
    }

    [Fact]
    public void PrimaryError_Extension_PrefersNonExceptional() {
        var ex = new Exception("raw");
        var r = Result.Failure(ex)
                      .WithError(new ValidationError(new List<string> { "f" }));
        var primary = r.PrimaryError();
        Assert.NotNull(primary);
        Assert.Equal("VALIDATION", primary.Code);
        Assert.Equal("f",          primary.Message);
    }
}
