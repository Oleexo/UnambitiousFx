using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Introspection;

public class ResultIntrospectionTests {
    [Fact]
    public void NonGeneric_Deconstruct_Success() {
        var r = Result.Success();
        r.Deconstruct(out var ok, out var err);
        Assert.True(ok);
        Assert.Null(err);
        Assert.Equal("Success", r.ToString()
                                 .Split(' ')[0]);
    }

    [Fact]
    public void NonGeneric_Deconstruct_Failure() {
        var ex = new CustomException();
        var r  = Result.Failure(ex);
        r.Deconstruct(out var ok, out var err);
        Assert.False(ok);
        Assert.Same(ex, err);
        Assert.Contains("Failure", r.ToString());
    }

    [Fact]
    public void SingleArity_HasException_And_HasError_Fallback() {
        var ex = new CustomException();
        var r  = Result.Failure<int>(ex);
        Assert.True(r.HasException<CustomException, int>());
        Assert.True(r.HasError<CustomException, int>()); // fallback to exception type
        Assert.False(r.HasException<ArgumentException, int>());
    }

    [Fact]
    public void SingleArity_ToNullable_SuccessAndFailure() {
        var ok           = Result.Success(42);
        var fail         = Result.Failure<int>(new Exception());
        var successValue = ok.ToNullable();
        var failedValue  = fail.ToNullable();
        Assert.Equal(42, successValue);
        // Failure returns default(T) for single arity; ensure it's default and different from success value.
        Assert.Equal(default, failedValue);
        Assert.NotEqual(successValue, failedValue);
    }

    [Fact]
    public void MultiArity2_Deconstruct_Success() {
        var r = Result.Success(1, 2);
        r.Deconstruct(out var ok, out (int a, int b)? val, out var err);
        Assert.True(ok);
        Assert.Equal((1, 2), val);
        Assert.Null(err);
        Assert.Contains("Success<", r.ToString());
    }

    [Fact]
    public void MultiArity2_Deconstruct_Failure() {
        var ex = new Exception("fail2");
        var r  = Result.Failure<int, int>(ex);
        r.Deconstruct(out var ok, out (int a, int b)? val, out var err);
        Assert.False(ok);
        Assert.False(val.HasValue);
        Assert.Same(ex, err);
    }

    [Fact]
    public void MultiArity3_ToNullable_Failure() {
        var r = Result.Failure<int, int, int>(new Exception());
        r.Deconstruct(out var ok, out (int a, int b, int c)? val, out var err);
        Assert.False(ok);
        Assert.Null(val);
        Assert.NotNull(err);
    }

    [Fact]
    public void MultiArity3_ToNullable_Success() {
        var r        = Result.Success(3, 4, 5);
        var nullable = r.ToNullable();
        Assert.Equal((3, 4, 5), nullable);
    }

    [Fact]
    public void ToString_Includes_Reasons_Count() {
        var r1 = Result.Success(10);
        var r2 = Result.Failure<int>(new Exception("boom"));
        Assert.Contains("reasons=", r1.ToString());
        Assert.Contains("reasons=", r2.ToString());
    }

    private sealed class CustomException : Exception {
        public CustomException(string m = "custom")
            : base(m) {
        }
    }
}
