using UnambitiousFx.Core.Results.Tasks;

namespace UnambitiousFx.Core.Options.Tasks;

public static partial class OptionExtensions {
    public static Task<Option<TOut>> BindAsync<TIn, TOut>(this Task<Option<TIn>> option,
                                                               Func<TIn, Option<TOut>>     bind)
        where TIn : notnull
        where TOut : notnull {
        return option.MatchAsync(bind, Option.None<TOut>);
    }
}
