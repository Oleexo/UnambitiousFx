namespace UnambitiousFx.Core.Maybe.ValueTasks;

public static partial class MaybeExtensions
{
    public static ValueTask<Maybe<TOut>> BindAsync<TIn, TOut>(this ValueTask<Maybe<TIn>> option,
                                                               Func<TIn, Maybe<TOut>> bind)
        where TIn : notnull
        where TOut : notnull
    {
        return option.MatchAsync(bind, Maybe.None<TOut>);
    }
}
