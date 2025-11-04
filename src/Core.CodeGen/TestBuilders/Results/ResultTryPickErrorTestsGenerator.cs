using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TryPickError extension methods (Task, ValueTask).
/// </summary>
internal sealed class ResultTryPickErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTryPickErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultTryPickErrorTestsGenerator(string baseNamespace,
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
        return GenerateVariants(arity,
                                ClassName,
                                (GenerateTaskTests,      true),
                                (GenerateValueTaskTests, true));
    }

    private ClassWriter GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTryPickErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TryPickError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureFoundTest(arity));
        cw.AddMethod(GenerateTaskFailureNotFoundTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTryPickErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TryPickError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureFoundTest(arity));
        cw.AddMethod(GenerateValueTaskFailureNotFoundTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"TryPickErrorTask_Arity{arity}_Success_ShouldReturnFalse", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureFoundTest(ushort arity) => new($"TryPickErrorTask_Arity{arity}_Failure_ShouldReturnFoundError", "async Task", GenerateTaskFailureFoundBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureNotFoundTest(ushort arity) => new($"TryPickErrorTask_Arity{arity}_Failure_ShouldReturnNotFound", "async Task", GenerateTaskFailureNotFoundBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"TryPickErrorValueTask_Arity{arity}_Success_ShouldReturnFalse", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureFoundTest(ushort arity) => new($"TryPickErrorValueTask_Arity{arity}_Failure_ShouldReturnFoundError", "async Task", GenerateValueTaskFailureFoundBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureNotFoundTest(ushort arity) => new($"TryPickErrorValueTask_Arity{arity}_Failure_ShouldReturnNotFound", "async Task", GenerateValueTaskFailureNotFoundBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateTryPickErrorTaskCall(arity);
        var given      = arity == 0 ? new[] { creation } : new[] { testValues, creation };
        return BuildTestBody(given,
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateTaskFailureFoundBody(ushort arity)
    {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorTaskCall(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.True(result.Success);", "Assert.NotNull(result.Error);", "Assert.Equal(\"Test error\", result.Error.Message);"]);
    }

    private string GenerateTaskFailureNotFoundBody(ushort arity)
    {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorTaskCallNotFound(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateTryPickErrorValueTaskCall(arity);
        var given      = arity == 0 ? new[] { creation } : new[] { testValues, creation };
        return BuildTestBody(given,
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateValueTaskFailureFoundBody(ushort arity)
    {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorValueTaskCall(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.True(result.Success);", "Assert.NotNull(result.Error);", "Assert.Equal(\"Test error\", result.Error.Message);"]);
    }

    private string GenerateValueTaskFailureNotFoundBody(ushort arity)
    {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorValueTaskCallNotFound(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateTryPickErrorTaskCall(ushort arity) => arity == 0 ? "var result = await taskResult.TryPickErrorAsync(e => Task.FromResult(e is Error));" : $"var result = await taskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => Task.FromResult(e is Error));";
    private string GenerateTryPickErrorTaskCallNotFound(ushort arity) => arity == 0 ? "var result = await taskResult.TryPickErrorAsync(e => Task.FromResult(e is ExceptionalError));" : $"var result = await taskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => Task.FromResult(e is ExceptionalError));";
    private string GenerateTryPickErrorValueTaskCall(ushort arity) => arity == 0 ? "var result = await valueTaskResult.TryPickErrorAsync(e => ValueTask.FromResult(e is Error));" : $"var result = await valueTaskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => ValueTask.FromResult(e is Error));";
    private string GenerateTryPickErrorValueTaskCallNotFound(ushort arity) => arity == 0 ? "var result = await valueTaskResult.TryPickErrorAsync(e => ValueTask.FromResult(e is ExceptionalError));" : $"var result = await valueTaskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => ValueTask.FromResult(e is ExceptionalError));";
}
