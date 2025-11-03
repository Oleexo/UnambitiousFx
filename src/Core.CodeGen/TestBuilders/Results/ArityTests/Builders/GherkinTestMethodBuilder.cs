using System.Text;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Utilities;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Builders;

/// <summary>
///     Builds test methods using Gherkin Given-When-Then structure.
/// </summary>
internal sealed class GherkinTestMethodBuilder : IGherkinTestBuilder {
    private readonly ResultAssertionGenerator _assertionGenerator;

    /// <summary>
    ///     Initializes a new instance of the GherkinTestMethodBuilder class.
    /// </summary>
    public GherkinTestMethodBuilder() {
        _assertionGenerator = new ResultAssertionGenerator();
    }

    /// <summary>
    ///     Builds a complete test method using Gherkin structure.
    /// </summary>
    /// <param name="methodName">The name of the method being tested.</param>
    /// <param name="arity">The arity of the Result type being tested.</param>
    /// <param name="scenario">The test scenario to generate.</param>
    /// <returns>The generated test method.</returns>
    public TestMethod BuildGherkinTest(string       methodName,
                                       ushort       arity,
                                       TestScenario scenario) {
        var testName     = GenerateTestName(methodName, arity, scenario);
        var givenSection = BuildGivenSection(scenario.InputData);
        var whenSection  = BuildWhenSection(GenerateMethodCall(methodName, arity, scenario));

        // Generate assertion code using the ResultAssertionGenerator
        var assertionCode = GenerateAssertionCode(scenario);
        var thenSection   = BuildThenSection(assertionCode);

        var body       = CombineGherkinSections(givenSection, whenSection, thenSection);
        var attributes = GenerateTestAttributes(scenario);

        return new TestMethod(
            testName,
            body,
            attributes,
            "void",
            "public"
        );
    }

    /// <summary>
    ///     Builds the Given section of a Gherkin test.
    /// </summary>
    /// <param name="testData">The test data for the Given section.</param>
    /// <returns>The Given section code.</returns>
    public string BuildGivenSection(TestData testData) {
        var sb = new StringBuilder();
        sb.AppendLine("// Given: Test data is prepared");

        if (!string.IsNullOrWhiteSpace(testData.SetupCode)) {
            sb.AppendLine(testData.SetupCode);
        }

        // Generate variable declarations for test values
        var valueIndex = 1;
        foreach (var typeValue in testData.Values) {
            sb.AppendLine($"var value{valueIndex} = {typeValue.TestValue};");
            valueIndex++;
        }

        // Generate Result creation based on expected state
        switch (testData.ExpectedState) {
            case ResultState.Success:
                sb.AppendLine(GenerateSuccessResultCreation(testData));
                break;
            case ResultState.Failure:
                sb.AppendLine(GenerateFailureResultCreation(testData));
                break;
            case ResultState.Exception:
                sb.AppendLine(GenerateExceptionResultCreation(testData));
                break;
            case ResultState.EdgeCase:
                sb.AppendLine(GenerateEdgeCaseResultCreation(testData));
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Builds the When section of a Gherkin test.
    /// </summary>
    /// <param name="methodCall">The method call for the When section.</param>
    /// <returns>The When section code.</returns>
    public string BuildWhenSection(string methodCall) {
        var sb = new StringBuilder();
        sb.AppendLine("// When: The method is executed");
        sb.AppendLine($"var result = {methodCall};");
        return sb.ToString();
    }

    /// <summary>
    ///     Builds the Then section of a Gherkin test.
    /// </summary>
    /// <param name="expectedResult">The expected result for the Then section.</param>
    /// <returns>The Then section code.</returns>
    public string BuildThenSection(string expectedResult) {
        var sb = new StringBuilder();
        sb.AppendLine("// Then: The result should match expectations");
        sb.AppendLine(expectedResult);
        return sb.ToString();
    }

    private string GenerateTestName(string       methodName,
                                    ushort       arity,
                                    TestScenario scenario) {
        var scenarioSuffix = scenario.Type switch {
            TestScenarioCategory.Success   => "Success",
            TestScenarioCategory.Failure   => "Failure",
            TestScenarioCategory.Exception => "Exception",
            TestScenarioCategory.EdgeCase  => "EdgeCase",
            TestScenarioCategory.Async     => "Async",
            _                              => "Test"
        };

        return $"{methodName}_ForArity{arity}{scenarioSuffix}_Should{GetExpectedBehavior(methodName, scenario)}";
    }

    private string GetExpectedBehavior(string       methodName,
                                       TestScenario scenario) {
        return methodName switch {
            "Match" => scenario.Type == TestScenarioCategory.Success
                           ? "ExecuteSuccessAction"
                           : "ExecuteFailureAction",
            "IfSuccess" => scenario.Type == TestScenarioCategory.Success
                               ? "ExecuteAction"
                               : "NotExecuteAction",
            "IfFailure" => scenario.Type == TestScenarioCategory.Failure
                               ? "ExecuteAction"
                               : "NotExecuteAction",
            "TryGet" => scenario.Type == TestScenarioCategory.Success
                            ? "ReturnTrueWithValues"
                            : "ReturnFalse",
            "Map" => scenario.Type == TestScenarioCategory.Success
                         ? "TransformValues"
                         : "PreserveFailure",
            "Bind" => scenario.Type == TestScenarioCategory.Success
                          ? "FlattenResult"
                          : "PreserveFailure",
            "Ensure" => scenario.Type == TestScenarioCategory.Success
                            ? "PassValidation"
                            : "FailValidation",
            "Tap" => "ExecuteSideEffect",
            "ValueOr" => scenario.Type == TestScenarioCategory.Success
                             ? "ReturnValues"
                             : "ReturnDefaultValues",
            _ => "BehaveCorrectly"
        };
    }

    private string GenerateMethodCall(string       methodName,
                                      ushort       arity,
                                      TestScenario scenario) {
        return methodName switch {
            "Match"     => "result.Match(success => success, failure => failure)",
            "IfSuccess" => "result.IfSuccess(values => { /* action */ })",
            "IfFailure" => "result.IfFailure(errors => { /* action */ })",
            "TryGet"    => GenerateTryGetCall(arity),
            "Map"       => GenerateMapCall(arity),
            "Bind"      => "result.Bind(values => Result.Success(values))",
            "Ensure"    => "result.Ensure(values => true, \"validation failed\")",
            "Tap"       => "result.Tap(values => { /* side effect */ })",
            "ValueOr"   => GenerateValueOrCall(arity),
            _           => $"result.{methodName}()"
        };
    }

    private string GenerateTryGetCall(ushort arity) {
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var outValue{i}"));
        return $"result.TryGet({outParams})";
    }

    private string GenerateMapCall(ushort arity) {
        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"value{i}"));
        return $"result.Map(({parameters}) => ({parameters}))";
    }

