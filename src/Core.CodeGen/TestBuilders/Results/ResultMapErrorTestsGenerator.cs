using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MapError extension methods (sync) and MapErrorAsync (Task, ValueTask).
/// </summary>
internal sealed class ResultMapErrorTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 0; // Changed to 0 to support arity 0 for MapErrorAsync
    private const string ClassName = "ResultMapErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorTestsGenerator(string baseNamespace,
                                        FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        var classes = new List<ClassWriter>();

        // Only generate sync tests for arity >= 1
        if (arity >= 1)
        {
            var sync = GenerateSyncTests(arity);
            if (sync != null)
            {
                classes.Add(sync);
            }
        }

        var task = GenerateTaskTests(arity);
        if (task != null)
        {
            task.UnderClass = ClassName;
            classes.Add(task);
        }

        var valueTask = GenerateValueTaskTests(arity);
        if (valueTask != null)
        {
            valueTask.UnderClass = ClassName;
            classes.Add(valueTask);
        }

        return classes;
    }

    private ClassWriter? GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MapError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task MapErrorAsync" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        if (arity == 0)
        {
            cw.AddMethod(GenerateTaskAwaitableFailureTest(arity));
        }
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask MapErrorAsync" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        if (arity == 0)
        {
            cw.AddMethod(GenerateValueTaskAwaitableFailureTest(arity));
        }
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapError_Arity{arity}_Success_ShouldNotMapError",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"MapError_Arity{arity}_Failure_ShouldMapError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrorTask_Arity{arity}_Success_ShouldNotMapError",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorTask_Arity{arity}_Failure_ShouldMapError",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrorValueTask_Arity{arity}_Success_ShouldNotMapError",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorValueTask_Arity{arity}_Failure_ShouldMapError",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskAwaitableFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorAsyncTaskAwaitable_Arity{arity}_Failure_ShouldMapErrors",
                                "async Task",
                                GenerateTaskAwaitableFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskAwaitableFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorAsyncValueTaskAwaitable_Arity{arity}_Failure_ShouldMapErrors",
                                "async Task",
                                GenerateValueTaskAwaitableFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateMapErrorSyncCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(mappedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateMapErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Contains("MAPPED", mappedResult.Errors.First().Message);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = arity == 0 ? "var result = Result.Success();" : GenerateResultCreation(arity);
        var call = GenerateMapErrorTaskCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(mappedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : GenerateFailureResultCreation(arity);
        var call = GenerateMapErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Contains("MAPPED", mappedResult.Errors.First().Message);
                """;
    }

    private string GenerateTaskAwaitableFailureBody(ushort arity)
    {
        var creation = "var taskResult = Task.FromResult(Result.Failure(\"Test error\"));";
        var call = "var mappedResult = await taskResult.MapErrorAsync(errors => Task.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {e.Message}\")).AsEnumerable()));";
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Contains("MAPPED", mappedResult.Errors.First().Message);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = arity == 0 ? "var result = Result.Success();" : GenerateResultCreation(arity);
        var call = GenerateMapErrorValueTaskCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(mappedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : GenerateFailureResultCreation(arity);
        var call = GenerateMapErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Contains("MAPPED", mappedResult.Errors.First().Message);
                """;
    }

    private string GenerateValueTaskAwaitableFailureBody(ushort arity)
    {
        var creation = "var valueTaskResult = ValueTask.FromResult(Result.Failure(\"Test error\"));";
        var call = "var mappedResult = await valueTaskResult.MapErrorAsync(errors => ValueTask.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {e.Message}\")).AsEnumerable()));";
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Contains("MAPPED", mappedResult.Errors.First().Message);
                """;
    }

    private string GenerateMapErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = result.MapError(errors => errors.Select(e => (IError)new Error($\"MAPPED: {e.Message}\")).AsEnumerable());";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var mappedResult = result.MapError<{typeParams}>(errors => errors.Select(e => (IError)new Error($\"MAPPED: {{e.Message}}\")).AsEnumerable());";
    }

    private string GenerateMapErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = await result.MapErrorAsync(errors => Task.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {e.Message}\")).AsEnumerable()));";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return
            $"var mappedResult = await result.MapErrorAsync<{typeParams}>(errors => Task.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {{e.Message}}\")).AsEnumerable()));";
    }

    private string GenerateMapErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = await result.MapErrorAsync(errors => ValueTask.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {e.Message}\")).AsEnumerable()));";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return
            $"var mappedResult = await result.MapErrorAsync<{typeParams}>(errors => ValueTask.FromResult(errors.Select(e => (IError)new Error($\"MAPPED: {{e.Message}}\")).AsEnumerable()));";
    }

    private string GenerateResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = Result.Success();";
        }
        if (arity == 1)
        {
            return "var result = Result.Success(value1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateTaskResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var taskResult = Task.FromResult(Result.Success());";
        }
        if (arity == 1)
        {
            return "var taskResult = Task.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var taskResult = Task.FromResult(Result.Success({values}));";
    }

    private string GenerateValueTaskResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success());";
        }
        if (arity == 1)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Success({values}));";
    }

    private string GenerateFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = Result.Failure(\"Test error\");";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateTaskFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var taskResult = Task.FromResult(Result.Failure(\"Test error\"));";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureResultCreation(ushort arity)
    {
        if (arity == 0)
        {
            return "var valueTaskResult = ValueTask.FromResult(Result.Failure(\"Test error\"));";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateTestValues(ushort arity)
    {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    private string GetTestValue(int index)
    {
        return index switch { 1 => "42", 2 => "\"test\"", 3 => "true", 4 => "3.14", 5 => "123L", _ => $"\"value{index}\"" };
    }

    private string GetTestType(int index)
    {
        return index switch { 1 => "int", 2 => "string", 3 => "bool", 4 => "double", 5 => "long", _ => "string" };
    }

    private IEnumerable<string> GetUsings()
    {
        return [
            "System",
            "System.Linq",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Reasons",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks"
        ];
    }
}
