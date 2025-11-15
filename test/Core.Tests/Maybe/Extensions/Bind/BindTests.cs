using UnambitiousFx.Core.Maybe;

namespace UnambitiousFx.Core.Tests.Maybe.Extensions.Bind;

public sealed class BindTests
{
    [Fact]
    public void GivenMultipleFunctions_WhenChained_ThenCallsTheLastFunction()
    {
        var option = GetUser("toto")
                    .Bind(GetLatestOrder)
                    .Bind(GetShippingInfo);

        if (option.Some(out var value))
        {
            Assert.Equal("Hello 42 from fx", value);
        }
        else
        {
            Assert.Fail("Result should be successful but was marked as failed");
        }

        Maybe<string> GetShippingInfo((string order, int user) tuple)
        {
            return $"Hello {tuple.user} from {tuple.order}";
        }

        Maybe<(string, int)> GetLatestOrder(int user)
        {
            return user == 42
                       ? ("fx", 42)
                       : ("fx", 24);
        }

        // Example of chaining operations that might fail
        Maybe<int> GetUser(string userId)
        {
            return userId == "toto"
                       ? 42
                       : 24;
        }
    }
}
