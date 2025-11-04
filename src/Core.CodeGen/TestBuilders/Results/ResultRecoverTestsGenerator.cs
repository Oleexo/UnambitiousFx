using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Recover extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultRecoverTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1; // Recover doesn't support arity 0 (no value to recover to)
    private const string ClassName = "ResultRecoverTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultRecoverTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultRecoverSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Recover" };
        cw.AddMethod(new MethodWriter($"Recover_Arity{arity}_Success_ShouldNotRecover", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Recover_Arity{arity}_Failure_ShouldRecover", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultRecoverTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Recover" };
        cw.AddMethod(new MethodWriter($"RecoverTask_Arity{arity}_Success_ShouldNotRecover", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"RecoverTask_Arity{arity}_Failure_ShouldRecover", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultRecoverValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Recover" };
        cw.AddMethod(new MethodWriter($"RecoverValueTask_Arity{arity}_Success_ShouldNotRecover", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"RecoverValueTask_Arity{arity}_Failure_ShouldRecover", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var given = new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when  = new[] { GenerateRecoverSyncCall(arity) };
        var then  = new[] { "Assert.True(recoveredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when  = new[] { GenerateRecoverSyncCall(arity) };
        var then  = GenerateRecoverSuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var given = new[] { GenerateTestValues(arity), GenerateTaskResultCreation(arity) };
        var when  = new[] { GenerateRecoverTaskCall(arity) };
        var then  = new[] { "Assert.True(recoveredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var given = new[] { GenerateTaskFailureResultCreation(arity) };
        var when  = new[] { GenerateRecoverTaskCall(arity) };
        var then  = GenerateRecoverSuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var given = new[] { GenerateTestValues(arity), GenerateValueTaskResultCreation(arity) };
        var when  = new[] { GenerateRecoverValueTaskCall(arity) };
        var then  = new[] { "Assert.True(recoveredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var given = new[] { GenerateValueTaskFailureResultCreation(arity) };
        var when  = new[] { GenerateRecoverValueTaskCall(arity) };
        var then  = GenerateRecoverSuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateRecoverSyncCall(ushort arity) {
        var typeParams    = GenerateTypeParams(arity);
        var recoveryValue = GetRecoveryValueExpression(arity);
        return $"var recoveredResult = result.Recover<{typeParams}>(errors => {recoveryValue});";
    }

    private string GenerateRecoverTaskCall(ushort arity) {
        var typeParams    = GenerateTypeParams(arity);
        var recoveryValue = GetRecoveryValueExpression(arity);
        return $"var recoveredResult = await taskResult.RecoverAsync<{typeParams}>(errors => Task.FromResult({recoveryValue}));";
    }

    private string GenerateRecoverValueTaskCall(ushort arity) {
        var typeParams    = GenerateTypeParams(arity);
        var recoveryValue = GetRecoveryValueExpression(arity);
        return $"var recoveredResult = await valueTaskResult.RecoverAsync<{typeParams}>(errors => ValueTask.FromResult({recoveryValue}));";
    }

    private string GetRecoveryValueExpression(ushort arity)
    {
        if (arity == 1)
        {
            return "999";
        }

        var recoveryValues = string.Join(", ", Enumerable.Range(1, arity).Select(GetRecoveryValue));
        return $"({recoveryValues})";
    }

    private string GenerateRecoverSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return """
                   Assert.True(recoveredResult.IsSuccess);
                   Assert.True(recoveredResult.TryGet(out var recoveredValue));
                   Assert.Equal(999, recoveredValue);
                   """;
        }

        return "Assert.True(recoveredResult.IsSuccess);";
    }

    private string GetRecoveryValue(int index)
    {
        return index switch { 1 => "999", 2 => "\"recovered\"", 3 => "false", 4 => "9.99", 5 => "888L", _ => $"\"recovered{index}\"" };
    }
}
