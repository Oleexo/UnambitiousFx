using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TapBoth extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapBothTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTapBothTests";
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    public ResultTapBothTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultTapBothSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapBoth" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapBothTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TapBoth" };

        // Tests for Result.TapBothAsync(Func<Task>, Func<IEnumerable<IError>, Task>)
        cw.AddMethod(GenerateResultTaskFuncSuccessTest(arity));
        cw.AddMethod(GenerateResultTaskFuncFailureTest(arity));

        // Tests for Task<Result>.TapBothAsync(Action, Action<IEnumerable<IError>>)
        cw.AddMethod(GenerateTaskResultActionSuccessTest(arity));
        cw.AddMethod(GenerateTaskResultActionFailureTest(arity));

        // Tests for Task<Result>.TapBothAsync(Func<Task>, Func<IEnumerable<IError>, Task>)
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));

        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapBothValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TapBoth" };

        // Tests for Result.TapBothAsync(Func<ValueTask>, Func<IEnumerable<IError>, ValueTask>)
        cw.AddMethod(GenerateResultValueTaskFuncSuccessTest(arity));
        cw.AddMethod(GenerateResultValueTaskFuncFailureTest(arity));

        // Tests for ValueTask<Result>.TapBothAsync(Action, Action<IEnumerable<IError>>)
        cw.AddMethod(GenerateValueTaskResultActionSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskResultActionFailureTest(arity));

        // Tests for ValueTask<Result>.TapBothAsync(Func<ValueTask>, Func<IEnumerable<IError>, ValueTask>)
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));

        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBoth_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBoth_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothValueTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothValueTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // Result.TapBothAsync(Func<ValueTask>, Func<IEnumerable<IError>, ValueTask>)
    private MethodWriter GenerateResultValueTaskFuncSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ResultWithFuncValueTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateResultValueTaskFuncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateResultValueTaskFuncFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ResultWithFuncValueTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateResultValueTaskFuncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // ValueTask<Result>.TapBothAsync(Action, Action<IEnumerable<IError>>)
    private MethodWriter GenerateValueTaskResultActionSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ValueTaskResultWithAction_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateValueTaskResultActionSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskResultActionFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ValueTaskResultWithAction_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateValueTaskResultActionFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // Result.TapBothAsync(Func<Task>, Func<IEnumerable<IError>, Task>)
    private MethodWriter GenerateResultTaskFuncSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ResultWithFuncTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateAsyncTestBody(arity, "Task", "result", "Task"),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateResultTaskFuncFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_ResultWithFuncTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateAsyncTestFailureBody(arity, "Task", "result", "Task"),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // Task<Result>.TapBothAsync(Action, Action<IEnumerable<IError>>)
    private MethodWriter GenerateTaskResultActionSuccessTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_TaskResultWithAction_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateAwaitableResultActionSuccessBody(arity, "Task", "taskResult"),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskResultActionFailureTest(ushort arity)
    {
        return new MethodWriter($"TapBothAsync_TaskResultWithAction_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateAwaitableResultActionFailureBody(arity, "Task", "taskResult"),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GenerateTapBothSyncCall(arity);
        var assertions = GenerateTapBothSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """.Replace("\n\n", "\n");
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateTapBothSyncCall(arity);
        return $"""
                // Given
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(successSideEffectExecuted);
                Assert.True(failureSideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    // Shared helper methods for async test generation
    private string GenerateAsyncTestBody(ushort arity, string asyncType, string resultVarName, string completedTaskType)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GenerateResultTapBothAsyncCall(arity, asyncType, resultVarName, completedTaskType);
        var assertions = GenerateTapBothSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """.Replace("\n\n", "\n");
    }

    private string GenerateAsyncTestFailureBody(ushort arity, string asyncType, string resultVarName, string completedTaskType)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateResultTapBothAsyncCall(arity, asyncType, resultVarName, completedTaskType);
        return $"""
                // Given
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(successSideEffectExecuted);
                Assert.True(failureSideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateAwaitableResultActionSuccessBody(ushort arity, string asyncType, string awaitableResultVarName)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateAwaitableResultCreation(arity, asyncType, awaitableResultVarName);
        var call = GenerateAwaitableResultTapBothAsyncActionCall(arity, awaitableResultVarName);
        var assertions = GenerateTapBothSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """.Replace("\n\n", "\n");
    }

    private string GenerateAwaitableResultActionFailureBody(ushort arity, string asyncType, string awaitableResultVarName)
    {
        var creation = GenerateAwaitableFailureResultCreation(arity, asyncType, awaitableResultVarName);
        var call = GenerateAwaitableResultTapBothAsyncActionCall(arity, awaitableResultVarName);
        return $"""
                // Given
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(successSideEffectExecuted);
                Assert.True(failureSideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateAwaitableResultCreation(arity, "Task", "taskResult");
        var call = GenerateAwaitableResultTapBothAsyncCall(arity, "Task", "taskResult");
        var assertions = GenerateTapBothSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """.Replace("\n\n", "\n");
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateAwaitableFailureResultCreation(arity, "Task", "taskResult");
        var call = GenerateAwaitableResultTapBothAsyncCall(arity, "Task", "taskResult");
        return $"""
                // Given
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(successSideEffectExecuted);
                Assert.True(failureSideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateAwaitableResultCreation(arity, "ValueTask", "valueTaskResult");
        var call = GenerateAwaitableResultTapBothAsyncCall(arity, "ValueTask", "valueTaskResult");
        var assertions = GenerateTapBothSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """.Replace("\n\n", "\n");
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateAwaitableFailureResultCreation(arity, "ValueTask", "valueTaskResult");
        var call = GenerateAwaitableResultTapBothAsyncCall(arity, "ValueTask", "valueTaskResult");
        return $"""
                // Given
                var successSideEffectExecuted = false;
                var failureSideEffectExecuted = false;
                {creation}
                // When
                {call}
                // Then
                Assert.False(successSideEffectExecuted);
                Assert.True(failureSideEffectExecuted);
                Assert.False(tappedResult.IsSuccess);
                """;
    }

    // Result.TapBothAsync(Func<ValueTask>, Func<IEnumerable<IError>, ValueTask>) bodies
    private string GenerateResultValueTaskFuncSuccessBody(ushort arity)
    {
        return GenerateAsyncTestBody(arity, "ValueTask", "result", "ValueTask");
    }

    private string GenerateResultValueTaskFuncFailureBody(ushort arity)
    {
        return GenerateAsyncTestFailureBody(arity, "ValueTask", "result", "ValueTask");
    }

    // ValueTask<Result>.TapBothAsync(Action, Action<IEnumerable<IError>>) bodies
    private string GenerateValueTaskResultActionSuccessBody(ushort arity)
    {
        return GenerateAwaitableResultActionSuccessBody(arity, "ValueTask", "valueTaskResult");
    }

    private string GenerateValueTaskResultActionFailureBody(ushort arity)
    {
        return GenerateAwaitableResultActionFailureBody(arity, "ValueTask", "valueTaskResult");
    }

    private string GenerateTapBothSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var tappedResult = result.TapBoth(() => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
        }
        if (arity == 1)
        {
            return "var tappedResult = result.TapBoth(x => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = result.TapBoth(({parameters}) => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
    }

    // Shared helper: Result.TapBothAsync with Func<Task/ValueTask>
    private string GenerateResultTapBothAsyncCall(ushort arity, string asyncType, string resultVarName, string completedTaskType)
    {
        var completedTask = asyncType == "Task" ? "Task.CompletedTask" : "ValueTask.CompletedTask";

        if (arity == 0)
        {
            return $"var tappedResult = await {resultVarName}.TapBothAsync(() => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
        }
        if (arity == 1)
        {
            return $"var tappedResult = await {resultVarName}.TapBothAsync(x => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = await {resultVarName}.TapBothAsync(({parameters}) => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
    }

    // Shared helper: Task/ValueTask<Result>.TapBothAsync with Action
    private string GenerateAwaitableResultTapBothAsyncActionCall(ushort arity, string awaitableResultVarName)
    {
        if (arity == 0)
        {
            return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(() => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
        }
        if (arity == 1)
        {
            return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(x => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(({parameters}) => successSideEffectExecuted = true, errors => failureSideEffectExecuted = true);";
    }

    // Shared helper: Task/ValueTask<Result>.TapBothAsync with Func<Task/ValueTask>
    private string GenerateAwaitableResultTapBothAsyncCall(ushort arity, string asyncType, string awaitableResultVarName)
    {
        var completedTask = asyncType == "Task" ? "Task.CompletedTask" : "ValueTask.CompletedTask";

        if (arity == 0)
        {
            return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(() => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
        }
        if (arity == 1)
        {
            return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(x => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"var tappedResult = await {awaitableResultVarName}.TapBothAsync(({parameters}) => {{ successSideEffectExecuted = true; return {completedTask}; }}, errors => {{ failureSideEffectExecuted = true; return {completedTask}; }});";
    }

    // Shared helper: Task/ValueTask<Result> creation
    private string GenerateAwaitableResultCreation(ushort arity, string asyncType, string varName)
    {
        var fromResultMethod = asyncType == "Task" ? "Task.FromResult" : "ValueTask.FromResult";

        if (arity == 0)
        {
            return $"var {varName} = {fromResultMethod}(Result.Success());";
        }
        if (arity == 1)
        {
            return $"var {varName} = {fromResultMethod}(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var {varName} = {fromResultMethod}(Result.Success({values}));";
    }

    // Shared helper: Task/ValueTask<Result> failure creation
    private string GenerateAwaitableFailureResultCreation(ushort arity, string asyncType, string varName)
    {
        var fromResultMethod = asyncType == "Task" ? "Task.FromResult" : "ValueTask.FromResult";

        if (arity == 0)
        {
            return $"var {varName} = {fromResultMethod}(Result.Failure(\"Test error\"));";
        }
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var {varName} = {fromResultMethod}(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateTapBothSuccessAssertions(ushort arity)
    {
        return """
               Assert.True(successSideEffectExecuted);
               Assert.False(failureSideEffectExecuted);
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
