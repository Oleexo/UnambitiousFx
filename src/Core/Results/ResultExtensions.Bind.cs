namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result Bind(this Result  result,
                              Func<Result> bind,
                              bool         copyReasonsAndMetadata = true) {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TOut1>(this Result         result,
                                            Func<Result<TOut1>> bind,
                                            bool                copyReasonsAndMetadata = true)
        where TOut1 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TOut1, TOut2>(this Result                result,
                                                          Func<Result<TOut1, TOut2>> bind,
                                                          bool                       copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TOut1, TOut2, TOut3>(this Result                       result,
                                                                        Func<Result<TOut1, TOut2, TOut3>> bind,
                                                                        bool                              copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TOut1, TOut2, TOut3, TOut4>(this Result                              result,
                                                                                      Func<Result<TOut1, TOut2, TOut3, TOut4>> bind,
                                                                                      bool                                     copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TOut1, TOut2, TOut3, TOut4, TOut5>(this Result                                     result,
                                                                                                    Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
                                                                                                    bool                                            copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result result,
                                                                                                                  Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
                                                                                                                  bool copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result result,
        Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>                                                                       bind,
        bool                                                                                                                                copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result result,
        Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool copyReasonsAndMetadata = true)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match(() => {
            var response = bind();
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1>(this Result<TValue1>  result,
                                       Func<TValue1, Result> bind,
                                       bool                  copyReasonsAndMetadata = true)
        where TValue1 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TOut1>(this Result<TValue1>         result,
                                                     Func<TValue1, Result<TOut1>> bind,
                                                     bool                         copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TOut1, TOut2>(this Result<TValue1>                result,
                                                                   Func<TValue1, Result<TOut1, TOut2>> bind,
                                                                   bool                                copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TOut1, TOut2, TOut3>(this Result<TValue1>                       result,
                                                                                 Func<TValue1, Result<TOut1, TOut2, TOut3>> bind,
                                                                                 bool                                       copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1>                              result,
                                                                                               Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4>> bind,
                                                                                               bool                                              copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1> result,
                                                                                                             Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
                                                                                                             bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>                                                                         bind,
        bool                                                                                                                                    copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match(v => {
            var response = bind(v);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2>(this Result<TValue1, TValue2>  result,
                                                Func<TValue1, TValue2, Result> bind,
                                                bool                           copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match((v1,
                             v2) => {
            var response = bind(v1, v2);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
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
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>  result,
                                                         Func<TValue1, TValue2, TValue3, Result> bind,
                                                         bool                                    copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TOut1>(this Result<TValue1, TValue2, TValue3>         result,
                                                                       Func<TValue1, TValue2, TValue3, Result<TOut1>> bind,
                                                                       bool                                           copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3>                result,
                                                                                     Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2>> bind,
                                                                                     bool                                                  copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                   Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3>> bind,
                                                                                                   bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                                 Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4>>
                                                                                                                     bind,
                                                                                                                 bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2, TValue3> result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3>                                            result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool                                                                              copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3>                                                   result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool                                                                                     copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3>                                                          result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool                                                                                            copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3) => {
            var response = bind(v1, v2, v3);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>  result,
                                                                  Func<TValue1, TValue2, TValue3, TValue4, Result> bind,
                                                                  bool                                             copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4>         result,
                                                                                Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1>> bind,
                                                                                bool                                                    copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                              Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2>> bind,
                                                                                              bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                            Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3>>
                                                                                                                bind,
                                                                                                            bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4>> bind,
        bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool                                                                                copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool                                                                                       copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool                                                                                              copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool                                                                                                     copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4) => {
            var response = bind(v1, v2, v3, v4);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5>  result,
                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result> bind,
                                                                           bool                                                      copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1>> bind,
                                                                                         bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                       Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2>> bind,
                                                                                                       bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3>> bind,
        bool                                                                           copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4>> bind,
        bool                                                                                  copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool                                                                                         copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool                                                                                                copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool                                                                                                       copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7,
                                                                                      TOut8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5,
                                                                                                 Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5) => {
            var response = bind(v1, v2, v3, v4, v5);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result> bind,
                                                                                    bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                  Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1>> bind,
                                                                                                  bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2>> bind,
        bool                                                                             copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3>> bind,
        bool                                                                                    copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4>> bind,
        bool                                                                                           copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool                                                                                                  copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool                                                                                                         copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6,
                                                                               TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                      Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                          Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
                                                                                      bool copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                      TOut6, TOut7, TOut8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                                               Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
                                                                                                           bool copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6) => {
            var response = bind(v1, v2, v3, v4, v5, v6);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result> bind,
                                                                                             bool copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1>> bind,
        bool                                                                               copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2>> bind,
        bool                                                                                      copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3>> bind,
        bool                                                                                             copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4>> bind,
        bool                                                                                                    copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool                                                                                                           copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
        bool                                                                                                                  copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                               TOut6, TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7,
                                                                                                 Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
                                                                                             bool copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                      TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool                                                                                                                                copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>  result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result> bind,
        bool                                                                                 copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1>> bind,
        bool                                                                                        copyReasonsAndMetadata = true)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull {
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2>> bind,
        bool                                                                                               copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3>> bind,
        bool                                                                                                      copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4>> bind,
        bool                                                                                                             copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind,
        bool                                                                                                                    copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                        TOut6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
                                                                               Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8,
                                                                                   Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind,
                                                                               bool copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4,
                                                                               TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind,
        bool                                                                                                                                  copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3,
                                                                                      TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind,
        bool                                                                                                                                         copyReasonsAndMetadata = true)
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
        return result.Match((v1,
                             v2,
                             v3,
                             v4,
                             v5,
                             v6,
                             v7,
                             v8) => {
            var response = bind(v1, v2, v3, v4, v5, v6, v7, v8);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        }, e => {
            var response = Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e);
            if (copyReasonsAndMetadata) {
                response.CopyReasonsAndMetadata(result);
            }

            return response;
        });
    }
}
