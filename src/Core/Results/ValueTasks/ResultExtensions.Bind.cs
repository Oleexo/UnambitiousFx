namespace UnambitiousFx.Core.Results.ValueTasks;

public static class ResultExtensions {
    public static ValueTask<Result> Bind(this Result        result,
                                    Func<ValueTask<Result>> bind) {
        return result.IsFaulted
                   ? new ValueTask<Result>(result)
                   : bind();
    }

    public static async ValueTask<Result> Bind(this ValueTask<Result>  awaitableResult,
                                          Func<ValueTask<Result>> bind) {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1>> Bind<TOut1>(this Result<TOut1>        result,
                                                  Func<ValueTask<Result<TOut1>>> bind)
        where TOut1 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1>> Bind<TOut1>(this ValueTask<Result<TOut1>>  awaitableResult,
                                                        Func<ValueTask<Result<TOut1>>> bind)
        where TOut1 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2>> Bind<TOut1, TOut2>(this Result<TOut1, TOut2>        result,
                                                                Func<ValueTask<Result<TOut1, TOut2>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2>> Bind<TOut1, TOut2>(this ValueTask<Result<TOut1, TOut2>>  awaitableResult,
                                                                      Func<ValueTask<Result<TOut1, TOut2>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3>> Bind<TOut1, TOut2, TOut3>(this Result<TOut1, TOut2, TOut3>        result,
                                                                              Func<ValueTask<Result<TOut1, TOut2, TOut3>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3>> Bind<TOut1, TOut2, TOut3>(this ValueTask<Result<TOut1, TOut2, TOut3>>  awaitableResult,
                                                                                    Func<ValueTask<Result<TOut1, TOut2, TOut3>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> Bind<TOut1, TOut2, TOut3, TOut4>(this Result<TOut1, TOut2, TOut3, TOut4>        result,
                                                                                            Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> Bind<TOut1, TOut2, TOut3, TOut4>(this ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>  awaitableResult,
                                                                                                  Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TOut1, TOut2, TOut3, TOut4, TOut5>        result,
                                                                                                          Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5>(
        this ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>  awaitableResult,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>        result,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>  awaitableResult,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>        result,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(result)
                   : bind();
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>  awaitableResult,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>        result,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.IsFaulted
                   ? new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(result)
                   : bind();
    }
    
    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>  awaitableResult,
        Func<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        if (result.IsFaulted) {
            return result;
        }

        return await bind();
    }
}
