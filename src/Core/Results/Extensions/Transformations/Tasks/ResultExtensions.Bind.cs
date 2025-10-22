namespace UnambitiousFx.Core.Results.Extensions.Transformations.Tasks;

public static partial class ResultExtensions {
    public static Task<Result<TOut1>> BindAsync<TValue1, TOut1>(this Result<TValue1>               result,
                                                                Func<TValue1, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async value1 => await bind(value1), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TOut1>(this Task<Result<TValue1>>         awaitableResult,
                                                                      Func<TValue1, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async value1 => await bind(value1), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TOut1, TOut2>(this Result<TValue1>                      result,
                                                                              Func<TValue1, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async value1 => await bind(value1), e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TOut1, TOut2>(this Task<Result<TValue1>>                awaitableResult,
                                                                                    Func<TValue1, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async value1 => await bind(value1), e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TOut1, TOut2, TOut3>(this Result<TValue1>                             result,
                                                                                            Func<TValue1, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async value1 => await bind(value1),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TOut1, TOut2, TOut3>(this Task<Result<TValue1>>                       awaitableResult,
                                                                                                  Func<TValue1, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async value1 => await bind(value1),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1>                                    result,
                                                                                                          Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async value1 => await bind(value1),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4>(this Task<Result<TValue1>> awaitableResult,
                                                                                                                Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async value1 => await bind(value1),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1> result,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>                                                                       bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async value1 => await bind(value1),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this Task<Result<TValue1>> awaitableResult,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>                                                                                   bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async value1 => await bind(value1),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1> result,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>                                                                              bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async value1 => await bind(value1),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1>>                                            awaitableResult,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async value1 => await bind(value1),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result<TValue1> result,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>                                                                                     bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async value1 => await bind(value1),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Task<Result<TValue1>>                                                   awaitableResult,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async value1 => await bind(value1),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1>                                                                result,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async value1 => await bind(value1),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1>>                                                          awaitableResult,
        Func<TValue1, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async value1 => await bind(value1),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TOut1>(this Result<TValue1, TValue2>               result,
                                                                         Func<TValue1, TValue2, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2) => await bind(value1, value2), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TOut1>(this Task<Result<TValue1, TValue2>>         awaitableResult,
                                                                               Func<TValue1, TValue2, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2) => await bind(value1, value2), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2>                      result,
                                                                                       Func<TValue1, TValue2, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2) => await bind(value1, value2), e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TOut1, TOut2>(this Task<Result<TValue1, TValue2>>                awaitableResult,
                                                                                             Func<TValue1, TValue2, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2) => await bind(value1, value2),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2>                             result,
                                                                                                     Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2) => await bind(value1, value2),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3>(this Task<Result<TValue1, TValue2>> awaitableResult,
                                                                                                           Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2) => await bind(value1, value2),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2> result,
                                                                                                                   Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4>>>
                                                                                                                       bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2) => await bind(value1, value2),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4>(this Task<Result<TValue1, TValue2>> awaitableResult,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4>>>                                                                                     bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2) => await bind(value1, value2),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2> result,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>                                                                                bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2) => await bind(value1, value2),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2>>                                     awaitableResult,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2) => await bind(value1, value2),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1, TValue2> result,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>                                                                                       bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2) => await bind(value1, value2),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2>>                                            awaitableResult,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2) => await bind(value1, value2),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2>                                                         result,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2) => await bind(value1, value2),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2>>                                                   awaitableResult,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2) => await bind(value1, value2),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2>                                                                result,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2) => await bind(value1, value2),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2>>                                                          awaitableResult,
        Func<TValue1, TValue2, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2) => await bind(value1, value2),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TOut1>(this Result<TValue1, TValue2, TValue3>               result,
                                                                                  Func<TValue1, TValue2, TValue3, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3) => await bind(value1, value2, value3), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TOut1>(this Task<Result<TValue1, TValue2, TValue3>>         awaitableResult,
                                                                                        Func<TValue1, TValue2, TValue3, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3) => await bind(value1, value2, value3), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3>                      result,
                                                                                                Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3) => await bind(value1, value2, value3),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2>(this Task<Result<TValue1, TValue2, TValue3>>                awaitableResult,
                                                                                                      Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3) => await bind(value1, value2, value3),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                              Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3>>>
                                                                                                                  bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3) => await bind(value1, value2, value3),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                                    Func<TValue1, TValue2, TValue3,
                                                                                                                        Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3) => await bind(value1, value2, value3),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3> result,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4>>>                                                                                  bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3) => await bind(value1, value2, value3),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3) => await bind(value1, value2, value3),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3>                                           result,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3) => await bind(value1, value2, value3),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3) => await bind(value1, value2, value3),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3>                                                  result,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3) => await bind(value1, value2, value3),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3) => await bind(value1, value2, value3),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3>                                                         result,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3) => await bind(value1, value2, value3),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3>>                                                   awaitableResult,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3) => await bind(value1, value2, value3),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3>                                                                result,
        Func<TValue1, TValue2, TValue3, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3) => await bind(value1, value2, value3),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7,
                                                                                                       TOut8>(this Task<Result<TValue1, TValue2, TValue3>> awaitableResult,
                                                                                                              Func<TValue1, TValue2, TValue3,
                                                                                                                  Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7,
                                                                                                                      TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3) => await bind(value1, value2, value3),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4>               result,
                                                                                           Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3,
                                                        value4) => await bind(value1, value2, value3, value4), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1>(this Task<Result<TValue1, TValue2, TValue3, TValue4>>         awaitableResult,
                                                                                                 Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3,
                                                              value4) => await bind(value1, value2, value3, value4), e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                         Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3,
                                                               value4) => await bind(value1, value2, value3, value4),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3,
                                                                     value4) => await bind(value1, value2, value3, value4),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                                       Func<TValue1, TValue2, TValue3, TValue4,
                                                                                                                           Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3,
                                                                      value4) => await bind(value1, value2, value3, value4),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                       awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3,
                                                                            value4) => await bind(value1, value2, value3, value4),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3,
                                                                             value4) => await bind(value1, value2, value3, value4),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3,
                                                                                   value4) => await bind(value1, value2, value3, value4),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                           result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3,
                                                                                    value4) => await bind(value1, value2, value3, value4),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3,
                                                                                          value4) => await bind(value1, value2, value3, value4),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                  result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3,
                                                                                           value4) => await bind(value1, value2, value3, value4),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3,
                                                                                                 value4) => await bind(value1, value2, value3, value4),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                         result,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3,
                                                                                                  value4) => await bind(value1, value2, value3, value4),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>
        BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Task<Result<TValue1, TValue2, TValue3, TValue4>> awaitableResult,
                                                                                                       Func<TValue1, TValue2, TValue3, TValue4,
                                                                                                           Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3,
                                                                                                        value4) => await bind(value1, value2, value3, value4),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6,
                                                                                                 TOut7, TOut8>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                               Func<TValue1, TValue2, TValue3, TValue4,
                                                                                                                   Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7,
                                                                                                                       TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3,
                                                                                                         value4) => await bind(value1, value2, value3, value4),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6,
                                                                                                       TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4>>                                                          awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4) => await bind(value1, value2, value3, value4),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5>               result,
                                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3,
                                                        value4,
                                                        value5) => await bind(value1, value2, value3, value4, value5),
                                                 e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>         awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3,
                                                              value4,
                                                              value5) => await bind(value1, value2, value3, value4, value5),
                                                       e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                  Func<TValue1, TValue2, TValue3, TValue4, TValue5,
                                                                                                                      Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3,
                                                               value4,
                                                               value5) => await bind(value1, value2, value3, value4, value5),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3,
                                                                     value4,
                                                                     value5) => await bind(value1, value2, value3, value4, value5),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                             result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3,
                                                                      value4,
                                                                      value5) => await bind(value1, value2, value3, value4, value5),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                       awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5) => await bind(value1, value2, value3, value4, value5),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3,
                                                                             value4,
                                                                             value5) => await bind(value1, value2, value3, value4, value5),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3,
                                                                                   value4,
                                                                                   value5) => await bind(value1, value2, value3, value4, value5),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3,
                                                                                    value4,
                                                                                    value5) => await bind(value1, value2, value3, value4, value5),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3,
                                                                                          value4,
                                                                                          value5) => await bind(value1, value2, value3, value4, value5),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                                  result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3,
                                                                                           value4,
                                                                                           value5) => await bind(value1, value2, value3, value4, value5),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3,
                                                                                                 value4,
                                                                                                 value5) => await bind(value1, value2, value3, value4, value5),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6,
                                                                                          TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                 Func<TValue1, TValue2, TValue3, TValue4, TValue5,
                                                                                                     Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3,
                                                                                                  value4,
                                                                                                  value5) => await bind(value1, value2, value3, value4, value5),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                                TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                                                   awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3,
                                                                                                        value4,
                                                                                                        value5) => await bind(value1, value2, value3, value4, value5),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                                 TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                                                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3,
                                                                                                         value4,
                                                                                                         value5) => await bind(value1, value2, value3, value4, value5),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4,
                                                                                                       TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5>>                                                          awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4,
                                                                                                               value5) => await bind(value1, value2, value3, value4, value5),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>               result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3,
                                                        value4,
                                                        value5,
                                                        value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                 e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>         awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3,
                                                              value4,
                                                              value5,
                                                              value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                       e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                      result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3,
                                                               value4,
                                                               value5,
                                                               value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3,
                                                                     value4,
                                                                     value5,
                                                                     value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                             result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3,
                                                                      value4,
                                                                      value5,
                                                                      value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                       awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5,
                                                                            value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3,
                                                                             value4,
                                                                             value5,
                                                                             value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3,
                                                                                   value4,
                                                                                   value5,
                                                                                   value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3,
                                                                                    value4,
                                                                                    value5,
                                                                                    value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3,
                                                                                          value4,
                                                                                          value5,
                                                                                          value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                                  result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3,
                                                                                           value4,
                                                                                           value5,
                                                                                           value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                         TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3,
                                                                                                 value4,
                                                                                                 value5,
                                                                                                 value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                          TOut6, TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                                            Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3,
                                                                                                  value4,
                                                                                                  value5,
                                                                                                  value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4,
                                                                                                TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                                                   awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3,
                                                                                                        value4,
                                                                                                        value5,
                                                                                                        value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4,
                                                                                                 TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                                                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3,
                                                                                                         value4,
                                                                                                         value5,
                                                                                                         value6) => await bind(value1, value2, value3, value4, value5, value6),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3,
                                                                                                       TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>>                                                          awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4,
                                                                                                               value5,
                                                                                                               value6) =>
                                                                                                            await bind(value1, value2, value3, value4, value5, value6),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>               result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3,
                                                        value4,
                                                        value5,
                                                        value6,
                                                        value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                 e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>         awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3,
                                                              value4,
                                                              value5,
                                                              value6,
                                                              value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                       e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                      result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3,
                                                               value4,
                                                               value5,
                                                               value6,
                                                               value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3,
                                                                     value4,
                                                                     value5,
                                                                     value6,
                                                                     value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                             result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3,
                                                                      value4,
                                                                      value5,
                                                                      value6,
                                                                      value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                       awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5,
                                                                            value6,
                                                                            value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3,
                                                                             value4,
                                                                             value5,
                                                                             value6,
                                                                             value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3,
                                                                                   value4,
                                                                                   value5,
                                                                                   value6,
                                                                                   value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                           result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3,
                                                                                    value4,
                                                                                    value5,
                                                                                    value6,
                                                                                    value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3,
                                                                                          value4,
                                                                                          value5,
                                                                                          value6,
                                                                                          value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                   TOut6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                          Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7,
                                                                                              Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3,
                                                                                           value4,
                                                                                           value5,
                                                                                           value6,
                                                                                           value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                         TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3,
                                                                                                 value4,
                                                                                                 value5,
                                                                                                 value6,
                                                                                                 value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                          TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                                         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3,
                                                                                                  value4,
                                                                                                  value5,
                                                                                                  value6,
                                                                                                  value7) => await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3,
                                                                                                TOut4, TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                                                   awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3,
                                                                                                        value4,
                                                                                                        value5,
                                                                                                        value6,
                                                                                                        value7) =>
                                                                                                     await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3,
                                                                                                 TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                                                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3,
                                                                                                         value4,
                                                                                                         value5,
                                                                                                         value6,
                                                                                                         value7) =>
                                                                                                      await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2,
                                                                                                       TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>>                                                          awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4,
                                                                                                               value5,
                                                                                                               value6,
                                                                                                               value7) =>
                                                                                                            await bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }

    public static Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>               result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull {
        return result.Match<Task<Result<TOut1>>>(async (value1,
                                                        value2,
                                                        value3,
                                                        value4,
                                                        value5,
                                                        value6,
                                                        value7,
                                                        value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                 e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static async Task<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>         awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1>>>(async (value1,
                                                              value2,
                                                              value3,
                                                              value4,
                                                              value5,
                                                              value6,
                                                              value7,
                                                              value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                       e => Task.FromResult(Result.Failure<TOut1>(e)));
    }

    public static Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                      result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                               value2,
                                                               value3,
                                                               value4,
                                                               value5,
                                                               value6,
                                                               value7,
                                                               value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                        e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static async Task<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2>>>(async (value1,
                                                                     value2,
                                                                     value3,
                                                                     value4,
                                                                     value5,
                                                                     value6,
                                                                     value7,
                                                                     value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                              e => Task.FromResult(Result.Failure<TOut1, TOut2>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                             result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                      value2,
                                                                      value3,
                                                                      value4,
                                                                      value5,
                                                                      value6,
                                                                      value7,
                                                                      value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                               e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                       awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3>>>(async (value1,
                                                                            value2,
                                                                            value3,
                                                                            value4,
                                                                            value5,
                                                                            value6,
                                                                            value7,
                                                                            value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                     e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                    result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                             value2,
                                                                             value3,
                                                                             value4,
                                                                             value5,
                                                                             value6,
                                                                             value7,
                                                                             value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                      e => Task.FromResult(Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                              awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4>>>(async (value1,
                                                                                   value2,
                                                                                   value3,
                                                                                   value4,
                                                                                   value5,
                                                                                   value6,
                                                                                   value7,
                                                                                   value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                            e => Task.FromResult(
                                                                                Result.Failure<TOut1, TOut2, TOut3, TOut4>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4,
                                                                            TOut5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
                                                                                   Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8,
                                                                                       Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                    value2,
                                                                                    value3,
                                                                                    value4,
                                                                                    value5,
                                                                                    value6,
                                                                                    value7,
                                                                                    value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                             e => Task.FromResult(
                                                                                 Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3,
                                                                                  TOut4, TOut5>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                                     awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (value1,
                                                                                          value2,
                                                                                          value3,
                                                                                          value4,
                                                                                          value5,
                                                                                          value6,
                                                                                          value7,
                                                                                          value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                   e => Task.FromResult(
                                                                                       Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3,
                                                                                   TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                  result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                           value2,
                                                                                           value3,
                                                                                           value4,
                                                                                           value5,
                                                                                           value6,
                                                                                           value7,
                                                                                           value8) => await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                    e => Task.FromResult(
                                                                                        Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2,
                                                                                         TOut3, TOut4, TOut5, TOut6>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                                            awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (value1,
                                                                                                 value2,
                                                                                                 value3,
                                                                                                 value4,
                                                                                                 value5,
                                                                                                 value6,
                                                                                                 value7,
                                                                                                 value8) =>
                                                                                              await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                          e => Task.FromResult(
                                                                                              Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2,
                                                                                          TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                  value2,
                                                                                                  value3,
                                                                                                  value4,
                                                                                                  value5,
                                                                                                  value6,
                                                                                                  value7,
                                                                                                  value8) =>
                                                                                               await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                           e => Task.FromResult(
                                                                                               Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                                                   awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (value1,
                                                                                                        value2,
                                                                                                        value3,
                                                                                                        value4,
                                                                                                        value5,
                                                                                                        value6,
                                                                                                        value7,
                                                                                                        value8) =>
                                                                                                     await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                 e => Task.FromResult(
                                                                                                     Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e)));
    }

    public static Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1,
                                                                                                 TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                         value2,
                                                                                                         value3,
                                                                                                         value4,
                                                                                                         value5,
                                                                                                         value6,
                                                                                                         value7,
                                                                                                         value8) =>
                                                                                                      await bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                                  e => Task
                                                                                                     .FromResult(
                                                                                                          Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                              e)));
    }

    public static async Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8,
                                                                                                       TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Task<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>>                                                          awaitableResult,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        var result = await awaitableResult;
        return await result.Match<Task<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (value1,
                                                                                                               value2,
                                                                                                               value3,
                                                                                                               value4,
                                                                                                               value5,
                                                                                                               value6,
                                                                                                               value7,
                                                                                                               value8) =>
                                                                                                            await bind(value1, value2, value3, value4, value5, value6, value7,
                                                                                                                       value8),
                                                                                                        e => Task
                                                                                                           .FromResult(
                                                                                                                Result
                                                                                                                   .Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
                                                                                                                        e)));
    }
}
