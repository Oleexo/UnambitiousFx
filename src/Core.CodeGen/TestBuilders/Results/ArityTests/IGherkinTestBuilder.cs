using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
///     Interface for building test methods using Gherkin Given-When-Then structure.
/// </summary>
internal interface IGherkinTestBuilder
{
    /// <summary>
    ///     Builds a complete test method using Gherkin structure.
    /// </summary>
    /// <param name="methodName">The name of the method being tested.</param>
    /// <param name="arity">The arity of the Result type being tested.</param>
    /// <param name="scenario">The test scenario to generate.</param>
    /// <returns>The generated test method.</returns>
    TestMethod BuildGherkinTest(string methodName,
                                ushort arity,
                                TestScenario scenario);

    /// <summary>
    ///     Builds the Given section of a Gherkin test.
    /// </summary>
    /// <param name="testData">The test data for the Given section.</param>
    /// <returns>The Given section code.</returns>
    string BuildGivenSection(TestData testData);

    /// <summary>
    ///     Builds the When section of a Gherkin test.
    /// </summary>
    /// <param name="methodCall">The method call for the When section.</param>
    /// <returns>The When section code.</returns>
    string BuildWhenSection(string methodCall);

    /// <summary>
    ///     Builds the Then section of a Gherkin test.
    /// </summary>
    /// <param name="expectedResult">The expected result for the Then section.</param>
    /// <returns>The Then section code.</returns>
    string BuildThenSection(string expectedResult);
}
