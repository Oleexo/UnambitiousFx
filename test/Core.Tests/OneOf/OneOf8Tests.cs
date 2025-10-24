using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf8Tests {
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFirst(42);

        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSecond("hello");

        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromThird_ShouldSetIsThird() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromThird(true);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.True(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromFourth_ShouldSetIsFourth() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFourth(3.14);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.True(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromFifth_ShouldSetIsFifth() {
        var date = new DateTime(2025, 10, 17, 1, 2, 3, DateTimeKind.Utc);
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFifth(date);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.True(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromSixth_ShouldSetIsSixth() {
        var id = Guid.NewGuid();
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSixth(id);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.True(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromSeventh_ShouldSetIsSeventh() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSeventh(123.45m);

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.True(result.IsSeventh);
        Assert.False(result.IsEighth);
    }

    [Fact]
    public void FromEighth_ShouldSetIsEighth() {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromEighth(TimeSpan.FromMinutes(5));

        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
        Assert.False(result.IsSixth);
        Assert.False(result.IsSeventh);
        Assert.True(result.IsEighth);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFirst(value);

        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Theory]
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSecond(value);

        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void FromThird_ShouldStoreValue(bool value) {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromThird(value);

        Assert.True(result.Third(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(3.14)]
    [InlineData(-2.5)]
    public void FromFourth_ShouldStoreValue(double value) {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFourth(value);

        Assert.True(result.Fourth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Fact]
    public void FromFifth_ShouldStoreValue() {
        var value = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFifth(value);

        Assert.True(result.Fifth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Fact]
    public void FromSixth_ShouldStoreValue() {
        var value = Guid.NewGuid();
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSixth(value);

        Assert.True(result.Sixth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Seventh(out _));
        Assert.False(result.Eighth(out _));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(12345)]
    [InlineData(-9876)]
    public void FromSeventh_ShouldStoreValue(decimal value) {
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSeventh(value);

        Assert.True(result.Seventh(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Eighth(out _));
    }

    [Fact]
    public void FromEighth_ShouldStoreValue() {
        var value = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30);
        var result = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromEighth(value);

        Assert.True(result.Eighth(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
        Assert.False(result.Sixth(out _));
        Assert.False(result.Seventh(out _));
    }

    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFirst(42);

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
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding First value");
                                     return 0;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding First value");
                                     return 0;
                                 });

        Assert.Equal(42, result);
    }

    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSecond("hello");

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
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Second value");
                                     return string.Empty;
                                 });

        Assert.Equal("hello", result);
    }

    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromThird(true);

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
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Third value");
                                     return false;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Third value");
                                     return false;
                                 });

        Assert.True(result);
    }

    [Fact]
    public void FromFourth_WhenMatchWithResponse_ShouldReturnFourthValue() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFourth(2.5);

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
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Fourth value");
                                     return 0.0;
                                 });

        Assert.Equal(2.5, result);
    }

    [Fact]
    public void FromFifth_WhenMatchWithResponse_ShouldReturnFifthValue() {
        var date = new DateTime(2025, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFifth(date);

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
                                 x => x,
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Fifth value");
                                     return DateTime.MinValue;
                                 });

        Assert.Equal(date, result);
    }

    [Fact]
    public void FromSixth_WhenMatchWithResponse_ShouldReturnSixthValue() {
        var id = Guid.NewGuid();
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSixth(id);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 },
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Sixth value");
                                     return Guid.Empty;
                                 });

        Assert.Equal(id, result);
    }

    [Fact]
    public void FromSeventh_WhenMatchWithResponse_ShouldReturnSeventhValue() {
        const decimal value = 999.123m;
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSeventh(value);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 },
                                 x => x,
                                 _ => {
                                     Assert.Fail("Eighth handler was called for OneOf holding Seventh value");
                                     return 0m;
                                 });

        Assert.Equal(value, result);
    }

    [Fact]
    public void FromEighth_WhenMatchWithResponse_ShouldReturnEighthValue() {
        var value = TimeSpan.FromSeconds(12345);
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromEighth(value);

        var result = oneOf.Match(_ => {
                                     Assert.Fail("First handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Second handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Third handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Fourth handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Fifth handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Sixth handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 _ => {
                                     Assert.Fail("Seventh handler was called for OneOf holding Eighth value");
                                     return TimeSpan.Zero;
                                 },
                                 x => x);

        Assert.Equal(value, result);
    }

    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFirst(42);

        oneOf.Match(x => { Assert.Equal(42, x); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding First value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding First value"); });
    }

    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSecond("hello");

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); },
                    x => { Assert.Equal("hello", x); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Second value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Second value"); });
    }

    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromThird(true);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); },
                    x => { Assert.True(x); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Third value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Third value"); });
    }

    [Fact]
    public void FromFourth_WhenMatch_ShouldCallFourthHandler() {
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFourth(3.14);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); },
                    x => { Assert.Equal(3.14, x); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Fourth value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Fourth value"); });
    }

    [Fact]
    public void FromFifth_WhenMatch_ShouldCallFifthHandler() {
        var date = new DateTime(2025, 10, 17, 1, 2, 3, DateTimeKind.Utc);
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromFifth(date);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Fifth value"); },
                    x => { Assert.Equal(date, x); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Fifth value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Fifth value"); });
    }

    [Fact]
    public void FromSixth_WhenMatch_ShouldCallSixthHandler() {
        var id = Guid.NewGuid();
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSixth(id);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Sixth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Sixth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Sixth value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Sixth value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Sixth value"); },
                    x => { Assert.Equal(id, x); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Sixth value"); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Sixth value"); });
    }

    [Fact]
    public void FromSeventh_WhenMatch_ShouldCallSeventhHandler() {
        const decimal value = 1.23m;
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromSeventh(value);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Seventh value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Seventh value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Seventh value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Seventh value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Seventh value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Seventh value"); },
                    x => { Assert.Equal(value, x); },
                    _ => { Assert.Fail("Eighth handler was called for OneOf holding Seventh value"); });
    }

    [Fact]
    public void FromEighth_WhenMatch_ShouldCallEighthHandler() {
        var span = TimeSpan.FromMinutes(10);
        var oneOf = OneOf<int, string, bool, double, DateTime, Guid, decimal, TimeSpan>.FromEighth(span);

        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Second handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Third handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Fourth handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Fifth handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Sixth handler was called for OneOf holding Eighth value"); },
                    _ => { Assert.Fail("Seventh handler was called for OneOf holding Eighth value"); },
                    x => { Assert.Equal(span, x); });
    }
}
