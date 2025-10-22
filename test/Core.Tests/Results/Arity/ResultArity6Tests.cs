using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Arity;

[TestSubject(typeof(Result<,,,,,>))]
public sealed class ResultArity6Tests {
    [Fact]
    public void Success_IsSuccess_ReturnsTrue() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        Assert.True(r.IsSuccess);
        Assert.False(r.IsFaulted);
    }

    [Fact]
    public void Failure_IsSuccess_ReturnsFalse() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        Assert.False(r.IsSuccess);
        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Success_MatchBaseWithoutResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        r.Match(
            success: () => { },
            failure: _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void Failure_MatchBaseWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        r.Match(
            success: () => Assert.Fail("Expected failure"),
            failure: e => Assert.Equal("boom", e.Message)
        );
    }

    [Fact]
    public void Success_MatchBaseWithResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        var v = r.Match(
            success: () => { return 24; },
            failure: _ => {
                Assert.Fail("Expected success");
                return 0;
            });

        Assert.Equal(24, v);
    }

    [Fact]
    public void Failure_MatchBaseWithResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        var msg = r.Match(
            success: () => {
                Assert.Fail("Expected failure");
                return "";
            },
            failure: e => {
                Assert.Equal("boom", e.Message);
                return e.Message;
            });

        Assert.Equal("boom", msg);
    }

    [Fact]
    public void Success_MatchWithoutResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        r.Match(
            success: (v1,
                      v2,
                      v3,
                      v4,
                      v5,
                      v6) => {
                Assert.Equal(42,    v1);
                Assert.Equal("foo", v2);
                Assert.True(v3);
                Assert.Equal(7,     v4);
                Assert.Equal("bar", v5);
                Assert.True(v6);
            },
            failure: _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void Failure_MatchWithoutResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        r.Match(
            success: (_,
                      _,
                      _,
                      _,
                      _,
                      _) => Assert.Fail("Expected failure"),
            failure: e => Assert.Equal("boom", e.Message)
        );
    }

    [Fact]
    public void Success_MatchWithResponse_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        var e = r.Match(
            success: (v1,
                      v2,
                      v3,
                      v4,
                      v5,
                      v6) => v1        +
                             v2.Length +
                             (v3
                                  ? 1
                                  : 0) +
                             v4        +
                             v5.Length +
                             (v6
                                  ? 1
                                  : 0),
            failure: _ => {
                Assert.Fail("Expected success");
                return 0;
            });

        Assert.Equal(57, e);
    }

    [Fact]
    public void Failure_MatchWithResponse_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        var e = r.Match(
            success: (_,
                      _,
                      _,
                      _,
                      _,
                      _) => {
                Assert.Fail("Expected failure");
                return "";
            },
            failure: e => e.Message);

        Assert.Equal("boom", e);
    }

    [Fact]
    public void Success_IfSuccess_CallsSuccessAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        var isCalled = false;
        r.IfSuccess((v1,
                     v2,
                     v3,
                     v4,
                     v5,
                     v6) => {
            Assert.Equal(42,    v1);
            Assert.Equal("foo", v2);
            Assert.True(v3);
            Assert.Equal(7,     v4);
            Assert.Equal("bar", v5);
            Assert.True(v6);
            isCalled = true;
        });

        Assert.True(isCalled);
        isCalled = false;

        r.IfSuccess(() => { isCalled = true; });
    }

    [Fact]
    public void Failure_IfSuccess_DoesNotCallSuccessAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        r.IfSuccess((_,
                     _,
                     _,
                     _,
                     _,
                     _) => Assert.Fail("Expected failure"));
        
        r.IfSuccess(() => Assert.Fail("Expected failure"));
    }

    [Fact]
    public void Success_IfFailure_DoesNotCallFailureAction() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        r.IfFailure(_ => Assert.Fail("Expected success"));
    }

    [Fact]
    public void Failure_IfFailure_CallsFailureAction() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        r.IfFailure(e => Assert.Equal("boom", e.Message));
    }

    [Fact]
    public void Success_Deconstruct_ReturnsValue() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        r.Deconstruct(out var ok, out var value, out var err);
        Assert.True(ok);
        Assert.Equal((42, "foo", true, 7, "bar", true), value);
        Assert.Null(err);
    }

    [Fact]
    public void Failure_Deconstruct_ReturnsErrorMessage() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        r.Deconstruct(out var ok, out var value, out var err);
        Assert.False(ok);
        Assert.Null(value);
        Assert.NotNull(err);
        Assert.Equal("boom", err.Message);
    }

    [Fact]
    public void Success_Ok_ReturnsValue() {
        var r = Result.Success(42, "foo", true, 7, "bar", true);

        if (r.Ok(out var value1, out var value2,out var value3,out var value4,out var value5,out var value6)) {
            Assert.Equal(42,    value1);
            Assert.Equal("foo", value2);
            Assert.True(value3);
            Assert.Equal(7,     value4);
            Assert.Equal("bar", value5);
            Assert.True(value6);
        }
        else {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void Failure_Ok_ReturnsErrorMessage() {
        var r = Result.Failure<int, string, bool, int, string, bool>(new Exception("boom"));

        if (!r.Ok(out _, out _,out _,out _,out _,out _,out var err)) {
            Assert.Equal("boom", err.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
        
        if (r.Ok(out _)) {
            Assert.Fail("Expected failure");
        }

        if (!r.Ok(out Exception? err2)) {
            Assert.Equal("boom", err2.Message);
        }
        else {
            Assert.Fail("Expected failure");
        }
    }
}
