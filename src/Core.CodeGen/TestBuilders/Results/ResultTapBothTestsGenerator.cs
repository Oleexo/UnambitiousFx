using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TapBoth extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapBothTestsGenerator : ResultTestGeneratorBase
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
        return GenerateVariants(arity, ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapBothSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapBoth" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultTapBoth{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} TapBoth" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncWithSyncActionsSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncWithSyncActionsFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.SideEffects.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
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

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"TapBoth{asyncType}_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"TapBoth{asyncType}_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncWithSyncActionsSuccessTest(ushort arity,
                                                                 string asyncType)
    {
        return new MethodWriter($"TapBoth{asyncType}WithSyncActions_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                "async Task",
                                GenerateAsyncWithSyncActionsSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncWithSyncActionsFailureTest(ushort arity,
                                                                 string asyncType)
    {
        return new MethodWriter($"TapBoth{asyncType}WithSyncActions_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
                                "async Task",
                                GenerateAsyncWithSyncActionsFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var values = GenerateTestValues(arity);
        var creation = arity == 0
                           ? "var result = Result.Success();"
                           : GenerateResultCreation(arity);
        var call = arity == 0
                       ? "var tappedResult = result.TapBoth(() => successExecuted = true, _ => failureExecuted = true);"
                       : $"var tappedResult = result.TapBoth<{GenerateTypeParams(arity)}>(({GenerateValueParams(arity)}) => successExecuted = true, _ => failureExecuted = true);";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", values, creation], [call], [
            "Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = arity == 0
                           ? "var result = Result.Failure(\"Test error\");"
                           : $"var result = Result.Failure<{GenerateTypeParams(arity)}>(\"Test error\");";
        var call = arity == 0
                       ? "var tappedResult = result.TapBoth(() => successExecuted = true, _ => failureExecuted = true);"
                       : $"var tappedResult = result.TapBoth<{GenerateTypeParams(arity)}>(({GenerateValueParams(arity)}) => successExecuted = true, _ => failureExecuted = true);";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], [
            "Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var values = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateAsyncTapBothCall(arity, asyncType, "successExecuted = true", "failureExecuted = true");
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", values, creation], [call], [
            "Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateAsyncTapBothCall(arity, asyncType, "successExecuted = true", "failureExecuted = true");
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], [
            "Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateAsyncTapBothCall(ushort arity,
                                            string asyncType,
                                            string successSideEffectCode,
                                            string failureSideEffectCode)
    {
        var returnStmt = $"return {asyncType}.CompletedTask;";

        if (arity == 0)
        {
            return $"var tappedResult = await taskResult.TapBothAsync(() => {{ {successSideEffectCode}; {returnStmt} }}, _ => {{ {failureSideEffectCode}; {returnStmt} }});";
        }

        return
            $"var tappedResult = await taskResult.TapBothAsync<{GenerateTypeParams(arity)}>(({GenerateValueParams(arity)}) => {{ {successSideEffectCode}; {returnStmt} }}, _ => {{ {failureSideEffectCode}; {returnStmt} }});";
    }

    private string GenerateAsyncWithSyncActionsSuccessBody(ushort arity,
                                                           string asyncType)
    {
        var values = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateAsyncTapBothWithSyncActionsCall(arity, "successExecuted = true", "failureExecuted = true");
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", values, creation], [call], [
            "Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateAsyncWithSyncActionsFailureBody(ushort arity,
                                                           string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateAsyncTapBothWithSyncActionsCall(arity, "successExecuted = true", "failureExecuted = true");
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], [
            "Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
        ]);
    }

    private string GenerateAsyncTapBothWithSyncActionsCall(ushort arity,
                                                           string successSideEffectCode,
                                                           string failureSideEffectCode)
    {
        if (arity == 0)
        {
            return $"var tappedResult = await taskResult.TapBothAsync(() => {successSideEffectCode}, _ => {failureSideEffectCode});";
        }

        return
            $"var tappedResult = await taskResult.TapBothAsync<{GenerateTypeParams(arity)}>(({GenerateValueParams(arity)}) => {successSideEffectCode}, _ => {failureSideEffectCode});";
    }
}
