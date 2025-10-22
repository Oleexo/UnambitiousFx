using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using TasksExt = UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.Recovery;

[TestSubject(typeof(TasksExt))]
public sealed class ResultRecoveryExtensionsTests {
    [Fact]
    public void Recover_Success_DoesNotInvoke_ReturnsSameValue() {
        var r      = Result.Success(5);
        var called = false;

        var recovered = r.Recover(_ => {
            called = true;
            return 0;
        });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(5, value);
        Assert.False(called);
    }

    [Fact]
    public void Recover_Failure_UsesFallbackValue() {
        var ex              = new InvalidOperationException("boom");
        var r               = Result.Failure<int>(ex);
        var passedSameError = false;

        var recovered = r.Recover(err => {
            passedSameError = ReferenceEquals(err, ex);
            return 42;
        });

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(42, value);
        Assert.True(passedSameError);
    }

    [Fact]
    public void Recover_WithConstantFallback_WhenFailure_UsesConstant() {
        var ex = new Exception("oops");
        var r  = Result.Failure<int>(ex);

        var recovered = r.Recover(99);

        Assert.True(recovered.Ok(out var value, out _));
        Assert.Equal(99, value);
    }
}
