#nullable enable

using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public sealed class ResultArity6Tests
{
    [Fact]
    public void Success_IsSuccess_ReturnsTrue() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        Assert.True(r.IsSuccess);
        Assert.False(r.IsFaulted);
    }
    
    [Fact]
    public void Failure_IsSuccess_ReturnsFalse() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        Assert.False(r.IsSuccess);
        Assert.True(r.IsFaulted);
    }
    
    [Fact]
    public void Success_MatchBaseWithoutResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        r.Match(
            success: () => { },
            failure: _ => Assert.Fail("Expected success")
        );
    }
    
    [Fact]
    public void Failure_MatchBaseWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        r.Match(
            success: () => Assert.Fail("Expected failure"),
            failure: e => Assert.Equal("boom", e.First().Message)
        );
    }
    
    [Fact]
    public void Success_MatchBaseWithResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        var v = r.Match(
            success: () => { return 24; },
            failure: _ => {
                Assert.Fail("Expected success");
                return 0;
            }
        );
        Assert.Equal(24, v);
    }
    
    [Fact]
    public void Failure_MatchBaseWithResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        var msg = r.Match(
            success: () => {
                Assert.Fail("Expected failure");
                return "";
            },
            failure: errors => {
                Assert.NotNull(errors);
                Assert.Single(errors);
                var error = errors.First();
                Assert.Equal("boom", error.Message);
                Assert.Equal("EXCEPTION", error.Code);
                Assert.NotNull(error.Exception);
                return error.Message;
            }
        );
        Assert.Equal("boom", msg);
    }
    
    [Fact]
    public void Success_MatchWithoutResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        r.Match(
            success: (v1, v2, v3, v4, v5, v6) => {
                Assert.Equal(42, v1);
                Assert.Equal("foo", v2);
                Assert.True(v3);
                Assert.Equal(3.14, v4);
                Assert.Equal(99.99m, v5);
                Assert.Equal(1000L, v6);
                            Assert.True(true);
            },
            failure: _ => Assert.Fail("Expected success")
        );
    }
    
    [Fact]
    public void Failure_MatchWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        var called = false;
        r.Match(
            success: (_, _, _, _, _, _) => Assert.Fail("Expected failure"),
            failure: errors => {
                called = true;
                Assert.Single(errors);
                Assert.Equal("boom", errors.First().Message);
            }
        );
        Assert.True(called);
    }
    
    [Fact]
    public void Success_MatchWithResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        var e = r.Match(
            success: (v1, v2, v3, v4, v5, v6) => v1,
            failure: _ => {
                Assert.Fail("Expected failure");
                return 0;
            }
        );
        Assert.Equal(42, e);
    }
    
    [Fact]
    public void Failure_MatchWithResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        var result = r.Match(
            success: (_, _, _, _, _, _) => {
                Assert.Fail("Expected failure");
                return "";
            },
            failure: errors => errors.First().Message
        );
        Assert.Equal("boom", result);
    }
    
    [Fact]
    public void Success_IfSuccess_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        var isCalled = false;
        r.IfSuccess((v1, v2, v3, v4, v5, v6) => {
            Assert.Equal(42, v1);
            Assert.Equal("foo", v2);
            Assert.True(v3);
            Assert.Equal(3.14, v4);
            Assert.Equal(99.99m, v5);
            Assert.Equal(1000L, v6);
            isCalled = true;
        });
        Assert.True(isCalled);
        isCalled = false;
        r.IfSuccess(() => { isCalled = true; });
    }
    
    [Fact]
    public void Failure_IfSuccess_DoesNotCallSuccessAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        r.IfSuccess((_, _, _, _, _, _) => Assert.Fail("Expected failure"));
        r.IfSuccess(() => Assert.Fail("Expected failure"));
    }
    
    [Fact]
    public void Success_IfFailure_DoesNotCallFailureAction() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        r.IfFailure(_ => Assert.Fail("Expected success"));
    }
    
    [Fact]
    public void Failure_IfFailure_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        var called = false;
        r.IfFailure(errors => {
            called = true;
            Assert.Single(errors);
            Assert.Equal("boom", errors.First().Message);
        });
        Assert.True(called);
    }
    
    [Fact]
    public void Success_Deconstruct_ReturnsValue() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        r.Deconstruct(out var ok, out var values, out var errors);
        Assert.True(ok);
        Assert.NotNull(values);
        Assert.Equal(42, values.Value.Item1);
        Assert.Equal("foo", values.Value.Item2);
        Assert.True(values.Value.Item3);
        Assert.Equal(3.14, values.Value.Item4);
        Assert.Equal(99.99m, values.Value.Item5);
        Assert.Equal(1000L, values.Value.Item6);
        Assert.Null(errors);
    }
    
    [Fact]
    public void Failure_Deconstruct_ReturnsErrorMessage() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        r.Deconstruct(out var ok, out var values, out var errors);
        Assert.False(ok);
        Assert.Null(values);
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("boom", errors.First().Message);
    }
    
    [Fact]
    public void Success_Ok_ReturnsValue() {
        var r = Result.Success(42, "foo", true, 3.14, 99.99m, 1000L);
        if (r.TryGet(out var value1, out var value2, out var value3, out var value4, out var value5, out var value6)) {
               Assert.Equal(42, value1);
            Assert.Equal("foo", value2);
            Assert.True(value3);
            Assert.Equal(3.14, value4);
            Assert.Equal(99.99m, value5);
            Assert.Equal(1000L, value6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    
    [Fact]
    public void Failure_Ok_ReturnsErrorMessage() {
        var r = Result.Failure<int, string, bool, double, decimal, long>(new Exception("boom"));
        // Test TryGet with both value and error out parameters
        if (!r.TryGet(out _, out _, out _, out _, out _, out _, out var errors)) {
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal("boom", errors.First().Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
        // Test TryGet with just value out parameter
        if (r.TryGet(out _, out _, out _, out _, out _, out _)) {
            Assert.Fail("Expected failure");
        }
        // Test TryGet with just error out parameter
        if (!r.TryGet(out IEnumerable<IError>? errors2)) {
            Assert.NotNull(errors2);
            Assert.Single(errors2);
            Assert.Equal("boom", errors2.First().Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
    
}
