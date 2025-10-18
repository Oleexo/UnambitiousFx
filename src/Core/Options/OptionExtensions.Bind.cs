namespace UnambitiousFx.Core.Options;

public static partial class OptionExtensions {
    public static Option<TOut> Bind<TIn, TOut>(this Option<TIn>  option,
                              Func<TIn, Option<TOut>> bind)
        where TIn : notnull
        where TOut : notnull {
        return option.Match(bind, Option.None<TOut>);
    }
}
