using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf2Tests
{
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string>.FromFirst(42);
        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
    }
    
    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string>.FromSecond("hello");
        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
    }
    
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    [Theory]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string>.FromFirst(value);
        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
    }
    
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    [Theory]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string>.FromSecond(value);
        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
    }
    
    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string>.FromFirst(42);
        var result = oneOf.Match(x => x, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); return default; });
        Assert.Equal(42, result);
    }
    
    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string>.FromSecond("hello");
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); return default; }, x => x);
        Assert.Equal("hello", result);
    }
    
    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string>.FromFirst(42);
        oneOf.Match(x => { Assert.Equal(42, x); }, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); });
    }
    
    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string>.FromSecond("hello");
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); }, x => { Assert.Equal("hello", x); });
    }
    
    [Fact]
    public void ImplicitConversion_FromFirst_ShouldWork() {
        OneOf<int, string> result = 42;
        Assert.True(result.IsFirst);
        Assert.True(result.First(out var value));
        Assert.Equal(42, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromSecond_ShouldWork() {
        OneOf<int, string> result = "hello";
        Assert.True(result.IsSecond);
        Assert.True(result.Second(out var value));
        Assert.Equal("hello", value);
    }
    
}
