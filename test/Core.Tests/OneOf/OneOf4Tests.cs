using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf4Tests
{
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string, bool, double>.FromFirst(42);
        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
    }
    
    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string, bool, double>.FromSecond("hello");
        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
    }
    
    [Fact]
    public void FromThird_ShouldSetIsThird() {
        var result = OneOf<int, string, bool, double>.FromThird(true);
        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.True(result.IsThird);
        Assert.False(result.IsFourth);
    }
    
    [Fact]
    public void FromFourth_ShouldSetIsFourth() {
        var result = OneOf<int, string, bool, double>.FromFourth(3.14);
        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.True(result.IsFourth);
    }
    
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    [Theory]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool, double>.FromFirst(value);
        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
    }
    
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    [Theory]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool, double>.FromSecond(value);
        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
    }
    
    [Fact]
    public void FromThird_ShouldStoreValue() {
        var result = OneOf<int, string, bool, double>.FromThird(true);
        Assert.True(result.Third(out var extracted));
        Assert.True(extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Fourth(out _));
    }
    
    [Fact]
    public void FromFourth_ShouldStoreValue() {
        var result = OneOf<int, string, bool, double>.FromFourth(3.14);
        Assert.True(result.Fourth(out var extracted));
        Assert.Equal(3.14, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
    }
    
    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool, double>.FromFirst(42);
        var result = oneOf.Match(x => x, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); return default; });
        Assert.Equal(42, result);
    }
    
    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool, double>.FromSecond("hello");
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); return default; }, x => x, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); return default; }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); return default; });
        Assert.Equal("hello", result);
    }
    
    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool, double>.FromThird(true);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); return default; }, x => x, _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); return default; });
        Assert.True(result);
    }
    
    [Fact]
    public void FromFourth_WhenMatchWithResponse_ShouldReturnFourthValue() {
        var oneOf = OneOf<int, string, bool, double>.FromFourth(3.14);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); return default; }, x => x);
        Assert.Equal(3.14, result);
    }
    
    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool, double>.FromFirst(42);
        oneOf.Match(x => { Assert.Equal(42, x); }, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); });
    }
    
    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool, double>.FromSecond("hello");
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); }, x => { Assert.Equal("hello", x); }, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); });
    }
    
    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool, double>.FromThird(true);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); }, x => { Assert.True(x); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); });
    }
    
    [Fact]
    public void FromFourth_WhenMatch_ShouldCallFourthHandler() {
        var oneOf = OneOf<int, string, bool, double>.FromFourth(3.14);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); }, x => { Assert.Equal(3.14, x); });
    }
    
    [Fact]
    public void ImplicitConversion_FromFirst_ShouldWork() {
        OneOf<int, string, bool, double> result = 42;
        Assert.True(result.IsFirst);
        Assert.True(result.First(out var value));
        Assert.Equal(42, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromSecond_ShouldWork() {
        OneOf<int, string, bool, double> result = "hello";
        Assert.True(result.IsSecond);
        Assert.True(result.Second(out var value));
        Assert.Equal("hello", value);
    }
    
    [Fact]
    public void ImplicitConversion_FromThird_ShouldWork() {
        OneOf<int, string, bool, double> result = true;
        Assert.True(result.IsThird);
        Assert.True(result.Third(out var value));
        Assert.True(value);
    }
    
    [Fact]
    public void ImplicitConversion_FromFourth_ShouldWork() {
        OneOf<int, string, bool, double> result = 3.14;
        Assert.True(result.IsFourth);
        Assert.True(result.Fourth(out var value));
        Assert.Equal(3.14, value);
    }
    
}
