using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.Tasks;

[TestSubject(typeof(ResultExtensions))]
public sealed class Arity3_ZipAsyncTaskTests {
    [Fact]
    public async Task ZipAsync_Arity3_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var zipped = await r1.ZipAsync(r2, r3);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
    }

    [Fact]
    public async Task ZipAsync_Arity3_FirstFailure_Propagates() {
        var ex = new Exception("a3-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var zipped = await r1.ZipAsync(r2, r3);

        Assert.False(zipped.Ok(out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity3_ThirdFailure_Propagates() {
        var ex = new Exception("a3-f3");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3);

        Assert.False(zipped.Ok(out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var sum = await r1.ZipAsync(r2, r3, (a, b, c) => a + b + c);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(6, value);
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a3-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var proj = await r1.ZipAsync(r2, r3, (a, b, c) => a + b + c);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_ThirdFailure_Propagates() {
        var ex = new Exception("a3-zp-f3");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, (a, b, c) => a + b + c);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
