using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    private static Result<T1, T2> ShapeError<T1, T2>(this Result<T1, T2>        result,
                                                     Func<IEnumerable<IError>, IEnumerable<IError>> shape)
        where T1 : notnull
        where T2 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3> ShapeError<T1, T2, T3>(this Result<T1, T2, T3>    result,
                                                             Func<IEnumerable<IError>, IEnumerable<IError>> shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3, T4> ShapeError<T1, T2, T3, T4>(this Result<T1, T2, T3, T4> result,
                                                                     Func<IEnumerable<IError>, IEnumerable<IError>>  shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3, T4, T5> ShapeError<T1, T2, T3, T4, T5>(this Result<T1, T2, T3, T4, T5> result,
                                                                             Func<IEnumerable<IError>, IEnumerable<IError>>      shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3, T4, T5, T6> ShapeError<T1, T2, T3, T4, T5, T6>(this Result<T1, T2, T3, T4, T5, T6> result,
                                                                                     Func<IEnumerable<IError>, IEnumerable<IError>>          shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3, T4, T5, T6, T7> ShapeError<T1, T2, T3, T4, T5, T6, T7>(this Result<T1, T2, T3, T4, T5, T6, T7> result,
                                                                                             Func<IEnumerable<IError>, IEnumerable<IError>>              shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));

    private static Result<T1, T2, T3, T4, T5, T6, T7, T8> ShapeError<T1, T2, T3, T4, T5, T6, T7, T8>(this Result<T1, T2, T3, T4, T5, T6, T7, T8> result,
                                                                                                     Func<IEnumerable<IError>, IEnumerable<IError>>                  shape)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
        where T5 : notnull
        where T6 : notnull
        where T7 : notnull
        where T8 : notnull => result.IsSuccess
                                  ? result
                                  : Preserve(result, result.MapError(shape));
}
