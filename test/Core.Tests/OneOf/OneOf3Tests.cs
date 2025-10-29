
namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf3Tests
{
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
    
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    [Theory]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool>.FromFirst(value);
        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
    }
    
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    [Theory]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool>.FromSecond(value);
        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
    }
    
    [Fact]
    public void FromThird_ShouldStoreValue() {
        var result = OneOf<int, string, bool>.FromThird(true);
        Assert.True(result.Third(out var extracted));
        Assert.Equal(true, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
    }
    
    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool>.FromFirst(42);
        var result = oneOf.Match(x => x, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); return default; });
        Assert.Equal(42, result);
    }
    
    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool>.FromSecond("hello");
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); return default; }, x => x, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); return default; });
        Assert.Equal("hello", result);
    }
    
    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool>.FromThird(true);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); return default; }, x => x);
        Assert.Equal(true, result);
    }
    
    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool>.FromFirst(42);
        oneOf.Match(x => { Assert.Equal(42, x); }, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); });
    }
    
    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool>.FromSecond("hello");
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); }, x => { Assert.Equal("hello", x); }, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); });
    }
    
    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool>.FromThird(true);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); }, x => { Assert.Equal(true, x); });
    }
    
    [Fact]
    public void ImplicitConversion_FromFirst_ShouldWork() {
        OneOf<int, string, bool> result = 42;
        Assert.True(result.IsFirst);
        Assert.True(result.First(out var value));
        Assert.Equal(42, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromSecond_ShouldWork() {
        OneOf<int, string, bool> result = "hello";
        Assert.True(result.IsSecond);
        Assert.True(result.Second(out var value));
        Assert.Equal("hello", value);
    }
    
    [Fact]
    public void ImplicitConversion_FromThird_ShouldWork() {
        OneOf<int, string, bool> result = true;
        Assert.True(result.IsThird);
        Assert.True(result.Third(out var value));
        Assert.Equal(true, value);
    }
    
}
