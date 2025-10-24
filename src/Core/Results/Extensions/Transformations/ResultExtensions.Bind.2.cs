namespace UnambitiousFx.Core.Results.Extensions.Transformations;

public static partial class ResultBindExtensions {
    public static Result Bind<TValue1, TValue2>(this Result<TValue1, TValue2>  result,
                                                Func<TValue1, TValue2, Result> bind,
                                                bool                           copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TOut1>(this Result<TValue1, TValue2>         result,
                                                              Func<TValue1, TValue2, Result<TOut1>> bind,
                                                              bool                                  copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2>                result,
                                                                            Func<TValue1, TValue2, Result<TOut1, TOut2>> bind,
                                                                            bool                                         copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2>                       result,
                                                                                          Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3>> bind,
                                                                                          bool                                                copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2> result,
                                                                                                        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4>> bind,
                                                                                                        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2> result,
                                                                                                                      Func<TValue1, TValue2,
                                                                                                                          Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
                                                                                                                      bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1, TValue2> result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2>                                                   result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool                                                                            copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2>                                                          result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool                                                                                   copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                result.CopyReasonsAndMetadata(response);
            }

            return response;
        });
    }
}
