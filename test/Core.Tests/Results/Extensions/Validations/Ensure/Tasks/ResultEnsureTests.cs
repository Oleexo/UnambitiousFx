using System.Threading.Tasks;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations.Tasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations.Ensure.Tasks;

[TestSubject(typeof(Result<>))]
public sealed class ResultEnsureTests {
    [Fact]
    public async Task EnsureAsync_Arity1_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(42);

        var r = await result.EnsureAsync(async v => await Task.FromResult(v > 0),
                                         async v => await Task.FromResult(new Exception($"v was {v}")));

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out var v));
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_PredicateFalse_ReturnsFailureWithFactoryException() {
        var result = Result.Success(0);

        var r = await result.EnsureAsync(async v => await Task.FromResult(v > 0),
                                         async v => await Task.FromResult(new Exception($"v was {v}")));

        Assert.True(r.IsFaulted);
        Assert.False(r.Ok(out var _));
        Assert.False(r.Ok(out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("v was 0", error!.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_FailureInput_ShortCircuits_DoesNotInvokePredicateOrFactory() {
        var initial = Result.Failure<int>("boom");
        var predicateCalled = false;
        var factoryCalled   = false;

        var r = await initial.EnsureAsync(async v => {
                                              predicateCalled = true;
                                              return await Task.FromResult(true);
                                          },
                                          async v => {
                                              factoryCalled = true;
                                              return await Task.FromResult(new Exception("should not happen"));
                                          });

        Assert.Same(initial, r);
        Assert.False(predicateCalled);
        Assert.False(factoryCalled);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_FromTask_PredicateTrue_ReturnsSuccess() {
        Task<Result<int>> awaitable = Task.FromResult(Result.Success(5));

        var r = await awaitable.EnsureAsync(async v => await Task.FromResult(v == 5),
                                            async v => await Task.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out var v));
        Assert.Equal(5, v);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(2, 3);

        var r = await result.EnsureAsync(async (a, b) => await Task.FromResult(a + b == 5),
                                         async (a, b) => await Task.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.Ok(out var a, out var b));
        Assert.Equal(2, a);
        Assert.Equal(3, b);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(2, 3);

        var r = await result.EnsureAsync(async (a, b) => await Task.FromResult(a + b == 6),
                                         async (a, b) => await Task.FromResult(new Exception($"{a}+{b} != 6")));

        Assert.True(r.IsFaulted);
        Assert.False(r.Ok(out var _, out var _));
        Assert.False(r.Ok(out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("2+3 != 6", error!.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (a, b) => {
                                              called = true;
                                              return await Task.FromResult(true);
                                          },
                                          async (a, b) => await Task.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }
}
