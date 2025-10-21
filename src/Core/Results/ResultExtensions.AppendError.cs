namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result AppendError(this Result result, string suffix) {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) || result.IsSuccess) return result; // no-op
        var mapped = result.MapError(e => new Exception(e.Message + suffix, e));
        return Preserve(result, mapped);
    }

    public static Result<T1> AppendError<T1>(this Result<T1> result, string suffix)
        where T1 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) || result.IsSuccess) return result; // no-op
        var mapped = result.MapError(e => new Exception(e.Message + suffix, e));
        return Preserve(result, mapped);
    }

    public static Result<T1, T2> AppendError<T1, T2>(this Result<T1, T2> result,
                                                     string              suffix)
        where T1 : notnull
        where T2 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3> AppendError<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                             string                  suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3, T4> AppendError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                     string                      suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3, T4, T5> AppendError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                             string                          suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3, T4, T5, T6> AppendError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                     string                              suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7> AppendError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                             string                                  suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> AppendError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                     string                                      suffix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (string.IsNullOrEmpty(suffix) ||
            result.IsSuccess) return result;
        return result.ShapeError(e => new Exception(e.Message + suffix, e));
    }
}
