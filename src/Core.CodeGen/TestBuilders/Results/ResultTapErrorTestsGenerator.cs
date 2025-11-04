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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapError" };
        cw.AddMethod(new MethodWriter($"TapError_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                      "void",
                                      GenerateSyncSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapError_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                      "void",
                                      GenerateSyncFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TapError" };
        cw.AddMethod(new MethodWriter($"TapErrorTask_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                      "async Task",
                                      GenerateTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapErrorTask_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                      "async Task",
                                      GenerateTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TapError" };
        cw.AddMethod(new MethodWriter($"TapErrorValueTask_Arity{arity}_Success_ShouldNotExecuteSideEffect",
                                      "async Task",
                                      GenerateValueTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapErrorValueTask_Arity{arity}_Failure_ShouldExecuteSideEffect",
                                      "async Task",
                                      GenerateValueTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.SideEffects.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Success();" : GenerateResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = result.TapError(_ => sideEffectExecuted = true);" : $"var tappedResult = result.TapError<{GenerateTypeParams(arity)}>(_ => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : $"var result = Result.Failure<{GenerateTypeParams(arity)}>(\"Test error\");";
        var call = arity == 0 ? "var tappedResult = result.TapError(errs => sideEffectExecuted = true);" : $"var tappedResult = result.TapError<{GenerateTypeParams(arity)}>(errs => sideEffectExecuted = true);";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var taskResult = Task.FromResult(Result.Success());" : GenerateTaskResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await taskResult.TapErrorAsync(_ => { sideEffectExecuted = true; return Task.CompletedTask; });" : $"var tappedResult = await taskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(_ => {{ sideEffectExecuted = true; return Task.CompletedTask; }});";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var taskResult = Task.FromResult(Result.Failure(\"Test error\"));" : GenerateTaskFailureResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await taskResult.TapErrorAsync(errs => { sideEffectExecuted = true; return Task.CompletedTask; });" : $"var tappedResult = await taskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(errs => {{ sideEffectExecuted = true; return Task.CompletedTask; }});";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var valueTaskResult = ValueTask.FromResult(Result.Success());" : GenerateValueTaskResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await valueTaskResult.TapErrorAsync(_ => { sideEffectExecuted = true; return ValueTask.CompletedTask; });" : $"var tappedResult = await valueTaskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(_ => {{ sideEffectExecuted = true; return ValueTask.CompletedTask; }});";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.False(sideEffectExecuted);", "Assert.True(tappedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var valueTaskResult = ValueTask.FromResult(Result.Failure(\"Test error\"));" : GenerateValueTaskFailureResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await valueTaskResult.TapErrorAsync(errs => { sideEffectExecuted = true; return ValueTask.CompletedTask; });" : $"var tappedResult = await valueTaskResult.TapErrorAsync<{GenerateTypeParams(arity)}>(errs => {{ sideEffectExecuted = true; return ValueTask.CompletedTask; }});";
        return BuildTestBody(["var sideEffectExecuted = false;", creation], [call], ["Assert.True(sideEffectExecuted);", "Assert.False(tappedResult.IsSuccess);"]);
    }
}
