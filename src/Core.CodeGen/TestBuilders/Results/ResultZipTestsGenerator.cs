using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Zip extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultZipTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 2;
    private const string ClassName           = "ResultZipTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultZipTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultZipSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Zip" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultZipTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Zip" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultZipValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Zip" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Zip_Arity{arity}_Success_ShouldZip",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Zip_Arity{arity}_Failure_ShouldNotZip",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ZipTask_Arity{arity}_Success_ShouldZip",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"ZipTask_Arity{arity}_Failure_ShouldNotZip",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ZipValueTask_Arity{arity}_Success_ShouldZip",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"ZipValueTask_Arity{arity}_Failure_ShouldNotZip",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreations(arity);
        var call       = GenerateZipSyncCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
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
        var testValues      = GenerateFailureTestValues(arity);
        var failureCreation = GenerateFailureResultCreations(arity);
        var call            = GenerateZipSyncCall(arity);
        return $"""
                // Given
                {testValues}
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(zippedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreations(arity);
        var call       = GenerateZipTaskCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
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
        var testValues = GenerateFailureTestValues(arity);
        var creation   = GenerateTaskFailureResultCreations(arity);
        var call       = GenerateZipTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(zippedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreations(arity);
        var call       = GenerateZipValueTaskCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
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
        var testValues = GenerateFailureTestValues(arity);
        var creation   = GenerateValueTaskFailureResultCreations(arity);
        var call       = GenerateZipValueTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(zippedResult.IsSuccess);
                """;
    }

    private string GenerateZipSyncCall(ushort arity) {
        if (arity == 2) {
            return "var zippedResult = result1.Zip(result2);";
        }

        var results = string.Join(", ", Enumerable.Range(2, arity - 1)
                                                  .Select(i => $"result{i}"));
        return $"var zippedResult = result1.Zip({results});";
    }

    private string GenerateZipTaskCall(ushort arity) {
        if (arity == 2) {
            return "var zippedResult = await taskResult1.ZipAsync(taskResult2);";
        }

        var results = string.Join(", ", Enumerable.Range(2, arity - 1)
                                                  .Select(i => $"taskResult{i}"));
        return $"var zippedResult = await taskResult1.ZipAsync({results});";
    }

    private string GenerateZipValueTaskCall(ushort arity) {
        if (arity == 2) {
            return "var zippedResult = await valueTaskResult1.ZipAsync(valueTaskResult2);";
        }

        var results = string.Join(", ", Enumerable.Range(2, arity - 1)
                                                  .Select(i => $"valueTaskResult{i}"));
        return $"var zippedResult = await valueTaskResult1.ZipAsync({results});";
    }

    private string GenerateZipSuccessAssertions(ushort arity) {
        return "Assert.True(zippedResult.IsSuccess);";
    }

    private string GenerateResultCreations(ushort arity) {
        var creations = new List<string>();
        for (var i = 1; i <= arity; i++) {
            creations.Add($"var result{i} = Result.Success(value{i});");
        }

        return string.Join("\n", creations);
    }

    private string GenerateTaskResultCreations(ushort arity) {
        var creations = new List<string>();
        for (var i = 1; i <= arity; i++) {
            creations.Add($"var taskResult{i} = Task.FromResult(Result.Success(value{i}));");
        }

        return string.Join("\n", creations);
    }

    private string GenerateValueTaskResultCreations(ushort arity) {
        var creations = new List<string>();
        for (var i = 1; i <= arity; i++) {
            creations.Add($"var valueTaskResult{i} = ValueTask.FromResult(Result.Success(value{i}));");
        }

        return string.Join("\n", creations);
    }

    private string GenerateFailureResultCreations(ushort arity) {
        var creations = new List<string>();
        creations.Add($"var result1 = Result.Failure<{GetTestType(1)}>(\"Test error\");");
        for (var i = 2; i <= arity; i++) {
            creations.Add($"var result{i} = Result.Success(value{i});");
        }

        return string.Join("\n", creations);
    }

    private string GenerateTaskFailureResultCreations(ushort arity) {
        var creations = new List<string>();
        creations.Add($"var taskResult1 = Task.FromResult(Result.Failure<{GetTestType(1)}>(\"Test error\"));");
        for (var i = 2; i <= arity; i++) {
            creations.Add($"var taskResult{i} = Task.FromResult(Result.Success(value{i}));");
        }

        return string.Join("\n", creations);
    }

    private string GenerateValueTaskFailureResultCreations(ushort arity) {
        var creations = new List<string>();
        creations.Add($"var valueTaskResult1 = ValueTask.FromResult(Result.Failure<{GetTestType(1)}>(\"Test error\"));");
        for (var i = 2; i <= arity; i++) {
            creations.Add($"var valueTaskResult{i} = ValueTask.FromResult(Result.Success(value{i}));");
        }

        return string.Join("\n", creations);
    }

    private string GenerateFailureTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(2, arity - 1)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
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
