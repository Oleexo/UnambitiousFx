using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;
using UnambitiousFx.Core.Results.Tasks;
using UnambitiousFx.Core.Results.ValueTasks;
using ResultExtensions = UnambitiousFx.Core.Results.Tasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Advanced;

public sealed class AdvancedResultExtensionsTests {
    // ---------------- MapErrors (arity 0) ----------------
    [Fact]
    public void MapErrors_Arity0_Failure_TransformsAndPreservesMetadataAndReasons() {
        var original = Result.Failure(new InvalidOperationException("boom"))
                             .WithError(new ConflictError("conflict"))
                             .WithMetadata("trace", "abc")
                             .WithMetadata("user",  42);
        var originalReasonCount = original.Reasons.Count;

        var mapped = original.MapErrors(errors => new AggregateException("wrapped:" + errors.Count, errors));

        Assert.False(mapped.Ok(out _));
        Assert.Equal(originalReasonCount, mapped.Reasons.Count); // reasons preserved
        Assert.True(mapped.Metadata.ContainsKey("trace"));
        Assert.True(mapped.Metadata.ContainsKey("user"));
        if (!mapped.Ok(out var err)) {
            var agg = Assert.IsType<AggregateException>(err);
            Assert.StartsWith("wrapped:", agg.Message);
            Assert.Equal(2, agg.InnerExceptions.Count); // primary + conflict error (without exception gives synthetic)
        }
    }

    [Fact]
    public void MapErrors_Arity0_Success_NoChange() {
        var r = Result.Success()
                      .WithMetadata("a", 1);
        var mapped = r.MapErrors(list => new Exception("should not run"));
        Assert.True(mapped.Ok(out _));
        Assert.True(ReferenceEquals(r, mapped));
    }

    // ---------------- MapErrors (arity 2) ----------------
    [Fact]
    public void MapErrors_Arity2_Failure_Transforms() {
        var original = Result.Failure<int, int>(new Exception("bad"))
                             .WithError(new ValidationError(new List<string> { "X missing" }));

        var mapped = original.MapErrors(errs => new Exception("combined:" + errs.Count));
        Assert.False(mapped.Ok(out (int, int) _, out var newErr));
        Assert.Equal("combined:" + 2, newErr.Message); // primary + validation error
    }

    // ---------------- TapBoth / TapEither ----------------
    [Fact]
    public void TapBoth_Arity1_Success_InvokesSuccessOnly() {
        var r            = Result.Success(5);
        var successCalls = 0;
        var failCalls    = 0;
        var tapped       = r.TapBoth(v => successCalls = v, _ => failCalls++);
        Assert.True(tapped.Ok(out var value, out _));
        Assert.Equal(5, value);
        Assert.Equal(5, successCalls);
        Assert.Equal(0, failCalls);
    }

