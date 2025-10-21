using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Flatten;

public sealed class ResultExtensionsTests {
    // Arity 1
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

    // Arity 2
    [Fact]
    public void Flatten_Arity2_Success_ProjectsInner() {
        var inner = Result.Success(1, 2);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b) values);
        Assert.Equal((1, 2), values);
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

    // Async Task
    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity1_Success() {
        var inner = Result.Success(10);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity2_Success() {
        var inner = Result.Success(7, 8);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b) values);
        Assert.Equal((7, 8), values);
    }

    // Async ValueTask
    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity1_Success() {
        var inner = Result.Success(5);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int>>>(outer).Flatten();

        flat.Ok(out var value);
        Assert.Equal(5, value);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity2_Success() {
        var inner = Result.Success(3, 4);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b) values);
        Assert.Equal((3, 4), values);
    }

    // Arity 3
    [Fact]
    public void Flatten_Arity3_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c) values);
        Assert.Equal((1, 2, 3), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity3_Success() {
        var inner = Result.Success(1, 2, 3);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c) values);
        Assert.Equal((1, 2, 3), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity3_Success() {
        var inner = Result.Success(1, 2, 3);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c) values);
        Assert.Equal((1, 2, 3), values);
    }

    // Arity 4
    [Fact]
    public void Flatten_Arity4_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c, int d) values);
        Assert.Equal((1, 2, 3, 4), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity4_Success() {
        var inner = Result.Success(1, 2, 3, 4);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d) values);
        Assert.Equal((1, 2, 3, 4), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity4_Success() {
        var inner = Result.Success(1, 2, 3, 4);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d) values);
        Assert.Equal((1, 2, 3, 4), values);
    }

    // Arity 5
    [Fact]
    public void Flatten_Arity5_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e) values);
        Assert.Equal((1, 2, 3, 4, 5), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity5_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e) values);
        Assert.Equal((1, 2, 3, 4, 5), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity5_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e) values);
        Assert.Equal((1, 2, 3, 4, 5), values);
    }

    // Arity 6
    [Fact]
    public void Flatten_Arity6_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f) values);
        Assert.Equal((1, 2, 3, 4, 5, 6), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity6_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f) values);
        Assert.Equal((1, 2, 3, 4, 5, 6), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity6_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int, int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f) values);
        Assert.Equal((1, 2, 3, 4, 5, 6), values);
    }

    // Arity 7
    [Fact]
    public void Flatten_Arity7_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity7_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity7_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int, int, int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7), values);
    }

    // Arity 8
    [Fact]
    public void Flatten_Arity8_Success_ProjectsInner() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var outer = Result.Success(inner);
        var flat  = outer.Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g, int h) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), values);
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

    [Fact]
    public async System.Threading.Tasks.Task Flatten_Task_Arity8_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var outer = Result.Success(inner);
        var flat  = await System.Threading.Tasks.Task.FromResult(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g, int h) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), values);
    }

    [Fact]
    public async System.Threading.Tasks.Task Flatten_ValueTask_Arity8_Success() {
        var inner = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var outer = Result.Success(inner);
        var flat  = await new ValueTask<Result<Result<int, int, int, int, int, int, int, int>>>(outer).Flatten();

        flat.Ok(out (int a, int b, int c, int d, int e, int f, int g, int h) values);
        Assert.Equal((1, 2, 3, 4, 5, 6, 7, 8), values);
    }
}
