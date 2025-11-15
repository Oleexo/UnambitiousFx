using System.Text;
using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Configuration;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Utilities;

/// <summary>
///     Generates assertion code for Result test scenarios using Core.XUnit patterns.
/// </summary>
internal sealed class ResultAssertionGenerator
{
    /// <summary>
    ///     Generates assertion code for successful Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for success scenarios.</returns>
    public string GenerateSuccessAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Use Core.XUnit assertion patterns for success scenarios
        if (scenario.Arity == 1)
        {
            sb.AppendLine("result.ShouldBeSuccess(out var value);");
            sb.AppendLine($"Assert.Equal({TestTypeHelper.GetTestValue(1)}, value);");
        }
        else
        {
            sb.AppendLine("result.ShouldBeSuccess(out var values);");
            GenerateMultiArityValueAssertions(sb, scenario.Arity);
        }

        // Method-specific success assertions
        switch (scenario.MethodName)
        {
            case "Match":
                sb.AppendLine("// Match should execute success action");
                break;
            case "IfSuccess":
                sb.AppendLine("// Action should have been executed");
                break;
            case "TryGet":
                sb.AppendLine("Assert.True(result);");
                GenerateTryGetValueAssertions(sb, scenario.Arity);
                break;
            case "ValueOr":
                GenerateValueOrSuccessAssertions(sb, scenario.Arity);
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Generates assertion code for failed Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for failure scenarios.</returns>
    public string GenerateFailureAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Use Core.XUnit assertion patterns for failure scenarios
        sb.AppendLine("result.ShouldBeFailure(out var errors);");
        sb.AppendLine("var firstError = errors.First();");
        sb.AppendLine("Assert.Equal(\"Test error\", firstError.Message);");

        // Method-specific failure assertions
        switch (scenario.MethodName)
        {
            case "Match":
                sb.AppendLine("// Match should execute failure action");
                break;
            case "IfSuccess":
                sb.AppendLine("// Action should not have been executed");
                break;
            case "IfFailure":
                sb.AppendLine("// Error action should have been executed");
                break;
            case "TryGet":
                sb.AppendLine("Assert.False(result);");
                GenerateTryGetFailureAssertions(sb, scenario.Arity);
                break;
            case "ValueOr":
                GenerateValueOrFailureAssertions(sb, scenario.Arity);
                break;
            case "Map":
            case "Bind":
                sb.AppendLine("// Result should preserve failure state");
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Generates assertion code for exception Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for exception scenarios.</returns>
    public string GenerateExceptionAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Use Core.XUnit assertion patterns for exception scenarios
        sb.AppendLine("result.ShouldBeFailure(out var errors);");
        sb.AppendLine("var firstError = errors.First();");
        sb.AppendLine("Assert.IsType<InvalidOperationException>(firstError.Exception);");
        sb.AppendLine("Assert.Equal(\"Test exception\", firstError.Exception.Message);");

        // Method-specific exception assertions
        switch (scenario.MethodName)
        {
            case "TryGet":
                sb.AppendLine("Assert.False(result);");
                GenerateTryGetFailureAssertions(sb, scenario.Arity);
                break;
            case "ValueOrThrow":
                sb.AppendLine("// Should throw the contained exception");
                break;
            default:
                sb.AppendLine("// Exception should be preserved in result");
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Generates assertion code for edge case Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for edge case scenarios.</returns>
    public string GenerateEdgeCaseAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Basic edge case assertions (usually success with edge values)
        sb.AppendLine("Assert.True(result.IsSuccess);");
        sb.AppendLine("Assert.False(result.IsFaulted);");

        // Edge case specific assertions
        switch (scenario.MethodName)
        {
            case "Map":
            case "Bind":
                GenerateEdgeCaseValueAssertions(sb, scenario.Arity);
                break;
            case "TryGet":
                sb.AppendLine("Assert.True(result);");
                GenerateEdgeCaseTryGetAssertions(sb, scenario.Arity);
                break;
            case "ValueOr":
                GenerateEdgeCaseValueOrAssertions(sb, scenario.Arity);
                break;
            default:
                sb.AppendLine("// Edge case values should be handled correctly");
                GenerateEdgeCaseValueAssertions(sb, scenario.Arity);
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Generates assertion code for async Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for async scenarios.</returns>
    public string GenerateAsyncAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Async-specific assertions
        sb.AppendLine("Assert.NotNull(result);");
        sb.AppendLine("Assert.True(result.IsCompleted);");

        // Delegate to appropriate assertion based on expected outcome
        switch (scenario.ExpectedResult.State)
        {
            case ResultState.Success:
                sb.AppendLine(GenerateSuccessAssertion(scenario));
                break;
            case ResultState.Failure:
                sb.AppendLine(GenerateFailureAssertion(scenario));
                break;
            case ResultState.Exception:
                sb.AppendLine(GenerateExceptionAssertion(scenario));
                break;
            default:
                sb.AppendLine("Assert.True(result.IsSuccess);");
                break;
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Generates assertion code for performance Result scenarios.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <returns>Assertion code for performance scenarios.</returns>
    public string GeneratePerformanceAssertion(TestScenario scenario)
    {
        var sb = new StringBuilder();

        // Performance-specific assertions
        sb.AppendLine("Assert.NotNull(result);");
        sb.AppendLine("// Performance assertions would be added here");
        sb.AppendLine("// e.g., execution time, memory usage, etc.");

        // Delegate to success assertion for functional correctness
        sb.AppendLine(GenerateSuccessAssertion(scenario));

        return sb.ToString();
    }

    /// <summary>
    ///     Generates clear assertion messages with arity-specific information.
    /// </summary>
    /// <param name="methodName">The method being tested.</param>
    /// <param name="arity">The arity of the Result type.</param>
    /// <param name="scenario">The test scenario type.</param>
    /// <returns>Descriptive assertion message.</returns>
    public string GenerateAssertionMessage(string methodName,
                                           ushort arity,
                                           TestScenarioCategory scenario)
    {
        var arityText = arity == 1
                            ? "single value"
                            : $"{arity} values";
        var scenarioText = scenario.ToString()
                                   .ToLowerInvariant();

        return $"Result<{GenerateArityTypeString(arity)}> {methodName} method should handle {scenarioText} scenario with {arityText} correctly";
    }

    /// <summary>
    ///     Generates assertion code with custom message for better test clarity.
    /// </summary>
    /// <param name="scenario">The test scenario.</param>
    /// <param name="customMessage">Custom assertion message.</param>
    /// <returns>Assertion code with custom message.</returns>
    public string GenerateAssertionWithMessage(TestScenario scenario,
                                               string customMessage)
    {
        var sb = new StringBuilder();

        // Add custom message as comment
        sb.AppendLine($"// {customMessage}");

        // Generate appropriate assertion based on scenario type
        var assertionCode = scenario.Type switch
        {
            TestScenarioCategory.Success => GenerateSuccessAssertion(scenario),
            TestScenarioCategory.Failure => GenerateFailureAssertion(scenario),
            TestScenarioCategory.Exception => GenerateExceptionAssertion(scenario),
            TestScenarioCategory.EdgeCase => GenerateEdgeCaseAssertion(scenario),
            TestScenarioCategory.Async => GenerateAsyncAssertion(scenario),
            TestScenarioCategory.Performance => GeneratePerformanceAssertion(scenario),
            _ => GenerateSuccessAssertion(scenario)
        };

        sb.AppendLine(assertionCode);
        return sb.ToString();
    }

    private void GenerateMultiArityValueAssertions(StringBuilder sb,
                                                   ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var expectedValue = TestTypeHelper.GetTestValue(i);
            sb.AppendLine($"Assert.Equal({expectedValue}, values.Item{i});");
        }
    }

    private void GenerateTryGetValueAssertions(StringBuilder sb,
                                               ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var expectedValue = TestTypeHelper.GetTestValue(i);
            sb.AppendLine($"Assert.Equal({expectedValue}, outValue{i});");
        }
    }

    private void GenerateTryGetFailureAssertions(StringBuilder sb,
                                                 ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var defaultValue = GetDefaultValue(TestTypeHelper.GetTestType(i));
            sb.AppendLine($"Assert.Equal({defaultValue}, outValue{i});");
        }
    }

    private void GenerateValueOrSuccessAssertions(StringBuilder sb,
                                                  ushort arity)
    {
        if (arity == 1)
        {
            sb.AppendLine($"Assert.Equal({TestTypeHelper.GetTestValue(1)}, result);");
        }
        else
        {
            for (var i = 1; i <= arity; i++)
            {
                var expectedValue = TestTypeHelper.GetTestValue(i);
                sb.AppendLine($"Assert.Equal({expectedValue}, result.Item{i});");
            }
        }
    }

    private void GenerateValueOrFailureAssertions(StringBuilder sb,
                                                  ushort arity)
    {
        if (arity == 1)
        {
            sb.AppendLine("Assert.Equal(default1, result);");
        }
        else
        {
            for (var i = 1; i <= arity; i++)
            {
                sb.AppendLine($"Assert.Equal(default{i}, result.Item{i});");
            }
        }
    }

    private void GenerateEdgeCaseValueAssertions(StringBuilder sb,
                                                 ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var typeName = TestTypeHelper.GetTestType(i);
            var edgeValue = GetEdgeCaseValue(typeName);
            sb.AppendLine($"Assert.Equal({edgeValue}, result.Value{i});");
        }
    }

    private void GenerateEdgeCaseTryGetAssertions(StringBuilder sb,
                                                  ushort arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            var typeName = TestTypeHelper.GetTestType(i);
            var edgeValue = GetEdgeCaseValue(typeName);
            sb.AppendLine($"Assert.Equal({edgeValue}, outValue{i});");
        }
    }

