using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Simple test generator for Result direct methods (Match, IfSuccess, IfFailure, TryGet).
///     Generates comprehensive tests for all arities following the same patterns as production code generators.
/// </summary>
internal sealed class ResultDirectMethodsTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultTests";
    private const string ExtensionsNamespace = "Results";

    /// <summary>
    ///     The direct methods that this generator creates tests for.
    /// </summary>
    private static readonly string[] DirectMethods = ["Match", "IfSuccess", "IfFailure", "TryGet"];

    public ResultDirectMethodsTestsGenerator(string               baseNamespace,
                                             FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(
                   baseNamespace,
                   StartArity,
                   ExtensionsNamespace,
                   ClassName,
                   fileOrganization,
                   true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var result = new List<ClassWriter>();

        // Generate sync tests
        var syncTestClass = GenerateSyncTests(arity);
        if (syncTestClass != null) {
            result.Add(syncTestClass);
        }

        // Generate Task async tests
        var taskTestClass = GenerateTaskTests(arity);
        if (taskTestClass != null) {
            taskTestClass.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
            result.Add(taskTestClass);
        }

        // Generate ValueTask async tests
        var valueTaskTestClass = GenerateValueTaskTests(arity);
        if (valueTaskTestClass != null) {
            valueTaskTestClass.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
            result.Add(valueTaskTestClass);
        }

        return result;
    }

    private ClassWriter? GenerateSyncTests(ushort arity) {
        var classWriter = new ClassWriter(Config.ClassName,
                                          Visibility.Public,
                                          ClassModifier.Abstract) {
            Region = $"Arity {arity}"
        };

        var hasTests = false;

        // Generate sync tests for IfSuccess, IfFailure, TryGet (Match is async-only)
        foreach (var method in DirectMethods.Where(m => m != "Match")) {
            var testMethods = GenerateSyncTestMethods(method, arity);
            foreach (var testMethod in testMethods) {
                classWriter.AddMethod(testMethod);
                hasTests = true;
            }
        }

        return hasTests
                   ? classWriter
                   : null;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var classWriter = new ClassWriter(Config.ClassName,
                                          Visibility.Public,
                                          ClassModifier.Abstract) {
            Region = $"Arity {arity}"
        };

        var hasTests = false;

        // Generate Task async tests for all methods
        foreach (var method in DirectMethods) {
            var testMethods = GenerateTaskTestMethods(method, arity);
            foreach (var testMethod in testMethods) {
                classWriter.AddMethod(testMethod);
                hasTests = true;
            }
        }

        return hasTests
                   ? classWriter
                   : null;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var classWriter = new ClassWriter(Config.ClassName,
                                          Visibility.Public,
                                          ClassModifier.Abstract) {
            Region = $"Arity {arity}"
        };

        var hasTests = false;

        // Generate ValueTask async tests for all methods
        foreach (var method in DirectMethods) {
            var testMethods = GenerateValueTaskTestMethods(method, arity);
            foreach (var testMethod in testMethods) {
                classWriter.AddMethod(testMethod);
                hasTests = true;
            }
        }

        return hasTests
                   ? classWriter
                   : null;
    }

    private IEnumerable<MethodWriter> GenerateSyncTestMethods(string methodName,
                                                              ushort arity) {
        // Generate success test
        yield return GenerateSyncSuccessTest(methodName, arity);

        // Generate failure test
        yield return GenerateSyncFailureTest(methodName, arity);
    }

    private IEnumerable<MethodWriter> GenerateTaskTestMethods(string methodName,
                                                              ushort arity) {
        // Generate success test
        yield return GenerateTaskSuccessTest(methodName, arity);

        // Generate failure test
        yield return GenerateTaskFailureTest(methodName, arity);
    }

    private IEnumerable<MethodWriter> GenerateValueTaskTestMethods(string methodName,
                                                                   ushort arity) {
        // Generate success test
        yield return GenerateValueTaskSuccessTest(methodName, arity);

        // Generate failure test
        yield return GenerateValueTaskFailureTest(methodName, arity);
    }

    private MethodWriter GenerateSyncSuccessTest(string methodName,
                                                 ushort arity) {
        var testName = $"{methodName}_Arity{arity}_Success_ShouldExecuteCorrectly";
        var body     = GenerateSyncSuccessTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "void",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private MethodWriter GenerateSyncFailureTest(string methodName,
                                                 ushort arity) {
        var testName = $"{methodName}_Arity{arity}_Failure_ShouldHandleCorrectly";
        var body     = GenerateSyncFailureTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "void",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private MethodWriter GenerateTaskSuccessTest(string methodName,
                                                 ushort arity) {
        var testName = $"{methodName}Task_Arity{arity}_Success_ShouldExecuteCorrectly";
        var body     = GenerateTaskSuccessTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "async Task",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private MethodWriter GenerateTaskFailureTest(string methodName,
                                                 ushort arity) {
        var testName = $"{methodName}Task_Arity{arity}_Failure_ShouldHandleCorrectly";
        var body     = GenerateTaskFailureTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "async Task",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private MethodWriter GenerateValueTaskSuccessTest(string methodName,
                                                      ushort arity) {
        var testName = $"{methodName}ValueTask_Arity{arity}_Success_ShouldExecuteCorrectly";
        var body     = GenerateValueTaskSuccessTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "async ValueTask",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private MethodWriter GenerateValueTaskFailureTest(string methodName,
                                                      ushort arity) {
        var testName = $"{methodName}ValueTask_Arity{arity}_Failure_ShouldHandleCorrectly";
        var body     = GenerateValueTaskFailureTestBody(methodName, arity);

        return new MethodWriter(
            testName,
            "async ValueTask",
            body,
            attributes: new[] { new AttributeReference("Fact") },
            usings: GetRequiredUsings()
        );
    }

    private string GenerateSyncSuccessTestBody(string methodName,
                                               ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity, testValues, true);
        var methodCall     = GenerateSyncMethodCall(methodName, arity);
        var assertions     = GenerateSuccessAssertions(methodName, arity);

        return $"""
                // Given: A successful Result with test values
                {resultCreation}

                // When: Calling {methodName}
                {methodCall}

                // Then: Should execute successfully
                {assertions}
                """;
    }

    private string GenerateSyncFailureTestBody(string methodName,
                                               ushort arity) {
        var resultCreation = GenerateFailureResultCreation(arity);
        var methodCall     = GenerateSyncMethodCall(methodName, arity);
        var assertions     = GenerateFailureAssertions(methodName, arity);

        return $"""
                // Given: A failed Result
                {resultCreation}

                // When: Calling {methodName}
                {methodCall}

                // Then: Should handle failure correctly
                {assertions}
                """;
    }

    private string GenerateTaskSuccessTestBody(string methodName,
                                               ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity, testValues, true);
        var methodCall     = GenerateTaskMethodCall(methodName, arity);
        var assertions     = GenerateSuccessAssertions(methodName, arity);

        return $"""
                // Given: A successful Result with test values
                {resultCreation}

                // When: Calling {methodName} (Task variant)
                {methodCall}

                // Then: Should execute successfully
                {assertions}
                """;
    }

    private string GenerateTaskFailureTestBody(string methodName,
                                               ushort arity) {
        var resultCreation = GenerateFailureResultCreation(arity);
        var methodCall     = GenerateTaskMethodCall(methodName, arity);
        var assertions     = GenerateFailureAssertions(methodName, arity);

        return $"""
                // Given: A failed Result
                {resultCreation}

                // When: Calling {methodName} (Task variant)
                {methodCall}

                // Then: Should handle failure correctly
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessTestBody(string methodName,
                                                    ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity, testValues, true);
        var methodCall     = GenerateValueTaskMethodCall(methodName, arity);
        var assertions     = GenerateSuccessAssertions(methodName, arity);

        return $"""
                // Given: A successful Result with test values
                {resultCreation}

                // When: Calling {methodName} (ValueTask variant)
                {methodCall}

                // Then: Should execute successfully
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureTestBody(string methodName,
                                                    ushort arity) {
        var resultCreation = GenerateFailureResultCreation(arity);
        var methodCall     = GenerateValueTaskMethodCall(methodName, arity);
        var assertions     = GenerateFailureAssertions(methodName, arity);

        return $"""
                // Given: A failed Result
                {resultCreation}

                // When: Calling {methodName} (ValueTask variant)
                {methodCall}

                // Then: Should handle failure correctly
                {assertions}
                """;
    }

    private string GenerateTestValues(ushort arity) {
        var values = new List<string>();
        for (var i = 1; i <= arity; i++) {
            values.Add($"var value{i} = {GetTestValue(i)};");
        }

        return string.Join("\n", values);
    }

    private string GenerateResultCreation(ushort arity,
                                          string testValues,
                                          bool   isSuccess) {
        if (arity == 1) {
            return $"{testValues}\nvar result = Result.Success(value1);";
        }

        var valuesList = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"value{i}"));
        return $"{testValues}\nvar result = Result.Success({valuesList});";
    }

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => GetTestType(i)));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateSyncMethodCall(string methodName,
                                          ushort arity) {
        return methodName switch {
            "IfSuccess" => "var executed = false;\nresult.IfSuccess(_ => executed = true);",
            "IfFailure" => "var executed = false;\nresult.IfFailure(_ => executed = true);",
            "TryGet"    => GenerateTryGetCall(arity),
            _           => $"var methodResult = result.{methodName}();"
        };
    }

    private string GenerateTaskMethodCall(string methodName,
                                          ushort arity) {
        return methodName switch {
            "Match"     => "var matchResult = await result.Match(async _ => \"success\", async _ => \"failure\");",
            "IfSuccess" => "var executed = false;\nawait result.IfSuccess(async _ => { executed = true; });",
            "IfFailure" => "var executed = false;\nawait result.IfFailure(async _ => { executed = true; });",
            "TryGet"    => GenerateTryGetCall(arity),
            _           => $"var methodResult = await result.{methodName}();"
        };
    }

    private string GenerateValueTaskMethodCall(string methodName,
                                               ushort arity) {
        return methodName switch {
            "Match"     => "var matchResult = await result.Match(async _ => \"success\", async _ => \"failure\");",
            "IfSuccess" => "var executed = false;\nawait result.IfSuccess(async _ => { executed = true; });",
            "IfFailure" => "var executed = false;\nawait result.IfFailure(async _ => { executed = true; });",
            "TryGet"    => GenerateTryGetCall(arity),
            _           => $"var methodResult = await result.{methodName}();"
        };
    }

    private string GenerateTryGetCall(ushort arity) {
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var outValue{i}"));
        return $"var tryGetResult = result.TryGet({outParams});";
    }

    private string GenerateSuccessAssertions(string methodName,
                                             ushort arity) {
        return methodName switch {
            "IfSuccess" => "Assert.True(executed);",
            "IfFailure" => "Assert.False(executed);",
            "TryGet"    => GenerateTryGetSuccessAssertions(arity),
            "Match"     => "Assert.Equal(\"success\", matchResult);",
            _           => "Assert.True(true); // Placeholder assertion"
        };
    }

    private string GenerateFailureAssertions(string methodName,
                                             ushort arity) {
        return methodName switch {
            "IfSuccess" => "Assert.False(executed);",
            "IfFailure" => "Assert.True(executed);",
            "TryGet"    => "Assert.False(tryGetResult);",
            "Match"     => "Assert.Equal(\"failure\", matchResult);",
            _           => "Assert.True(true); // Placeholder assertion"
        };
    }

    private string GenerateTryGetSuccessAssertions(ushort arity) {
        var assertions = new List<string> { "Assert.True(tryGetResult);" };
        for (var i = 1; i <= arity; i++) {
            assertions.Add($"Assert.Equal({GetTestValue(i)}, outValue{i});");
        }

        return string.Join("\n", assertions);
    }

    private string GetTestValue(int index) {
        return index switch {
            1 => "42",
            2 => "\"test\"",
            3 => "true",
            4 => "3.14",
            5 => "123L",
            _ => $"\"value{index}\""
        };
    }

    private string GetTestType(int index) {
        return index switch {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "long",
            _ => "string"
        };
    }

    private IEnumerable<string> GetRequiredUsings() {
        return new[] {
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results"
        };
    }
}
