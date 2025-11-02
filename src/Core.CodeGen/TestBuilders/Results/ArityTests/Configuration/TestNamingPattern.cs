namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;

/// <summary>
/// Defines naming patterns for generated test methods and classes.
/// </summary>
internal sealed class TestNamingPattern
{
    /// <summary>
    /// Gets the suffix for test classes.
    /// </summary>
    public string TestClassSuffix { get; init; } = "Tests";

    /// <summary>
    /// Gets the pattern for test method names.
    /// </summary>
    public string TestMethodPattern { get; init; } = "{MethodName}_ForArity{Arity}{Scenario}_Should{ExpectedBehavior}";

    /// <summary>
    /// Gets the suffix for success test scenarios.
    /// </summary>
    public string SuccessTestSuffix { get; init; } = "Success";

    /// <summary>
    /// Gets the suffix for failure test scenarios.
    /// </summary>
    public string FailureTestSuffix { get; init; } = "Failure";

    /// <summary>
    /// Gets the suffix for exception test scenarios.
    /// </summary>
    public string ExceptionTestSuffix { get; init; } = "Exception";

    /// <summary>
    /// Gets the suffix for async test scenarios.
    /// </summary>
    public string AsyncTestSuffix { get; init; } = "Async";

    /// <summary>
    /// Gets the suffix for edge case test scenarios.
    /// </summary>
    public string EdgeCaseTestSuffix { get; init; } = "EdgeCase";
}