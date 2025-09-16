namespace UnambitiousFx.Core.Results.Tasks;

public static partial class ResultExtensions {
    public static Result<TOut1> Select<TValue1, TOut1>(this Result<TValue1> result,
                                                               Func<TValue1, TOut1> select)
        where TValue1 : notnull
        where TOut1 : notnull {
        return result.Bind<TOut1>(v1 => Result.Success(select(v1)));
    }
}
