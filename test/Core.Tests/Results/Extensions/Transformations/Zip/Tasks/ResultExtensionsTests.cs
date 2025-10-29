using UnambitiousFx.Core.Results.Reasons;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip.Tasks;

[TestSubject(typeof(ResultZipExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 2

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

        Assert.False(zipped.TryGet(out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity2_SecondFailure_Propagates() {
        var ex = new Exception("e2");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Failure<string>(ex));

        var zipped = await r1.ZipAsync(r2);

        Assert.False(zipped.TryGet(out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(20));
        var r2 = Task.FromResult(Result.Success(22));

        var sum = await r1.ZipAsync(r2, (a,
                                         b) => a + b);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(42, value);
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, (a,
                                          b) => a + b);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity2_WithProjector_SecondFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(1));

        var proj = await r1.ZipAsync(r2, (a,
                                          b) => a + b);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 3

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

        Assert.False(zipped.TryGet(out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity3_ThirdFailure_Propagates() {
        var ex = new Exception("a3-f3");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3);

        Assert.False(zipped.TryGet(out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var sum = await r1.ZipAsync(r2, r3, (a,
                                             b,
                                             c) => a + b + c);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(6, value);
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a3-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));

        var proj = await r1.ZipAsync(r2, r3, (a,
                                              b,
                                              c) => a + b + c);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity3_WithProjector_ThirdFailure_Propagates() {
        var ex = new Exception("a3-zp-f3");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, (a,
                                              b,
                                              c) => a + b + c);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task ZipAsync_Arity4_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));

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
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));

        var zipped = await r1.ZipAsync(r2, r3, r4);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity4_FourthFailure_Propagates() {
        var ex = new Exception("a4-f4");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));

        var sum = await r1.ZipAsync(r2, r3, r4, (a,
                                                 b,
                                                 c,
                                                 d) => a + b + c + d);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a4-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));

        var proj = await r1.ZipAsync(r2, r3, r4, (a,
                                                  b,
                                                  c,
                                                  d) => a + b + c + d);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity4_WithProjector_FourthFailure_Propagates() {
        var ex = new Exception("a4-zp-f4");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, (a,
                                                  b,
                                                  c,
                                                  d) => a + b + c + d);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 5

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

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
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

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity5_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, (a,
                                                     b,
                                                     c,
                                                     d,
                                                     e) => a + b + c + d + e);

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

        var proj = await r1.ZipAsync(r2, r3, r4, r5, (a,
                                                      b,
                                                      c,
                                                      d,
                                                      e) => a + b + c + d + e);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity5_WithProjector_FifthFailure_Propagates() {
        var ex = new Exception("a5-zp-f5");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, (a,
                                                      b,
                                                      c,
                                                      d,
                                                      e) => a + b + c + d + e);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task ZipAsync_Arity6_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));

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
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity6_SixthFailure_Propagates() {
        var ex = new Exception("a6-f6");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, r6, (a,
                                                         b,
                                                         c,
                                                         d,
                                                         e,
                                                         f) => a + b + c + d + e + f);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(21, value);
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a6-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, (a,
                                                          b,
                                                          c,
                                                          d,
                                                          e,
                                                          f) => a + b + c + d + e + f);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity6_WithProjector_SixthFailure_Propagates() {
        var ex = new Exception("a6-zp-f6");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, (a,
                                                          b,
                                                          c,
                                                          d,
                                                          e,
                                                          f) => a + b + c + d + e + f);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task ZipAsync_Arity7_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
        Assert.Equal(4, values.Item4);
        Assert.Equal(5, values.Item5);
        Assert.Equal(6, values.Item6);
        Assert.Equal(7, values.Item7);
    }

    [Fact]
    public async Task ZipAsync_Arity7_FirstFailure_Propagates() {
        var ex = new Exception("a7-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity7_SeventhFailure_Propagates() {
        var ex = new Exception("a7-f7");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity7_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, (a,
                                                             b,
                                                             c,
                                                             d,
                                                             e,
                                                             f,
                                                             g) => a + b + c + d + e + f + g);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(28, value);
    }

    [Fact]
    public async Task ZipAsync_Arity7_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a7-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, (a,
                                                              b,
                                                              c,
                                                              d,
                                                              e,
                                                              f,
                                                              g) => a + b + c + d + e + f + g);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity7_WithProjector_SeventhFailure_Propagates() {
        var ex = new Exception("a7-zp-f7");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, (a,
                                                              b,
                                                              c,
                                                              d,
                                                              e,
                                                              f,
                                                              g) => a + b + c + d + e + f + g);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task ZipAsync_Arity8_AllSuccess_ReturnsTuple() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Success(8));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8);

        zipped.ShouldBeSuccess(out var values);
        Assert.Equal(1, values.Item1);
        Assert.Equal(2, values.Item2);
        Assert.Equal(3, values.Item3);
        Assert.Equal(4, values.Item4);
        Assert.Equal(5, values.Item5);
        Assert.Equal(6, values.Item6);
        Assert.Equal(7, values.Item7);
        Assert.Equal(8, values.Item8);
    }

    [Fact]
    public async Task ZipAsync_Arity8_FirstFailure_Propagates() {
        var ex = new Exception("a8-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Success(8));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity8_EighthFailure_Propagates() {
        var ex = new Exception("a8-f8");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Failure<int>(ex));

        var zipped = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8);

        Assert.False(zipped.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity8_WithProjector_Success_ReturnsProjected() {
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Success(8));

        var sum = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8, (a,
                                                                 b,
                                                                 c,
                                                                 d,
                                                                 e,
                                                                 f,
                                                                 g,
                                                                 h) => a + b + c + d + e + f + g + h);

        sum.ShouldBeSuccess(out var value);
        Assert.Equal(36, value);
    }

    [Fact]
    public async Task ZipAsync_Arity8_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a8-zp-f1");
        var r1 = Task.FromResult(Result.Failure<int>(ex));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Success(8));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8, (a,
                                                                  b,
                                                                  c,
                                                                  d,
                                                                  e,
                                                                  f,
                                                                  g,
                                                                  h) => a + b + c + d + e + f + g + h);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public async Task ZipAsync_Arity8_WithProjector_EighthFailure_Propagates() {
        var ex = new Exception("a8-zp-f8");
        var r1 = Task.FromResult(Result.Success(1));
        var r2 = Task.FromResult(Result.Success(2));
        var r3 = Task.FromResult(Result.Success(3));
        var r4 = Task.FromResult(Result.Success(4));
        var r5 = Task.FromResult(Result.Success(5));
        var r6 = Task.FromResult(Result.Success(6));
        var r7 = Task.FromResult(Result.Success(7));
        var r8 = Task.FromResult(Result.Failure<int>(ex));

        var proj = await r1.ZipAsync(r2, r3, r4, r5, r6, r7, r8, (a,
                                                                  b,
                                                                  c,
                                                                  d,
                                                                  e,
                                                                  f,
                                                                  g,
                                                                  h) => a + b + c + d + e + f + g + h);

        Assert.False(proj.TryGet(out _, out var err));
        { var firstError = err?.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    #endregion
}
