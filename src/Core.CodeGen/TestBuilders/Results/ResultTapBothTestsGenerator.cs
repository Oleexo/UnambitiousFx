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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTapBothSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TapBoth" };
        cw.AddMethod(new MethodWriter($"TapBoth_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                      "void",
                                      GenerateSyncSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapBoth_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
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
        var cw = new ClassWriter($"ResultTapBothTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TapBoth" };
        cw.AddMethod(new MethodWriter($"TapBothTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                      "async Task",
                                      GenerateTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapBothTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
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
        var cw = new ClassWriter($"ResultTapBothValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TapBoth" };
        cw.AddMethod(new MethodWriter($"TapBothValueTask_Arity{arity}_Success_ShouldExecuteSuccessSideEffect",
                                      "async Task",
                                      GenerateValueTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TapBothValueTask_Arity{arity}_Failure_ShouldExecuteFailureSideEffect",
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
        var call = arity == 0 ? "var tappedResult = result.TapBoth(() => successExecuted = true, _ => failureExecuted = true);" : $"var tappedResult = result.TapBoth<{GenerateTypeParams(arity)}>(() => successExecuted = true, _ => failureExecuted = true);";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
                             ]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : $"var result = Result.Failure<{GenerateTypeParams(arity)}>(\"Test error\");";
        var call = arity == 0 ? "var tappedResult = result.TapBoth(() => successExecuted = true, _ => failureExecuted = true);" : $"var tappedResult = result.TapBoth<{GenerateTypeParams(arity)}>(() => successExecuted = true, _ => failureExecuted = true);";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
                             ]);
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var taskResult = Task.FromResult(Result.Success());" : GenerateTaskResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await taskResult.TapBothAsync(() => { successExecuted = true; return Task.CompletedTask; }, _ => { failureExecuted = true; return Task.CompletedTask; });" : $"var tappedResult = await taskResult.TapBothAsync<{GenerateTypeParams(arity)}>(() => {{ successExecuted = true; return Task.CompletedTask; }}, _ => {{ failureExecuted = true; return Task.CompletedTask; }});";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
                             ]);
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var taskResult = Task.FromResult(Result.Failure(\"Test error\"));" : GenerateTaskFailureResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await taskResult.TapBothAsync(() => { successExecuted = true; return Task.CompletedTask; }, _ => { failureExecuted = true; return Task.CompletedTask; });" : $"var tappedResult = await taskResult.TapBothAsync<{GenerateTypeParams(arity)}>(() => {{ successExecuted = true; return Task.CompletedTask; }}, _ => {{ failureExecuted = true; return Task.CompletedTask; }});";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
                             ]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var valueTaskResult = ValueTask.FromResult(Result.Success());" : GenerateValueTaskResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await valueTaskResult.TapBothAsync(() => { successExecuted = true; return ValueTask.CompletedTask; }, _ => { failureExecuted = true; return ValueTask.CompletedTask; });" : $"var tappedResult = await valueTaskResult.TapBothAsync<{GenerateTypeParams(arity)}>(() => {{ successExecuted = true; return ValueTask.CompletedTask; }}, _ => {{ failureExecuted = true; return ValueTask.CompletedTask; }});";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.True(successExecuted);", "Assert.False(failureExecuted);", "Assert.True(tappedResult.IsSuccess);"
                             ]);
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var valueTaskResult = ValueTask.FromResult(Result.Failure(\"Test error\"));" : GenerateValueTaskFailureResultCreation(arity);
        var call = arity == 0 ? "var tappedResult = await valueTaskResult.TapBothAsync(() => { successExecuted = true; return ValueTask.CompletedTask; }, _ => { failureExecuted = true; return ValueTask.CompletedTask; });" : $"var tappedResult = await valueTaskResult.TapBothAsync<{GenerateTypeParams(arity)}>(() => {{ successExecuted = true; return ValueTask.CompletedTask; }}, _ => {{ failureExecuted = true; return ValueTask.CompletedTask; }});";
        return BuildTestBody(["var successExecuted = false;", "var failureExecuted = false;", creation], [call], ["Assert.False(successExecuted);", "Assert.True(failureExecuted);", "Assert.False(tappedResult.IsSuccess);"
                             ]);
    }
}
