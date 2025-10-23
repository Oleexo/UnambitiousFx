using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Transformations.Map.Tasks;

[TestSubject(typeof(ResultMapExtensions))]
public class ResultMapExtensionsTests {
    #region Arity 1

    [Fact]
    public async Task MapAsync_Arity1_Result_Success_ReturnsMapped() {
        var r = Result.Success(1);

        var mapped = await r.MapAsync(x => Task.FromResult(x + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal(2, v);
    }

    [Fact]
    public async Task MapAsync_Arity1_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int>(ex);

        var mapped = await r.MapAsync(x => Task.FromResult(x + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity1_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int>(ex);

        _ = await r.MapAsync(x => {
            called = true;
            return Task.FromResult(x + 1);
        });

        Assert.False(called);
    }

    // Task<Result<T>> with async mapper
    [Fact]
    public async Task MapAsync_Arity1_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1));

        var mapped = await rTask.MapAsync(x => Task.FromResult(x + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal(2, v);
    }

    [Fact]
    public async Task MapAsync_Arity1_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int>(ex));

        var mapped = await rTask.MapAsync(x => Task.FromResult(x + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity1_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int>(ex));

        _ = await rTask.MapAsync(x => {
            called = true;
            return Task.FromResult(x + 1);
        });

        Assert.False(called);
    }

    // Task<Result<T>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity1_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1));

        var mapped = await rTask.MapAsync(x => x + 1);

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal(2, v);
    }

