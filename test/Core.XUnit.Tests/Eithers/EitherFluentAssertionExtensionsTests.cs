using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.XUnit.Fluent;

namespace UnambitiousFx.Core.XUnit.Tests.Eithers;

public sealed class EitherFluentAssertionExtensionsTests {
    [Fact]
    public void EnsureLeft_Chaining() {
        Either<int,string>.FromLeft(10)
            .EnsureLeft()
            .And(l => Assert.Equal(10, l))
            .Map(l => l + 2)
            .And(l => Assert.Equal(12, l));
    }

    [Fact]
    public void EnsureRight_Chaining() {
        Either<int,string>.FromRight("abc")
            .EnsureRight()
            .And(r => Assert.Equal("abc", r))
            .Map(r => r + "!")
            .And(r => Assert.Equal("abc!", r));
    }

    [Fact]
    public async Task Async_Task_EnsureLeft() {
        var assertion = await Task.FromResult(Either<int,string>.FromLeft(5)).EnsureLeft();
        assertion.And(v => Assert.Equal(5, v));
    }

    [Fact]
    public async Task Async_ValueTask_EnsureRight() {
        var assertion = await new ValueTask<Either<int,string>>(Either<int,string>.FromRight("x")).EnsureRight();
        assertion.And(r => Assert.Equal("x", r));
    }
}

