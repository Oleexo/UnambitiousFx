namespace UnambitiousFx.Core.Results;

public static partial class ResultExtensions
{
    public static Result<TValue> Recover<TValue>(this Result<TValue> result, Func<Exception, TValue> recover)
        where TValue : notnull
    {
        if (result.Ok(out var value, out var error))
        {
            return Result.Success(value);
        }

        var fallback = recover(error);
        return Result.Success(fallback);
    }

    public static Result<TValue> Recover<TValue>(this Result<TValue> result, TValue fallback)
        where TValue : notnull
        => result.Recover(_ => fallback);

    public static Result<TValue> RecoverWith<TValue>(this Result<TValue> result, Func<Exception, Result<TValue>> recover)
        where TValue : notnull
    {
        if (result.Ok(out var value, out var error))
        {
            return Result.Success(value);
        }

        return recover(error);
    }
}
