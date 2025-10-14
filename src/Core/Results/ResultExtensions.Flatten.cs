namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result<TValue1> Flatten<TValue1>(this Result<Result<TValue1>> result)
        where TValue1 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2> Flatten<TValue1, TValue2>(this Result<Result<TValue1, TValue2>> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3> Flatten<TValue1, TValue2, TValue3>(this Result<Result<TValue1, TValue2, TValue3>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4> Flatten<TValue1, TValue2, TValue3, TValue4>(this Result<Result<TValue1, TValue2, TValue3, TValue4>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5> Flatten<TValue1, TValue2, TValue3, TValue4, TValue5>(
        this Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> Flatten<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(
        this Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> Flatten<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        return result.Bind(inner => inner);
    }

    public static Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> Flatten<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        return result.Bind(inner => inner);
    }
}
