using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(Result))]
public sealed class ResultArity0Tests {
    [Fact]
    public void Success_Ok_ReturnsTrue() {
        var r = Result.Success();

        var ok = r.Ok(out _);

        Assert.True(ok);
    }

    [Fact]
    public void Failure_Ok_ReturnsFalse() {
        var r = Result.Failure("boom");

        var ok = r.Ok(out _);

        Assert.False(ok);
    }

    [Fact]
    public void Success_Match_CallsSuccessAction() {
        var r = Result.Success();

        var called = false;
        r.Match(() => called = true, _ => called = false);

        Assert.True(called);
    }

    [Fact]
    public void Failure_Match_CallsFailureAction() {
        var r = Result.Failure("boom");

        var called = false;
        r.Match(() => called = false, _ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void Success_Bind_InvokesBinder() {
        var r = Result.Success();

        var invoked = false;
        var next    = r.Bind(() => {
            invoked = true;
            return Result.Success();
        });

        Assert.True(invoked);
    }

    [Fact]
    public void Failure_Bind_DoesNotInvokeBinder() {
        var r = Result.Failure("boom");

        var invoked = false;
        var next    = r.Bind(() => {
            invoked = true;
            return Result.Success();
        });

        Assert.False(invoked);
    }
}
