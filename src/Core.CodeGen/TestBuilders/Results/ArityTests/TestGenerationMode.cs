namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
/// Defines the modes of test generation for controlling scope and depth.
/// </summary>
internal enum TestGenerationMode
{
    /// <summary>
    /// Generate only core functionality tests, minimal coverage.
    /// </summary>
    Minimal,

    /// <summary>
    /// Generate comprehensive tests including all scenarios and edge cases.
    /// </summary>
    Comprehensive,

    /// <summary>
    /// Generate focused tests for specific method/arity combinations.
    /// </summary>
    Focused
}