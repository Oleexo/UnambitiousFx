using JetBrains.Annotations;
using UnambitiousFx.Core.Results;
using UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks;

namespace UnambitiousFx.Core.Tests.Results.Extensions.SideEffects.TapBoth.ValueTasks;

[TestSubject(typeof(ResultTapBothExtensions))]
public sealed class ResultExtensionsTests {
    #region Arity 1

    [Fact]
    public async Task TapBothAsync_Arity1_Success_InvokesSuccess() {
        var r       = Result.Success(1);
        var success = 0;
        await r.TapBothAsync(_ => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity1_Success_DoesNotInvokeError() {
        var r     = Result.Success(1);
        var error = 0;
        await r.TapBothAsync(_ => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity1_Failure_InvokesError() {
        var r     = Result.Failure<int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync(_ => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity1_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync(_ => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity1_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(1));
        var success   = 0;
        await awaitable.TapBothAsync(_ => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity1_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int>>(Result.Success(1));
        var error     = 0;
        await awaitable.TapBothAsync(_ => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity1_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync(_ => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity1_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int>>(Result.Failure<int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync(_ => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 2

    [Fact]
    public async Task TapBothAsync_Arity2_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2);
        var success = 0;
        await r.TapBothAsync((_,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity2_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2);
        var error = 0;
        await r.TapBothAsync((_,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity2_Failure_InvokesError() {
        var r     = Result.Failure<int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity2_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity2_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity2_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Success(1, 2));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity2_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity2_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int>>(Result.Failure<int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 3

    [Fact]
    public async Task TapBothAsync_Arity3_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity3_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity3_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity3_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity3_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity3_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Success(1, 2, 3));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity3_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity3_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int>>(Result.Failure<int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 4

    [Fact]
    public async Task TapBothAsync_Arity4_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3, 4);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity4_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3, 4);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity4_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity4_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity4_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity4_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Success(1, 2, 3, 4));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity4_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity4_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int>>(Result.Failure<int, int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 5

    [Fact]
    public async Task TapBothAsync_Arity5_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3, 4, 5);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity5_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3, 4, 5);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity5_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity5_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity5_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity5_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity5_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity5_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int>>(Result.Failure<int, int, int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 6

    [Fact]
    public async Task TapBothAsync_Arity6_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3, 4, 5, 6);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity6_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3, 4, 5, 6);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity6_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity6_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int, int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity6_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity6_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity6_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity6_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 7

    [Fact]
    public async Task TapBothAsync_Arity7_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity7_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3, 4, 5, 6, 7);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity7_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity7_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int, int, int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity7_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity7_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity7_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity7_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion

    #region Arity 8

    [Fact]
    public async Task TapBothAsync_Arity8_Success_InvokesSuccess() {
        var r       = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Arity8_Success_DoesNotInvokeError() {
        var r     = Result.Success(1, 2, 3, 4, 5, 6, 7, 8);
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity8_Failure_InvokesError() {
        var r     = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var error = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Arity8_Failure_DoesNotInvokeSuccess() {
        var r       = Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom"));
        var success = 0;
        await r.TapBothAsync((_,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _,
                              _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity8_Success_InvokesSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(1, success);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity8_Success_DoesNotInvokeError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Success(1, 2, 3, 4, 5, 6, 7, 8));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(0, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity8_Failure_InvokesError() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom")));
        var error     = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => new ValueTask(), _ => {
            error++;
            return new ValueTask();
        });
        Assert.Equal(1, error);
    }

    [Fact]
    public async Task TapBothAsync_Awaitable_Arity8_Failure_DoesNotInvokeSuccess() {
        var awaitable = new ValueTask<Result<int, int, int, int, int, int, int, int>>(Result.Failure<int, int, int, int, int, int, int, int>(new Exception("boom")));
        var success   = 0;
        await awaitable.TapBothAsync((_,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _,
                                      _) => {
            success++;
            return new ValueTask();
        }, _ => new ValueTask());
        Assert.Equal(0, success);
    }

    #endregion
}
