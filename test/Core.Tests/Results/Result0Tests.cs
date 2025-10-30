#nullable enable

using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results;

public sealed class ResultArity0Tests
{
    [Fact]
    public void Success_IsSuccess_ReturnsTrue() {
        var r = Result.Success();
        Assert.True(r.IsSuccess);
        Assert.False(r.IsFaulted);
    }
    
    [Fact]
    public void Failure_IsSuccess_ReturnsFalse() {
        var r = Result.Failure(new Exception("boom"));
        Assert.False(r.IsSuccess);
        Assert.True(r.IsFaulted);
    }
    
    [Fact]
    public void Success_MatchBaseWithoutResponse_CallsSuccessAction() {
        var r = Result.Success();
        r.Match(
            success: () => { },
            failure: _ => Assert.Fail("Expected success")
        );
    }
    
    [Fact]
    public void Failure_MatchBaseWithoutResponse_CallsFailureAction() {
        var r = Result.Failure(new Exception("boom"));
        r.Match(
            success: () => Assert.Fail("Expected failure"),
            failure: e => Assert.Equal("boom", e.First().Message)
        );
    }
    
    [Fact]
    public void Success_MatchBaseWithResponse_CallsSuccessAction() {
        var r = Result.Success();
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
        var r = Result.Failure(new Exception("boom"));
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
    public void Success_IfSuccess_CallsSuccessAction() {
        var r = Result.Success();
        var isCalled = false;
        r.IfSuccess(() => { isCalled = true; });
        Assert.True(isCalled);
    }
    
    [Fact]
    public void Failure_IfSuccess_DoesNotCallSuccessAction() {
        var r = Result.Failure(new Exception("boom"));
        r.IfSuccess(() => Assert.Fail("Expected failure"));
    }
    
    [Fact]
    public void Success_IfFailure_DoesNotCallFailureAction() {
        var r = Result.Success();
        r.IfFailure(_ => Assert.Fail("Expected success"));
    }
    
    [Fact]
    public void Failure_IfFailure_CallsFailureAction() {
        var r = Result.Failure(new Exception("boom"));
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
        var r = Result.Success();
        r.Deconstruct(out var ok, out var errors);
        Assert.True(ok);
        Assert.Null(errors);
    }
    
    [Fact]
    public void Failure_Deconstruct_ReturnsErrorMessage() {
        var r = Result.Failure(new Exception("boom"));
        r.Deconstruct(out var ok, out var errors);
        Assert.False(ok);
        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("boom", errors.First().Message);
    }
    
    [Fact]
    public void Success_TryGet_ReturnsTrue() {
        var r = Result.Success();
        if (r.TryGet(out var errors)) {
            Assert.Null(errors);
        }
        else {
            Assert.Fail("Expected success");
        }
    }
    
    [Fact]
    public void Failure_TryGet_ReturnsErrorMessage() {
        var r = Result.Failure(new Exception("boom"));
        if (!r.TryGet(out var errors)) {
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal("boom", errors.First().Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
    
}