    private void GenerateEdgeCaseValueOrAssertions(StringBuilder sb,
                                                   ushort arity)
    {
        if (arity == 1)
        {
            var edgeValue = GetEdgeCaseValue(TestTypeHelper.GetTestType(1));
            sb.AppendLine($"Assert.Equal({edgeValue}, result);");
        }
        else
        {
            for (var i = 1; i <= arity; i++)
            {
                var typeName = TestTypeHelper.GetTestType(i);
                var edgeValue = GetEdgeCaseValue(typeName);
                sb.AppendLine($"Assert.Equal({edgeValue}, result.Item{i});");
            }
        }
    }

    private string GetDefaultValue(string typeName)
    {
        return typeName switch
        {
            "int" => "0",
            "string" => "null",
            "bool" => "false",
            "double" => "0.0",
            "decimal" => "0m",
            "long" => "0L",
            "char" => "'\\0'",
            "float" => "0f",
            _ => "default"
        };
    }

    private string GetEdgeCaseValue(string typeName)
    {
        return typeName switch
        {
            "int" => "int.MaxValue",
            "string" => "string.Empty",
            "bool" => "false",
            "double" => "double.MaxValue",
            "decimal" => "decimal.MaxValue",
            "long" => "long.MaxValue",
            "char" => "'\\0'",
            "float" => "float.MaxValue",
            _ => TestTypeHelper.GetTestValue(1)
        };
    }

    private string GenerateArityTypeString(ushort arity)
    {
        var types = new List<string>();
        for (var i = 1; i <= arity; i++)
        {
            types.Add(TestTypeHelper.GetTestType(i));
        }

        return string.Join(", ", types);
    }
}
