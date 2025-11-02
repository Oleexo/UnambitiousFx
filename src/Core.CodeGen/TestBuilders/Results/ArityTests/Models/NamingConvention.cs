namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
/// Represents naming conventions extracted from existing test files.
/// </summary>
internal sealed class NamingConvention
{
    /// <summary>
    /// Gets the suffix used for test classes.
    /// </summary>
    public string TestClassSuffix { get; init; } = "Tests";

    /// <summary>
    /// Gets the pattern used for test method names.
    /// </summary>
    public string TestMethodPattern { get; init; }

    /// <summary>
    /// Gets the suffix used for success test methods.
    /// </summary>
    public string SuccessTestSuffix { get; init; }

    /// <summary>
    /// Gets the suffix used for failure test methods.
    /// </summary>
    public string FailureTestSuffix { get; init; }

    /// <summary>
    /// Gets the suffix used for exception test methods.
    /// </summary>
    public string ExceptionTestSuffix { get; init; }

    /// <summary>
    /// Gets the suffix used for async test methods.
    /// </summary>
    public string AsyncTestSuffix { get; init; }

    /// <summary>
    /// Initializes a new instance of the NamingConvention class.
    /// </summary>
    /// <param name="testClassSuffix">The test class suffix.</param>
    /// <param name="testMethodPattern">The test method pattern.</param>
    /// <param name="successTestSuffix">The success test suffix.</param>
    /// <param name="failureTestSuffix">The failure test suffix.</param>
    /// <param name="exceptionTestSuffix">The exception test suffix.</param>
    /// <param name="asyncTestSuffix">The async test suffix.</param>
    public NamingConvention(
        string testClassSuffix = "Tests",
        string testMethodPattern = "",
        string successTestSuffix = "",
        string failureTestSuffix = "",
        string exceptionTestSuffix = "",
        string asyncTestSuffix = "")
    {
        TestClassSuffix = testClassSuffix;
        TestMethodPattern = testMethodPattern;
        SuccessTestSuffix = successTestSuffix;
        FailureTestSuffix = failureTestSuffix;
        ExceptionTestSuffix = exceptionTestSuffix;
        AsyncTestSuffix = asyncTestSuffix;
    }
}