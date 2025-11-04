using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Tap extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTapTests";
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    public ResultTapTestsGenerator(string baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        // Original produced multiple classes; replicate with factories
        var sync      = (Func<ushort, ClassWriter?>)GenerateSyncTests;
        var asyncResT = (Func<ushort, ClassWriter?>)(a => GenerateAsyncResultTests(a, false));
        var asyncResV = (Func<ushort, ClassWriter?>)(a => GenerateAsyncResultTests(a, true));
        var asyncAwT  = (Func<ushort, ClassWriter?>)(a => GenerateAsyncAwaitableTests(a, false));
        var asyncAwV  = (Func<ushort, ClassWriter?>)(a => GenerateAsyncAwaitableTests(a, true));
        return GenerateVariants(arity, ClassName, (sync, false), (asyncResT, true), (asyncResV, true), (asyncAwT, true), (asyncAwV, true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultTapSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Tap" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncResultTests(ushort arity, bool isValueTask) {
        var type = isValueTask ? "ValueTask" : "Task";
        var cw   = new ClassWriter($"ResultTapAsync{type}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - TapAsync(Result, Func<{type}>)" };
        cw.AddMethod(GenerateAsyncResultSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncResultFailureTest(arity, isValueTask));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{type}s";
        return cw;
    }

    private ClassWriter GenerateAsyncAwaitableTests(ushort arity, bool isValueTask) {
        var type = isValueTask ? "ValueTask" : "Task";
        var cw   = new ClassWriter($"ResultTapAsyncAwaitable{type}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - TapAsync({type}<Result>, Action/Func<{type}>)" };
        cw.AddMethod(GenerateAsyncAwaitableActionSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableActionFailureTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableFuncSuccessTest(arity, isValueTask));
        cw.AddMethod(GenerateAsyncAwaitableFuncFailureTest(arity, isValueTask));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{type}s";
        return cw;
    }

    // Sync
    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Tap_Arity{arity}_Success_ShouldExecuteSideEffect", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Tap_Arity{arity}_Failure_ShouldNotExecuteSideEffect", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var call       = arity == 0 ? "var tappedResult = result.Tap(() => sideEffectExecuted = true);" :
                           arity == 1 ? "var tappedResult = result.Tap(x => sideEffectExecuted = true);" :
                           $"var tappedResult = result.Tap(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        return BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = arity == 0 ? "var tappedResult = result.Tap(() => sideEffectExecuted = true);" :
                           arity == 1 ? "var tappedResult = result.Tap(x => sideEffectExecuted = true);" :
                           $"var tappedResult = result.Tap(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", failureCreation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    // Async Result (Func<Task/ValueTask>)
    private MethodWriter GenerateAsyncResultSuccessTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await result.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var body       = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
                                       ]);
        return new MethodWriter($"TapAsync_{type}_Arity{arity}_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncResultFailureTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var failure    = GenerateFailureResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await result.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", failure], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_{type}_Arity{arity}_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    // Awaitable Action (Task<Result>/ValueTask<Result>)
    private MethodWriter GenerateAsyncAwaitableActionSuccessTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var varName    = isValueTask ? "valueTaskResult" : "taskResult";
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = isValueTask ? GenerateValueTaskResultCreation(arity) : GenerateTaskResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await {varName}.TapAsync({lambdaHead} => sideEffectExecuted = true);";
        var body       = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
                                       ]);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Action_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableActionFailureTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var varName    = isValueTask ? "valueTaskResult" : "taskResult";
        var creation   = isValueTask ? GenerateValueTaskFailureResultCreation(arity) : GenerateTaskFailureResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await {varName}.TapAsync({lambdaHead} => sideEffectExecuted = true);";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Action_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    // Awaitable Func (returns Task/ValueTask)
    private MethodWriter GenerateAsyncAwaitableFuncSuccessTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var varName    = isValueTask ? "valueTaskResult" : "taskResult";
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = isValueTask ? GenerateValueTaskResultCreation(arity) : GenerateTaskResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await {varName}.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var body       = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
                                       ]);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Func_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableFuncFailureTest(ushort arity, bool isValueTask) {
        var type       = isValueTask ? "ValueTask" : "Task";
        var varName    = isValueTask ? "valueTaskResult" : "taskResult";
        var creation   = isValueTask ? GenerateValueTaskFailureResultCreation(arity) : GenerateTaskFailureResultCreation(arity);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await {varName}.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {type}.CompletedTask; }});";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_{type}_Awaitable_Arity{arity}_Func_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()], usings: GetUsings());
    }
}
