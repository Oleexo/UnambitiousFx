using System.Diagnostics;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
/// Represents a fluent assertion for failure cases, enabling chaining of custom assertions
/// or message validation on exceptions.
/// </summary>
[DebuggerStepThrough]
public readonly struct FailureAssertion {
    /// <summary>
    /// Represents the exception instance used internally for failure assertions.
    /// </summary>
    private readonly Exception _error;

    /// <summary>
    /// Represents a fluent assertion mechanism for handling failure cases in the context of test results.
    /// </summary>
    internal FailureAssertion(Exception error) {
        _error = error;
    }

    /// <summary>
    /// Gets the exception associated with the failure assertion.
    /// </summary>
    /// <remarks>
    /// Represents the error information related to a failed operation,
    /// allowing further assertions or evaluations on the exception.
    /// </remarks>
    public Exception Error => _error;

    /// <summary>
    /// Applies the specified assertion action to the encapsulated exception and returns the current failure assertion instance to allow method chaining.
    /// </summary>
    /// <param name="assert">The action to be applied to the encapsulated exception.</param>
    /// <returns>The current <see cref="FailureAssertion"/> instance to allow method chaining.</returns>
    public FailureAssertion And(Action<Exception> assert) {
        assert(_error);
        return this;
    }

    /// <summary>
    /// Asserts that the message of the exception matches the expected value and continues the failure assertion chain.
    /// </summary>
    /// <param name="expected">The expected error message to assert against.</param>
    /// <returns>The current instance of <see cref="FailureAssertion"/> for further chaining.</returns>
    public FailureAssertion AndMessage(string expected) {
        Assert.Equal(expected, _error.Message);
        return this;
    }
}