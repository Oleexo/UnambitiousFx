using System.Globalization;
using UnambitiousFx.Core.OneOf;

namespace UnambitiousFx.Core.Tests.OneOf;

public sealed class OneOfAritiesTests {
    [Fact]
    public void OneOf2_FromFirst_ShouldSetIsFirst() {
        var o = OneOf<int, string>.FromFirst(42);
        Assert.True(o.IsFirst);
        Assert.False(o.IsSecond);
        var result = o.Match(i => i.ToString(), s => s);
        Assert.Equal("42", result);
        Assert.True(o.First(out var first));
        Assert.Equal(42, first);
        Assert.False(o.Second(out _));
    }

    [Fact]
    public void OneOf2_FromSecond_ShouldSetIsSecond() {
        var o = OneOf<int, string>.FromSecond("hi");
        Assert.False(o.IsFirst);
        Assert.True(o.IsSecond);
        var result = o.Match(i => i.ToString(), s => s + "!");
        Assert.Equal("hi!", result);
        Assert.True(o.Second(out var second));
        Assert.Equal("hi", second);
        Assert.False(o.First(out _));
    }

    [Fact]
    public void OneOf2_ImplicitConversion_First() {
        OneOf<int, string> o = 7; // implicit
        Assert.True(o.IsFirst);
        Assert.Equal("7", o.Match(i => i.ToString(), s => s));
    }

    [Fact]
    public void OneOf2_ImplicitConversion_Second() {
        OneOf<int, string> o = "val"; // implicit
        Assert.True(o.IsSecond);
        Assert.Equal("val", o.Match(i => i.ToString(), s => s));
    }

    [Fact]
    public void OneOf3_AllFactories_ShouldSelectCorrectBranch() {
        var a = OneOf<int, string, bool>.FromFirst(1);
        var b = OneOf<int, string, bool>.FromSecond("x");
        var c = OneOf<int, string, bool>.FromThird(true);
        Assert.True(a.IsFirst);
        Assert.True(b.IsSecond);
        Assert.True(c.IsThird);
        Assert.Equal("one",   a.Match(_ => "one", _ => "two", _ => "three"));
        Assert.Equal("two",   b.Match(_ => "one", _ => "two", _ => "three"));
        Assert.Equal("three", c.Match(_ => "one", _ => "two", _ => "three"));
    }

    [Fact]
    public void OneOf4_AllFactories() {
        var d = OneOf<int, string, bool, decimal>.FromFourth(9.5m);
        Assert.True(d.IsFourth);
        var res = d.Match(_ => "1", _ => "2", _ => "3", m => m.ToString(CultureInfo.InvariantCulture));
        Assert.Equal("9.5", res);
    }

    [Fact]
    public void OneOf5_AllFactories() {
        var e = OneOf<int, string, bool, decimal, long>.FromFifth(123L);
        Assert.True(e.IsFifth);
        var res = e.Match(_ => 1, _ => 2, _ => 3, _ => 4, l => (int)l / 123);
        Assert.Equal(1, res);
    }

    [Fact]
    public void OneOf6_AllFactories() {
        var f = OneOf<int, string, bool, decimal, long, double>.FromSixth(3.14);
        Assert.True(f.IsSixth);
        var res = f.Match(_ => 1, _ => 2, _ => 3, _ => 4, _ => 5, d => d > 3
                                                                           ? 6
                                                                           : 0);
        Assert.Equal(6, res);
    }

    [Fact]
    public void OneOf7_AllFactories() {
        var g = OneOf<int, string, bool, decimal, long, double, char>.FromSeventh('Z');
        Assert.True(g.IsSeventh);
        var res = g.Match(_ => 1, _ => 2, _ => 3, _ => 4, _ => 5, _ => 6, c => c == 'Z'
                                                                                   ? 7
                                                                                   : 0);
        Assert.Equal(7, res);
    }

    [Fact]
    public void OneOf8_AllFactories() {
        var h = OneOf<int, string, bool, decimal, long, double, char, Guid>.FromEighth(Guid.Parse("11111111-1111-1111-1111-111111111111"));
        Assert.True(h.IsEighth);
        var res = h.Match(_ => 1, _ => 2, _ => 3, _ => 4, _ => 5, _ => 6, _ => 7, g => g.ToString()
                                                                                        .StartsWith("1111")
                                                                                           ? 8
                                                                                           : 0);
        Assert.Equal(8, res);
    }

    [Fact]
    public void OneOf8_OutExtract_ShouldReturnCorrectFlag() {
        var h = OneOf<int, string, bool, decimal, long, double, char, Guid>.FromThird(true);
        Assert.True(h.Third(out var third));
        Assert.True(third);
        Assert.False(h.Fourth(out _));
        Assert.False(h.Eighth(out _));
    }
}
