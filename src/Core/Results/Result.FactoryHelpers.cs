namespace UnambitiousFx.Core.Results;

using System; // Needed for Func<> and exceptions
using System.Collections.Generic;
using System.Linq;
using UnambitiousFx.Core.Results.Reasons;

public abstract partial class Result {
    /// <summary>
    /// Creates a Result<T> from a possibly null value. If the value is non-null, returns Success(value); otherwise creates a failure using the provided error factory.
    /// </summary>
    /// <param name="value">The value to evaluate for nullability.</param>
    /// <param name="errorFactory">A factory function to create an error if the value is null.</param>
    /// <returns>
    /// A Result<T> that represents a success if the value is non-null, or a failure with the error produced by <paramref name="errorFactory"/> if the value is null.
    /// </returns>
    public static Result<T> FromNullable<T>(T?           value,
                                            Func<IError> errorFactory)
        where T : notnull {
        if (errorFactory == null) throw new ArgumentNullException(nameof(errorFactory));
        return value is not null ? Success(value) : Failure<T>(errorFactory());
    }

    /// <summary>
    /// Creates a Result<T> from a possibly null value. If the value is null, returns a NotFoundError using the provided resource and identifier.
    /// </summary>
    /// <param name="value">The value to check for null.</param>
    /// <param name="resource">The resource name associated with the value.</param>
    /// <param name="identifier">The identifier of the resource.</param>
    /// <typeparam name="T">The type of the value, which must be non-nullable.</typeparam>
    /// <returns>
    /// A Result<T> which is a success if the value is not null,
    /// or a failure with a NotFoundError if the value is null.
    /// </returns>
    public static Result<T> FromNullable<T>(T?     value,
                                            string resource,
                                            string identifier)
        where T : notnull {
        return value is not null
                   ? Success(value) : Failure<T>(new NotFoundError(resource, identifier));
    }

    /// <summary>
    /// Creates a Result (non-generic) based on a boolean condition. Returns a Success result if the condition is true; otherwise, returns a Failure result using the provided error factory.
    /// </summary>
    /// <param name="condition">A boolean value determining the success or failure of the result.</param>
    /// <param name="errorFactory">A function that produces an error to populate the Failure result if the condition is false.</param>
    /// <returns>A Success result if the condition is true, or a Failure result with the error created by the error factory if the condition is false.</returns>
    public static Result FromCondition(bool         condition,
                                       Func<IError> errorFactory) {
        if (errorFactory == null) throw new ArgumentNullException(nameof(errorFactory));
        return condition ? Success() : Failure(errorFactory());
    }

    /// <summary>
    /// Creates a Result<T> by applying a predicate to the provided value. If the predicate returns true, the method returns a success result containing the value; otherwise, it creates a failure using the provided error factory.
    /// </summary>
    /// <param name="value">The value to be validated by the predicate.</param>
    /// <param name="predicate">A function that determines whether the value is valid.</param>
    /// <param name="errorFactory">A factory function that produces an error when the predicate fails.</param>
    /// <returns>A Result containing the value if the predicate succeeds; otherwise, a failure Result with the generated error.</returns>
    public static Result<T> FromCondition<T>(T             value,
                                             Func<T, bool> predicate,
                                             Func<IError>  errorFactory) where T : notnull {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (errorFactory == null) throw new ArgumentNullException(nameof(errorFactory));
        return predicate(value) ? Success(value) : Failure<T>(errorFactory());
    }

    /// <summary>
    /// Builds a non-generic Result from validation failures. Returns Success() if the collection of failures is empty; otherwise, returns Failure containing a ValidationError.
    /// </summary>
    /// <param name="failures">A collection of validation failure messages.</param>
    /// <param name="extra">An optional dictionary containing additional metadata related to the validation errors.</param>
    /// <returns>A Result indicating success or failure depending on the presence of validation failures.</returns>
    public static Result FromValidation(IEnumerable<string>                   failures,
                                        IReadOnlyDictionary<string, object?>? extra = null) {
        if (failures == null) throw new ArgumentNullException(nameof(failures));
        var list = failures as IReadOnlyList<string> ?? failures.ToList();
        return list.Count == 0 ? Success() : Failure(new ValidationError(list, extra));
    }

    /// <summary>
    /// Creates a Result<T> from validation failures. If no failures are provided, returns Success(value); otherwise, creates a failure with the specified validation errors.
    /// </summary>
    /// <param name="value">The value to return if validation succeeds.</param>
    /// <param name="failures">A collection of validation failure messages. If not empty, a failure result is created.</param>
    /// <param name="extra">Optional additional metadata to include with the validation errors.</param>
    /// <returns>A Result<T> that represents the outcome of the validation.</returns>
    public static Result<T> FromValidation<T>(T                                     value,
                                              IEnumerable<string>                   failures,
                                              IReadOnlyDictionary<string, object?>? extra = null) where T : notnull {
        if (failures == null) throw new ArgumentNullException(nameof(failures));
        var list = failures as IReadOnlyList<string> ?? failures.ToList();
        return list.Count == 0 ? Success(value) : Failure<T>(new ValidationError(list, extra));
    }
}
