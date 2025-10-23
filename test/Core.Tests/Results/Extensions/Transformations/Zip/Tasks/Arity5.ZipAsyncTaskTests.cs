using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.Tasks;

[TestSubject(typeof(ResultExtensions))]
public sealed class Arity5_ZipAsyncTaskTests {
    [Fact]
    public async Task ZipAsync_Arity5_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
        Assert.Equal(4, values.Item4);
        Assert.Equal(5, values.Item5);
    }

    [Fact]
    public async Task ZipAsync_Arity5_FirstFailure_Propagates() {
        var ex = new Exception("a5-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity5_FifthFailure_Propagates() {
        var ex = new Exception("a5-f5");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity5_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, (a, b, c, d, e) => a + b + c + d + e);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(15, value);
    }

    [Fact]
    public async Task ZipAsync_Arity5_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a5-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, (a, b, c, d, e) => a + b + c + d + e);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity5_WithProjector_FifthFailure_Propagates() {
        var ex = new Exception("a5-zp-f5");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, (a, b, c, d, e) => a + b + c + d + e);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
