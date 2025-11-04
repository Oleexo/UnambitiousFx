using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.FilterError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFilterErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultFilterErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFilterErrorTestsGenerator(string baseNamespace,
                                           FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultFilterErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync FilterError" };
        cw.AddMethod(new MethodWriter($"FilterError_Arity{arity}_Success_ShouldNotFilterError", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"FilterError_Arity{arity}_Failure_ShouldFilterError", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFilterErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task FilterError" };
        cw.AddMethod(new MethodWriter($"FilterErrorTask_Arity{arity}_Success_ShouldNotFilterError", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"FilterErrorTask_Arity{arity}_Failure_ShouldFilterError", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFilterErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask FilterError" };
        cw.AddMethod(new MethodWriter($"FilterErrorValueTask_Arity{arity}_Success_ShouldNotFilterError", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"FilterErrorValueTask_Arity{arity}_Failure_ShouldFilterError", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateFilterErrorSyncCall(arity) };
        var then = new[] { "Assert.True(filteredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when = new[] { GenerateFilterErrorSyncCall(arity) };
        var then = new[] { "Assert.False(filteredResult.IsSuccess);", "Assert.Single(filteredResult.Errors);", "Assert.Equal(\"Test error\", filteredResult.Errors.First().Message);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateTaskResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateTaskResultCreation(arity) };
        var when = new[] { GenerateFilterErrorTaskCall(arity) };
        var then = new[] { "Assert.True(filteredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var given = new[] { GenerateTaskFailureResultCreation(arity) };
        var when = new[] { GenerateFilterErrorTaskCall(arity) };
        var then = new[] { "Assert.False(filteredResult.IsSuccess);", "Assert.Single(filteredResult.Errors);", "Assert.Equal(\"Test error\", filteredResult.Errors.First().Message);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateValueTaskResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateValueTaskResultCreation(arity) };
        var when = new[] { GenerateFilterErrorValueTaskCall(arity) };
        var then = new[] { "Assert.True(filteredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var given = new[] { GenerateValueTaskFailureResultCreation(arity) };
        var when = new[] { GenerateFilterErrorValueTaskCall(arity) };
        var then = new[] { "Assert.False(filteredResult.IsSuccess);", "Assert.Single(filteredResult.Errors);", "Assert.Equal(\"Test error\", filteredResult.Errors.First().Message);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateFilterErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = result.FilterError(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = result.FilterError<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }

    private string GenerateFilterErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = await taskResult.FilterErrorAsync(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = await taskResult.FilterErrorAsync<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }

    private string GenerateFilterErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = await valueTaskResult.FilterErrorAsync(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = await valueTaskResult.FilterErrorAsync<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }
}
