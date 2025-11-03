namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents assertion patterns extracted from existing test files.
/// </summary>
internal sealed class AssertionPattern {
    /// <summary>
    ///     Initializes a new instance of the AssertionPattern class.
    /// </summary>
    /// <param name="assertionLibrary">The assertion library.</param>
    /// <param name="successPatterns">The success patterns.</param>
    /// <param name="failurePatterns">The failure patterns.</param>
    /// <param name="exceptionPatterns">The exception patterns.</param>
    /// <param name="valuePatterns">The value patterns.</param>
    public AssertionPattern(string              assertionLibrary,
                            IEnumerable<string> successPatterns,
                            IEnumerable<string> failurePatterns,
                            IEnumerable<string> exceptionPatterns,
                            IEnumerable<string> valuePatterns) {
        AssertionLibrary  = assertionLibrary  ?? throw new ArgumentNullException(nameof(assertionLibrary));
        SuccessPatterns   = successPatterns   ?? throw new ArgumentNullException(nameof(successPatterns));
        FailurePatterns   = failurePatterns   ?? throw new ArgumentNullException(nameof(failurePatterns));
        ExceptionPatterns = exceptionPatterns ?? throw new ArgumentNullException(nameof(exceptionPatterns));
        ValuePatterns     = valuePatterns     ?? throw new ArgumentNullException(nameof(valuePatterns));
    }

    /// <summary>
    ///     Gets the common assertion library used in tests.
    /// </summary>
    public string AssertionLibrary { get; init; }

    /// <summary>
    ///     Gets the patterns used for success assertions.
    /// </summary>
    public IEnumerable<string> SuccessPatterns { get; init; }

    /// <summary>
    ///     Gets the patterns used for failure assertions.
    /// </summary>
    public IEnumerable<string> FailurePatterns { get; init; }

    /// <summary>
    ///     Gets the patterns used for exception assertions.
    /// </summary>
    public IEnumerable<string> ExceptionPatterns { get; init; }

    /// <summary>
    ///     Gets the patterns used for value assertions.
    /// </summary>
    public IEnumerable<string> ValuePatterns { get; init; }
}
