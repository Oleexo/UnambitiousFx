using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations;
using ResultExtensions = UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Zip;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultExtensionsTests {
    [Fact]
    public void Zip_Arity2_BothSuccess_ReturnsTuple() {
        var r1 = Result.Success(10);
        var r2 = Result.Success("x");

        var zipped = r1.Zip(r2);

        Assert.True(zipped.Ok(out var value1, out var value2, out _));
        Assert.Equal(10,  value1);
        Assert.Equal("x", value2);
    }

    [Fact]
    public void Zip_Arity2_FirstFailure_Propagates() {
        var ex = new Exception("e1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success("x");

        var zipped = r1.Zip(r2);

        Assert.False(zipped.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity2_SecondFailure_Propagates() {
        var ex = new Exception("e2");
        var r1 = Result.Success(1);
        var r2 = Result.Failure<string>(ex);

        var zipped = r1.Zip(r2);

        Assert.False(zipped.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity2_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(20);
        var r2 = Result.Success(22);

        var sum = r1.Zip(r2, (a,
                              b) => a + b);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Zip_Arity2_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Result.Success(1);
        var r2 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, (a,
                               b) => a + b);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity2_WithProjector_SecondFailure_Propagates() {
        var ex = new Exception("zp");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(1);

        var proj = r1.Zip(r2, (a,
                               b) => a + b);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity3_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);

        var zipped = r1.Zip(r2, r3);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out _));
        Assert.Equal(1, value1);
        Assert.Equal(2, value2);
        Assert.Equal(3, value3);
    }

    [Fact]
    public void Zip_Arity3_FirstFailure_Propagates() {
        var ex = new Exception("a3-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);

        var zipped = r1.Zip(r2, r3);

        Assert.False(zipped.Ok(out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity3_ThirdFailure_Propagates() {
        var ex = new Exception("a3-f3");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3);

        Assert.False(zipped.Ok(out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity3_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);

        var sum = r1.Zip(r2, r3, (a,
                                  b,
                                  c) => a + b + c);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(6, value);
    }

    [Fact]
    public void Zip_Arity3_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a3-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);

        var proj = r1.Zip(r2, r3, (a,
                                   b,
                                   c) => a + b + c);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity3_WithProjector_ThirdFailure_Propagates() {
        var ex = new Exception("a3-zp-f3");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, (a,
                                   b,
                                   c) => a + b + c);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity4_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);

        var zipped = r1.Zip(r2, r3, r4);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out var value4, out _));
        Assert.Equal(10, value1);
        Assert.Equal(20, value2);
        Assert.Equal(30, value3);
        Assert.Equal(40, value4);
    }

    [Fact]
    public void Zip_Arity4_FirstFailure_Propagates() {
        var ex = new Exception("a4-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);

        var zipped = r1.Zip(r2, r3, r4);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity4_FourthFailure_Propagates() {
        var ex = new Exception("a4-f4");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3, r4);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity4_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);

        var sum = r1.Zip(r2, r3, r4, (a,
                                      b,
                                      c,
                                      d) => a + b + c + d);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(10, value);
    }

    [Fact]
    public void Zip_Arity4_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a4-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);

        var proj = r1.Zip(r2, r3, r4, (a,
                                       b,
                                       c,
                                       d) => a + b + c + d);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity4_WithProjector_FourthFailure_Propagates() {
        var ex = new Exception("a4-zp-f4");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, r4, (a,
                                       b,
                                       c,
                                       d) => a + b + c + d);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity5_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);

        var zipped = r1.Zip(r2, r3, r4, r5);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out _));
        Assert.Equal(10, value1);
        Assert.Equal(20, value2);
        Assert.Equal(30, value3);
        Assert.Equal(40, value4);
        Assert.Equal(50, value5);   
    }

    [Fact]
    public void Zip_Arity5_FirstFailure_Propagates() {
        var ex = new Exception("a5-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);

        var zipped = r1.Zip(r2, r3, r4, r5);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity5_FifthFailure_Propagates() {
        var ex = new Exception("a5-f5");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3, r4, r5);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity5_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);

        var sum = r1.Zip(r2, r3, r4, r5, (a,
                                          b,
                                          c,
                                          d,
                                          e) => a + b + c + d + e);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(15, value);
    }

    [Fact]
    public void Zip_Arity5_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a5-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);

        var proj = r1.Zip(r2, r3, r4, r5, (a,
                                           b,
                                           c,
                                           d,
                                           e) => a + b + c + d + e);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity5_WithProjector_FifthFailure_Propagates() {
        var ex = new Exception("a5-zp-f5");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, r4, r5, (a,
                                           b,
                                           c,
                                           d,
                                           e) => a + b + c + d + e);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity6_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);

        var zipped = r1.Zip(r2, r3, r4, r5, r6);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out _));
        Assert.Equal(10, value1);
        Assert.Equal(20, value2);
        Assert.Equal(30, value3);
        Assert.Equal(40, value4);
        Assert.Equal(50, value5);
        Assert.Equal(60, value6);  
    }

    [Fact]
    public void Zip_Arity6_FirstFailure_Propagates() {
        var ex = new Exception("a6-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);

        var zipped = r1.Zip(r2, r3, r4, r5, r6);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity6_SixthFailure_Propagates() {
        var ex = new Exception("a6-f6");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3, r4, r5, r6);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity6_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);

        var sum = r1.Zip(r2, r3, r4, r5, r6, (a,
                                              b,
                                              c,
                                              d,
                                              e,
                                              f) => a + b + c + d + e + f);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(21, value);
    }

    [Fact]
    public void Zip_Arity6_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a6-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);

        var proj = r1.Zip(r2, r3, r4, r5, r6, (a,
                                               b,
                                               c,
                                               d,
                                               e,
                                               f) => a + b + c + d + e + f);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity6_WithProjector_SixthFailure_Propagates() {
        var ex = new Exception("a6-zp-f6");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, r4, r5, r6, (a,
                                               b,
                                               c,
                                               d,
                                               e,
                                               f) => a + b + c + d + e + f);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity7_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out _));
        Assert.Equal(10, value1);
        Assert.Equal(20, value2);
        Assert.Equal(30, value3);
        Assert.Equal(40, value4);
        Assert.Equal(50, value5);
        Assert.Equal(60, value6);
        Assert.Equal(70, value7);
    }

    [Fact]
    public void Zip_Arity7_FirstFailure_Propagates() {
        var ex = new Exception("a7-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity7_SeventhFailure_Propagates() {
        var ex = new Exception("a7-f7");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity7_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);

        var sum = r1.Zip(r2, r3, r4, r5, r6, r7, (a,
                                                  b,
                                                  c,
                                                  d,
                                                  e,
                                                  f,
                                                  g) => a + b + c + d + e + f + g);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(28, value);
    }

    [Fact]
    public void Zip_Arity7_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a7-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);

        var proj = r1.Zip(r2, r3, r4, r5, r6, r7, (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e,
                                                   f,
                                                   g) => a + b + c + d + e + f + g);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity7_WithProjector_SeventhFailure_Propagates() {
        var ex = new Exception("a7-zp-f7");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, r4, r5, r6, r7, (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e,
                                                   f,
                                                   g) => a + b + c + d + e + f + g);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity8_AllSuccess_ReturnsTuple() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Success(8);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7, r8);

        Assert.True(zipped.Ok(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6, out var value7, out var value8, out _));
        Assert.Equal(10, value1);
        Assert.Equal(20, value2);
        Assert.Equal(30, value3);
        Assert.Equal(40, value4);
        Assert.Equal(50, value5);
        Assert.Equal(60, value6);
        Assert.Equal(70, value7);
        Assert.Equal(80, value8);
    }

    [Fact]
    public void Zip_Arity8_FirstFailure_Propagates() {
        var ex = new Exception("a8-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Success(8);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7, r8);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity8_EighthFailure_Propagates() {
        var ex = new Exception("a8-f8");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Failure<int>(ex);

        var zipped = r1.Zip(r2, r3, r4, r5, r6, r7, r8);

        Assert.False(zipped.Ok(out _, out _, out _, out _, out _, out _, out _, out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity8_WithProjector_Success_ReturnsProjected() {
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Success(8);

        var sum = r1.Zip(r2, r3, r4, r5, r6, r7, r8, (a,
                                                      b,
                                                      c,
                                                      d,
                                                      e,
                                                      f,
                                                      g,
                                                      h) => a + b + c + d + e + f + g + h);

        Assert.True(sum.Ok(out var value, out _));
        Assert.Equal(36, value);
    }

    [Fact]
    public void Zip_Arity8_WithProjector_FirstFailure_Propagates() {
        var ex = new Exception("a8-zp-f1");
        var r1 = Result.Failure<int>(ex);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Success(8);

        var proj = r1.Zip(r2, r3, r4, r5, r6, r7, r8, (a,
                                                       b,
                                                       c,
                                                       d,
                                                       e,
                                                       f,
                                                       g,
                                                       h) => a + b + c + d + e + f + g + h);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Zip_Arity8_WithProjector_EighthFailure_Propagates() {
        var ex = new Exception("a8-zp-f8");
        var r1 = Result.Success(1);
        var r2 = Result.Success(2);
        var r3 = Result.Success(3);
        var r4 = Result.Success(4);
        var r5 = Result.Success(5);
        var r6 = Result.Success(6);
        var r7 = Result.Success(7);
        var r8 = Result.Failure<int>(ex);

        var proj = r1.Zip(r2, r3, r4, r5, r6, r7, r8, (a,
                                                       b,
                                                       c,
                                                       d,
                                                       e,
                                                       f,
                                                       g,
                                                       h) => a + b + c + d + e + f + g + h);

        Assert.False(proj.Ok(out _, out var err));
        Assert.Same(ex, err);
    }
}
