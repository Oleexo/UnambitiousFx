using JetBrains.Annotations;
using UnambitiousFx.Core.Maybe;

namespace UnambitiousFx.Core.Tests.Maybe;

[TestSubject(typeof(Maybe<>))]
public sealed class MaybeTests {
    [Fact]
    public void GivenASomeOption_WhenCallingSome_ThenReturnsTrue() {
        var option = Maybe<int>.Some(42);

        var b = option.Some(out _);

        Assert.True(b);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingSome_ThenReturnsTheValue() {
        var option = Maybe<int>.Some(42);

        if (option.Some(out var value)) {
            Assert.Equal(42, value);
        }
        else {
            Assert.Fail("Some branch should be reached when testing a Some option");
        }
    }

    [Fact]
    public void GivenANoneOption_WhenCallingSome_ThenReturnsFalse() {
        var option = Maybe<int>.None();

        var b = option.Some(out _);

        Assert.False(b);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingIfSome_ThenCallsTheAction() {
        var option = Maybe<int>.Some(42);

        var called = false;
        option.IfSome(_ => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingIfSome_ThenDoesNotCallTheAction() {
        var option = Maybe<int>.None();

        var called = false;
        option.IfSome(_ => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingIfNone_ThenDoesNotCallTheAction() {
        var option = Maybe<int>.Some(42);

        var called = false;
        option.IfNone(() => called = true);

        Assert.False(called);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingIfNone_ThenCallsTheAction() {
        var option = Maybe<int>.None();

        var called = false;
        option.IfNone(() => called = true);

        Assert.True(called);
    }

    [Fact]
    public void GivenASomeOption_IsSomeShouldBeTrue() {
        var option = Maybe<int>.Some(42);

        Assert.True(option.IsSome);
    }

    [Fact]
    public void GivenASomeOption_IsNoneShouldBeFalse() {
        var option = Maybe<int>.Some(42);

        Assert.False(option.IsNone);
    }

    [Fact]
    public void GivenANoneOption_IsSomeShouldBeFalse() {
        var option = Maybe<int>.None();

        Assert.False(option.IsSome);
    }

    [Fact]
    public void GivenANoneOption_IsNoneShouldBeTrue() {
        var option = Maybe<int>.None();

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task GivenASomeOption_WhenCallingAsyncIfSome_ThenCallsTheAction() {
        var option = Maybe<int>.Some(42);

        var called = false;
        await option.IfSome(_ => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.True(called);
    }

    [Fact]
    public async Task GivenANoneOption_WhenCallingAsyncIfSome_ThenDoesNotCallTheAction() {
        var option = Maybe<int>.None();

        var called = false;
        await option.IfSome(_ => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.False(called);
    }

    [Fact]
    public async Task GivenASomeOption_WhenCallingAsyncIfNone_ThenDoesNotCallTheAction() {
        var option = Maybe<int>.Some(42);

        var called = false;
        await option.IfNone(() => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.False(called);
    }

    [Fact]
    public async Task GivenANoneOption_WhenCallingAsyncIfNone_ThenCallsTheAction() {
        var option = Maybe<int>.None();

        var called = false;
        await option.IfNone(() => {
            called = true;
            return ValueTask.CompletedTask;
        });

        Assert.True(called);
    }

    [Fact]
    public void GivenASomeOption_CaseShouldReturnTheValue() {
        var option = Maybe<int>.Some(42);

        Assert.Equal(42, option.Case);
    }

    [Fact]
    public void GivenANoneOption_CaseShouldReturnNull() {
        var option = Maybe<int>.None();

        Assert.Null(option.Case);
    }


    
    [Fact]
    public void GivenASomeOption_WhenCallingMatch_ThenCallsSomeAction() {
        var option = Maybe<int>.Some(42);
        var someCalled = false;

        option.Match(
            some: _ => someCalled = true,
            none: () => Assert.Fail("None action should not be called for Some option")
        );

        Assert.True(someCalled);
    }

    [Fact]
    public void GivenASomeOption_WhenCallingMatch_ThenDoesNotCallNoneAction() {
        var option = Maybe<int>.Some(42);
        var noneCalled = false;

        option.Match(
            some: _ => { },
            none: () => noneCalled = true
        );

        Assert.False(noneCalled);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingMatch_ThenCallsNoneAction() {
        var option = Maybe<int>.None();
        var noneCalled = false;

        option.Match(
            some: _ => Assert.Fail("Some action should not be called for None option"),
            none: () => noneCalled = true
        );

        Assert.True(noneCalled);
    }

    [Fact]
    public void GivenANoneOption_WhenCallingMatch_ThenDoesNotCallSomeAction() {
        var option = Maybe<int>.None();
        var someCalled = false;

        option.Match(
            some: _ => someCalled = true,
            none: () => { }
        );

        Assert.False(someCalled);
    }
    
    [Fact]
    public void GivenANoneOption_WhenCallingMatchWithReturn_ThenReturnsNoneValue() {
        var option = Maybe<int>.None();
        const string expectedValue = "none";

        var result = option.Match(
            some: _ => "some",
            none: () => expectedValue
        );

        Assert.Equal(expectedValue, result);
    }
}