    [Fact]
    public void TapBoth_Arity1_Failure_InvokesFailureOnly() {
        var        ex           = new Exception("err");
        var        r            = Result.Failure<int>(ex);
        var        successCalls = 0;
        Exception? captured     = null;
        var        tapped       = r.TapBoth(_ => successCalls++, e => captured = e);
        Assert.False(tapped.Ok(out var _, out var err));
        Assert.Equal(ex, err);
        Assert.Equal(0,  successCalls);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapBoth_Arity2_Failure_InvokesFailureOnly() {
        var        ex           = new Exception("f");
        var        r            = Result.Failure<int, int>(ex);
        var        successCalls = 0;
        Exception? captured     = null;
        var tapped = r.TapBoth((a,
                                b) => successCalls++, e => captured = e);
        Assert.False(tapped.Ok(out (int, int) _, out var err));
        Assert.Equal(ex, err);
        Assert.Equal(0,  successCalls);
        Assert.Equal(ex, captured);
    }

    [Fact]
    public void TapEither_Alias_Works_Arity2() {
        var r         = Result.Success(1, 2);
        var sum       = 0;
        var failCalls = 0;
        var tapped = r.TapEither((a,
                                  b) => sum = a + b, _ => failCalls++);
        Assert.True(tapped.Ok(out (int, int) t, out _));
        Assert.Equal(3, sum);
        Assert.Equal(0, failCalls);
    }

    // ---------------- TapErrorAsync / MapErrorAsync (non-generic) ----------------
    [Fact]
    public async System.Threading.Tasks.Task TapErrorAsync_NonGeneric_Failure_Taps() {
        var ex     = new Exception("boom");
        var r      = Result.Failure(ex);
        var tapped = 0;
        var after = await r.TapErrorAsync(e => {
            tapped++;
            return System.Threading.Tasks.Task.CompletedTask;
        });
        Assert.False(after.Ok(out _));
        Assert.Equal(1, tapped);
    }

    [Fact]
    public async System.Threading.Tasks.Task TapErrorAsync_NonGeneric_Success_NoTap() {
        var r      = Result.Success();
        var tapped = 0;
        var after = await r.TapErrorAsync(e => {
            tapped++;
            return System.Threading.Tasks.Task.CompletedTask;
        });
        Assert.True(after.Ok(out _));
        Assert.Equal(0, tapped);
    }

    [Fact]
    public async System.Threading.Tasks.Task MapErrorAsync_NonGeneric_Failure_Maps() {
        var r = Result.Failure(new InvalidOperationException("orig"));
        var mapped = await ResultExtensions.MapErrorAsync(r, async e => {
            await System.Threading.Tasks.Task.CompletedTask;
            return new Exception("mapped", e);
        });
        Assert.False(mapped.Ok(out _));
        if (!mapped.Ok(out var err)) {
            Assert.Equal("mapped", err.Message);
            Assert.IsType<InvalidOperationException>(err.InnerException);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task MapErrorAsync_NonGeneric_Success_NoChange() {
        var r      = Result.Success();
        var mapped = await r.MapErrorAsync(e => System.Threading.Tasks.Task.FromResult(new Exception("mapped")));
        Assert.True(mapped.Ok(out _));
    }

    [Fact]
    public async System.Threading.Tasks.Task MapErrorAsync_NonGeneric_ValueTaskVariant() {
        var r      = Result.Failure(new Exception("x"));
        var mapped = await r.MapErrorAsync(e => new ValueTask<Exception>(new InvalidOperationException("y", e)));
        Assert.False(mapped.Ok(out _));
        if (!mapped.Ok(out var err)) {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal("y", err.Message);
        }
    }

    [Fact]
    public async System.Threading.Tasks.Task TapErrorAsync_NonGeneric_ValueTaskVariant() {
        var r    = Result.Failure(new Exception("x"));
        var taps = 0;
        var after = await r.TapErrorAsync(e => {
            taps++;
            return new ValueTask();
        });
        Assert.False(after.Ok(out _));
        Assert.Equal(1, taps);
    }

    // ---------------- Generic async (ensure existing generator still works) ----------------
    [Fact]
    public async System.Threading.Tasks.Task MapErrorAsync_Generic_Arity1_Transforms() {
        var r = Result.Failure<int>(new Exception("e1"));
        var mapped = await ResultExtensions.MapErrorAsync(r, async e => {
            await System.Threading.Tasks.Task.CompletedTask;
            return new Exception("e2", e);
        });
        Assert.False(mapped.Ok(out var _, out var err));
        Assert.Equal("e2", err.Message);
        Assert.NotNull(err.InnerException);
    }

    [Fact]
    public async System.Threading.Tasks.Task TapErrorAsync_Generic_Arity2_Failure() {
        var r    = Result.Failure<int, int>(new Exception("e1"));
        var taps = 0;
        var mapped = await r.TapErrorAsync(e => {
            taps++;
            return System.Threading.Tasks.Task.CompletedTask;
        });
        Assert.False(mapped.Ok(out (int, int) _, out _));
        Assert.Equal(1, taps);
    }

    [Fact]
    public async System.Threading.Tasks.Task MapErrorAsync_Generic_Arity2_Transforms() {
        var r = Result.Failure<int, int>(new Exception("e1"));
        var mapped = await ResultExtensions.MapErrorAsync(r, async e => {
            await System.Threading.Tasks.Task.CompletedTask;
            return new Exception("e2", e);
        });
        Assert.False(mapped.Ok(out (int, int) _, out var err));
        Assert.Equal("e2", err.Message);
    }

    // ---------------- MapErrors high arity smoke ----------------
    [Fact]
    public void MapErrors_Arity4_Failure() {
        var r = Result.Failure<int, int, int, int>(new Exception("root"))
                      .WithError(new ConflictError("c"));
        var mapped = r.MapErrors(list => new Exception("agg:" + list.Count));
        Assert.False(mapped.Ok(out (int, int, int, int) _, out var err));
        Assert.Equal("agg:2", err.Message); // root + conflict
    }
}
