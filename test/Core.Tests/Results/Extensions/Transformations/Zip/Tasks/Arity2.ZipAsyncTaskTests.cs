using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.Tasks;

[TestSubject(typeof(ResultZipExtensions))]
public sealed class Arity2_ZipAsyncTaskTests {
    [Fact]
    public async Task ZipAsync_Arity2_BothSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(10));
        var r2 = Task.FromResult(Result.Success("x"));

        var zipped = await r1.ZipAsync(r2);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(10,  values.Item1);
        Assert.Equal("x", values.Item2);
    }

    [Fact]
    public async Task ZipAsync_Arity2_FirstFailure_Propagates() {
        var ex = new Exception("e1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success("x"));

        var zipped = await r1.ZipAsync(r2);

        Assert.False(zipped.Ok(out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity2_SecondFailure_Propagates() {
        var ex = new Exception("e2");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Failure<string>(ex));

        var zipped = await r1.ZipAsync(r2);

        Assert.False(zipped.Ok(out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(20));
        var r2 = Task.FromResult(Result.Success(22));

        var sum = await r1.ZipAsync(r2, (a, b) => a + b);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(42, value);
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, (a, b) => a + b);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_SecondFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(1));

        var proj = await r1.ZipAsync(r2, (a, b) => a + b);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
