namespace UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks;

public static partial class ResultBindExtensions {
    public static ValueTask<Result> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                       Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result>> bind,
                                                                                       bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<ValueTask<Result>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result>(response);
        });
    }

    public static async ValueTask<Result> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result>> bind,
                                                                                            bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result> bind,
                                                                                            bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                     Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1>>> bind,
                                                                                                     bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        return result.Match<ValueTask<Result<TOut1>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1>>(response);
        });
    }

    public static async ValueTask<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1>>> bind,
                                                                                                           bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1>> bind,
                                                                                                           bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2>>> bind,
                                                                                                                    bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2>>> bind,
                                                                                                                         bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2>> bind,
                                                                                                                         bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                  Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3>>> bind,
                                                                                                                                  bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3>>> bind,
                                                                                                                                        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3>> bind,
                                                                                                                                        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                              Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind,
                                                                                                                                              bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                      Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4>>> bind,
                                                                                                                                                      bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                      Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4>> bind,
                                                                                                                                                      bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                                              Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind,
                                                                                                                                                              bool copyReasonsAndMetadata = true)
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
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>> bind,
                                                                                                                                                                    bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
                                                                                                                                                                    bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind,
                                                                                                                                                                            bool copyReasonsAndMetadata = true)
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
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>> bind,
                                                                                                                                                                                bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
                                                                                                                                                                                bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                                                                          Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind,
                                                                                                                                                                                          bool copyReasonsAndMetadata = true)
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
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>> bind,
                                                                                                                                                                                            bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                            Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
                                                                                                                                                                                            bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                                                                                                                        Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind,
                                                                                                                                                                                                        bool copyReasonsAndMetadata = true)
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
        return result.Match<ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>>(async (v1, v2, v3, v4, v5) => {
            var response = await bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return new ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(response);
        });
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5, ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>> bind,
                                                                                                                                                                                                                bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return await result.BindAsync(bind, copyReasonsAndMetadata);
    }

    public static async ValueTask<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> BindAsync<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this ValueTask<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> awaitable,
                                                                                                                                                                                                                Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
                                                                                                                                                                                                                bool copyReasonsAndMetadata = true)
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
        var result = await awaitable;
        return result.Match((v1, v2, v3, v4, v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, err => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(err);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }
}
