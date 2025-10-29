namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static Result PrependError(this Result result,
                                      string      prefix) {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result; // no-op
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1> PrependError<T1>(this Result<T1> result,
                                              string          prefix)
        where T1 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result; // no-op
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2> PrependError<T1, T2>(this Result<T1, T2> result,
                                                      string              prefix)
        where T1 : notnull
        where T2 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3> PrependError<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                              string                  prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3, T4> PrependError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                      string                      prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3, T4, T5> PrependError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                              string                          prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3, T4, T5, T6> PrependError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                      string                              prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7> PrependError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                              string                                  prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> PrependError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                      string                                      prefix)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull {
        ArgumentNullException.ThrowIfNull(result);
        if (string.IsNullOrEmpty(prefix) ||
            result.IsSuccess) return result;
        return result.MapErrors(errs => new Exception(prefix + errs[0].Message, errs[0]));
    }
}
