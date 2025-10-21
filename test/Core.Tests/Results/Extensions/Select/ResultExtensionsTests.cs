using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results.Extensions.Select;

public sealed class ResultExtensionsTests {
    [Fact]
    public void Select_Arity1_Success_ProjectsValue() {
        var r = Result.Success(5)
                      .Select(x => x * 2); // 10

        r.Ok(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public void Select_Arity1_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int>(error)
                      .Select(x => x * 2);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity2_Success_ProjectsValue() {
        var r = Result.Success(2, 3)
                      .Select((x, y) => x + y); // 5

        r.Ok(out var value);
        Assert.Equal(5, value);
    }

    [Fact]
    public void Select_Arity2_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int>(error)
                      .Select((x, y) => x + y);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity3_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3)
                      .Select((x, y, z) => x + y + z); // 6

        r.Ok(out var value);
        Assert.Equal(6, value);
    }

    [Fact]
    public void Select_Arity3_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int>(error)
                      .Select((x, y, z) => x + y + z);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity4_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4)
                      .Select((a, b, c, d) => a + b + c + d); // 10

        r.Ok(out var value);
        Assert.Equal(10, value);
    }

    [Fact]
    public void Select_Arity4_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int>(error)
                      .Select((a, b, c, d) => a + b + c + d);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity5_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5)
                      .Select((a, b, c, d, e) => a + b + c + d + e); // 15

        r.Ok(out var value);
        Assert.Equal(15, value);
    }

    [Fact]
    public void Select_Arity5_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int>(error)
                      .Select((a, b, c, d, e) => a + b + c + d + e);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity6_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6)
                      .Select((a, b, c, d, e, f) => a + b + c + d + e + f); // 21

        r.Ok(out var value);
        Assert.Equal(21, value);
    }

    [Fact]
    public void Select_Arity6_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int>(error)
                      .Select((a, b, c, d, e, f) => a + b + c + d + e + f);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity7_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7)
                      .Select((a, b, c, d, e, f, g) => a + b + c + d + e + f + g); // 28

        r.Ok(out var value);
        Assert.Equal(28, value);
    }

    [Fact]
    public void Select_Arity7_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int, int>(error)
                      .Select((a, b, c, d, e, f, g) => a + b + c + d + e + f + g);

        Assert.True(r.IsFaulted);
    }

    [Fact]
    public void Select_Arity8_Success_ProjectsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8)
                      .Select((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h); // 36

        r.Ok(out var value);
        Assert.Equal(36, value);
    }

    [Fact]
    public void Select_Arity8_Failure_IsFaulted() {
        var error = new InvalidOperationException("fail");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(error)
                      .Select((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

        Assert.True(r.IsFaulted);
    }
}
