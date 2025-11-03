using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Map extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMapTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultMapTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultMapTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultMapSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Map" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Map" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Map" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Map_Arity{arity}_Success_ShouldTransform",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Map_Arity{arity}_Failure_ShouldNotTransform",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"MapTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"MapTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"MapValueTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"MapValueTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateMapSyncCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GenerateMapSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateMapTaskCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateMapTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateMapValueTaskCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateMapValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateMapSyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Map(x => x * 2);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = result.Map(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await taskResult.MapAsync(x => x * 2);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = await taskResult.MapAsync(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapValueTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await valueTaskResult.MapAsync(x => x * 2);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = await valueTaskResult.MapAsync(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.True(transformedResult.IsSuccess);
                   Assert.True(transformedResult.TryGet(out var mappedValue));
                   Assert.Equal(84, mappedValue);
                   """;
        }

        return "Assert.True(transformedResult.IsSuccess);";
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
            "UnambitiousFx.Core.Results.Extensions.Transformations",
            "UnambitiousFx.Core.Results.Extensions.Transformations.Tasks",
            "UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks"
        ];
    }
}
