using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ToNullable extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultToNullableTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultToNullableTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultToNullableTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultToNullableSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ToNullable" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ToNullable" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ToNullable" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"ToNullable_Arity{arity}_Success_ShouldReturnValue",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"ToNullable_Arity{arity}_Failure_ShouldReturnNull",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ToNullableTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"ToNullableTask_Arity{arity}_Failure_ShouldReturnNull",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ToNullableValueTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"ToNullableValueTask_Arity{arity}_Failure_ShouldReturnNull",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateToNullableSyncCall(arity);
        var assertions = GenerateToNullableSuccessAssertions(arity);
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
        var call            = GenerateToNullableSyncCall(arity);
        var assertions      = GenerateToNullableFailureAssertions(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateToNullableTaskCall(arity);
        var assertions = GenerateToNullableSuccessAssertions(arity);
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
        var creation   = GenerateTaskFailureResultCreation(arity);
        var call       = GenerateToNullableTaskCall(arity);
        var assertions = GenerateToNullableFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateToNullableValueTaskCall(arity);
        var assertions = GenerateToNullableSuccessAssertions(arity);
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
        var creation   = GenerateValueTaskFailureResultCreation(arity);
        var call       = GenerateToNullableValueTaskCall(arity);
        var assertions = GenerateToNullableFailureAssertions(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateToNullableSyncCall(ushort arity) {
        return "var nullableValue = result.ToNullable();";
    }

    private string GenerateToNullableTaskCall(ushort arity) {
        return "var nullableValue = await taskResult.ToNullableAsync();";
    }

    private string GenerateToNullableValueTaskCall(ushort arity) {
        return "var nullableValue = await valueTaskResult.ToNullableAsync();";
    }

    private string GenerateToNullableSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(value1, nullableValue);
                   """;
        }

        var assertions = new List<string> {
            "Assert.NotNull(nullableValue);"
        };
        assertions.AddRange(Enumerable.Range(1, arity)
                                      .Select(i => $"Assert.Equal(value{i}, nullableValue.Value.Item{i});"));
        return string.Join("\n", assertions);
    }

    private string GenerateToNullableFailureAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(default, nullableValue);
                   """;
        }

        return """
               Assert.Null(nullableValue);
               """;
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
            "UnambitiousFx.Core.Results.Extensions.ValueAccess",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks"
        ];
    }
}
