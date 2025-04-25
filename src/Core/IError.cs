namespace Oleexo.UnambitiousFx.Core;

/// <summary>
///     Represents a contract for defining application errors with extended information.
/// </summary>
public interface IError {
    /// <summary>
    ///     Gets the descriptive message associated with the error.
    /// </summary>
    /// <remarks>
    ///     This property provides details about the nature of the error, typically in the form of
    ///     a human-readable string that describes what went wrong. The value is assigned during the
    ///     error's creation and cannot be modified afterward.
    /// </remarks>
    string Message { get; }

    /// <summary>
    ///     Gets the unique identifier string for the specific error.
    ///     The property is used to distinguish and categorize errors
    ///     within the system for easier handling and debugging.
    /// </summary>
    string Code { get; }

    /// <summary>
    ///     Represents an optional exception associated with the current error.
    ///     Provides additional context about the error when available.
    /// </summary>
    /// <remarks>
    ///     This property retrieves the exception associated with the error, if any.
    ///     It is useful for tracking error details or debugging purposes where a specific exception
    ///     is linked to the error.
    /// </remarks>
    /// <value>
    ///     The exception instance that caused the error or null if no exception is available.
    /// </value>
    Exception? Exception { get; }

    /// <summary>
    ///     A collection of child errors associated with the current error.
    ///     This property helps capture additional context or details by linking related errors together.
    /// </summary>
    IEnumerable<IError> Children { get; }

    /// Provides additional information related to the error.
    /// This property contains a dictionary where the keys represent custom metadata keys associated with the error,
    /// and the values contain corresponding information.
    /// It allows users to attach meaningful and context-specific details to the error object.
    Dictionary<string, string> AdditionalInfo { get; }
}
