using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ValueOr extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultValueOrTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultValueOrTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrTestsGenerator(string               baseNamespace,
                                       FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var classes = new List<ClassWriter>();
        var sync    = GenerateSyncTests(arity);
        if (sync != null) {
            classes.Add(sync);
        }

        var task = GenerateTaskTests(arity);
        if (task != null) {
            task.UnderClass = ClassName;
            classes.Add(task);
        }

        var valueTask = GenerateValueTaskTests(arity);
        if (valueTask != null) {
            valueTask.UnderClass = ClassName;
            classes.Add(valueTask);
        }

        return classes;
    }

    private ClassWriter? GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOr" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateSyncFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ValueOr" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateTaskFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ValueOr" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateValueTaskFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOr_Arity{arity}_Success_ShouldReturnValue",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"ValueOr_Arity{arity}_Failure_ShouldReturnFallback",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "void",
                                GenerateSyncSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue",
                                "void",
                                GenerateSyncFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOrTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"ValueOrTask_Arity{arity}_Failure_ShouldReturnFallback",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrTaskWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateTaskSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrTaskWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue",
                                "async Task",
                                GenerateTaskFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOrValueTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"ValueOrValueTask_Arity{arity}_Failure_ShouldReturnFallback",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrValueTaskWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateValueTaskSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrValueTaskWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue",
                                "async Task",
                                GenerateValueTaskFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrSyncCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var fallbackValues  = GenerateFallbackValues(arity);
        var call            = GenerateValueOrSyncCall(arity);
        var assertions      = GenerateValueOrFailureAssertions(arity);
        return $"""
                // Given
                {failureCreation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity) {
        var failureCreation   = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return $"""
                // Given
                {failureCreation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateTaskResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrTaskCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation       = GenerateTaskFailureResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrTaskCall(arity);
        var assertions     = GenerateValueOrFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateTaskResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateValueTaskResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrValueTaskCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation       = GenerateValueTaskFailureResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrValueTaskCall(arity);
        var assertions     = GenerateValueOrFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                {fallbackValues}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateValueTaskResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateValueTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueOrSyncCall(ushort arity) {
        if (arity == 1) {
            return "var actualValue = result.ValueOr(fallback1);";
        }

        var fallbacks = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"fallback{i}"));
        return $"var actualValue = result.ValueOr({fallbacks});";
    }

    private string GenerateValueOrSyncWithFactoryCall(ushort arity) {
        return "var actualValue = result.ValueOr(factory);";
    }

    private string GenerateValueOrTaskCall(ushort arity) {
        if (arity == 1) {
            return "var actualValue = await taskResult.ValueOrAsync(fallback1);";
        }

        var fallbacks = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"fallback{i}"));
        return $"var actualValue = await taskResult.ValueOrAsync({fallbacks});";
    }

    private string GenerateValueOrTaskWithFactoryCall(ushort arity) {
        return "var actualValue = await taskResult.ValueOrAsync(factory);";
    }

    private string GenerateValueOrValueTaskCall(ushort arity) {
        if (arity == 1) {
            return "var actualValue = await valueTaskResult.ValueOrAsync(fallback1);";
        }

        var fallbacks = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"fallback{i}"));
        return $"var actualValue = await valueTaskResult.ValueOrAsync({fallbacks});";
    }

    private string GenerateValueOrValueTaskWithFactoryCall(ushort arity) {
        return "var actualValue = await valueTaskResult.ValueOrAsync(factory);";
    }

    private string GenerateValueOrSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(value1, actualValue);
                   """;
        }

        var assertions = string.Join("\n", Enumerable.Range(1, arity)
                                                     .Select(i => $"Assert.Equal(value{i}, actualValue.Item{i});"));
        return assertions;
    }

    private string GenerateValueOrFailureAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(fallback1, actualValue);
                   """;
        }

        var assertions = string.Join("\n", Enumerable.Range(1, arity)
                                                     .Select(i => $"Assert.Equal(fallback{i}, actualValue.Item{i});"));
        return assertions;
    }

    private string GenerateValueOrFactoryFailureAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(fallback1, actualValue);
                   """;
        }

        var assertions = string.Join("\n", Enumerable.Range(1, arity)
                                                     .Select(i => $"Assert.Equal(fallback{i}, actualValue.Item{i});"));
        return assertions;
    }

    private string GenerateFallbackValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var fallback{i} = {GetFallbackValue(i)};"));
    }

    private string GenerateFactoryDefinition(ushort arity) {
        if (arity == 1) {
            return "var fallback1 = 999;\nFunc<int> factory = () => fallback1;";
        }

        var fallbackDeclarations = string.Join("\n", Enumerable.Range(1, arity)
                                                               .Select(i => $"var fallback{i} = {GetFallbackValue(i)};"));
        var tupleType   = $"({string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType))})";
        var tupleValues = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))})";
        return $"{fallbackDeclarations}\nFunc<{tupleType}> factory = () => {tupleValues};";
    }

    private string GenerateResultCreation(ushort arity) {
        if (arity == 1) {
            return "var result = Result.Success(value1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateTaskResultCreation(ushort arity) {
        if (arity == 1) {
            return "var taskResult = Task.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var taskResult = Task.FromResult(Result.Success({values}));";
    }

    private string GenerateValueTaskResultCreation(ushort arity) {
        if (arity == 1) {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Success({values}));";
    }

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateTaskFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    private string GetTestValue(int index) {
        return index switch { 1 => "42", 2 => "\"test\"", 3 => "true", 4 => "3.14", 5 => "123L", _ => $"\"value{index}\"" };
    }

    private string GetFallbackValue(int index) {
        return index switch { 1 => "999", 2 => "\"fallback\"", 3 => "false", 4 => "9.99", 5 => "999L", _ => $"\"fallback{index}\"" };
    }

    private string GetTestType(int index) {
        return index switch { 1 => "int", 2 => "string", 3 => "bool", 4 => "double", 5 => "long", _ => "string" };
    }

    private IEnumerable<string> GetUsings() {
        return [
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks"
        ];
    }
}
