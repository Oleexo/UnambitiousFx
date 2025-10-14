// filepath: /Users/maxime.charlesn2f.com/dev/oleexo/UnambitiousFx/test/Core.Tests/Results/Core/ResultBindTryAsyncTests.cs
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Core;

public class ResultBindTryAsyncTests {
    [Fact]
    public async Task BindTryAsync_SingleToSingle_Success() {
        var r = Result.Success(21);
        var mapped = await r.BindTryAsync(async v => {
            await Task.Delay(1);
            return Result.Success(v * 2);
        });
        Assert.True(mapped.Ok(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public async Task BindTryAsync_SingleToSingle_ExceptionCaptured() {
        var r = Result.Success(5);
        var mapped = await r.BindTryAsync<int, int>(async _ => {
            await Task.Delay(1);
            throw new InvalidOperationException("boom");
        });
        Assert.False(mapped.Ok(out _, out _));
        Assert.Contains(mapped.Reasons, rr => rr is UnambitiousFx.Core.Results.Reasons.ExceptionalError);
    }

    [Fact]
    public async Task BindTryAsync_SingleToTuple2_Success() {
        var r = Result.Success(10);
        var mapped = await r.BindTryAsync<int, int, int>(async v => {
            await Task.Delay(1);
            return Result.Success(v, v + 1);
        });
        Assert.True(mapped.Ok(out (int a, int b) values, out _));
        Assert.Equal(10, values.a);
        Assert.Equal(11, values.b);
    }

    [Fact]
    public async Task BindTryAsync_SingleToTuple3_ExceptionCaptured() {
        var r = Result.Success(1);
        var mapped = await r.BindTryAsync<int, int, int, int>(async _ => {
            await Task.Delay(1);
            throw new Exception("fail3");
        });
        Assert.False(mapped.Ok(out _, out _));
    }

    [Fact]
    public async Task BindTryAsync_ValueTaskSource_Propagates() {
        var src = new ValueTask<Result<int>>(Result.Success(7));
        var mapped = await src.BindTryAsync<int, int>(async v => {
            await Task.Yield();
            return Result.Success(v + 5);
        });
        Assert.True(mapped.Ok(out var value, out _));
        Assert.Equal(12, value);
    }
}