    private string GenerateValueOrCall(ushort arity) {
        var defaultValues = string.Join(", ", Enumerable.Range(1, arity)
                                                        .Select(i => $"default{i}"));
        return $"result.ValueOr({defaultValues})";
    }

    private string GenerateSuccessResultCreation(TestData testData) {
        var values = string.Join(", ", testData.Values.Select(v => v.TestValue));
        return $"var result = Result.Success({values});";
    }

    private string GenerateFailureResultCreation(TestData testData) {
        return "var result = Result.Failure<"                             +
               string.Join(", ", testData.Values.Select(v => v.TypeName)) +
               ">(\"Test error\");";
    }

    private string GenerateExceptionResultCreation(TestData testData) {
        return "var result = Result.Exception<"                           +
               string.Join(", ", testData.Values.Select(v => v.TypeName)) +
               ">(new InvalidOperationException(\"Test exception\"));";
    }

    private string GenerateEdgeCaseResultCreation(TestData testData) {
        // For edge cases, create a success result with edge case values
        var edgeValues = testData.Values.Select(v =>
                                                    v.TypeName switch {
                                                        "string" => "string.Empty",
                                                        "int"    => "int.MaxValue",
                                                        "bool"   => "false",
                                                        _        => v.TestValue
                                                    });

        var values = string.Join(", ", edgeValues);
        return $"var result = Result.Success({values});";
    }

    private string CombineGherkinSections(string givenSection,
                                          string whenSection,
                                          string thenSection) {
        var sb = new StringBuilder();
        sb.AppendLine(givenSection);
        sb.AppendLine();
        sb.AppendLine(whenSection);
        sb.AppendLine();
        sb.AppendLine(thenSection);
        return sb.ToString();
    }

    private IEnumerable<string> GenerateTestAttributes(TestScenario scenario) {
        yield return "Fact";

        if (scenario.Type == TestScenarioCategory.Async) {
            yield return "Async";
        }
    }

    /// <summary>
    ///     Generates assertion code based on the test scenario using ResultAssertionGenerator.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>The generated assertion code.</returns>
    private string GenerateAssertionCode(TestScenario scenario) {
        return scenario.Type switch {
            TestScenarioCategory.Success     => _assertionGenerator.GenerateSuccessAssertion(scenario),
            TestScenarioCategory.Failure     => _assertionGenerator.GenerateFailureAssertion(scenario),
            TestScenarioCategory.Exception   => _assertionGenerator.GenerateExceptionAssertion(scenario),
            TestScenarioCategory.EdgeCase    => _assertionGenerator.GenerateEdgeCaseAssertion(scenario),
            TestScenarioCategory.Async       => _assertionGenerator.GenerateAsyncAssertion(scenario),
            TestScenarioCategory.Performance => _assertionGenerator.GeneratePerformanceAssertion(scenario),
            _                                => _assertionGenerator.GenerateSuccessAssertion(scenario)
        };
    }
}
