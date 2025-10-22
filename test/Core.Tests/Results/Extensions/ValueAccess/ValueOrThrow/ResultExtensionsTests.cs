using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.ValueAccess;

namespace UnambitiousFx.Core.Tests.Results.Extensions.ValueAccess.ValueOrThrow;

public sealed class ResultExtensionsTests {
    [Fact]
    public void ValueOrThrow_Arity1_Success_ReturnsValue() {
        var r = Result.Success(42);
        var v = r.ValueOrThrow();
        Assert.Equal(42, v);
    }

    [Fact]
    public void ValueOrThrow_Arity1_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity1_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity1_Success_FactoryNotInvoked() {
        var r      = Result.Success(5);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity2_Success_ReturnsValue() {
        var r = Result.Success(1, 2);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
    }

    [Fact]
    public void ValueOrThrow_Arity2_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity2_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity2_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity3_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
    }

    [Fact]
    public void ValueOrThrow_Arity3_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity3_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity3_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity4_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3, 4);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
    }

    [Fact]
    public void ValueOrThrow_Arity4_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity4_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity4_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3, 4);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity5_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3, 4, 5);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
    }

    [Fact]
    public void ValueOrThrow_Arity5_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity5_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity5_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3, 4, 5);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity6_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
    }

    [Fact]
    public void ValueOrThrow_Arity6_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int, int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity6_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int, int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity6_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity7_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
    }

    [Fact]
    public void ValueOrThrow_Arity7_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity7_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int, int, int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity7_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }

    [Fact]
    public void ValueOrThrow_Arity8_Success_ReturnsValue() {
        var r = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var v = r.ValueOrThrow();
        Assert.Equal(1, v.Item1);
        Assert.Equal(2, v.Item2);
        Assert.Equal(3, v.Item3);
        Assert.Equal(4, v.Item4);
        Assert.Equal(5, v.Item5);
        Assert.Equal(6, v.Item6);
        Assert.Equal(7, v.Item7);
        Assert.Equal(8, v.Item8);
    }

    [Fact]
    public void ValueOrThrow_Arity8_Failure_ThrowsOriginal() {
        var ex     = new InvalidOperationException("boom");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var thrown = Assert.Throws<InvalidOperationException>(() => r.ValueOrThrow());
        Assert.Same(ex, thrown);
    }

    [Fact]
    public void ValueOrThrow_Arity8_Failure_FactoryTransforms() {
        var ex     = new ArgumentException("bad");
        var r      = Result.Failure<int, int, int, int, int, int, int, int>(ex);
        var custom = Assert.Throws<ApplicationException>(() => r.ValueOrThrow(e => new ApplicationException("wrapped", e)));
        Assert.IsType<ArgumentException>(custom.InnerException);
    }

    [Fact]
    public void ValueOrThrow_Arity8_Success_FactoryNotInvoked() {
        var r      = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var called = false;
        var _ = r.ValueOrThrow(e => {
            called = true;
            return new Exception();
        });
        Assert.False(called);
    }
}
