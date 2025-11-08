namespace UnambitiousFx.Core.Maybe;

public static class MaybeExtensions
{
    public static Maybe<TOut> Bind<TIn, TOut>(this Maybe<TIn> maybe,
                                               Func<TIn, Maybe<TOut>> bind)
        where TIn : notnull
        where TOut : notnull
    {
        return maybe.Match(bind, Maybe.None<TOut>);
    }
}
