namespace UnambitiousFx.Core.Results.Extensions.SideEffects;

public static class ResultTapBothExtensions {
    public static Result TapBoth(this Result       result,
                                 Action            onSuccess,
                                 Action<Exception> onFailure) {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1> TapBoth<TValue1>(this Result<TValue1> result,
                                                   Action<TValue1>      onSuccess,
                                                   Action<Exception>    onFailure)
        where TValue1 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2> TapBoth<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                                     Action<TValue1, TValue2>      onSuccess,
                                                                     Action<Exception>             onFailure)
        where TValue1 : notnull
        where TValue2 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3> TapBoth<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                                                       Action<TValue1, TValue2, TValue3>      onSuccess,
                                                                                       Action<Exception>                      onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> TapBoth<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                                         Action<TValue1, TValue2, TValue3, TValue4>      onSuccess,
                                                                                                         Action<Exception>                               onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> TapBoth<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5>      onSuccess,
        Action<Exception>                                        onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> TapBoth<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>      onSuccess,
        Action<Exception>                                                 onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> TapBoth<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>      onSuccess,
        Action<Exception>                                                          onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> TapBoth<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Action<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>      onSuccess,
        Action<Exception>                                                                   onFailure)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        result.Match(onSuccess, onFailure);
        return result;
    }
}
