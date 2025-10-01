using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Results;

namespace UnambitiousFx.Core.XUnit.Tests.Results;

public sealed class ResultPredicateAssertionExtensionsTests {
    [Fact]
    public void Arity1_ShouldBeSuccessWhere_PredicateTrue() {
        Result.Success(42).ShouldBeSuccessWhere(v => v > 40);
    }

    [Fact]
    public void Arity1_ShouldBeFailureWhere_PredicateTrue() {
        Result.Failure<int>(new InvalidOperationException("boom"))
              .ShouldBeFailureWhere(ex => ex is InvalidOperationException);
    }

    [Fact]
    public void Arity2_ShouldBeSuccessWhere_PredicateTrue() {
        Result.Success(1, 2)
              .ShouldBeSuccessWhere(tuple => tuple.Item1 + tuple.Item2 == 3);
    }

    [Fact]
    public void Arity3_ShouldBeSuccessWhere_PredicateTrue() {
        Result.Success(1, 2, 3)
              .ShouldBeSuccessWhere(t => t.Item1 + t.Item2 + t.Item3 == 6, "Sum should match");
    }

    [Fact]
    public void Arity8_ShouldBeSuccessWhere_PredicateTrue() {
        Result.Success(1,2,3,4,5,6,7,8)
              .ShouldBeSuccessWhere(t => t.Item8 == 8);
    }

    [Fact]
    public void CustomMessageOverload_Success() {
        Result.Success(10).ShouldBeSuccess(out var v, "Should succeed");
        Assert.Equal(10, v);
    }

    [Fact]
    public void CustomMessageOverload_Failure() {
        Result.Failure<int>(new Exception("x"))
              .ShouldBeFailure(out var ex, "Should fail");
        Assert.Equal("x", ex.Message);
    }
}

