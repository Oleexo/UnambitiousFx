using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public class ResultFactoryHelpersTests {
    [Fact]
    public void FromNullable_ValuePresent_Success() {
        var r = Result.FromNullable("hello", () => new NotFoundError("res", "id"));
        Assert.True(r.IsSuccess);
        string? captured = null;
        var     failure  = false;
        r.Match(v => captured = v, _ => failure = true);
        Assert.False(failure);
        Assert.Equal("hello", captured);
    }

    [Fact]
    public void FromNullable_ValueMissing_NotFound() {
        string? missing = null;
        var     r       = Result.FromNullable(missing, "user", "42");
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is NotFoundError nf && nf.Identifier == "42");
    }

    [Fact]
    public void FromCondition_NonGeneric_True() {
        var r = Result.FromCondition(true, () => new ConflictError("conflict"));
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public void FromCondition_NonGeneric_False() {
        var r = Result.FromCondition(false, () => new ConflictError("conflict"));
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ConflictError);
    }

    [Fact]
    public void FromCondition_Generic_PredicateTrue() {
        var r = Result.FromCondition(10, v => v > 5, () => new ValidationError(new[] { "too small" }));
        Assert.True(r.IsSuccess);
        var captured = 0;
        var failure  = false;
        r.Match(v => captured = v, _ => failure = true);
        Assert.False(failure);
        Assert.Equal(10, captured);
    }

    [Fact]
    public void FromCondition_Generic_PredicateFalse() {
        var r = Result.FromCondition(3, v => v > 5, () => new ValidationError(new[] { "too small" }));
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError ve && ve.Failures.Contains("too small"));
    }

    [Fact]
    public void FromValidation_NonGeneric_Success() {
        var r = Result.FromValidation(Array.Empty<string>());
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public void FromValidation_NonGeneric_Failure() {
        var r = Result.FromValidation(new[] { "bad" });
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError);
    }

    [Fact]
    public void FromValidation_Generic_Success() {
        var r = Result.FromValidation(5, Array.Empty<string>());
        Assert.True(r.IsSuccess);
        var captured = 0;
        var failure  = false;
        r.Match(v => captured = v, _ => failure = true);
        Assert.False(failure);
        Assert.Equal(5, captured);
    }

    [Fact]
    public void FromValidation_Generic_Failure() {
        var r = Result.FromValidation(5, new[] { "bad" });
        Assert.True(r.IsFaulted);
        Assert.Contains(r.Reasons, rr => rr is ValidationError);
    }

    [Fact]
    public void Implicit_Success_Lift_Works() {
        var r = Result.Success(42);
        Assert.True(r.IsSuccess);
        var captured = 0;
        var failure  = false;
        r.Match(v => captured = v, _ => failure = true);
        Assert.False(failure);
        Assert.Equal(42, captured);
    }
}
