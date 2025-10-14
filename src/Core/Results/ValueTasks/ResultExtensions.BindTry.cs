namespace UnambitiousFx.Core.Results.ValueTasks;

public static partial class ResultExtensions {
    // Single input value -> Result<TOut1>
    public static ValueTask<Result<TOut1>> BindTryAsync<TValue1, TOut1>(this Result<TValue1> result,
                                                                        Func<TValue1, ValueTask<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull {
        return result.Match<ValueTask<Result<TOut1>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1>(ex); }
        }, e => new ValueTask<Result<TOut1>>(Result.Failure<TOut1>(e)));
    }

    public static async ValueTask<Result<TOut1>> BindTryAsync<TValue1, TOut1>(this ValueTask<Result<TValue1>> awaitableResult,
                                                                              Func<TValue1, ValueTask<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1>(ex); }
        }, e => new ValueTask<Result<TOut1>>(Result.Failure<TOut1>(e)));
    }

    // 1 input -> 2 outputs
    public static ValueTask<Result<TOut1, TOut2>> BindTryAsync<TValue1, TOut1, TOut2>(this Result<TValue1> result,
                                                                                      Func<TValue1, ValueTask<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull where TOut2 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1, TOut2>(ex); }
        }, e => new ValueTask<Result<TOut1, TOut2>>(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2>> BindTryAsync<TValue1, TOut1, TOut2>(this ValueTask<Result<TValue1>> awaitableResult,
                                                                                            Func<TValue1, ValueTask<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1, TOut2>(ex); }
        }, e => new ValueTask<Result<TOut1, TOut2>>(Result.Failure<TOut1, TOut2>(e)));
    }

    // 1 input -> 3 outputs
    public static ValueTask<Result<TOut1, TOut2, TOut3>> BindTryAsync<TValue1, TOut1, TOut2, TOut3>(this Result<TValue1> result,
                                                                                                    Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3>(ex); }
        }, e => new ValueTask<Result<TOut1, TOut2, TOut3>>(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3>> BindTryAsync<TValue1, TOut1, TOut2, TOut3>(this ValueTask<Result<TValue1>> awaitableResult,
                                                                                                          Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3>>>(async v => {
            try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3>(ex); }
        }, e => new ValueTask<Result<TOut1, TOut2, TOut3>>(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1> result,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>>(async v => { try { return await bind(v); } catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4>(this ValueTask<Result<TValue1>> awaitableResult,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1> result,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this ValueTask<Result<TValue1>> awaitableResult,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1> result,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this ValueTask<Result<TValue1>> awaitableResult,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result<TValue1> result,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull where TOut7 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this ValueTask<Result<TValue1>> awaitableResult,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result<TValue1> result,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull where TOut7 : notnull where TOut8 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e)));
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindTryAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this ValueTask<Result<TValue1>> awaitableResult,
        Func<TValue1, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull where TOut1 : notnull where TOut2 : notnull where TOut3 : notnull where TOut4 : notnull where TOut5 : notnull where TOut6 : notnull where TOut7 : notnull where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async v => { try { return await bind(v);} catch (Exception ex) { return Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(ex);} },
            e => new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e)));
    }

    // 2 input source -> 1 output
    public static ValueTask<Result<TOut1>> BindTryAsync<TValue1, TValue2, TOut1>(this Result<TValue1, TValue2> result,
        Func<TValue1, TValue2, ValueTask<Result<TOut1>>> bind)
        where TValue1 : notnull where TValue2 : notnull where TOut1 : notnull {
        return result.Match<ValueTask<Result<TOut1>>>(async (v1,v2) => { try { return await bind(v1,v2);} catch (Exception ex){ return Result.Failure<TOut1>(ex);} },
            e => new ValueTask<Result<TOut1>>(Result.Failure<TOut1>(e)));
    }
}
