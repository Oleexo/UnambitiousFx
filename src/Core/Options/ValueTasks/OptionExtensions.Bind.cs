namespace UnambitiousFx.Core.Options.ValueTasks;

public static partial class OptionExtensions {
    public static ValueTask<Option<TOut>> BindAsync<TIn, TOut>(this ValueTask<Option<TIn>> option,
                                                               Func<TIn, Option<TOut>>     bind)
        where TIn : notnull
        where TOut : notnull {
        return option.MatchAsync(bind, Option.None<TOut>);
    }
}
