using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.SideEffects;

public static class ResultTapErrorExtensions {
    public static Result TapError(this Result       result,
                                  Action<IEnumerable<IError>> tap) {
        result.IfFailure(tap);
        return result;
    }
    
    public static Result<TValue> TapError<TValue>(this Result<TValue> result,
                                                  Action<IEnumerable<IError>>   tap)
        where TValue : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2> TapError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                      Action<IEnumerable<IError>>             tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3> TapError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                        Action<IEnumerable<IError>>                      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> TapError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                          Action<IEnumerable<IError>>                               tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> TapError<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Action<IEnumerable<IError>>                                        tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Action<IEnumerable<IError>>                                                 tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Action<IEnumerable<IError>>                                                          tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        result.IfFailure(tap);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> TapError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Action<IEnumerable<IError>>                                                                   tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        result.IfFailure(tap);
        return result;
    }
}
