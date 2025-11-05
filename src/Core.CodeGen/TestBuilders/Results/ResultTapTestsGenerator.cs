using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Tap extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0;
    private const string ClassName           = "ResultTapTests";
    private const string ExtensionsNamespace = "Results.Extensions.SideEffects";

    public ResultTapTestsGenerator(string               baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        // Original produced multiple classes; replicate with factories
        var sync      = (Func<ushort, ClassWriter?>)GenerateSyncTests;
        var asyncResT = (Func<ushort, ClassWriter?>)(a => GenerateAsyncResultTests(a, "Task"));
        var asyncResV = (Func<ushort, ClassWriter?>)(a => GenerateAsyncResultTests(a, "ValueTask"));
        var asyncAwT  = (Func<ushort, ClassWriter?>)(a => GenerateAsyncAwaitableTests(a, "Task"));
        var asyncAwV  = (Func<ushort, ClassWriter?>)(a => GenerateAsyncAwaitableTests(a, "ValueTask"));
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

    private ClassWriter GenerateAsyncResultTests(ushort arity,
                                                 string asyncType) {
        var cw = new ClassWriter($"ResultTapAsyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - TapAsync(Result, Func<{asyncType}>)" };
        cw.AddMethod(GenerateAsyncResultSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncResultFailureTest(arity, asyncType));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private ClassWriter GenerateAsyncAwaitableTests(ushort arity,
                                                    string asyncType) {
        var cw = new ClassWriter($"ResultTapAsyncAwaitableTestsArity{arity}", Visibility.Public)
            { Region = $"Arity {arity} - TapAsync({asyncType}<Result>, Action/Func<{asyncType}>)" };
        cw.AddMethod(GenerateAsyncAwaitableActionSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncAwaitableActionFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncAwaitableFuncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncAwaitableFuncFailureTest(arity, asyncType));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    // Sync
    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Tap_Arity{arity}_Success_ShouldExecuteSideEffect", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Tap_Arity{arity}_Failure_ShouldNotExecuteSideEffect", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = result.Tap(() => sideEffectExecuted = true);" :
                   arity == 1 ? "var tappedResult = result.Tap(x => sideEffectExecuted = true);" :
                                $"var tappedResult = result.Tap(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        return BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = result.Tap(() => sideEffectExecuted = true);" :
                   arity == 1 ? "var tappedResult = result.Tap(x => sideEffectExecuted = true);" :
                                $"var tappedResult = result.Tap(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", failureCreation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    // Async Result (Func<Task/ValueTask>)
    private MethodWriter GenerateAsyncResultSuccessTest(ushort arity,
                                                        string asyncType) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation   = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {asyncType}.CompletedTask; }});";
        var body = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], [
            "Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
        return new MethodWriter($"TapAsync_Arity{arity}_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncResultFailureTest(ushort arity,
                                                        string asyncType) {
        var failure    = GenerateAsyncFailureResultCreation(arity, asyncType);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {asyncType}.CompletedTask; }});";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", failure], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_Arity{arity}_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // Awaitable Action (Task<Result>/ValueTask<Result>)
    private MethodWriter GenerateAsyncAwaitableActionSuccessTest(ushort arity,
                                                                 string asyncType) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation   = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => sideEffectExecuted = true);";
        var body = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], [
            "Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
        return new MethodWriter($"TapAsync_Awaitable_Arity{arity}_Action_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableActionFailureTest(ushort arity,
                                                                 string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);

        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => sideEffectExecuted = true);";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_Awaitable_Arity{arity}_Action_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    // Awaitable Func (returns Task/ValueTask)
    private MethodWriter GenerateAsyncAwaitableFuncSuccessTest(ushort arity,
                                                               string asyncType) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation   = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {asyncType}.CompletedTask; }});";
        var body = BuildTestBody([testValues, "var sideEffectExecuted = false;", creation], [call], [
            "Assert.True(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
        return new MethodWriter($"TapAsync_Awaitable_Arity{arity}_Func_Success_ShouldExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncAwaitableFuncFailureTest(ushort arity,
                                                               string asyncType) {
        var creation   = GenerateAsyncFailureResultCreation(arity, asyncType);
        var lambdaHead = arity == 0 ? "()" : arity == 1 ? "x" : $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))})";
        var call       = $"var tappedResult = await taskResult.TapAsync({lambdaHead} => {{ sideEffectExecuted = true; return {asyncType}.CompletedTask; }});";
        var body       = BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
        return new MethodWriter($"TapAsync_Awaitable_Arity{arity}_Func_Failure_ShouldNotExecuteSideEffect", "async Task", body, attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }
}
