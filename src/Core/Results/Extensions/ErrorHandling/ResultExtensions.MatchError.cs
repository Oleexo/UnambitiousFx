using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    // -------------------- MatchError (non generic) --------------------
    public static TOut MatchError<TError, TOut>(this Result        result,
                                                Func<TError, TOut> onMatch,
                                                Func<TOut>         onElse)
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    // Generic arities MatchError
    public static TOut MatchError<TError, TValue1, TOut>(this Result<TValue1> result,
                                                         Func<TError, TOut>   onMatch,
                                                         Func<TOut>           onElse)
        where TValue1 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);
        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TOut>(this Result<TValue1, TValue2> result,
                                                                  Func<TError, TOut>            onMatch,
                                                                  Func<TOut>                    onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);
        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TOut>(this Result<TValue1, TValue2, TValue3> result,
                                                                           Func<TError, TOut>                     onMatch,
                                                                           Func<TOut>                             onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TOut>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                                    Func<TError, TOut>                              onMatch,
                                                                                    Func<TOut>                                      onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                             Func<TError, TOut>                                       onMatch,
                                                                                             Func<TOut>                                               onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TOut>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                                      Func<TError, TOut>                                                onMatch,
                                                                                                      Func<TOut>                                                        onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<TError, TOut>                                                         onMatch,
        Func<TOut>                                                                 onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }

    public static TOut MatchError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8, TOut>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<TError, TOut>                                                                  onMatch,
        Func<TOut>                                                                          onElse)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull
        where TError : class, IError {
        ArgumentNullException.ThrowIfNull(onMatch);

        ArgumentNullException.ThrowIfNull(onElse);

        var match = result.Reasons.OfType<TError>()
                          .FirstOrDefault();
        return match is not null
                   ? onMatch(match)
                   : onElse();
    }
}
