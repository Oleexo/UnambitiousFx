namespace UnambitiousFx.Core.Maybe.Tasks;

public static partial class MaybeExtensions
{
    public static Task<Maybe<TOut>> BindAsync<TIn, TOut>(this Task<Maybe<TIn>> option,
                                                          Func<TIn, Maybe<TOut>> bind)
        where TIn : notnull
        where TOut : notnull
    {
        return option.MatchAsync(bind, Maybe.None<TOut>);
    }
}
