namespace Oleexo.UnambitiousFx.Core;

/// <summary>
///     Represents an application error with basic details such as message, code, exception,
///     additional information, and child errors. Provides utilities to add additional context to errors
///     and supports hierarchical error structures.
/// </summary>
public record Error : IError {
    /// <summary>
    ///     Represents the universal code for aggregate errors where multiple errors are present.
    ///     This constant value is typically used as a default code for wrapping or representing
    ///     a collection of errors, providing a consistent way to identify aggregate error scenarios
    ///     across the application.
    /// </summary>
    public const string AggregateErrorCode = "AGGREGATE_ERROR";

    private Dictionary<string, string>? _additionalInfo;
    private IEnumerable<IError>?        _children;

    /// <summary>
    ///     Represents an error that encapsulates a message, optional error code, exception details,
    ///     additional contextual information, and supports hierarchical organization of child errors.
    /// </summary>
    public Error(string  message,
                 string? code = null) {
        Message = message;
        if (code is not null) {
            Code = code;
        }
    }

    /// <summary>
    ///     Represents an application error, encapsulating a message, optional code, additional context, and
    ///     hierarchical child errors for more detailed error representation.
    /// </summary>
    public Error(IEnumerable<IError> errors,
                 string?             message = null,
                 string?             code    = null) {
        AddChildren(errors);
        Message = message ?? "Multiple errors occurred. See Children for details.";
        if (code is not null) {
            Code = code;
        }
        else {
            Code = AggregateErrorCode;
        }
    }

    /// <summary>
    ///     Represents an application error, providing details such as message, code, exception,
    ///     additional contextual information, and a hierarchical structure for child errors.
    /// </summary>
    public Error(Exception exception,
                 string?   code = null)
        : this(exception.Message) {
        Exception = exception;
        if (code is not null) {
            Code = code;
        }
    }

    /// <inheritdoc />
    public IEnumerable<IError> Children => _children ??= [];

    /// <inheritdoc />
    public string Message { get; }

    /// <inheritdoc />
    public string Code { get; } = string.Empty;

    /// <inheritdoc />
    public Exception? Exception { get; }

    /// <inheritdoc />
    public Dictionary<string, string> AdditionalInfo => _additionalInfo ??= new Dictionary<string, string>();

    /// <summary>
    ///     Adds a collection of child errors to the current error, allowing for hierarchical error organization.
    /// </summary>
    /// <param name="errors">The collection of errors to add as children.</param>
    protected void AddChildren(IEnumerable<IError> errors) {
        _children = _children is null
                        ? errors
                        : _children.Concat(errors);
    }

    /// <summary>
    ///     Defines an implicit conversion operator that creates an instance of the <see cref="Error" /> class
    ///     from an <see cref="Exception" />, preserving the exception message as the error message and optionally
    ///     setting other error details.
    /// </summary>
    /// <param name="exception">The exception to convert into an error.</param>
    /// <returns>An <see cref="Error" /> object initialized with the provided exception details.</returns>
    public static implicit operator Error(Exception exception) {
        return new Error(exception);
    }

    /// <summary>
    ///     Defines an implicit conversion operator that creates an <see cref="Error" /> instance
    ///     from a provided string message.
    /// </summary>
    /// <param name="message">The error message to create the <see cref="Error" /> instance with.</param>
    /// <returns>An <see cref="Error" /> instance with the specified message.</returns>
    public static implicit operator Error(string message) {
        return new Error(message);
    }
}
