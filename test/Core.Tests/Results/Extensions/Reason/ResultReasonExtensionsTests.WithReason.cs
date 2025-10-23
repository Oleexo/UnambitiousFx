using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests {
    [Fact]
    public void WithReason_AttachesProvidedReason() {
        var reason = new SuccessReason("ok", new Dictionary<string, object?>());
        var result = Result.Success()
                           .WithReason(reason);

        Assert.Same(reason, result.Reasons[0]);
    }

    [Fact]
    public void WithReason_ReturnsSameInstance() {
        var initial = Result.Success();
        var updated = initial.WithReason(new SuccessReason("x", new Dictionary<string, object?>()));

        Assert.Same(initial, updated);
    }
}
