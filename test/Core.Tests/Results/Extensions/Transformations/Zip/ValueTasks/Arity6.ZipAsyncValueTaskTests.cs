using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.ValueTasks;

[TestSubject(typeof(ResultExtensions))]
public sealed class Arity6_ZipAsyncValueTaskTests {
    [Fact]
    public async Task ZipAsync_Arity6_AllSuccess_ReturnsTuple() {
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Success(6));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
        Assert.Equal(4, values.Item4);
        Assert.Equal(5, values.Item5);
        Assert.Equal(6, values.Item6);
    }

    [Fact]
    public async Task ZipAsync_Arity6_FirstFailure_Propagates() {
        var ex = new Exception("a6-f1");
        var r1 = ValueTask.FromResult(Result.Failure<int>(ex));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Success(6));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity6_SixthFailure_Propagates() {
        var ex = new Exception("a6-f6");
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_Success_ReturnsProjected() {
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Success(6));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, r6, (a, b, c, d, e, f) => a + b + c + d + e + f);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(21, value);
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a6-zp-f1");
        var r1 = ValueTask.FromResult(Result.Failure<int>(ex));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Success(6));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, (a, b, c, d, e, f) => a + b + c + d + e + f);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_SixthFailure_Propagates() {
        var ex = new Exception("a6-zp-f6");
        var r1 = ValueTask.FromResult(Result.Success(1));
        var r2 = ValueTask.FromResult(Result.Success(2));
        var r3 = ValueTask.FromResult(Result.Success(3));
        var r4 = ValueTask.FromResult(Result.Success(4));
        var r5 = ValueTask.FromResult(Result.Success(5));
        var r6 = ValueTask.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, (a, b, c, d, e, f) => a + b + c + d + e + f);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
