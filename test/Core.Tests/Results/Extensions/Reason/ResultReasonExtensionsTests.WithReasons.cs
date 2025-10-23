using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Reason;

public sealed partial class ResultReasonExtensionsTests {
    [Fact]
    public void WithReasons_AddsAllReasons() {
        var r1 = new SuccessReason("r1", new Dictionary<string, object?>());
        var r2 = new SuccessReason("r2", new Dictionary<string, object?>());

        var result = Result.Success()
                           .WithReasons(new IReason[] { r1, r2 });

        Assert.Equal(2, result.Reasons.Count);
    }

    [Fact]
    public void WithReasons_PreservesOrder() {
        var r1 = new SuccessReason("r1", new Dictionary<string, object?>());
        var r2 = new SuccessReason("r2", new Dictionary<string, object?>());

        var result = Result.Success()
                           .WithReasons(new IReason[] { r1, r2 });

        Assert.Same(r1, result.Reasons[0]);
    }
}