    [Fact]
    public async Task MapAsync_Arity1_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int>(ex));

        var mapped = await rTask.MapAsync(x => x + 1);

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity1_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int>(ex));

        _ = await rTask.MapAsync(x => {
            called = true;
            return x + 1;
        });

        Assert.False(called);
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task MapAsync_Arity2_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2);

        var mapped = await r.MapAsync((a,
                                       b) => Task.FromResult((a + 10, b * 2)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((11, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity2_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int>(ex);

        var mapped = await r.MapAsync((a,
                                       b) => Task.FromResult((a + 10, b * 2)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity2_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int>(ex);

        _ = await r.MapAsync((a,
                              b) => {
            called = true;
            return Task.FromResult((a + 10, b * 2));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2>> with async mapper
    [Fact]
    public async Task MapAsync_Arity2_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2));

        var mapped = await rTask.MapAsync((a,
                                           b) => Task.FromResult((a + 10, b * 2)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((11, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity2_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b) => Task.FromResult((a + 10, b * 2)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity2_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b) => {
            called = true;
            return Task.FromResult((a + 10, b * 2));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity2_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2));

        var mapped = await rTask.MapAsync((a,
                                           b) => (a + 10, b * 2));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((11, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity2_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b) => (a + 10, b * 2));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity2_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b) => {
            called = true;
            return (a + 10, b * 2);
        });

        Assert.False(called);
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task MapAsync_Arity3_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c) => Task.FromResult((a + 1, b + 1, c + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity3_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int>(ex);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c) => Task.FromResult((a + 1, b + 1, c + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity3_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int>(ex);

        _ = await r.MapAsync((a,
                              b,
                              c) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2,T3>> with async mapper
    [Fact]
    public async Task MapAsync_Arity3_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c) => Task.FromResult((a + 1, b + 1, c + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity3_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c) => Task.FromResult((a + 1, b + 1, c + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity3_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2,T3>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity3_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c) => (a + 1, b + 1, c + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4), v);
    }

    [Fact]
    public async Task MapAsync_Arity3_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c) => (a + 1, b + 1, c + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity3_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c) => {
            called = true;
            return (a + 1, b + 1, c + 1);
        });

        Assert.False(called);
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task MapAsync_Arity4_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c,
                                       d) => Task.FromResult((a + 1, b + 1, c + 1, d + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5), v);
    }

    [Fact]
    public async Task MapAsync_Arity4_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int>(ex);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c,
                                       d) => Task.FromResult((a + 1, b + 1, c + 1, d + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity4_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int, int>(ex);

        _ = await r.MapAsync((a,
                              b,
                              c,
                              d) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2,T3,T4>> with async mapper
    [Fact]
    public async Task MapAsync_Arity4_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d) => Task.FromResult((a + 1, b + 1, c + 1, d + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5), v);
    }

    [Fact]
    public async Task MapAsync_Arity4_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d) => Task.FromResult((a + 1, b + 1, c + 1, d + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity4_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c,
                                  d) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1,T2,T3,T4>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity4_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d) => (a + 1, b + 1, c + 1, d + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5), v);
    }

    [Fact]
    public async Task MapAsync_Arity4_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d) => (a + 1, b + 1, c + 1, d + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity4_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c,
                                  d) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1);
        });

        Assert.False(called);
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task MapAsync_Arity5_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c,
                                       d,
                                       e) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task MapAsync_Arity5_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int>(ex);

        var mapped = await r.MapAsync((a,
                                       b,
                                       c,
                                       d,
                                       e) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity5_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int, int, int>(ex);

        _ = await r.MapAsync((a,
                              b,
                              c,
                              d,
                              e) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T5>> with async mapper
    [Fact]
    public async Task MapAsync_Arity5_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d,
                                           e) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task MapAsync_Arity5_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d,
                                           e) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity5_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c,
                                  d,
                                  e) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T5>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity5_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d,
                                           e) => (a + 1, b + 1, c + 1, d + 1, e + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6), v);
    }

    [Fact]
    public async Task MapAsync_Arity5_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a,
                                           b,
                                           c,
                                           d,
                                           e) => (a + 1, b + 1, c + 1, d + 1, e + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity5_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a,
                                  b,
                                  c,
                                  d,
                                  e) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1);
        });

        Assert.False(called);
    }

    #endregion
    
    #region Arity 6
    [Fact]
    public async Task MapAsync_Arity6_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);

        var mapped = await r.MapAsync((a, b, c, d, e, f) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task MapAsync_Arity6_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int>(ex);

        var mapped = await r.MapAsync((a, b, c, d, e, f) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity6_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int, int, int, int>(ex);

        _ = await r.MapAsync((a, b, c, d, e, f) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T6>> with async mapper
    [Fact]
    public async Task MapAsync_Arity6_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task MapAsync_Arity6_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity6_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T6>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity6_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7), v);
    }

    [Fact]
    public async Task MapAsync_Arity6_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity6_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1);
        });

        Assert.False(called);
    }
    #endregion
    
    #region Arity 7
      [Fact]
    public async Task MapAsync_Arity7_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);

        var mapped = await r.MapAsync((a, b, c, d, e, f, g) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task MapAsync_Arity7_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int>(ex);

        var mapped = await r.MapAsync((a, b, c, d, e, f, g) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity7_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);

        _ = await r.MapAsync((a, b, c, d, e, f, g) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T7>> with async mapper
    [Fact]
    public async Task MapAsync_Arity7_Task_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task MapAsync_Arity7_Task_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity7_Task_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f, g) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));
        });

        Assert.False(called);
    }

    // Task<Result<T1..T7>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity7_TaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8), v);
    }

    [Fact]
    public async Task MapAsync_Arity7_TaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity7_TaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f, g) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1);
        });

        Assert.False(called);
    }
    #endregion
    
    #region Arity 8
     [Fact]
    public async Task MapAsync_Arity8_Result_Success_ReturnsMapped() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);

        var mapped = await r.MapAsync((a, b, c, d, e, f, g, h) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8, 9), v);
    }

    [Fact]
    public async Task MapAsync_Arity8_Result_Failure_PropagatesError() {
        var ex = new Exception("boom");
        var r  = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var mapped = await r.MapAsync((a, b, c, d, e, f, g, h) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity8_Result_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        _ = await r.MapAsync((a, b, c, d, e, f, g, h) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));
        });

        Assert.False(called);
    }

    // ValueTask<Result<T1..T8>> with async mapper
    [Fact]
    public async Task MapAsync_Arity8_ValueTask_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g, h) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1)));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8, 9), v);
    }

    [Fact]
    public async Task MapAsync_Arity8_ValueTask_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g, h) => Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1)));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity8_ValueTask_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f, g, h) => {
            called = true;
            return Task.FromResult((a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));
        });

        Assert.False(called);
    }

    // ValueTask<Result<T1..T8>> with sync mapper
    [Fact]
    public async Task MapAsync_Arity8_ValueTaskSyncMapper_Success_ReturnsMapped() {
        var rTask = Task.FromResult(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g, h) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));

        mapped.ShouldBeSuccess(out var v);
        Assert.Equal((2, 3, 4, 5, 6, 7, 8, 9), v);
    }

    [Fact]
    public async Task MapAsync_Arity8_ValueTaskSyncMapper_Failure_PropagatesError() {
        var ex    = new Exception("boom");
        var rTask = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));

        var mapped = await rTask.MapAsync((a, b, c, d, e, f, g, h) => (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1));

        mapped.ShouldBeFailure(out var err);
        Assert.Same(ex, err);
    }

    [Fact]
    public async Task MapAsync_Arity8_ValueTaskSyncMapper_Failure_DoesNotInvokeMap() {
        var ex     = new Exception("boom");
        var called = false;
        var rTask  = Task.FromResult(Result.Failure<int, int, int, int, int, int, int, int>(ex));

        _ = await rTask.MapAsync((a, b, c, d, e, f, g, h) => {
            called = true;
            return (a + 1, b + 1, c + 1, d + 1, e + 1, f + 1, g + 1, h + 1);
        });

        Assert.False(called);
    }
    #endregion
}
