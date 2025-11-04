using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MapError extension methods (sync) and MapErrorAsync (Task, ValueTask).
/// </summary>
internal sealed class ResultMapErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTestsIfApplicable, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter? GenerateSyncTestsIfApplicable(ushort arity)
    {
        if (arity == 0) return null;
        var cw = new ClassWriter($"ResultMapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MapError" };
        cw.AddMethod(new MethodWriter($"MapError_Arity{arity}_Success_ShouldNotMapError",
                                      "void",
                                      GenerateSyncSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MapError_Arity{arity}_Failure_ShouldMapError",
                                      "void",
                                      GenerateSyncFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task MapErrorAsync" };
        cw.AddMethod(new MethodWriter($"MapErrorTask_Arity{arity}_Success_ShouldNotMapError",
                                      "async Task",
                                      GenerateTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MapErrorTask_Arity{arity}_Failure_ShouldMapError",
                                      "async Task",
                                      GenerateTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        if (arity == 0) cw.AddMethod(new MethodWriter($"MapErrorAsyncTaskAwaitable_Arity{arity}_Failure_ShouldMapErrors",
                                                      "async Task",
                                                      GenerateTaskAwaitableFailureBody(),
                                                      attributes: [new FactAttributeReference()],
                                                      usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask MapErrorAsync" };
        cw.AddMethod(new MethodWriter($"MapErrorValueTask_Arity{arity}_Success_ShouldNotMapError",
                                      "async Task",
                                      GenerateValueTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MapErrorValueTask_Arity{arity}_Failure_ShouldMapError",
                                      "async Task",
                                      GenerateValueTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        if (arity == 0) cw.AddMethod(new MethodWriter($"MapErrorAsyncValueTaskAwaitable_Arity{arity}_Failure_ShouldMapErrors",
                                                      "async Task",
                                                      GenerateValueTaskAwaitableFailureBody(),
                                                      attributes: [new FactAttributeReference()],
                                                      usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(e => new Error(e.Message + \" MAPPED\"));";
        return BuildTestBody([testValues, creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(e => new Error(e.Message + \" MAPPED\"));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Success();" : GenerateResultCreation(arity);
        var wrap = arity == 0 ? "Task.FromResult(result)" : "taskResult";
        var call = arity == 0 ? "var mappedResult = await Task.FromResult(result).MapErrorAsync(e => Task.FromResult(new Error(e.Message + \" MAPPED\")));" : $"var mappedResult = await taskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(e => Task.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : GenerateFailureResultCreation(arity);
        var call = arity == 0 ? "var mappedResult = await Task.FromResult(result).MapErrorAsync(e => Task.FromResult(new Error(e.Message + \" MAPPED\")));" : $"var mappedResult = await taskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(e => Task.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateTaskAwaitableFailureBody()
    {
        const string creation = "var result = Result.Failure(\"Test error\");";
        const string call = "var mappedResult = await Task.FromResult(result).MapErrorAsync(e => Task.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Success();" : GenerateResultCreation(arity);
        var call = arity == 0 ? "var mappedResult = await ValueTask.FromResult(result).MapErrorAsync(e => ValueTask.FromResult(new Error(e.Message + \" MAPPED\")));" : $"var mappedResult = await valueTaskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(e => ValueTask.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = arity == 0 ? "var result = Result.Failure(\"Test error\");" : GenerateFailureResultCreation(arity);
        var call = arity == 0 ? "var mappedResult = await ValueTask.FromResult(result).MapErrorAsync(e => ValueTask.FromResult(new Error(e.Message + \" MAPPED\")));" : $"var mappedResult = await valueTaskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(e => ValueTask.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateValueTaskAwaitableFailureBody()
    {
        const string creation = "var result = Result.Failure(\"Test error\");";
        const string call = "var mappedResult = await ValueTask.FromResult(result).MapErrorAsync(e => ValueTask.FromResult(new Error(e.Message + \" MAPPED\")));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }
}
