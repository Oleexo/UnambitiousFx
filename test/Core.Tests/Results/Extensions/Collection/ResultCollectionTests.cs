using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Collection;

[TestSubject(typeof(ResultExtensions))]
public sealed class ResultCollectionTests {
    [Fact]
    public void Sequence_AllSuccess_ReturnsList() {
        var results = new[] { Result.Success(1), Result.Success(2), Result.Success(3) };

        var sequenced = results.Sequence();

        Assert.True(sequenced.Ok(out var list, out _));
        Assert.Equal(new[] { 1, 2, 3 }, list);
    }

    [Fact]
    public void Sequence_FirstFailure_PropagatesError() {
        var ex      = new Exception("bad");
        var results = new[] { Result.Failure<int>(ex), Result.Success(2), Result.Success(3) };

        var sequenced = results.Sequence();

        Assert.False(sequenced.Ok(out var _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Traverse_AllSuccess_ReturnsList() {
        var items = new[] { "a", "b", "c" };

        var traversed = items.Traverse(s => Result.Success(s.ToUpperInvariant()));

        Assert.True(traversed.Ok(out var list, out _));
        Assert.Equal(new[] { "A", "B", "C" }, list);
    }

    [Fact]
    public void Traverse_WithFailure_PropagatesError() {
        var items = new[] { "1", "x", "3" };
        var ex    = new FormatException("x");

        var traversed = items.Traverse(s => s == "x"
                                                ? Result.Failure<string>(ex)
                                                : Result.Success(s));

        Assert.False(traversed.Ok(out var _, out var err));
        Assert.Same(ex, err);
    }

    [Fact]
    public void Partition_Mixed_SplitsCorrectly() {
        var ex1 = new Exception("e1");
        var ex2 = new Exception("e2");
        var results = new[] {
            Result.Success(1),
            Result.Failure<int>(ex1),
            Result.Success(3),
            Result.Failure<int>(ex2)
        };

        var (oks, errors) = results.Partition();

        Assert.Equal(new[] { 1, 3 }, oks);
        Assert.Equal(2,              errors.Count);
        Assert.Same(ex1, errors[0]);
        Assert.Same(ex2, errors[1]);
    }

    [Fact]
    public void Combine_AllSuccess_ReturnsSuccess() {
        var results = new[] { Result.Success(), Result.Success(), Result.Success() };

        var combined = results.Combine();

        Assert.True(combined.Ok(out _));
    }

    [Fact]
    public void Combine_WithFailures_ReturnsAggregateWithAllErrors() {
        var e1      = new Exception("a");
        var e2      = new Exception("b");
        var results = new[] { Result.Success(), Result.Failure(e1), Result.Failure(e2) };

        var combined = results.Combine();

        Assert.False(combined.Ok(out var err));
        var agg = Assert.IsType<AggregateException>(err);
        Assert.Contains(e1, agg.InnerExceptions);
        Assert.Contains(e2, agg.InnerExceptions);
    }
}
