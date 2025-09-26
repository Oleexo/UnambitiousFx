using JetBrains.Annotations;
using UnambitiousFx.Core.Results;

namespace UnambitiousFx.Core.Tests.Results;

[TestSubject(typeof(Result))]
public sealed class ResultMapErrorExtensionsTests
{
    // Arity 0
    [Fact]
    public void MapError_Arity0_Success_NoChange()
    {
        var r = Result.Success();
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        Assert.True(mapped.Ok(out _));
        Assert.Equal(0, called);
    }

    [Fact]
    public void MapError_Arity0_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 1
    [Fact]
    public void MapError_Arity1_Success_NoChange()
    {
        var r = Result.Success(1);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out var value, out _))
        {
            Assert.Equal(1, value);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity1_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out int _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 2
    [Fact]
    public void MapError_Arity2_Success_NoChange()
    {
        var r = Result.Success(1, 2);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int) values, out _))
        {
            var (a, b) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity2_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 3
    [Fact]
    public void MapError_Arity3_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int) values, out _))
        {
            var (a, b, c) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity3_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 4
    [Fact]
    public void MapError_Arity4_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3, 4);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int, int) values, out _))
        {
            var (a, b, c, d) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity4_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 5
    [Fact]
    public void MapError_Arity5_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3, 4, 5);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int, int, int) values, out _))
        {
            var (a, b, c, d, e) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity5_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 6
    [Fact]
    public void MapError_Arity6_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int, int, int, int) values, out _))
        {
            var (a, b, c, d, e, f) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity6_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int, int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 7
    [Fact]
    public void MapError_Arity7_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int, int, int, int, int) values, out _))
        {
            var (a, b, c, d, e, f, g) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(7, g);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity7_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int, int, int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }

    // Arity 8
    [Fact]
    public void MapError_Arity8_Success_NoChange()
    {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = 0;

        var mapped = r.MapError(_ => { called++; return new Exception("wrapped"); });

        if (mapped.Ok(out (int, int, int, int, int, int, int, int) values, out _))
        {
            var (a, b, c, d, e, f, g, h) = values;
            Assert.Equal(1, a);
            Assert.Equal(2, b);
            Assert.Equal(3, c);
            Assert.Equal(4, d);
            Assert.Equal(5, e);
            Assert.Equal(6, f);
            Assert.Equal(7, g);
            Assert.Equal(8, h);
            Assert.Equal(0, called);
        }
        else
        {
            Assert.Fail("Expected success");
        }
    }

    [Fact]
    public void MapError_Arity8_Failure_TransformsError()
    {
        var ex = new Exception("boom");
        var r = Result.Failure<int, int, int, int, int, int, int, int>(ex);

        var mapped = r.MapError(e => new InvalidOperationException("wrapped", e));

        if (!mapped.Ok(out (int, int, int, int, int, int, int, int) _, out var err))
        {
            Assert.IsType<InvalidOperationException>(err);
            Assert.Equal(ex, err.InnerException);
        }
        else
        {
            Assert.Fail("Expected failure");
        }
    }
}
