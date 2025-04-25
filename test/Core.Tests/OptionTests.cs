namespace Oleexo.UnambitiousFx.Core.Tests;

public sealed class OptionTests {
    [Fact]
    public void GivenASomeOption_WhenCallingSome_ThenReturnsTrue() {
        var option = Option<int>.Some(42);

        var b = option.Some(out _);

        Assert.True(b);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingSome_ThenReturnsTheValue() {
        var option = Option<int>.Some(42);

        if (option.Some(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Some branch should be reached when testing a Some option");
        }
    }

    [Fact]
    public void GivenANoneOption_WhenCallingSome_ThenReturnsFalse() {
        var option = Option<int>.None();

        var b = option.Some(out _);

        Assert.False(b);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingIfSome_ThenCallsTheAction() {
        var option = Option<int>.Some(42);

        var called = false;
        option.IfSome(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingIfSome_ThenDoesNotCallTheAction() {
        var option = Option<int>.None();

        var called = false;
        option.IfSome(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingIfNone_ThenDoesNotCallTheAction() {
        var option = Option<int>.Some(42);

        var called = false;
        option.IfNone(() => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingIfNone_ThenCallsTheAction() {
        var option = Option<int>.None();

        var called = false;
        option.IfNone(() => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenASomeOption_IsSomeShouldBeTrue() {
        var option = Option<int>.Some(42);

        Assert.True(option.IsSome);
    }

    [Fact]
    public void GivenASomeOption_IsNoneShouldBeFalse() {
        var option = Option<int>.Some(42);

        Assert.False(option.IsNone);
    }

    [Fact]
    public void GivenANoneOption_IsSomeShouldBeFalse() {
        var option = Option<int>.None();

        Assert.False(option.IsSome);
    }

    [Fact]
    public void GivenANoneOption_IsNoneShouldBeTrue() {
        var option = Option<int>.None();

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task GivenASomeOption_WhenCallingAsyncIfSome_ThenCallsTheAction() {
        var option = Option<int>.Some(42);

        var called = false;
        await option.IfSome(_ => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.True(called);
    }

    [Fact]
    public async Task GivenANoneOption_WhenCallingAsyncIfSome_ThenDoesNotCallTheAction() {
        var option = Option<int>.None();

        var called = false;
        await option.IfSome(_ => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.False(called);
    }

    [Fact]
    public async Task GivenASomeOption_WhenCallingAsyncIfNone_ThenDoesNotCallTheAction() {
        var option = Option<int>.Some(42);

        var called = false;
        await option.IfNone(() => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.False(called);
    }

    [Fact]
    public async Task GivenANoneOption_WhenCallingAsyncIfNone_ThenCallsTheAction() {
        var option = Option<int>.None();

        var called = false;
        await option.IfNone(() => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.True(called);
    }

    [Fact]
    public void GivenASomeOption_CaseShouldReturnTheValue() {
        var option = Option<int>.Some(42);

        Assert.Equal(42, option.Case);
    }

    [Fact]
    public void GivenANoneOption_CaseShouldReturnNull() {
        var option = Option<int>.None();

        Assert.Null(option.Case);
    }
}
