using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.ValueTasks;

[TestSubject(typeof(ResultExtensions))]
public sealed class Arity4_ZipAsyncValueTaskTests {
    [Fact]
    public async Task ZipAsync_Arity4_AllSuccess_ReturnsTuple() {
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));

        var zipped = await r1.ZipAsync(r2, r3, r4);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
        Assert.Equal(4, values.Item4);
    }

    [Fact]
    public async Task ZipAsync_Arity4_FirstFailure_Propagates() {
        var ex = new Exception("a4-f1");
        var r1 = ValueTask.FromResult(Result.Failure<int>(ex));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));

        var zipped = await r1.ZipAsync(r2, r3, r4);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity4_FourthFailure_Propagates() {
        var ex = new Exception("a4-f4");
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_Success_ReturnsProjected() {
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));

        var sum = await r1.ZipAsync(r2, r3, r4, (a, b, c, d) => a + b + c + d);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a4-zp-f1");
        var r1 = ValueTask.FromResult(Result.Failure<int>(ex));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));

        var proj = await r1.ZipAsync(r2, r3, r4, (a, b, c, d) => a + b + c + d);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_FourthFailure_Propagates() {
        var ex = new Exception("a4-zp-f4");
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, (a, b, c, d) => a + b + c + d);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
