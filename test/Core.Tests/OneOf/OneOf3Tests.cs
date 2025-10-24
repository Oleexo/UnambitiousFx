using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf3Tests {
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string, bool>.FromFirst(42);

        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
    }

    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string, bool>.FromSecond("hello");

        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
        Assert.False(result.IsThird);
    }

    [Fact]
    public void FromThird_ShouldSetIsThird() {
        var result = OneOf<int, string, bool>.FromThird(true);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.True(result.IsThird);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool>.FromFirst(value);

        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
    }

    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool>.FromSecond(value);

        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FromThird_ShouldStoreValue(bool value) {
        var result = OneOf<int, string, bool>.FromThird(value);

        Assert.True(result.Third(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
    }

    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool>.FromFirst(42);

        var result = oneOf.Match(x => x,
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding First value");
                                     return 0;
                                 });

        Assert.Equal(42, result);
    }

    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool>.FromSecond("hello");

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Second value");
                                     return "";
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Second value");
                                     return "";
                                 });

        Assert.Equal("hello", result);
    }

    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool>.FromThird(true);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 x => x);

        Assert.True(result);
    }

    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool>.FromFirst(42);

        oneOf.Match(x => { Assert.Equal(42, x); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding First value"); });
    }

    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool>.FromSecond("hello");

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); },
                    x => { Assert.Equal("hello", x); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); });
    }

    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool>.FromThird(true);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); },
                    x => { Assert.True(x); });
    }
}
