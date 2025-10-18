using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.FindError;

public class ResultFindErrorTests {
    private sealed record DemoError(string Id) : ErrorBase("DEMO", $"Demo {Id}");
    private sealed record OtherError() : ErrorBase("OTHER", "Other");

    [Fact]
    public void FindError_ReturnsFirstMatchingError_NonGeneric() {
        var r = Result.Failure(new DemoError("1"))
                      .WithError(new DemoError("2"))
                      .WithError(new OtherError());

        var match = r.FindError(e => e.Code == "DEMO" && e.Message.Contains("1"));
        Assert.NotNull(match);
        Assert.Contains("Demo 1", match.Message);
    }

    [Fact]
    public void TryPickError_FindsError_GenericSingleArity() {
        var r = Result.Failure<int>(new OtherError())
                      .WithError(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError de && de.Id == "X", out var picked);
        Assert.True(ok);
        Assert.IsType<DemoError>(picked);
    }

    [Fact]
    public void TryPickError_ReturnsFalse_WhenNoMatch() {
        var r = Result.Failure<int>(new OtherError());
        var ok = r.TryPickError(e => e.Code == "MISSING", out var picked);
        Assert.False(ok);
        Assert.Null(picked);
    }

    [Fact]
    public void FindError_MultiArity3_Works() {
        var r = Result.Failure<int,int,int>(new DemoError("A"));
        var found = r.FindError<int,int,int>(e => e is DemoError de && de.Id == "A");
        Assert.NotNull(found);
    }

    [Fact]
    public void FindError_NullPredicate_Throws() {
        var r = Result.Failure(new DemoError("1"));
        Assert.Throws<ArgumentNullException>(() => r.FindError(null!));
    }
}

