using System.Diagnostics;
using UnambitiousFx.Core.Results.Reasons;
using Xunit;

namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Represents a fluent assertion for failure cases, enabling chaining of custom assertions
///     or message validation on errors.
/// </summary>
[DebuggerStepThrough]
public readonly struct FailureAssertion
{
    /// <summary>
    ///     Represents the error collection used internally for failure assertions.
    /// </summary>
    private readonly IEnumerable<IError> _errors;

    /// <summary>
    ///     Represents a fluent assertion mechanism for handling failure cases in the context of test results.
    /// </summary>
    internal FailureAssertion(IEnumerable<IError> errors)
    {
        _errors = errors;
    }

    /// <summary>
    ///     Gets the errors associated with the failure assertion.
    /// </summary>
    /// <remarks>
    ///     Represents the error information related to a failed operation,
    ///     allowing further assertions or evaluations on the errors.
    /// </remarks>
    public IEnumerable<IError> Errors => _errors;

    /// <summary>
    ///     Applies the specified assertion action to the encapsulated errors and returns the current failure assertion
    ///     instance to allow method chaining.
    /// </summary>
    /// <param name="assert">The action to be applied to the encapsulated errors.</param>
    /// <returns>The current <see cref="FailureAssertion" /> instance to allow method chaining.</returns>
    public FailureAssertion And(Action<IEnumerable<IError>> assert)
    {
        assert(_errors);
        return this;
    }

    /// <summary>
    ///     Asserts that the message of the first error matches the expected value and continues the failure assertion chain.
    /// </summary>
    /// <param name="expected">The expected error message to assert against.</param>
    /// <returns>The current instance of <see cref="FailureAssertion" /> for further chaining.</returns>
    public FailureAssertion AndMessage(string expected)
    {
        var firstError = _errors.FirstOrDefault();
        var actualMessage = firstError?.Message ?? string.Empty;
        Assert.Equal(expected, actualMessage);
        return this;
    }
}
