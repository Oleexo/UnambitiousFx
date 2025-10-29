using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Validations;
using UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Validations.Ensure.ValueTasks;

[TestSubject(typeof(ResultEnsureExtensions))]
public sealed class ResultEnsureTests {
    [Fact]
    public async Task EnsureAsync_Arity1_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(42);

        var r = await result.EnsureAsync(async v1 => await ValueTask.FromResult(v1 > 0),
                                         async v1 => await ValueTask.FromResult(new Exception($"v was {v1}")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var v));
        Assert.Equal(42, v);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_PredicateFalse_ReturnsFailureWithFactoryException() {
        var result = Result.Success(0);

        var r = await result.EnsureAsync(async v => await ValueTask.FromResult(v > 0),
                                         async v => await ValueTask.FromResult(new Exception($"v was {v}")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _));
        Assert.False(r.TryGet(out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("v was 0", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_FailureInput_ShortCircuits_DoesNotInvokePredicateOrFactory() {
        var initial         = Result.Failure<int>("boom");
        var predicateCalled = false;
        var factoryCalled   = false;

        var r = await initial.EnsureAsync(async _ => {
                                              predicateCalled = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async _ => {
                                              factoryCalled = true;
                                              return await ValueTask.FromResult(new Exception("should not happen"));
                                          });

        Assert.Same(initial, r);
        Assert.False(predicateCalled);
        Assert.False(factoryCalled);
    }

    [Fact]
    public async Task EnsureAsync_Arity1_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(5));

        var r = await awaitable.EnsureAsync(async b => await ValueTask.FromResult(b == 5),
                                            async _ => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var v));
        Assert.Equal(5, v);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(2, 3);

        var r = await result.EnsureAsync(async (v1,
                                                v2) => await ValueTask.FromResult(v1 + v2 == 5),
                                         async (_,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b));
        Assert.Equal(2, a);
        Assert.Equal(3, b);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(2, 3);

        var r = await result.EnsureAsync(async (a,
                                                b) => await ValueTask.FromResult(a + b == 6),
                                         async (a,
                                                b) => await ValueTask.FromResult(new Exception($"{a}+{b} != 6")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _));
        Assert.False(r.TryGet(out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("2+3 != 6", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity2_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity3_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3) => await ValueTask.FromResult(v1 + v2 + v3 == 6),
                                         async (_,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c));
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
    }

    [Fact]
    public async Task EnsureAsync_Arity3_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c) => await ValueTask.FromResult(a + b + c == 7),
                                         async (a,
                                                b,
                                                c) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c} != 7")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3 != 7", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity3_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity4_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3, 4);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3,
                                                v4) => await ValueTask.FromResult(v1 + v2 + v3 + v4 == 10),
                                         async (_,
                                                _,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c, out var d));
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
    }

    [Fact]
    public async Task EnsureAsync_Arity4_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3, 4);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c,
                                                d) => await ValueTask.FromResult(a + b + c + d == 11),
                                         async (a,
                                                b,
                                                c,
                                                d) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c}+{d} != 11")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3+4 != 11", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity4_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity5_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3, 4, 5);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3,
                                                v4,
                                                v5) => await ValueTask.FromResult(v1 + v2 + v3 + v4 + v5 == 15),
                                         async (_,
                                                _,
                                                _,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c, out var d, out var e));
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
    }

    [Fact]
    public async Task EnsureAsync_Arity5_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3, 4, 5);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c,
                                                d,
                                                e) => await ValueTask.FromResult(a + b + c + d + e == 16),
                                         async (a,
                                                b,
                                                c,
                                                d,
                                                e) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c}+{d}+{e} != 16")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3+4+5 != 16", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity5_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity6_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3, 4, 5, 6);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3,
                                                v4,
                                                v5,
                                                v6) => await ValueTask.FromResult(v1 + v2 + v3 + v4 + v5 + v6 == 21),
                                         async (_,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c, out var d, out var e, out var f));
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
        Assert.Equal(6, f);
    }

    [Fact]
    public async Task EnsureAsync_Arity6_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3, 4, 5, 6);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f) => await ValueTask.FromResult(a + b + c + d + e + f == 22),
                                         async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c}+{d}+{e}+{f} != 22")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3+4+5+6 != 22", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity6_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int, int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity7_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3,
                                                v4,
                                                v5,
                                                v6,
                                                v7) => await ValueTask.FromResult(v1 + v2 + v3 + v4 + v5 + v6 + v7 == 28),
                                         async (_,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c, out var d, out var e, out var f, out var g));
        Assert.Equal(1, a);
        Assert.Equal(2, b);
        Assert.Equal(3, c);
        Assert.Equal(4, d);
        Assert.Equal(5, e);
        Assert.Equal(6, f);
        Assert.Equal(7, g);
    }

    [Fact]
    public async Task EnsureAsync_Arity7_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f,
                                                g) => await ValueTask.FromResult(a + b + c + d + e + f + g == 29),
                                         async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f,
                                                g) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c}+{d}+{e}+{f}+{g} != 29")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3+4+5+6+7 != 29", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity7_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int, int, int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }

    [Fact]
    public async Task EnsureAsync_Arity8_PredicateTrue_ReturnsSuccess() {
        var result = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var r = await result.EnsureAsync(async (v1,
                                                v2,
                                                v3,
                                                v4,
                                                v5,
                                                v6,
                                                v7,
                                                v8) => await ValueTask.FromResult(v1 + v2 + v3 + v4 + v5 + v6 + v7 + v8 == 36),
                                         async (_,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _,
                                                _) => await ValueTask.FromResult(new Exception("sum mismatch")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h));
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
    public async Task EnsureAsync_Arity8_PredicateFalse_ReturnsFailure() {
        var result = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var r = await result.EnsureAsync(async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f,
                                                g,
                                                h) => await ValueTask.FromResult(a + b + c + d + e + f + g + h == 37),
                                         async (a,
                                                b,
                                                c,
                                                d,
                                                e,
                                                f,
                                                g,
                                                h) => await ValueTask.FromResult(new Exception($"{a}+{b}+{c}+{d}+{e}+{f}+{g}+{h} != 37")));

        Assert.True(r.IsFaulted);
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _, out _, out _));
        Assert.False(r.TryGet(out _, out _, out _, out _, out _, out _, out _, out _, out var error));
        Assert.NotNull(error);
        Assert.Equal("1+2+3+4+5+6+7+8 != 37", error.Message);
    }

    [Fact]
    public async Task EnsureAsync_Arity8_FailureInput_ShortCircuits() {
        var initial = Result.Failure<int, int, int, int, int, int, int, int>("boom");
        var called  = false;

        var r = await initial.EnsureAsync(async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => {
                                              called = true;
                                              return await ValueTask.FromResult(true);
                                          },
                                          async (_,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _,
                                                 _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.Same(initial, r);
        Assert.False(called);
    }
    [Fact]
    public async Task EnsureAsync_Arity2_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(2, 3));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b) => await ValueTask.FromResult(a + b == 5),
                                            async (_,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1));
        Assert.Equal(2, a1);
        Assert.Equal(3, b1);
    }

    [Fact]
    public async Task EnsureAsync_Arity3_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c) => await ValueTask.FromResult(a + b + c == 6),
                                            async (_,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
    }

    [Fact]
    public async Task EnsureAsync_Arity4_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c,
                                                   d) => await ValueTask.FromResult(a + b + c + d == 10),
                                            async (_,
                                                   _,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1, out var d1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
        Assert.Equal(4, d1);
    }

    [Fact]
    public async Task EnsureAsync_Arity5_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e) => await ValueTask.FromResult(a + b + c + d + e == 15),
                                            async (_,
                                                   _,
                                                   _,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1, out var d1, out var e1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
        Assert.Equal(4, d1);
        Assert.Equal(5, e1);
    }

    [Fact]
    public async Task EnsureAsync_Arity6_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e,
                                                   f) => await ValueTask.FromResult(a + b + c + d + e + f == 21),
                                            async (_,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1, out var d1, out var e1, out var f1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
        Assert.Equal(4, d1);
        Assert.Equal(5, e1);
        Assert.Equal(6, f1);
    }

    [Fact]
    public async Task EnsureAsync_Arity7_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e,
                                                   f,
                                                   g) => await ValueTask.FromResult(a + b + c + d + e + f + g == 28),
                                            async (_,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1, out var d1, out var e1, out var f1, out var g1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
        Assert.Equal(4, d1);
        Assert.Equal(5, e1);
        Assert.Equal(6, f1);
        Assert.Equal(7, g1);
    }

    [Fact]
    public async Task EnsureAsync_Arity8_FromTask_PredicateTrue_ReturnsSuccess() {
        var awaitable = ValueTask.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));

        var r = await awaitable.EnsureAsync(async (a,
                                                   b,
                                                   c,
                                                   d,
                                                   e,
                                                   f,
                                                   g,
                                                   h) => await ValueTask.FromResult(a + b + c + d + e + f + g + h == 36),
                                            async (_,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _,
                                                   _) => await ValueTask.FromResult(new Exception("nope")));

        Assert.True(r.IsSuccess);
        Assert.True(r.TryGet(out var a1, out var b1, out var c1, out var d1, out var e1, out var f1, out var g1, out var h1));
        Assert.Equal(1, a1);
        Assert.Equal(2, b1);
        Assert.Equal(3, c1);
        Assert.Equal(4, d1);
        Assert.Equal(5, e1);
        Assert.Equal(6, f1);
        Assert.Equal(7, g1);
        Assert.Equal(8, h1);
    }
}
