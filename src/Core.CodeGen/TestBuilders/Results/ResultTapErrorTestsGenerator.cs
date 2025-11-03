using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TapError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapErrorTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTapErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    public ResultTapErrorTestsGenerator(string baseNamespace,
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
        var sync = GenerateSyncTests(arity);
        if (sync != null)
        {
            classes.Add(sync);
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
        var cw = new ClassWriter($"ResultTapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncTaskCallbackSuccessTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncTaskCallbackFailureTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncValueTaskCallbackSuccessTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncValueTaskCallbackFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TapError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskWithSyncActionSuccessTest(arity));
        cw.AddMethod(GenerateTaskWithSyncActionFailureTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncTaskCallbackSuccessTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncTaskCallbackFailureTest(arity));

        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TapError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskWithSyncActionSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskWithSyncActionFailureTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncValueTaskCallbackSuccessTest(arity));
        cw.AddMethod(GenerateSyncWithAsyncValueTaskCallbackFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapError_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"TapError_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncWithAsyncTaskCallbackSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncWithTaskCallback_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateSyncWithAsyncTaskCallbackSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncWithAsyncTaskCallbackFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncWithTaskCallback_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateSyncWithAsyncTaskCallbackFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncWithAsyncValueTaskCallbackSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncWithValueTaskCallback_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateSyncWithAsyncValueTaskCallbackSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncWithAsyncValueTaskCallbackFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncWithValueTaskCallback_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateSyncWithAsyncValueTaskCallbackFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorTask_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorTask_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorValueTask_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorValueTask_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskWithSyncActionSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncTaskWithSyncAction_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateTaskWithSyncActionSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskWithSyncActionFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncTaskWithSyncAction_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateTaskWithSyncActionFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskWithSyncActionSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncValueTaskWithSyncAction_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskWithSyncActionSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskWithSyncActionFailureTest(ushort arity)
    {
        return new MethodWriter($"TapErrorAsyncValueTaskWithSyncAction_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskWithSyncActionFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateTapErrorSyncCall(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.True(tappedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateTapErrorSyncCall(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {failureCreation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncWithAsyncTaskCallbackSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        return $$$"""
                  // Given
                  {{{testValues}}}
                  var sideEffectExecuted = false;
                  {{{creation}}}
                  // When
                  var tappedResult = await result.TapErrorAsync(errors => { sideEffectExecuted = true; return Task.CompletedTask; });
                  // Then
                  Assert.False(sideEffectExecuted);
                  Assert.True(tappedResult.IsSuccess);
                  """;
    }

    private string GenerateSyncWithAsyncTaskCallbackFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $$$"""
                  // Given
                  var sideEffectExecuted = false;
                  {{{failureCreation}}}
                  // When
                  var tappedResult = await result.TapErrorAsync(errors => { sideEffectExecuted = true; return Task.CompletedTask; });
                  // Then
                  {{{assertions}}}
                  """;
    }

    private string GenerateSyncWithAsyncValueTaskCallbackSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        return $$$"""
                  // Given
                  {{{testValues}}}
                  var sideEffectExecuted = false;
                  {{{creation}}}
                  // When
                  var tappedResult = await result.TapErrorAsync(errors => { sideEffectExecuted = true; return ValueTask.CompletedTask; });
                  // Then
                  Assert.False(sideEffectExecuted);
                  Assert.True(tappedResult.IsSuccess);
                  """;
    }

    private string GenerateSyncWithAsyncValueTaskCallbackFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $$$"""
                  // Given
                  var sideEffectExecuted = false;
                  {{{failureCreation}}}
                  // When
                  var tappedResult = await result.TapErrorAsync(errors => { sideEffectExecuted = true; return ValueTask.CompletedTask; });
                  // Then
                  {{{assertions}}}
                  """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        var call = GenerateTapErrorTaskCall(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.True(tappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call = GenerateTapErrorTaskCall(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        var call = GenerateTapErrorValueTaskCall(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.True(tappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call = GenerateTapErrorValueTaskCall(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskWithSyncActionSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                var tappedResult = await taskResult.TapErrorAsync(errors => sideEffectExecuted = true);
                // Then
                Assert.False(sideEffectExecuted);
                Assert.True(tappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskWithSyncActionFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                var tappedResult = await taskResult.TapErrorAsync(errors => sideEffectExecuted = true);
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskWithSyncActionSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                var tappedResult = await valueTaskResult.TapErrorAsync(errors => sideEffectExecuted = true);
                // Then
                Assert.False(sideEffectExecuted);
                Assert.True(tappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskWithSyncActionFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var assertions = GenerateTapErrorFailureAssertions(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                var tappedResult = await valueTaskResult.TapErrorAsync(errors => sideEffectExecuted = true);
                // Then
                {assertions}
                """;
    }

    private string GenerateTapErrorSyncCall(ushort arity)
    {
        return "var tappedResult = result.TapError(errors => sideEffectExecuted = true);";
    }

    private string GenerateTapErrorTaskCall(ushort arity)
    {
        return "var tappedResult = await taskResult.TapErrorAsync(errors => { sideEffectExecuted = true; return Task.CompletedTask; });";
    }

    private string GenerateTapErrorValueTaskCall(ushort arity)
    {
        return "var tappedResult = await valueTaskResult.TapErrorAsync(errors => { sideEffectExecuted = true; return ValueTask.CompletedTask; });";
    }

    private string GenerateTapErrorFailureAssertions(ushort arity)
    {
        return """
               Assert.True(sideEffectExecuted);
               Assert.False(tappedResult.IsSuccess);
               """;
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
        if (arity == 0)
        {
            return string.Empty;
        }
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
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Extensions.SideEffects",
            "UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks",
            "UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks"
        ];
    }
}
