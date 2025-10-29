using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public sealed class ResultArity1Tests
{
    [Fact]
    public void Success_IsSuccess_ReturnsTrue() {
        var r = Result.Success(42);
        Assert.True(r.IsSuccess);
        Assert.False(r.IsFaulted);
    }
    
    [Fact]
    public void Failure_IsSuccess_ReturnsFalse() {
        var r = Result.Failure<int>(new Exception("boom"));
        Assert.False(r.IsSuccess);
        Assert.True(r.IsFaulted);
    }
    
    [Fact]
    public void Success_MatchBaseWithoutResponse_CallsSuccessAction() {
        var r = Result.Success(42);
        r.Match(
            success: () => { },
            failure: _ => Assert.Fail("Expected success")
        );
    }
    
    [Fact]
    public void Failure_MatchBaseWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int>(new Exception("boom"));
        r.Match(
            success: () => Assert.Fail("Expected failure"),
            failure: e => Assert.Equal("boom", e.First().Message)
        );
    }
    
    [Fact]
    public void Success_MatchBaseWithResponse_CallsSuccessAction() {
        var r = Result.Success(42);
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
        var r = Result.Failure<int>(new Exception("boom"));
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
        var r = Result.Success(42);
        r.Match(
            success: (v) => {
                Assert.Equal(42, v);
                            Assert.True(true);
            },
            failure: _ => Assert.Fail("Expected success")
        );
    }
    
    [Fact]
    public void Failure_MatchWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int>(new Exception("boom"));
        var called = false;
        r.Match(
            success: (_) => Assert.Fail("Expected failure"),
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
        var r = Result.Success(42);
        var e = r.Match(
            success: (v) => v,
            failure: _ => {
                Assert.Fail("Expected failure");
                return 0;
            }
        );
        Assert.Equal(42, e);
    }
    
    [Fact]
    public void Failure_MatchWithResponse_CallsFailureAction() {
        var r = Result.Failure<int>(new Exception("boom"));
        var result = r.Match(
            success: (_) => {
                Assert.Fail("Expected failure");
                return "";
            },
            failure: errors => errors.First().Message
        );
        Assert.Equal("boom", result);
    }
    
    [Fact]
    public void Success_IfSuccess_CallsSuccessAction() {
        var r = Result.Success(42);
        var isCalled = false;
        r.IfSuccess((v) => {
            Assert.Equal(42, v);
            isCalled = true;
        });
        Assert.True(isCalled);
        isCalled = false;
        r.IfSuccess(() => { isCalled = true; });
    }
    
    [Fact]
    public void Failure_IfSuccess_DoesNotCallSuccessAction() {
        var r = Result.Failure<int>(new Exception("boom"));
        r.IfSuccess((_) => Assert.Fail("Expected failure"));
        r.IfSuccess(() => Assert.Fail("Expected failure"));
    }
    
    [Fact]
    public void Success_IfFailure_DoesNotCallFailureAction() {
        var r = Result.Success(42);
        r.IfFailure(_ => Assert.Fail("Expected success"));
    }
    
    [Fact]
    public void Failure_IfFailure_CallsFailureAction() {
        var r = Result.Failure<int>(new Exception("boom"));
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
        var r = Result.Success(42);
        r.Deconstruct(out var ok, out var value, out var errors);
        Assert.True(ok);
        Assert.Equal(42, value);
        Assert.Null(errors);
    }
    
    [Fact]
    public void Failure_Deconstruct_ReturnsErrorMessage() {
        var r = Result.Failure<int>(new Exception("boom"));
        r.Deconstruct(out var ok, out var value, out var errors);
        Assert.False(ok);
        Assert.Equal(0, value);
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("boom", errors.First().Message);
    }
    
    [Fact]
    public void Success_Ok_ReturnsValue() {
        var r = Result.Success(42);
        if (r.TryGet(out var value1)) {
               Assert.Equal(42, value1);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    
    [Fact]
    public void Failure_Ok_ReturnsErrorMessage() {
        var r = Result.Failure<int>(new Exception("boom"));
        // Test TryGet with both value and error out parameters
        if (!r.TryGet(out _, out var errors)) {
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal("boom", errors.First().Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
        // Test TryGet with just value out parameter
        if (r.TryGet(out _)) {
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
