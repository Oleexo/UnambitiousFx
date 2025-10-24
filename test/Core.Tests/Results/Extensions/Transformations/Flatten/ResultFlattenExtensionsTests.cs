using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Flatten;

[TestSubject(typeof(ResultFlattenExtensions))]
public sealed class ResultFlattenExtensionsTests {
    #region Arity 1

    [Fact]
    public void Flatten_Arity1_Success_ProjectsInner() {
        var inner = Result.Success(42);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var value);
        Assert.Equal(42, value);
    }

    [Fact]
    public void Flatten_Arity1_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity1_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 2

    [Fact]
    public void Flatten_Arity2_Success_ProjectsInner() {
        var inner = Result.Success(1, 2);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
    }

    [Fact]
    public void Flatten_Arity2_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity2_InnerFailure_IsFaulted() {
        var ex    = new ArgumentException("inner");
        var inner = Result.Failure<int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 3

    [Fact]
    public void Flatten_Arity3_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
    }

    [Fact]
    public void Flatten_Arity3_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity3_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 4

    [Fact]
    public void Flatten_Arity4_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c, out var d);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
    }

    [Fact]
    public void Flatten_Arity4_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity4_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 5

    [Fact]
    public void Flatten_Arity5_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c, out var d, out var e);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
    }

    [Fact]
    public void Flatten_Arity5_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity5_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 6

    [Fact]
    public void Flatten_Arity6_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c, out var d, out var e, out var f);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
        Assert.Equal(6, f);
    }

    [Fact]
    public void Flatten_Arity6_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int, int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity6_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 7

    [Fact]
    public void Flatten_Arity7_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
        Assert.Equal(6, f);
        Assert.Equal(7, g);
    }

    [Fact]
    public void Flatten_Arity7_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int, int, int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity7_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 8

    [Fact]
    public void Flatten_Arity8_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
        Assert.Equal(6, f);
        Assert.Equal(7, g);
        Assert.Equal(8, h);
    }

    [Fact]
    public void Flatten_Arity8_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int, int, int, int, int, int, int, int>>(ex);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public void Flatten_Arity8_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        Assert.True(flat.IsFaulted);
    }

    #endregion
}
