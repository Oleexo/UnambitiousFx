using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using TasksExt = UnambitiousFx.Core.Results.Tasks.ResultExtensions;
using ValueTasksExt = UnambitiousFx.Core.Results.ValueTasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultTraverseSequenceAsyncTests
{
    // Tasks - TraverseAsync
    [Fact]
    public async Task Tasks_TraverseAsync_AllSuccess_ReturnsList()
    {
        var items = new[] { 1, 2, 3 };

        var result = await TasksExt.TraverseAsync(items, i => Task.FromResult(Result.Success(i * 10)));

        Assert.True(result.Ok(out var list, out _));
        Assert.Equal(new[] { 10, 20, 30 }, list);
    }

    [Fact]
    public async Task Tasks_TraverseAsync_FirstFailure_Propagates()
    {
        var items = new[] { 0, 1, 2 };
        var ex = new Exception("boom");

        var result = await TasksExt.TraverseAsync(items, i => i == 0 ? Task.FromResult(Result.Failure<int>(ex)) : Task.FromResult(Result.Success(i)));

        Assert.False(result.Ok(out List<int>? _, out Exception? err));
        Assert.Same(ex, err!);
    }

    // Tasks - SequenceAsync
    [Fact]
    public async Task Tasks_SequenceAsync_AllSuccess_ReturnsList()
    {
        var tasks = new[]
        {
            Task.FromResult(Result.Success(1)),
            Task.FromResult(Result.Success(2)),
            Task.FromResult(Result.Success(3))
        };

        var result = await TasksExt.SequenceAsync(tasks);

        Assert.True(result.Ok(out var list, out _));
        Assert.Equal(new[] { 1, 2, 3 }, list);
    }

    [Fact]
    public async Task Tasks_SequenceAsync_FirstFailure_Propagates()
    {
        var ex = new Exception("seq");
        var tasks = new[]
        {
            Task.FromResult(Result.Failure<int>(ex)),
            Task.FromResult(Result.Success(2))
        };

        var result = await TasksExt.SequenceAsync(tasks);

        Assert.False(result.Ok(out List<int>? _, out Exception? err));
        Assert.Same(ex, err!);
    }

    // ValueTasks - TraverseAsync
    [Fact]
    public async Task ValueTasks_TraverseAsync_AllSuccess_ReturnsList()
    {
        var items = new[] { 1, 2, 3 };

        var result = await ValueTasksExt.TraverseAsync(items, i => new ValueTask<Result<int>>(Result.Success(i + 1)));

        Assert.True(result.Ok(out var list, out _));
        Assert.Equal(new[] { 2, 3, 4 }, list);
    }

    [Fact]
    public async Task ValueTasks_TraverseAsync_FirstFailure_Propagates()
    {
        var items = new[] { 5, -1, 6 };
        var ex = new Exception("vt");

        var result = await ValueTasksExt.TraverseAsync(items, i => i < 0
            ? new ValueTask<Result<int>>(Result.Failure<int>(ex))
            : new ValueTask<Result<int>>(Result.Success(i)));

        Assert.False(result.Ok(out List<int>? _, out Exception? err));
        Assert.Same(ex, err!);
    }

    // ValueTasks - SequenceAsync
    [Fact]
    public async Task ValueTasks_SequenceAsync_AllSuccess_ReturnsList()
    {
        var vts = new ValueTask<Result<int>>[]
        {
            new(Result.Success(3)),
            new(Result.Success(4))
        };

        var result = await ValueTasksExt.SequenceAsync(vts);

        Assert.True(result.Ok(out var list, out _));
        Assert.Equal(new[] { 3, 4 }, list);
    }

    [Fact]
    public async Task ValueTasks_SequenceAsync_FirstFailure_Propagates()
    {
        var ex = new Exception("svt");
        var vts = new ValueTask<Result<int>>[]
        {
            new(Result.Failure<int>(ex)),
            new(Result.Success(1))
        };

        var result = await ValueTasksExt.SequenceAsync(vts);

        Assert.False(result.Ok(out List<int>? _, out Exception? err));
        Assert.Same(ex, err!);
    }
}
