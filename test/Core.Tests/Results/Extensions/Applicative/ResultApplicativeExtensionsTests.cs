using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultApplicativeExtensionsTests
{
    [Fact]
    public void Apply_FunctionAndArgSuccess_Applies()
    {
        Result<Func<int, int>> rf = Result.Success<Func<int, int>>(x => x * 2);
        var ra = Result.Success(21);

        var r = ResultExtensions.Apply(rf, ra);

        Assert.True(r.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Apply_FunctionFailure_PropagatesError()
    {
        var ex = new Exception("ferr");
        Result<Func<int, int>> rf = Result.Failure<Func<int, int>>(ex);
        var ra = Result.Success(1);

        var r = ResultExtensions.Apply(rf, ra);

        Assert.False(r.Ok(out int _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Apply_ArgFailure_PropagatesError()
    {
        var ex = new Exception("aerr");
        Result<Func<int, int>> rf = Result.Success<Func<int, int>>(x => x + 1);
        var ra = Result.Failure<int>(ex);

        var r = ResultExtensions.Apply(rf, ra);

        Assert.False(r.Ok(out int _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_BothSuccess_ReturnsTuple()
    {
        var r1 = Result.Success(10);
        var r2 = Result.Success("x");

        var zipped = ResultExtensions.Zip(r1, r2);

        Assert.True(zipped.Ok(out var tuple, out _));
        Assert.Equal((10, "x"), tuple);
    }

    [Fact]
    public void Zip_FirstFailure_Propagates()
    {
        var ex = new Exception("e1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success("x");

        var zipped = ResultExtensions.Zip(r1, r2);

        Assert.False(zipped.Ok(out (int, string) _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_SecondFailure_Propagates()
    {
        var ex = new Exception("e2");
        var r1 = Result.Success(1);
        var r2 = Result.Failure<string>(ex);

        var zipped = ResultExtensions.Zip(r1, r2);

        Assert.False(zipped.Ok(out (int, string) _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_WithProjector_Success_ReturnsProjected()
    {
        var r1 = Result.Success(20);
        var r2 = Result.Success(22);

        var sum = ResultExtensions.Zip(r1, r2, (a, b) => a + b);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Zip_WithProjector_Failure_Propagates()
    {
        var ex = new Exception("zp");
        var r1 = Result.Success(1);
        var r2 = Result.Failure<int>(ex);

        var proj = ResultExtensions.Zip(r1, r2, (a, b) => a + b);

        Assert.False(proj.Ok(out int _, out var err));
        Assert.Same(ex, err);
    }
}
