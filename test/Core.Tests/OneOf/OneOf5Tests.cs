using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf5Tests {
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string, bool, double, DateTime>.FromFirst(42);

        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }

    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string, bool, double, DateTime>.FromSecond("hello");

        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }

    [Fact]
    public void FromThird_ShouldSetIsThird() {
        var result = OneOf<int, string, bool, double, DateTime>.FromThird(true);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.True(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }

    [Fact]
    public void FromFourth_ShouldSetIsFourth() {
        var result = OneOf<int, string, bool, double, DateTime>.FromFourth(3.14);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.True(result.IsFourth);
        Assert.False(result.IsFifth);
    }

    [Fact]
    public void FromFifth_ShouldSetIsFifth() {
        var now = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var result = OneOf<int, string, bool, double, DateTime>.FromFifth(now);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.True(result.IsFifth);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool, double, DateTime>.FromFirst(value);

        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }

    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool, double, DateTime>.FromSecond(value);

        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FromThird_ShouldStoreValue(bool value) {
        var result = OneOf<int, string, bool, double, DateTime>.FromThird(value);

        Assert.True(result.Third(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(3.14)]
    [InlineData(-2.5)]
    public void FromFourth_ShouldStoreValue(double value) {
        var result = OneOf<int, string, bool, double, DateTime>.FromFourth(value);

        Assert.True(result.Fourth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fifth(out _));
    }

    [Fact]
    public void FromFifth_ShouldStoreValue() {
        var value = new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var result = OneOf<int, string, bool, double, DateTime>.FromFifth(value);

        Assert.True(result.Fifth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
    }

    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFirst(42);

        var result = oneOf.Match(x => x,
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding First value");
                                     return 0;
                                 });

        Assert.Equal(42, result);
    }

    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromSecond("hello");

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 });

        Assert.Equal("hello", result);
    }

    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromThird(true);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Third value");
                                     return false;
                                 });

        Assert.True(result);
    }

    [Fact]
    public void FromFourth_WhenMatchWithResponse_ShouldReturnFourthValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFourth(2.5);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 });

        Assert.Equal(2.5, result);
    }

    [Fact]
    public void FromFifth_WhenMatchWithResponse_ShouldReturnFifthValue() {
        var value = new DateTime(2020, 5, 6, 7, 8, 9, DateTimeKind.Utc);
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFifth(value);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 x => x);

        Assert.Equal(value, result);
    }

    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFirst(42);

        oneOf.Match(x => { Assert.Equal(42, x); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding First value"); });
    }

    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromSecond("hello");

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); },
                    x => { Assert.Equal("hello", x); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Second value"); });
    }

    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromThird(true);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); },
                    x => { Assert.True(x); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Third value"); });
    }

    [Fact]
    public void FromFourth_WhenMatch_ShouldCallFourthHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFourth(3.14);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); },
                    x => { Assert.Equal(3.14, x); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Fourth value"); });
    }

    [Fact]
    public void FromFifth_WhenMatch_ShouldCallFifthHandler() {
        var value = new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var oneOf = OneOf<int, string, bool, double, DateTime>.FromFifth(value);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Fifth value"); },
                    x => { Assert.Equal(value, x); });
    }
}
