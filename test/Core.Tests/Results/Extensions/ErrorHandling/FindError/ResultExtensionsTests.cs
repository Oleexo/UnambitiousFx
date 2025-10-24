using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.FindError;

public sealed class ResultExtensionsTests {
    // Arity 0
    [Fact]
    public void FindError_Arity0_Match_ReturnsError() {
        var r = Result.Failure(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity0_NoMatch_ReturnsNull() {
        var r = Result.Failure(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity0_Match_ReturnsTrue() {
        var r = Result.Failure(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity0_NoMatch_ReturnsFalse() {
        var r = Result.Failure(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    [Fact]
    public void FindError_Arity0_NullPredicate_Throws() {
        var r = Result.Failure(new DemoError("1"));
        Assert.Throws<ArgumentNullException>(() => r.FindError(null!));
    }

    // Arity 1
    [Fact]
    public void FindError_Arity1_Match_ReturnsError() {
        var r = Result.Failure<int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity1_NoMatch_ReturnsNull() {
        var r = Result.Failure<int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity1_Match_ReturnsTrue() {
        var r = Result.Failure<int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity1_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 2
    [Fact]
    public void FindError_Arity2_Match_ReturnsError() {
        var r = Result.Failure<int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity2_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity2_Match_ReturnsTrue() {
        var r = Result.Failure<int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity2_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 3
    [Fact]
    public void FindError_Arity3_Match_ReturnsError() {
        var r = Result.Failure<int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity3_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity3_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity3_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 4
    [Fact]
    public void FindError_Arity4_Match_ReturnsError() {
        var r = Result.Failure<int, int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity4_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity4_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity4_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 5
    [Fact]
    public void FindError_Arity5_Match_ReturnsError() {
        var r = Result.Failure<int, int, int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity5_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity5_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity5_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 6
    [Fact]
    public void FindError_Arity6_Match_ReturnsError() {
        var r = Result.Failure<int, int, int, int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity6_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity6_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity6_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 7
    [Fact]
    public void FindError_Arity7_Match_ReturnsError() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity7_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity7_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity7_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    // Arity 8
    [Fact]
    public void FindError_Arity8_Match_ReturnsError() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new DemoError("1")).WithError(new OtherError());
        var match = r.FindError(e => e is DemoError);
        Assert.NotNull(match);
    }

    [Fact]
    public void FindError_Arity8_NoMatch_ReturnsNull() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new OtherError());
        var match = r.FindError(e => e.Code == "DEMO");
        Assert.Null(match);
    }

    [Fact]
    public void TryPickError_Arity8_Match_ReturnsTrue() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new DemoError("X"));
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.True(ok);
    }

    [Fact]
    public void TryPickError_Arity8_NoMatch_ReturnsFalse() {
        var r = Result.Failure<int, int, int, int, int, int, int, int>(new OtherError());
        var ok = r.TryPickError(e => e is DemoError, out var _);
        Assert.False(ok);
    }

    private sealed record DemoError(string Id) : ErrorBase("DEMO", $"Demo {Id}");
    private sealed record OtherError() : ErrorBase("OTHER", "Other");
}
