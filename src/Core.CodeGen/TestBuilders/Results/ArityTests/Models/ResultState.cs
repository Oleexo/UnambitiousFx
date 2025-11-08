namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents the possible states of a Result type.
/// </summary>
internal enum ResultState
{
    /// <summary>
    ///     The Result represents a successful operation.
    /// </summary>
    Success,

    /// <summary>
    ///     The Result represents a failed operation with errors.
    /// </summary>
    Failure,

    /// <summary>
    ///     The Result represents an operation that threw an exception.
    /// </summary>
    Exception,

    /// <summary>
    ///     The Result represents an edge case or boundary condition.
    /// </summary>
    EdgeCase
}
