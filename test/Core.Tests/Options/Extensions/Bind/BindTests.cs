using UnambitiousFx.Core.Options;

namespace UnambitiousFx.Core.Tests.Options.Extensions.Bind;

public sealed class BindTests {
    [Fact]
    public void GivenMultipleFunctions_WhenChained_ThenCallsTheLastFunction() {
        var option = GetUser("toto")
                    .Bind(GetLatestOrder)
                    .Bind(GetShippingInfo);

        if (option.Some(out var value)) {
            Assert.Equal("Hello 42 from fx", value);
        }
        else {
            Assert.Fail("Result should be successful but was marked as failed");
        }

        Option<string> GetShippingInfo((string order, int user) tuple) {
            return $"Hello {tuple.user} from {tuple.order}";
        }

        Option<(string, int)> GetLatestOrder(int user) {
            return user == 42
                       ? ("fx", 42)
                       : ("fx", 24);
        }

        // Example of chaining operations that might fail
        Option<int> GetUser(string userId) {
            return userId == "toto"
                       ? 42
                       : 24;
        }
    }
}
