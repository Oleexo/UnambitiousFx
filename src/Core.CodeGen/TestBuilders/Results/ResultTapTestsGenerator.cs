using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Tap extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTapTests";
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    public ResultTapTestsGenerator(string baseNamespace,
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
        // Sync Tap
        classes.Add(GenerateSyncTests(arity));
        // Async Tap for Result (Func<Task>, Func<ValueTask>)
        classes.Add(GenerateAsyncResultTests(arity, false));
        classes.Add(GenerateAsyncResultTests(arity, true));
        // Async Tap for Task<Result> and ValueTask<Result> (Action, Func<Task/ValueTask>)
        classes.Add(GenerateAsyncAwaitableTests(arity, false));
        classes.Add(GenerateAsyncAwaitableTests(arity, true));
        return classes;
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Tap" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncResultTests(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var cw = new ClassWriter($"ResultTapAsync{type}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - TapAsync(Result, Func<{type}>)" };
        cw.AddMethod(GenerateAsyncResultSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncResultFailureTest(arity, isValueTask));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{type}s";
        return cw;
    }

    private ClassWriter GenerateAsyncAwaitableTests(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var cw = new ClassWriter($"ResultTapAsyncAwaitable{type}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - TapAsync({type}<Result>, Action/Func<{type}>)" };
        cw.AddMethod(GenerateAsyncAwaitableActionSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableActionFailureTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableFuncSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableFuncFailureTest(arity, isValueTask));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{type}s";
        return cw;
    }
    // Async Tap for direct Result (Func<Task> / Func<ValueTask>)
    private MethodWriter GenerateAsyncResultSuccessTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await result.TapAsync(() => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : arity == 1
            ? $"var tappedResult = await result.TapAsync(x => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : $"var tappedResult = await result.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var assertions = GenerateTapSuccessAssertions(arity);
        return new MethodWriter($"TapAsync_{type}_Arity{arity}_Success_ShouldExecuteSideEffect",
            "async Task",
            $"// Given\n{testValues}\nvar sideEffectExecuted = false;\n{creation}\n// When\n{call}\n// Then\n{assertions}",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateAsyncResultFailureTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await result.TapAsync(() => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : arity == 1
            ? $"var tappedResult = await result.TapAsync(x => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : $"var tappedResult = await result.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        return new MethodWriter($"TapAsync_{type}_Arity{arity}_Failure_ShouldNotExecuteSideEffect",
            "async Task",
            $"// Given\nvar sideEffectExecuted = false;\n{failureCreation}\n// When\n{call}\n// Then\nAssert.False(sideEffectExecuted);\nAssert.False(tappedResult.IsSuccess);",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    // Async Tap for Task<Result> and ValueTask<Result> (Action)
    private MethodWriter GenerateAsyncAwaitableActionSuccessTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var varName = isValueTask ? "valueTaskResult" : "taskResult";
        var testValues = GenerateTestValues(arity);
        var creation = isValueTask ? GenerateValueTaskResultCreation(arity) : GenerateTaskResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await {varName}.TapAsync(() => sideEffectExecuted = true);"
            : arity == 1
            ? $"var tappedResult = await {varName}.TapAsync(x => sideEffectExecuted = true);"
            : $"var tappedResult = await {varName}.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        var assertions = GenerateTapSuccessAssertions(arity);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Action_Success_ShouldExecuteSideEffect",
            "async Task",
            $"// Given\n{testValues}\nvar sideEffectExecuted = false;\n{creation}\n// When\n{call}\n// Then\n{assertions}",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableActionFailureTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var varName = isValueTask ? "valueTaskResult" : "taskResult";
        var creation = isValueTask ? GenerateValueTaskFailureResultCreation(arity) : GenerateTaskFailureResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await {varName}.TapAsync(() => sideEffectExecuted = true);"
            : arity == 1
            ? $"var tappedResult = await {varName}.TapAsync(x => sideEffectExecuted = true);"
            : $"var tappedResult = await {varName}.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Action_Failure_ShouldNotExecuteSideEffect",
            "async Task",
            $"// Given\nvar sideEffectExecuted = false;\n{creation}\n// When\n{call}\n// Then\nAssert.False(sideEffectExecuted);\nAssert.False(tappedResult.IsSuccess);",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    // Async Tap for Task<Result> and ValueTask<Result> (Func<Task/ValueTask>)
    private MethodWriter GenerateAsyncAwaitableFuncSuccessTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var varName = isValueTask ? "valueTaskResult" : "taskResult";
        var testValues = GenerateTestValues(arity);
        var creation = isValueTask ? GenerateValueTaskResultCreation(arity) : GenerateTaskResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await {varName}.TapAsync(() => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : arity == 1
            ? $"var tappedResult = await {varName}.TapAsync(x => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : $"var tappedResult = await {varName}.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var assertions = GenerateTapSuccessAssertions(arity);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Func_Success_ShouldExecuteSideEffect",
            "async Task",
            $"// Given\n{testValues}\nvar sideEffectExecuted = false;\n{creation}\n// When\n{call}\n// Then\n{assertions}",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableFuncFailureTest(ushort arity, bool isValueTask)
    {
        var type = isValueTask ? "ValueTask" : "Task";
        var varName = isValueTask ? "valueTaskResult" : "taskResult";
        var creation = isValueTask ? GenerateValueTaskFailureResultCreation(arity) : GenerateTaskFailureResultCreation(arity);
        var call = arity == 0
            ? $"var tappedResult = await {varName}.TapAsync(() => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : arity == 1
            ? $"var tappedResult = await {varName}.TapAsync(x => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});"
            : $"var tappedResult = await {varName}.TapAsync(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Func_Failure_ShouldNotExecuteSideEffect",
            "async Task",
            $"// Given\nvar sideEffectExecuted = false;\n{creation}\n// When\n{call}\n// Then\nAssert.False(sideEffectExecuted);\nAssert.False(tappedResult.IsSuccess);",
            attributes: [new FactAttributeReference()],
            usings: GetUsings());
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"Tap_Arity{arity}_Success_ShouldExecuteSideEffect",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"Tap_Arity{arity}_Failure_ShouldNotExecuteSideEffect",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapTask_Arity{arity}_Success_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapTask_Arity{arity}_Failure_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapValueTask_Arity{arity}_Success_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapValueTask_Arity{arity}_Failure_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateTapSyncCall(arity);
        var assertions = GenerateTapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateTapSyncCall(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        var call = GenerateTapTaskCall(arity);
        var assertions = GenerateTapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call = GenerateTapTaskCall(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        var call = GenerateTapValueTaskCall(arity);
        var assertions = GenerateTapSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call = GenerateTapValueTaskCall(arity);
        return $"""
                // Given
                var sideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(sideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateTapSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var tappedResult = result.Tap(() => sideEffectExecuted = true);";
        }
        if (arity == 1)
        {
            return "var tappedResult = result.Tap(x => sideEffectExecuted = true);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = result.Tap(({parameters}) => sideEffectExecuted = true);";
    }

    private string GenerateTapTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var tappedResult = await taskResult.TapAsync(() => { sideEffectExecuted = true; return Task.CompletedTask; });";
        }
        if (arity == 1)
        {
            return "var tappedResult = await taskResult.TapAsync(x => { sideEffectExecuted = true; return Task.CompletedTask; });";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = await taskResult.TapAsync(({parameters}) => {{ sideEffectExecuted = true; return Task.CompletedTask; }});";
    }

    private string GenerateTapValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var tappedResult = await valueTaskResult.TapAsync(() => { sideEffectExecuted = true; return ValueTask.CompletedTask; });";
        }
        if (arity == 1)
        {
            return "var tappedResult = await valueTaskResult.TapAsync(x => { sideEffectExecuted = true; return ValueTask.CompletedTask; });";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = await valueTaskResult.TapAsync(({parameters}) => {{ sideEffectExecuted = true; return ValueTask.CompletedTask; }});";
    }

    private string GenerateTapSuccessAssertions(ushort arity)
    {
        return """
               Assert.True(sideEffectExecuted);
               Assert.True(tappedResult.IsSuccess);
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
