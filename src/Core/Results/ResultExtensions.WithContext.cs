namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions {
    public static Result WithContext(this Result result,
                                     string      context) => result.PrependError(context);

    public static Result<T1> WithContext<T1>(this Result<T1> result,
                                             string          context)
        where T1 : notnull => result.PrependError(context);

    public static Result<T1, T2> WithContext<T1, T2>(this Result<T1, T2> result,
                                                     string              context)
        where T1 : notnull
        where T2 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3> WithContext<T1, T2, T3>(this Result<T1, T2, T3> result,
                                                             string                  context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3, T4> WithContext<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                     string                      context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3, T4, T5> WithContext<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                             string                          context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3, T4, T5, T6> WithContext<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                     string                              context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3, T4, T5, T6, T7> WithContext<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                             string                                  context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull => result.PrependError(context);

    public static Result<T1, T2, T3, T4, T5, T6, T7, T8> WithContext<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                     string                                      context)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull => result.PrependError(context);
}
