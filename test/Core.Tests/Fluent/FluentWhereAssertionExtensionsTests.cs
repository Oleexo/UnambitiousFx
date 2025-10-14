using UnambitiousFx.Core.Eithers;
using UnambitiousFx.Core.Options;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.XUnit.Fluent;

namespace UnambitiousFx.Core.Tests.Fluent;

public sealed class FluentWhereAssertionExtensionsTests {
    [Fact]
    public void ResultSuccess_Where_Chains() {
        Result.Success(50)
              .EnsureSuccess()
              .Where(v => v == 50)
              .Map(v => v + 1)
              .Where(v => v == 51)
              .And(v => Assert.Equal(51, v));
    }

    [Fact]
    public void ResultFailure_Where_Chains() {
        Result.Failure<int>(new Exception("boom"))
              .EnsureFailure()
              .Where(e => e.Message == "boom")
              .AndMessage("boom");
    }

    [Fact]
    public void OptionSome_Where_Chains() {
        Option.Some(5)
              .EnsureSome()
              .Where(v => v > 3)
              .Map(v => v * 2)
              .Where(v => v == 10)
              .And(v => Assert.Equal(10, v));
    }

    [Fact]
    public void EitherLeft_WhereLeft_Chains() {
        Either<int, string>.FromLeft(7)
                           .EnsureLeft()
                           .WhereLeft(l => l == 7)
                           .Map(l => l + 3)
                           .WhereLeft(l => l == 10)
                           .And(l => Assert.Equal(10, l));
    }

    [Fact]
    public void EitherRight_WhereRight_Chains() {
        Either<int, string>.FromRight("abc")
                           .EnsureRight()
                           .WhereRight(r => r.StartsWith("a"))
                           .Map(r => r + "!")
                           .WhereRight(r => r.EndsWith("!"))
                           .And(r => Assert.Equal("abc!", r));
    }

    [Fact]
    public async Task AsyncEnsureSuccess_WithWhere() {
        var assertion = await Task.FromResult(Result.Success(9))
                                  .EnsureSuccess();
        assertion.Where(v => v == 9)
                 .And(v => Assert.Equal(9, v));
    }
}
