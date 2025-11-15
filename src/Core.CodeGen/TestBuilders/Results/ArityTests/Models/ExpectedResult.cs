namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents the expected result for a test scenario.
/// </summary>
internal sealed class ExpectedResult
{
    /// <summary>
    ///     Initializes a new instance of the ExpectedResult class.
    /// </summary>
    /// <param name="state">The expected state.</param>
    /// <param name="assertionCode">The assertion code.</param>
    /// <param name="validationChecks">The validation checks.</param>
    public ExpectedResult(ResultState state,
                          string assertionCode,
                          IEnumerable<string> validationChecks)
    {
        State = state;
        AssertionCode = assertionCode ?? throw new ArgumentNullException(nameof(assertionCode));
        ValidationChecks = validationChecks ?? throw new ArgumentNullException(nameof(validationChecks));
    }

    /// <summary>
    ///     Gets the expected Result state.
    /// </summary>
    public ResultState State { get; init; }

    /// <summary>
    ///     Gets the assertion code to validate the result.
    /// </summary>
    public string AssertionCode { get; init; }

    /// <summary>
    ///     Gets additional validation checks for the result.
    /// </summary>
    public IEnumerable<string> ValidationChecks { get; init; }
}
