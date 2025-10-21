namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result Tap(this Result       result,
                             Action tap) {
        result.Match(tap,_ => { });
        return result;
    }
    
    public static Result<TValue> Tap<TValue>(this Result<TValue> result,
                                             Action<TValue>      tap)
        where TValue : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2> Tap<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                 Action<TValue1, TValue2>      tap)
        where TValue1 : notnull
        where TValue2 : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3> Tap<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                   Action<TValue1, TValue2, TValue3>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Tap<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                     Action<TValue1, TValue2, TValue3, TValue4>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Tap<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        result.Match((value1,
                      value2,
                      value3,
                      value4,
                      value5) => tap(value1, value2, value3, value4, value5), _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        result.Match(tap, _ => { });
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Tap<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>      tap)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        result.Match(tap, _ => { });
        return result;
    }
}
