using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results.Extensions.ErrorHandling;

public static partial class ResultExtensions {
    public static bool HasError<TError>(this Result result) {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1>(this Result<TValue1> result)
        where TValue1 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2>(this Result<TValue1, TValue2> result)
        where TValue1 : notnull
        where TValue2 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3>(this Result<TValue1, TValue2, TValue3> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4>(this Result<TValue1, TValue2, TValue3, TValue4> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6>(this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }

    public static bool HasError<TError, TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8>(
        this Result<TValue1, TValue2, TValue3, TValue4, TValue5, TValue6, TValue7, TValue8> result)
        where TValue1 : notnull
        where TValue2 : notnull
        where TValue3 : notnull
        where TValue4 : notnull
        where TValue5 : notnull
        where TValue6 : notnull
        where TValue7 : notnull
        where TValue8 : notnull {
        if (typeof(Exception).IsAssignableFrom(typeof(TError))) {
            return result.Reasons.OfType<ExceptionalError>()
                         .Any(e => e.Exception is TError);
        }
        return result.Reasons.OfType<TError>()
                     .Any();
    }
}
