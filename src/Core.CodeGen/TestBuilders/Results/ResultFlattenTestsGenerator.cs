using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Flatten extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFlattenTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultFlattenTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultFlattenTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultFlattenSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Flatten" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFlattenTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Flatten" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFlattenValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Flatten" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Flatten_Arity{arity}_Success_ShouldFlatten",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Flatten_Arity{arity}_Failure_ShouldNotFlatten",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"FlattenTask_Arity{arity}_Success_ShouldFlatten",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"FlattenTask_Arity{arity}_Failure_ShouldNotFlatten",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"FlattenValueTask_Arity{arity}_Success_ShouldFlatten",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"FlattenValueTask_Arity{arity}_Failure_ShouldNotFlatten",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateNestedResultCreation(arity);
        var call       = GenerateFlattenSyncCall(arity);
        var assertions = GenerateFlattenSuccessAssertions(arity);
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
        var failureCreation = GenerateFailureNestedResultCreation(arity);
        var call            = GenerateFlattenSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(flattenedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskNestedResultCreation(arity);
        var call       = GenerateFlattenTaskCall(arity);
        var assertions = GenerateFlattenSuccessAssertions(arity);
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
        var creation = GenerateTaskFailureNestedResultCreation(arity);
        var call     = GenerateFlattenTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(flattenedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskNestedResultCreation(arity);
        var call       = GenerateFlattenValueTaskCall(arity);
        var assertions = GenerateFlattenSuccessAssertions(arity);
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
        var creation = GenerateValueTaskFailureNestedResultCreation(arity);
        var call     = GenerateFlattenValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(flattenedResult.IsSuccess);
                """;
    }

    private string GenerateFlattenSyncCall(ushort arity) {
        return "var flattenedResult = nestedResult.Flatten();";
    }

    private string GenerateFlattenTaskCall(ushort arity) {
        return "var flattenedResult = await taskNestedResult.FlattenAsync();";
    }

    private string GenerateFlattenValueTaskCall(ushort arity) {
        return "var flattenedResult = await valueTaskNestedResult.FlattenAsync();";
    }

    private string GenerateFlattenSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.True(flattenedResult.IsSuccess);
                   Assert.True(flattenedResult.TryGet(out var flattenedValue));
                   Assert.Equal(42, flattenedValue);
                   """;
        }

        return "Assert.True(flattenedResult.IsSuccess);";
    }

    private string GenerateNestedResultCreation(ushort arity) {
        if (arity == 1) {
            return "var nestedResult = Result.Success(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var nestedResult = Result.Success(Result.Success({values}));";
    }

    private string GenerateTaskNestedResultCreation(ushort arity) {
        if (arity == 1) {
            return "var taskNestedResult = Task.FromResult(Result.Success(Result.Success(value1)));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var taskNestedResult = Task.FromResult(Result.Success(Result.Success({values})));";
    }

    private string GenerateValueTaskNestedResultCreation(ushort arity) {
        if (arity == 1) {
            return "var valueTaskNestedResult = ValueTask.FromResult(Result.Success(Result.Success(value1)));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var valueTaskNestedResult = ValueTask.FromResult(Result.Success(Result.Success({values})));";
    }

    private string GenerateFailureNestedResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var nestedResult = Result.Failure<Result<{typeParams}>>(\"Test error\");";
    }

    private string GenerateTaskFailureNestedResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskNestedResult = Task.FromResult(Result.Failure<Result<{typeParams}>>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureNestedResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskNestedResult = ValueTask.FromResult(Result.Failure<Result<{typeParams}>>(\"Test error\"));";
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
