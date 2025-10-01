using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SelectMany;

public sealed class ResultSelectManyTests {
    [Fact]
    public void SelectMany_SimpleSuccess_ChainsAndProjects() {
        var r = Result.Success(5)
                      .SelectMany(x => Result.Success(x * 2), (x,
                                                               y) => x + y); // 5 + 10 = 15

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out int value));
        Assert.Equal(15, value);
    }

    [Fact]
    public void SelectMany_FirstFailure_ShortCircuits() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int>(error)
                      .SelectMany(x => Result.Success(x * 2), (x,
                                                               y) => x + y);

        Assert.True(r.IsFaulted);
        Assert.False(r.Ok(out int _));
    }

    [Fact]
    public void SelectMany_SecondFailure_Propagates() {
        var innerError = new Exception("inner");
        var r = Result.Success(3)
                      .SelectMany(_ => Result.Failure<int>(innerError), (x,
                                                                         y) => x + y);

        Assert.True(r.IsFaulted);
        Assert.False(r.Ok(out int _));
    }

    [Fact]
    public void SelectMany_MultiArity_SourceTuple() {
        var r = Result.Success(2, 3)
                      .SelectMany((a,
                                   b) => Result.Success(a + b), (a,
                                                                 b,
                                                                 sum) => $"{a}+{b}={sum}");

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out string? value));
        Assert.Equal("2+3=5", value);
    }

    [Fact]
    public async System.Threading.Tasks.Task SelectMany_Async_Chains() {
        var r = await Result.Success(4)
                            .SelectMany(async x => {
                                 await System.Threading.Tasks.Task.Delay(10);
                                 return Result.Success(x * 3); // 12
                             }, (x,
                                 y) => x * y); // 4 * 12 = 48

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out int value));
        Assert.Equal(48, value);
    }

    [Fact]
    public void SelectMany_QueryExpression_Works() {
        var result =
            from a in Result.Success(10)
            from b in Result.Success(5)
            select a + b; // 15

        Assert.True(result.IsSuccess);
        Assert.True(result.Ok(out int sum));
        Assert.Equal(15, sum);
    }

    [Fact]
    public async System.Threading.Tasks.Task Select_Async_TaskSource() {
        Task<Result<int>> source    = System.Threading.Tasks.Task.FromResult(Result.Success(21));
        var               projected = await source.Select(x => x * 2); // 42
        Assert.True(projected.IsSuccess);
        Assert.True(projected.Ok(out int value));
        Assert.Equal(42, value);
    }

    [Fact]
    public async System.Threading.Tasks.Task Select_ValueTask_Source() {
        ValueTask<Result<int>> source = new(Result.Success(7));

        var projected = await UnambitiousFx.Core.Results.ValueTasks.ResultExtensions.Select(source, x => x + 5); // 12
        Assert.True(projected.IsSuccess);
        Assert.True(projected.Ok(out int value));
        Assert.Equal(12, value);
    }
}
