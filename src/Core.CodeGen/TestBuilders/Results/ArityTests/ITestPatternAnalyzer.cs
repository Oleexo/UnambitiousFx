using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
/// Interface for analyzing existing test patterns to extract conventions and structures.
/// </summary>
internal interface ITestPatternAnalyzer
{
    /// <summary>
    /// Analyzes existing test files in the specified directory to extract patterns.
    /// </summary>
    /// <param name="testDirectory">The directory containing existing test files.</param>
    /// <returns>The extracted test pattern information.</returns>
    TestPattern AnalyzeExistingTests(string testDirectory);

    /// <summary>
    /// Extracts naming conventions from existing test files.
    /// </summary>
    /// <returns>The naming convention pattern.</returns>
    NamingConvention ExtractNamingConvention();

    /// <summary>
    /// Extracts assertion patterns from existing test files.
    /// </summary>
    /// <returns>The assertion pattern information.</returns>
    AssertionPattern ExtractAssertionPattern();
}