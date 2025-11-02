namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;

/// <summary>
/// Defines the categories of test scenarios that can be generated.
/// </summary>
internal enum TestScenarioCategory
{
    /// <summary>
    /// Success scenarios where operations complete successfully.
    /// </summary>
    Success,

    /// <summary>
    /// Failure scenarios where operations fail with expected errors.
    /// </summary>
    Failure,

    /// <summary>
    /// Exception scenarios where operations throw exceptions.
    /// </summary>
    Exception,

    /// <summary>
    /// Edge case scenarios with boundary conditions and unusual inputs.
    /// </summary>
    EdgeCase,

    /// <summary>
    /// Async scenarios for testing asynchronous operations.
    /// </summary>
    Async,

    /// <summary>
    /// Performance scenarios for testing operation efficiency.
    /// </summary>
    Performance
}