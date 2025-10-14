namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result Bind(this Result  result,
                              Func<Result> bind) {
        return result.Match<Result>(() => bind(), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TOut1>(this Result         result,
                                            Func<Result<TOut1>> bind)
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>(() => bind(), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TOut1, TOut2>(this Result                result,
                                                          Func<Result<TOut1, TOut2>> bind)
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>(() => bind(), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TOut1, TOut2, TOut3>(this Result                       result,
                                                                        Func<Result<TOut1, TOut2, TOut3>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>(() => bind(), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TOut1, TOut2, TOut3, TOut4>(this Result                              result,
                                                                                      Func<Result<TOut1, TOut2, TOut3, TOut4>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>(() => bind(), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TOut1, TOut2, TOut3, TOut4, TOut5>(this Result                                     result,
                                                                                                    Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(() => bind(), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result                                            result,
                                                                                                                  Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(() => bind(), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result result,
        Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>                                                                       bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(() => bind(), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result result,
        Func<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>                                                                              bind)
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(
            () => bind(), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1>(this Result<TValue1>  result,
                                       Func<TValue1, Result> bind)
        where TValue1 : notnull {
        return result.Match<Result>(value1 => bind(value1), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TOut1>(this Result<TValue1>         result,
                                                     Func<TValue1, Result<TOut1>> bind)
        where TValue1 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>(value1 => bind(value1), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TOut1, TOut2>(this Result<TValue1>                result,
                                                                   Func<TValue1, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>(value1 => bind(value1), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TOut1, TOut2, TOut3>(this Result<TValue1>                       result,
                                                                                 Func<TValue1, Result<TOut1, TOut2, TOut3>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>(value1 => bind(value1), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1>                              result,
                                                                                               Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>(value1 => bind(value1), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1>                                     result,
                                                                                                             Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>(value1 => bind(value1), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>                                                                         bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>(value1 => bind(value1), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>                                                                                bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>(value1 => bind(value1),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(this Result<TValue1> result,
        Func<TValue1, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>                                                                                       bind)
        where TValue1 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull
        where TOut8 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>(value1 => bind(value1),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2>(this Result<TValue1, TValue2>  result,
                                                Func<TValue1, TValue2, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Match<Result>((value1,
                                     value2) => bind(value1, value2), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TOut1>(this Result<TValue1, TValue2>         result,
                                                              Func<TValue1, TValue2, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2) => bind(value1, value2), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TOut1, TOut2>(this Result<TValue1, TValue2>                result,
                                                                            Func<TValue1, TValue2, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2) => bind(value1, value2), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2>                       result,
                                                                                          Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2) => bind(value1, value2), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2>                              result,
                                                                                                        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2) => bind(value1, value2), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2> result,
                                                                                                                      Func<TValue1, TValue2,
                                                                                                                          Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2) => bind(value1, value2), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this Result<TValue1, TValue2> result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>                                                                                  bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2) => bind(value1, value2), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2>                                                   result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull
        where TOut7 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2) => bind(value1, value2),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2>                                                          result,
        Func<TValue1, TValue2, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2) => bind(value1, value2),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3>  result,
                                                         Func<TValue1, TValue2, TValue3, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3) => bind(value1, value2, value3), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TOut1>(this Result<TValue1, TValue2, TValue3>         result,
                                                                       Func<TValue1, TValue2, TValue3, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3) => bind(value1, value2, value3), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3>                result,
                                                                                     Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3) => bind(value1, value2, value3), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3>                       result,
                                                                                                   Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3) => bind(value1, value2, value3), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3> result,
                                                                                                                 Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4>>
                                                                                                                     bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3) => bind(value1, value2, value3), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5>(this Result<TValue1, TValue2, TValue3> result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5>>                                                                                    bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3) => bind(value1, value2, value3), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3>                                            result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull
        where TOut6 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3) => bind(value1, value2, value3),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3>                                                   result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3) => bind(value1, value2, value3),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3>                                                          result,
        Func<TValue1, TValue2, TValue3, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3) => bind(value1, value2, value3),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4>  result,
                                                                  Func<TValue1, TValue2, TValue3, TValue4, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3,
                                     value4) => bind(value1, value2, value3, value4), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4>         result,
                                                                                Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3,
                                            value4) => bind(value1, value2, value3, value4), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4>                result,
                                                                                              Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3,
                                                   value4) => bind(value1, value2, value3, value4), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                            Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3>>
                                                                                                                bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3,
                                                          value4) => bind(value1, value2, value3, value4), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4>>                                                                                      bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3,
                                                                 value4) => bind(value1, value2, value3, value4), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull
        where TOut5 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3,
                                                                        value4) => bind(value1, value2, value3, value4), e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3,
                                                                               value4) => bind(value1, value2, value3, value4),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3,
                                                                                      value4) => bind(value1, value2, value3, value4),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3,
                                                                                             value4) => bind(value1, value2, value3, value4),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5>  result,
                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3,
                                     value4,
                                     value5) => bind(value1, value2, value3, value4, value5), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5>         result,
                                                                                         Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3,
                                            value4,
                                            value5) => bind(value1, value2, value3, value4, value5), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                                       Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3,
                                                   value4,
                                                   value5) => bind(value1, value2, value3, value4, value5), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5) => bind(value1, value2, value3, value4, value5), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull
        where TOut4 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3,
                                                                 value4,
                                                                 value5) => bind(value1, value2, value3, value4, value5), e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3,
                                                                        value4,
                                                                        value5) => bind(value1, value2, value3, value4, value5),
                                                                       e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3,
                                                                               value4,
                                                                               value5) => bind(value1, value2, value3, value4, value5),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3,
                                                                                      value4,
                                                                                      value5) => bind(value1, value2, value3, value4, value5),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7,
                                                                                      TOut8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5,
                                                                                                 Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3,
                                                                                             value4,
                                                                                             value5) => bind(value1, value2, value3, value4, value5),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>  result,
                                                                                    Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3,
                                     value4,
                                     value5,
                                     value6) => bind(value1, value2, value3, value4, value5, value6), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>         result,
                                                                                                  Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3,
                                            value4,
                                            value5,
                                            value6) => bind(value1, value2, value3, value4, value5, value6), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3,
                                                   value4,
                                                   value5,
                                                   value6) => bind(value1, value2, value3, value4, value5, value6), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TOut1 : notnull
        where TOut2 : notnull
        where TOut3 : notnull {
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6) => bind(value1, value2, value3, value4, value5, value6), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3,
                                                                 value4,
                                                                 value5,
                                                                 value6) => bind(value1, value2, value3, value4, value5, value6),
                                                                e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3,
                                                                        value4,
                                                                        value5,
                                                                        value6) => bind(value1, value2, value3, value4, value5, value6),
                                                                       e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3,
                                                                               value4,
                                                                               value5,
                                                                               value6) => bind(value1, value2, value3, value4, value5, value6),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6,
                                                                               TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                      Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                          Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3,
                                                                                      value4,
                                                                                      value5,
                                                                                      value6) => bind(value1, value2, value3, value4, value5, value6),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                                      TOut6, TOut7, TOut8>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                           Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6,
                                                                                                               Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3,
                                                                                             value4,
                                                                                             value5,
                                                                                             value6) => bind(value1, value2, value3, value4, value5, value6),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>  result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3,
                                     value4,
                                     value5,
                                     value6,
                                     value7) => bind(value1, value2, value3, value4, value5, value6, value7), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3,
                                            value4,
                                            value5,
                                            value6,
                                            value7) => bind(value1, value2, value3, value4, value5, value6, value7), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TOut1 : notnull
        where TOut2 : notnull {
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3,
                                                   value4,
                                                   value5,
                                                   value6,
                                                   value7) => bind(value1, value2, value3, value4, value5, value6, value7), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6,
                                                          value7) => bind(value1, value2, value3, value4, value5, value6, value7), e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3,
                                                                 value4,
                                                                 value5,
                                                                 value6,
                                                                 value7) => bind(value1, value2, value3, value4, value5, value6, value7),
                                                                e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3,
                                                                        value4,
                                                                        value5,
                                                                        value6,
                                                                        value7) => bind(value1, value2, value3, value4, value5, value6, value7),
                                                                       e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                            result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3,
                                                                               value4,
                                                                               value5,
                                                                               value6,
                                                                               value7) => bind(value1, value2, value3, value4, value5, value6, value7),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                               TOut6, TOut7>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
                                                                                             Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7,
                                                                                                 Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3,
                                                                                      value4,
                                                                                      value5,
                                                                                      value6,
                                                                                      value7) => bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut1, TOut2, TOut3, TOut4,
                                                                                      TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3,
                                                                                             value4,
                                                                                             value5,
                                                                                             value6,
                                                                                             value7) => bind(value1, value2, value3, value4, value5, value6, value7),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }

    public static Result Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>  result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Match<Result>((value1,
                                     value2,
                                     value3,
                                     value4,
                                     value5,
                                     value6,
                                     value7,
                                     value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8), e => Result.Failure(e));
    }

    public static Result<TOut1> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>         result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1>> bind)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TOut1 : notnull {
        return result.Match<Result<TOut1>>((value1,
                                            value2,
                                            value3,
                                            value4,
                                            value5,
                                            value6,
                                            value7,
                                            value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8), e => Result.Failure<TOut1>(e));
    }

    public static Result<TOut1, TOut2> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2>> bind)
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
        return result.Match<Result<TOut1, TOut2>>((value1,
                                                   value2,
                                                   value3,
                                                   value4,
                                                   value5,
                                                   value6,
                                                   value7,
                                                   value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8), e => Result.Failure<TOut1, TOut2>(e));
    }

    public static Result<TOut1, TOut2, TOut3> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                       result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3>>((value1,
                                                          value2,
                                                          value3,
                                                          value4,
                                                          value5,
                                                          value6,
                                                          value7,
                                                          value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                         e => Result.Failure<TOut1, TOut2, TOut3>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                              result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4>>((value1,
                                                                 value2,
                                                                 value3,
                                                                 value4,
                                                                 value5,
                                                                 value6,
                                                                 value7,
                                                                 value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                e => Result.Failure<TOut1, TOut2, TOut3, TOut4>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4, TOut5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                     result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5>>((value1,
                                                                        value2,
                                                                        value3,
                                                                        value4,
                                                                        value5,
                                                                        value6,
                                                                        value7,
                                                                        value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                       e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4, TOut5,
                                                                        TOut6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
                                                                               Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8,
                                                                                   Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>((value1,
                                                                               value2,
                                                                               value3,
                                                                               value4,
                                                                               value5,
                                                                               value6,
                                                                               value7,
                                                                               value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                              e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3, TOut4,
                                                                               TOut5, TOut6, TOut7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                   result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>((value1,
                                                                                      value2,
                                                                                      value3,
                                                                                      value4,
                                                                                      value5,
                                                                                      value6,
                                                                                      value7,
                                                                                      value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                     e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>(e));
    }

    public static Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8> Bind<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut1, TOut2, TOut3,
                                                                                      TOut4, TOut5, TOut6, TOut7, TOut8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>                                                          result,
        Func<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>> bind)
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
        return result.Match<Result<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>>((value1,
                                                                                             value2,
                                                                                             value3,
                                                                                             value4,
                                                                                             value5,
                                                                                             value6,
                                                                                             value7,
                                                                                             value8) => bind(value1, value2, value3, value4, value5, value6, value7, value8),
                                                                                            e => Result.Failure<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TOut8>(e));
    }
}
