using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TapError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTapErrorTestsGenerator : ResultTestGeneratorBase
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
        return GenerateVariants(arity, ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultTapError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} TapError" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncWithSyncActionSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncWithSyncActionFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.SideEffects.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
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

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"TapError{asyncType}_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"TapError{asyncType}_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncWithSyncActionSuccessTest(ushort arity,
                                                                string asyncType)
    {
        return new MethodWriter($"TapError{asyncType}WithSyncAction_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                "async Task",
                                GenerateAsyncWithSyncActionSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncWithSyncActionFailureTest(ushort arity,
                                                                string asyncType)
    {
        return new MethodWriter($"TapError{asyncType}WithSyncAction_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                "async Task",
                                GenerateAsyncWithSyncActionFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var creation = arity == 0
                           ? "var result = Result.Success();"
                           : GenerateResultCreation(arity);
        var values = GenerateTestValues(arity);
        var call = arity == 0
                       ? "var tappedResult = result.TapError(_ => sideEffectExecuted = true);"
                       : $"var tappedResult = result.TapError<{GenerateTypeParams(arity)}>(_ => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", values, creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = arity == 0
                           ? "var result = Result.Failure(\"Test error\");"
                           : $"var result = Result.Failure<{GenerateTypeParams(arity)}>(\"Test error\");";
        var call = arity == 0
                       ? "var tappedResult = result.TapError(errs => sideEffectExecuted = true);"
                       : $"var tappedResult = result.TapError<{GenerateTypeParams(arity)}>(errs => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var values = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateAsyncTapErrorCall(arity, asyncType, "sideEffectExecuted = true");
        return BuildTestBody(["var sideEffectExecuted = false;", values, creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateAsyncTapErrorCall(arity, asyncType, "sideEffectExecuted = true");
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncTapErrorCall(ushort arity,
                                             string asyncType,
                                             string sideEffectCode)
    {
        var returnStmt = $"return {asyncType}.CompletedTask;";

        if (arity == 0)
        {
            return $"var tappedResult = await taskResult.TapErrorAsync(_ => {{ {sideEffectCode}; {returnStmt} }});";
        }

        return $"var tappedResult = await taskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(_ => {{ {sideEffectCode}; {returnStmt} }});";
    }

    private string GenerateAsyncWithSyncActionSuccessBody(ushort arity,
                                                          string asyncType)
    {
        var values = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateAsyncTapErrorWithSyncActionCall(arity, asyncType, "sideEffectExecuted = true");
        return BuildTestBody(["var sideEffectExecuted = false;", values, creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncWithSyncActionFailureBody(ushort arity,
                                                          string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateAsyncTapErrorWithSyncActionCall(arity, asyncType, "sideEffectExecuted = true");
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncTapErrorWithSyncActionCall(ushort arity,
                                                           string asyncType,
                                                           string sideEffectCode)
    {
        if (arity == 0)
        {
            return $"var tappedResult = await taskResult.TapErrorAsync(_ => {sideEffectCode});";
        }

        return $"var tappedResult = await taskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(_ => {sideEffectCode});";
    }
}
