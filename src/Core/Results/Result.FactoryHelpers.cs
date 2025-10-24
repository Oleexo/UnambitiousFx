using UnambitiousFx.Core.Results.Reasons;

namespace UnambitiousFx.Core.Results;

// Needed for Func<> and exceptions

public abstract partial class Result {
    public static Result<T> FromNullable<T>(T?           value,
                                            Func<IError> errorFactory)
        where T : notnull {
        if (errorFactory == null) {
            throw new ArgumentNullException(nameof(errorFactory));
        }

        return value is not null
                   ? Success(value)
                   : Failure<T>(errorFactory());
    }

    public static Result<T> FromNullable<T>(T?     value,
                                            string resource,
                                            string identifier)
        where T : notnull {
        return value is not null
                   ? Success(value)
                   : Failure<T>(new NotFoundError(resource, identifier));
    }

    public static Result FromCondition(bool         condition,
                                       Func<IError> errorFactory) {
        if (errorFactory == null) {
            throw new ArgumentNullException(nameof(errorFactory));
        }

        return condition
                   ? Success()
                   : Failure(errorFactory());
    }

    public static Result<T> FromCondition<T>(T             value,
                                             Func<T, bool> predicate,
                                             Func<IError>  errorFactory)
        where T : notnull {
        if (predicate == null) {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (errorFactory == null) {
            throw new ArgumentNullException(nameof(errorFactory));
        }

        return predicate(value)
                   ? Success(value)
                   : Failure<T>(errorFactory());
    }

    /// <summary>
    ///     Builds a non-generic Result from validation failures. Returns Success() if the collection of failures is empty;
    ///     otherwise, returns Failure containing a ValidationError.
    /// </summary>
    /// <param name="failures">A collection of validation failure messages.</param>
    /// <param name="extra">An optional dictionary containing additional metadata related to the validation errors.</param>
    /// <returns>A Result indicating success or failure depending on the presence of validation failures.</returns>
    public static Result FromValidation(IEnumerable<string>                   failures,
                                        IReadOnlyDictionary<string, object?>? extra = null) {
        if (failures == null) {
            throw new ArgumentNullException(nameof(failures));
        }

        var list = failures as IReadOnlyList<string> ?? failures.ToList();
        return list.Count == 0
                   ? Success()
                   : Failure(new ValidationError(list, extra));
    }

    public static Result<T> FromValidation<T>(T                                     value,
                                              IEnumerable<string>                   failures,
                                              IReadOnlyDictionary<string, object?>? extra = null)
        where T : notnull {
        if (failures == null) {
            throw new ArgumentNullException(nameof(failures));
        }

        var list = failures as IReadOnlyList<string> ?? failures.ToList();
        return list.Count == 0
                   ? Success(value)
                   : Failure<T>(new ValidationError(list, extra));
    }
}
