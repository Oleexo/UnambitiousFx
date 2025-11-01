using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ErrorHandling;
using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ErrorHandling.MapErrors;

[TestSubject(typeof(ResultMapErrorsExtensions))]
public sealed class ResultExtensionsTests
{
    #region Arity 0

    [Fact]
    public void MapErrors_Arity0_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity0_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success();
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity0_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity0_WhenFailure_ShouldInvokeMapFunctionWithAllErrors()
    {
        // Arrange
        var error1 = new ExceptionalError(new Exception("error1"));
        var error2 = new ExceptionalError(new Exception("error2"));
        var result = Result.Failure([error1, error2]);
        IEnumerable<IError>? receivedErrors = null;

        // Act
        result.MapErrors(errs =>
        {
            receivedErrors = errs.ToList();
            return new ExceptionalError(new Exception("mapped"));
        });

        // Assert
        Assert.NotNull(receivedErrors);
        Assert.Equal(2, receivedErrors.Count());
        Assert.Contains(error1, receivedErrors);
        Assert.Contains(error2, receivedErrors);
    }

    [Fact]
    public void MapErrors_Arity0_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            () => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.IsType<ExceptionalError>(error);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    [Fact]
    public void MapErrors_Arity0_WhenFailureWithMultipleErrors_ShouldConsolidateIntoSingleError()
    {
        // Arrange
        var error1 = new ExceptionalError(new Exception("error1"));
        var error2 = new ExceptionalError(new Exception("error2"));
        var result = Result.Failure([error1, error2]);

        // Act
        var mapped = result.MapErrors(errs =>
            new ExceptionalError(new AggregateException("consolidated", errs.Select(e => e.Exception!)))
        );

        // Assert
        mapped.Match(
            () => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.IsType<AggregateException>(error.Exception);
                var aggEx = (AggregateException)error.Exception!;
                Assert.Equal(2, aggEx.InnerExceptions.Count);
            }
        );
    }

    #endregion

    #region Arity 1

    [Fact]
    public void MapErrors_Arity1_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            value => Assert.Equal(42, value),
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity1_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(42);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity1_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity1_WhenFailure_ShouldInvokeMapFunctionWithAllErrors()
    {
        // Arrange
        var error1 = new ExceptionalError(new Exception("error1"));
        var error2 = new ExceptionalError(new Exception("error2"));
        var result = Result.Failure<int>([error1, error2]);
        IEnumerable<IError>? receivedErrors = null;

        // Act
        result.MapErrors(errs =>
        {
            receivedErrors = errs.ToList();
            return new ExceptionalError(new Exception("mapped"));
        });

        // Assert
        Assert.NotNull(receivedErrors);
        Assert.Equal(2, receivedErrors.Count());
        Assert.Contains(error1, receivedErrors);
        Assert.Contains(error2, receivedErrors);
    }

    [Fact]
    public void MapErrors_Arity1_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            _ => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.IsType<ExceptionalError>(error);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    [Fact]
    public void MapErrors_Arity1_WhenFailureWithMultipleErrors_ShouldConsolidateIntoSingleError()
    {
        // Arrange
        var error1 = new ExceptionalError(new Exception("error1"));
        var error2 = new ExceptionalError(new Exception("error2"));
        var result = Result.Failure<int>([error1, error2]);

        // Act
        var mapped = result.MapErrors(errs =>
            new ExceptionalError(new AggregateException("consolidated", errs.Select(e => e.Exception!)))
        );

        // Assert
        mapped.Match(
            _ => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.IsType<AggregateException>(error.Exception);
                var aggEx = (AggregateException)error.Exception!;
                Assert.Equal(2, aggEx.InnerExceptions.Count);
            }
        );
    }

    #endregion

    #region Arity 2

    [Fact]
    public void MapErrors_Arity2_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two");

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity2_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two");
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity2_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity2_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 3

    [Fact]
    public void MapErrors_Arity3_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity3_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity3_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity3_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 4

    [Fact]
    public void MapErrors_Arity4_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c, d) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
                Assert.True(d);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity4_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity4_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double, bool>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity4_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double, bool>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___, ____) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 5

    [Fact]
    public void MapErrors_Arity5_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c, d, e) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
                Assert.True(d);
                Assert.Equal(5L, e);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity5_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity5_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double, bool, long>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity5_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double, bool, long>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___, ____, _____) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 6

    [Fact]
    public void MapErrors_Arity6_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c, d, e, f) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
                Assert.True(d);
                Assert.Equal(5L, e);
                Assert.Equal(6m, f);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity6_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity6_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double, bool, long, decimal>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity6_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double, bool, long, decimal>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___, ____, _____, ______) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 7

    [Fact]
    public void MapErrors_Arity7_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m, 7f);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c, d, e, f, g) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
                Assert.True(d);
                Assert.Equal(5L, e);
                Assert.Equal(6m, f);
                Assert.Equal(7f, g);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity7_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m, 7f);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity7_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double, bool, long, decimal, float>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity7_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double, bool, long, decimal, float>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___, ____, _____, ______, _______) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion

    #region Arity 8

    [Fact]
    public void MapErrors_Arity8_WhenSuccess_ShouldReturnUnchangedResult()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m, 7f, (byte)8);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("should not be called")));

        // Assert
        Assert.True(mapped.IsSuccess);
        mapped.Match(
            (a, b, c, d, e, f, g, h) =>
            {
                Assert.Equal(1, a);
                Assert.Equal("two", b);
                Assert.Equal(3.0, c);
                Assert.True(d);
                Assert.Equal(5L, e);
                Assert.Equal(6m, f);
                Assert.Equal(7f, g);
                Assert.Equal((byte)8, h);
            },
            _ => Assert.Fail("Expected success")
        );
    }

    [Fact]
    public void MapErrors_Arity8_WhenSuccess_ShouldNotInvokeMapFunction()
    {
        // Arrange
        var result = Result.Success(1, "two", 3.0, true, 5L, 6m, 7f, (byte)8);
        var mapFunctionCalled = false;

        // Act
        result.MapErrors(_ =>
        {
            mapFunctionCalled = true;
            return new ExceptionalError(new Exception("should not be called"));
        });

        // Assert
        Assert.False(mapFunctionCalled);
    }

    [Fact]
    public void MapErrors_Arity8_WhenFailure_ShouldReturnFailureResult()
    {
        // Arrange
        var originalError = new ExceptionalError(new Exception("original"));
        var result = Result.Failure<int, string, double, bool, long, decimal, float, byte>(originalError);

        // Act
        var mapped = result.MapErrors(_ => new ExceptionalError(new Exception("mapped")));

        // Assert
        Assert.False(mapped.IsSuccess);
    }

    [Fact]
    public void MapErrors_Arity8_WhenFailure_ShouldReturnResultWithMappedError()
    {
        // Arrange
        var originalException = new InvalidOperationException("original");
        var result = Result.Failure<int, string, double, bool, long, decimal, float, byte>(originalException);
        var mappedException = new ArgumentException("mapped");

        // Act
        var mapped = result.MapErrors(errs => new ExceptionalError(mappedException));

        // Assert
        mapped.Match(
            (_, __, ___, ____, _____, ______, _______, ________) => Assert.Fail("Expected failure"),
            errors =>
            {
                var error = Assert.Single(errors);
                Assert.Equal(mappedException, error.Exception);
            }
        );
    }

    #endregion
}
