using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;

// async Task flatten

// async ValueTask flatten

namespace UnambitiousFx.Core.Tests.Results.Extensions.Flatten;

public class ResultFlattenTests {
    [Fact]
    public void Flatten_SingleArity_Success() {
        var inner = Result.Success(42);
        var outer = Result.Success(inner); // Result<Result<int>>
        var flat  = outer.Flatten();
        Assert.True(flat.IsSuccess);
        Assert.True(flat.Ok(out var value));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Flatten_SingleArity_OuterFailure() {
        var ex    = new InvalidOperationException("outer");
        var outer = Result.Failure<Result<int>>(ex);
        var flat  = outer.Flatten();
        Assert.True(flat.IsFaulted);
        Assert.False(flat.Ok(out var _));
    }

    [Fact]
    public void Flatten_SingleArity_InnerFailure() {
        var ex    = new Exception("inner");
        var inner = Result.Failure<int>(ex);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();
        Assert.True(flat.IsFaulted);
        Assert.False(flat.Ok(out var _));
    }

    [Fact]
    public void Flatten_MultiArity_Success() {
        var inner = Result.Success(1, 2, 3);
        var outer = Result.Success(inner); // Result<Result<int,int,int>>
        var flat  = outer.Flatten();
        Assert.True(flat.IsSuccess);
        Assert.True(flat.Ok(out (int a, int b, int c) tuple));
        Assert.Equal((1, 2, 3), tuple);
    }

    [Fact]
    public void Flatten_MultiArity_InnerFailure() {
        var innerFail = Result.Failure<int, int>(new ArgumentException("bad"));
        var outer     = Result.Success(innerFail);
        var flat      = outer.Flatten();
        Assert.True(flat.IsFaulted);
        Assert.False(flat.Ok(out (int a, int b) _));
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Async_Task() {
        var inner = Result.Success(10);
        var outer = Result.Success(inner);
        var flat = await System.Threading.Tasks.Task.FromResult(outer)
                               .Flatten();
        Assert.True(flat.IsSuccess);
        Assert.True(flat.Ok(out var v));
        Assert.Equal(10, v);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Async_ValueTask() {
        var inner = Result.Success(7, 8);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int>>>(outer).Flatten();
        Assert.True(flat.IsSuccess);
        Assert.True(flat.Ok(out (int a, int b) tuple));
        Assert.Equal((7, 8), tuple);
    }
}
