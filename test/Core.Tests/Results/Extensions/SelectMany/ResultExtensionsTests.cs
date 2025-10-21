using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Tasks;
using ResultExtensions = UnambitiousFx.Core.Results.ValueTasks.ResultExtensions;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SelectMany;

public sealed class ResultExtensionsTests {
    [Fact]
    public void SelectMany_Arity1_Success_ProjectsValue() {
        var r = Result.Success(5)
                      .SelectMany(x => Result.Success(x * 2), (x,
                                                               y) => x + y); // 5 + 10 = 15

        r.Ok(out var value);
        Assert.Equal(15, value);
    }

    [Fact]
    public void SelectMany_Arity1_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int>(error)
                      .SelectMany(x => Result.Success(x * 2), (x,
                                                               y) => x + y);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity1_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(3)
                      .SelectMany(_ => Result.Failure<int>(innerError), (x,
                                                                         y) => x + y);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity2_Success_ProjectsValue() {
        var r = Result.Success(2, 3)
                      .SelectMany((x, y) => Result.Success(x + y), (x, y, z) => x + y + z); // 2 + 3 + 5 = 10

        r.Ok(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public void SelectMany_Arity2_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int>(error)
                      .SelectMany((x, y) => Result.Success(x + y), (x, y, z) => x + y + z);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity2_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(2, 3)
                      .SelectMany((_, _) => Result.Failure<int>(innerError), (x, y, z) => x + y + z);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity3_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3)
                      .SelectMany((a, b, c) => Result.Success(a + b + c), (a, b, c, s) => a + b + c + s); // (1+2+3) + 6 = 12

        r.Ok(out var value);
        Assert.Equal(12, value);
    }

    [Fact]
    public void SelectMany_Arity3_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int>(error)
                      .SelectMany((a, b, c) => Result.Success(a + b + c), (a, b, c, s) => a + b + c + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity3_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3)
                      .SelectMany((_, _, _) => Result.Failure<int>(innerError), (a, b, c, s) => a + b + c + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity4_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4)
                      .SelectMany((a, b, c, d) => Result.Success(a + b + c + d), (a, b, c, d, s) => a + b + c + d + s); // 10 + 10 = 20

        r.Ok(out var value);
        Assert.Equal(20, value);
    }

    [Fact]
    public void SelectMany_Arity4_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int>(error)
                      .SelectMany((a, b, c, d) => Result.Success(a + b + c + d), (a, b, c, d, s) => a + b + c + d + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity4_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3, 4)
                      .SelectMany((_, _, _, _) => Result.Failure<int>(innerError), (a, b, c, d, s) => a + b + c + d + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity5_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .SelectMany((a, b, c, d, e) => Result.Success(a + b + c + d + e), (a, b, c, d, e, s) => a + b + c + d + e + s); // 15 + 15 = 30

        r.Ok(out var value);
        Assert.Equal(30, value);
    }

    [Fact]
    public void SelectMany_Arity5_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int>(error)
                      .SelectMany((a, b, c, d, e) => Result.Success(a + b + c + d + e), (a, b, c, d, e, s) => a + b + c + d + e + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity5_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3, 4, 5)
                      .SelectMany((_, _, _, _, _) => Result.Failure<int>(innerError), (a, b, c, d, e, s) => a + b + c + d + e + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity6_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6)
                      .SelectMany((a, b, c, d, e, f) => Result.Success(a + b + c + d + e + f), (a, b, c, d, e, f, s) => a + b + c + d + e + f + s); // 21 + 21 = 42

        r.Ok(out var value);
        Assert.Equal(42, value);
    }

    [Fact]
    public void SelectMany_Arity6_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int>(error)
                      .SelectMany((a, b, c, d, e, f) => Result.Success(a + b + c + d + e + f), (a, b, c, d, e, f, s) => a + b + c + d + e + f + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity6_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3, 4, 5, 6)
                      .SelectMany((_, _, _, _, _, _) => Result.Failure<int>(innerError), (a, b, c, d, e, f, s) => a + b + c + d + e + f + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity7_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7)
                      .SelectMany((a, b, c, d, e, f, g) => Result.Success(a + b + c + d + e + f + g), (a, b, c, d, e, f, g, s) => a + b + c + d + e + f + g + s); // 28 + 28 = 56

        r.Ok(out var value);
        Assert.Equal(56, value);
    }

    [Fact]
    public void SelectMany_Arity7_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int, int>(error)
                      .SelectMany((a, b, c, d, e, f, g) => Result.Success(a + b + c + d + e + f + g), (a, b, c, d, e, f, g, s) => a + b + c + d + e + f + g + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity7_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7)
                      .SelectMany((_, _, _, _, _, _, _) => Result.Failure<int>(innerError), (a, b, c, d, e, f, g, s) => a + b + c + d + e + f + g + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity8_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                      .SelectMany((a, b, c, d, e, f, g, h) => Result.Success(a + b + c + d + e + f + g + h), (a, b, c, d, e, f, g, h, s) => a + b + c + d + e + f + g + h + s); // 36 + 36 = 72

        r.Ok(out var value);
        Assert.Equal(72, value);
    }

    [Fact]
    public void SelectMany_Arity8_FirstFailure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(error)
                      .SelectMany((a, b, c, d, e, f, g, h) => Result.Success(a + b + c + d + e + f + g + h), (a, b, c, d, e, f, g, h, s) => a + b + c + d + e + f + g + h + s);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void SelectMany_Arity8_SecondFailure_IsFaulted() {
        var innerError = new Exception("inner");
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                      .SelectMany((_, _, _, _, _, _, _, _) => Result.Failure<int>(innerError), (a, b, c, d, e, f, g, h, s) => a + b + c + d + e + f + g + h + s);

        Assert.True(r.IsFaulted);
    }
}
