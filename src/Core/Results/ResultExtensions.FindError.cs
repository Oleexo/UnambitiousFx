using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

/// <summary>
///     Error inspection helpers: locate a specific attached error reason via predicate.
///     Mirrors the arity pattern used by other introspection helpers to ensure discoverability.
/// </summary>
public static partial class ResultExtensions {
    public static IError? FindError(this Result        result,
                                    Func<IError, bool> predicate) {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError(this Result        result,
                                    Func<IError, bool> predicate,
                                    out IError?        error) {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1>(this Result<TValue1> result,
                                             Func<IError, bool>   predicate)
        where TValue1 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1>(this Result<TValue1> result,
                                             Func<IError, bool>   predicate,
                                             out IError?          error)
        where TValue1 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                      Func<IError, bool>            predicate)
        where TValue1 : notnull
        where TValue2 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2>(this Result<TValue1, TValue2> result,
                                                      Func<IError, bool>            predicate,
                                                      out IError?                   error)
        where TValue1 : notnull
        where TValue2 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                               Func<IError, bool>                     predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result,
                                                               Func<IError, bool>                     predicate,
                                                               out IError?                            error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                        Func<IError, bool>                              predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result,
                                                                        Func<IError, bool>                              predicate,
                                                                        out IError?                                     error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                 Func<IError, bool>                                       predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result,
                                                                                 Func<IError, bool>                                       predicate,
                                                                                 out IError?                                              error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                          Func<IError, bool>                                                predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result,
                                                                                          Func<IError, bool>                                                predicate,
                                                                                          out IError?                                                       error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<IError, bool>                                                         predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result,
        Func<IError, bool>                                                         predicate,
        out IError?                                                                error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }

    public static IError? FindError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<IError, bool>                                                                  predicate)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        ArgumentNullException.ThrowIfNull(predicate);

        return result.Reasons.OfType<IError>()
                     .FirstOrDefault(predicate);
    }

    public static bool TryPickError<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result,
        Func<IError, bool>                                                                  predicate,
        out IError?                                                                         error)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        error = result.Reasons.OfType<IError>()
                      .FirstOrDefault(predicate);
        return error is not null;
    }
}
