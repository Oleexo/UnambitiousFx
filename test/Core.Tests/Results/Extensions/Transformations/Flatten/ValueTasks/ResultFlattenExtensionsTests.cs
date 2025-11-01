using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Flatten.ValueTasks;

[TestSubject(typeof(ResultFlattenExtensions))]
public sealed class ResultFlattenExtensionsTests {
    #region Arity 1

    [Fact]
    public async Task FlattenAsync_Arity1_Success_ProjectsInner() {
        var inner = Result.Success(42);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task FlattenAsync_Arity1_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity1_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task FlattenAsync_Arity2_Success_ProjectsInner() {
        var inner = Result.Success(1, 2);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity2_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity2_InnerFailure_IsFaulted() {
        var ex    = new ArgumentException("inner");
        var inner = Result.Failure<int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task FlattenAsync_Arity3_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity3_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity3_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task FlattenAsync_Arity4_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity4_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity4_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task FlattenAsync_Arity5_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity5_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity5_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task FlattenAsync_Arity6_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity6_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int, int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity6_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task FlattenAsync_Arity7_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity7_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int, int, int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity7_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task FlattenAsync_Arity8_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        flat.ShouldBeSuccess(out var v);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task FlattenAsync_Arity8_OuterFailure_IsFaulted() {
        var ex    = new InvalidOperationException("outer");
        var outer = ValueTask.FromResult(Result.Failure<Result<int, int, int, int, int, int, int, int>>(ex));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    [Fact]
    public async Task FlattenAsync_Arity8_InnerFailure_IsFaulted() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var outer = ValueTask.FromResult(Result.Success(inner));

        var flat = await outer.FlattenAsync();

        Assert.True(flat.IsFaulted);
    }

    #endregion
}
