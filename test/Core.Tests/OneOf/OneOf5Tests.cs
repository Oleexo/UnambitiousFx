
namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOf5Tests
{
    [Fact]
    public void FromFirst_ShouldSetIsFirst() {
        var result = OneOf<int, string, bool, double, decimal>.FromFirst(42);
        Assert.True(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }
    
    [Fact]
    public void FromSecond_ShouldSetIsSecond() {
        var result = OneOf<int, string, bool, double, decimal>.FromSecond("hello");
        Assert.False(result.IsFirst);
        Assert.True(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }
    
    [Fact]
    public void FromThird_ShouldSetIsThird() {
        var result = OneOf<int, string, bool, double, decimal>.FromThird(true);
        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.True(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.False(result.IsFifth);
    }
    
    [Fact]
    public void FromFourth_ShouldSetIsFourth() {
        var result = OneOf<int, string, bool, double, decimal>.FromFourth(3.14);
        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.True(result.IsFourth);
        Assert.False(result.IsFifth);
    }
    
    [Fact]
    public void FromFifth_ShouldSetIsFifth() {
        var result = OneOf<int, string, bool, double, decimal>.FromFifth(99.99m);
        Assert.False(result.IsFirst);
        Assert.False(result.IsSecond);
        Assert.False(result.IsThird);
        Assert.False(result.IsFourth);
        Assert.True(result.IsFifth);
    }
    
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-1)]
    [Theory]
    public void FromFirst_ShouldStoreValue(int value) {
        var result = OneOf<int, string, bool, double, decimal>.FromFirst(value);
        Assert.True(result.First(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }
    
    [InlineData("")]
    [InlineData("hello")]
    [InlineData("test string")]
    [Theory]
    public void FromSecond_ShouldStoreValue(string value) {
        var result = OneOf<int, string, bool, double, decimal>.FromSecond(value);
        Assert.True(result.Second(out var extracted));
        Assert.Equal(value, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }
    
    [Fact]
    public void FromThird_ShouldStoreValue() {
        var result = OneOf<int, string, bool, double, decimal>.FromThird(true);
        Assert.True(result.Third(out var extracted));
        Assert.Equal(true, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Fourth(out _));
        Assert.False(result.Fifth(out _));
    }
    
    [Fact]
    public void FromFourth_ShouldStoreValue() {
        var result = OneOf<int, string, bool, double, decimal>.FromFourth(3.14);
        Assert.True(result.Fourth(out var extracted));
        Assert.Equal(3.14, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fifth(out _));
    }
    
    [Fact]
    public void FromFifth_ShouldStoreValue() {
        var result = OneOf<int, string, bool, double, decimal>.FromFifth(99.99m);
        Assert.True(result.Fifth(out var extracted));
        Assert.Equal(99.99m, extracted);
        Assert.False(result.First(out _));
        Assert.False(result.Second(out _));
        Assert.False(result.Third(out _));
        Assert.False(result.Fourth(out _));
    }
    
    [Fact]
    public void FromFirst_WhenMatchWithResponse_ShouldReturnFirstValue() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFirst(42);
        var result = oneOf.Match(x => x, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); return default; }, _ => { Assert.Fail("Fifth handler was called for OneOf holding First value"); return default; });
        Assert.Equal(42, result);
    }
    
    [Fact]
    public void FromSecond_WhenMatchWithResponse_ShouldReturnSecondValue() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromSecond("hello");
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); return default; }, x => x, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); return default; }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); return default; }, _ => { Assert.Fail("Fifth handler was called for OneOf holding Second value"); return default; });
        Assert.Equal("hello", result);
    }
    
    [Fact]
    public void FromThird_WhenMatchWithResponse_ShouldReturnThirdValue() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromThird(true);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); return default; }, x => x, _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); return default; }, _ => { Assert.Fail("Fifth handler was called for OneOf holding Third value"); return default; });
        Assert.Equal(true, result);
    }
    
    [Fact]
    public void FromFourth_WhenMatchWithResponse_ShouldReturnFourthValue() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFourth(3.14);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); return default; }, x => x, _ => { Assert.Fail("Fifth handler was called for OneOf holding Fourth value"); return default; });
        Assert.Equal(3.14, result);
    }
    
    [Fact]
    public void FromFifth_WhenMatchWithResponse_ShouldReturnFifthValue() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFifth(99.99m);
        var result = oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fifth value"); return default; }, _ => { Assert.Fail("Second handler was called for OneOf holding Fifth value"); return default; }, _ => { Assert.Fail("Third handler was called for OneOf holding Fifth value"); return default; }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Fifth value"); return default; }, x => x);
        Assert.Equal(99.99m, result);
    }
    
    [Fact]
    public void FromFirst_WhenMatch_ShouldCallFirstHandler() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFirst(42);
        oneOf.Match(x => { Assert.Equal(42, x); }, _ => { Assert.Fail("Second handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding First value"); }, _ => { Assert.Fail("Fifth handler was called for OneOf holding First value"); });
    }
    
    [Fact]
    public void FromSecond_WhenMatch_ShouldCallSecondHandler() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromSecond("hello");
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Second value"); }, x => { Assert.Equal("hello", x); }, _ => { Assert.Fail("Third handler was called for OneOf holding Second value"); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Second value"); }, _ => { Assert.Fail("Fifth handler was called for OneOf holding Second value"); });
    }
    
    [Fact]
    public void FromThird_WhenMatch_ShouldCallThirdHandler() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromThird(true);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Third value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Third value"); }, x => { Assert.Equal(true, x); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Third value"); }, _ => { Assert.Fail("Fifth handler was called for OneOf holding Third value"); });
    }
    
    [Fact]
    public void FromFourth_WhenMatch_ShouldCallFourthHandler() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFourth(3.14);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fourth value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Fourth value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding Fourth value"); }, x => { Assert.Equal(3.14, x); }, _ => { Assert.Fail("Fifth handler was called for OneOf holding Fourth value"); });
    }
    
    [Fact]
    public void FromFifth_WhenMatch_ShouldCallFifthHandler() {
        var oneOf = OneOf<int, string, bool, double, decimal>.FromFifth(99.99m);
        oneOf.Match(_ => { Assert.Fail("First handler was called for OneOf holding Fifth value"); }, _ => { Assert.Fail("Second handler was called for OneOf holding Fifth value"); }, _ => { Assert.Fail("Third handler was called for OneOf holding Fifth value"); }, _ => { Assert.Fail("Fourth handler was called for OneOf holding Fifth value"); }, x => { Assert.Equal(99.99m, x); });
    }
    
    [Fact]
    public void ImplicitConversion_FromFirst_ShouldWork() {
        OneOf<int, string, bool, double, decimal> result = 42;
        Assert.True(result.IsFirst);
        Assert.True(result.First(out var value));
        Assert.Equal(42, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromSecond_ShouldWork() {
        OneOf<int, string, bool, double, decimal> result = "hello";
        Assert.True(result.IsSecond);
        Assert.True(result.Second(out var value));
        Assert.Equal("hello", value);
    }
    
    [Fact]
    public void ImplicitConversion_FromThird_ShouldWork() {
        OneOf<int, string, bool, double, decimal> result = true;
        Assert.True(result.IsThird);
        Assert.True(result.Third(out var value));
        Assert.Equal(true, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromFourth_ShouldWork() {
        OneOf<int, string, bool, double, decimal> result = 3.14;
        Assert.True(result.IsFourth);
        Assert.True(result.Fourth(out var value));
        Assert.Equal(3.14, value);
    }
    
    [Fact]
    public void ImplicitConversion_FromFifth_ShouldWork() {
        OneOf<int, string, bool, double, decimal> result = 99.99m;
        Assert.True(result.IsFifth);
        Assert.True(result.Fifth(out var value));
        Assert.Equal(99.99m, value);
    }
    
}
