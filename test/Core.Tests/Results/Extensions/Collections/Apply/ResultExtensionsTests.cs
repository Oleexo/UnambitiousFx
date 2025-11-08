using UnambitiousFx.Core.Results.Reasons;
using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.Collections;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Collections.Apply;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultExtensionsTests
{
    [Fact]
    public void Apply_FunctionAndArgSuccess_Applies()
    {
        var rf = Result.Success<Func<int, int>>(x => x * 2);
        var ra = Result.Success(21);

        var r = rf.Apply(ra);

        Assert.True(r.TryGet(out var value, out _));
        Assert.Equal(42, value);
    }

    [Fact]
    public void Apply_FunctionFailure_PropagatesError()
    {
        var ex = new Exception("ferr");
        var rf = Result.Failure<Func<int, int>>(ex);
        var ra = Result.Success(1);

        var r = rf.Apply(ra);

        Assert.False(r.TryGet(out _, out var err));
        { var firstError = err.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }

    [Fact]
    public void Apply_ArgFailure_PropagatesError()
    {
        var ex = new Exception("aerr");
        var rf = Result.Success<Func<int, int>>(x => x + 1);
        var ra = Result.Failure<int>(ex);

        var r = rf.Apply(ra);

        Assert.False(r.TryGet(out _, out var err));
        { var firstError = err.OfType<ExceptionalError>().FirstOrDefault(); Assert.NotNull(firstError); Assert.Same(ex, firstError.Exception); }
    }
}
