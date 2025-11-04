using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MatchError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMatchErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultMatchErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMatchErrorTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultMatchErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MatchError" };
        cw.AddMethod(new MethodWriter($"MatchError_Arity{arity}_Success_ShouldReturnDefault", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MatchError_Arity{arity}_Failure_ShouldMatchError", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task MatchError" };
        cw.AddMethod(new MethodWriter($"MatchErrorTask_Arity{arity}_Success_ShouldReturnDefault", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MatchErrorTask_Arity{arity}_Failure_ShouldMatchError", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask MatchError" };
        cw.AddMethod(new MethodWriter($"MatchErrorValueTask_Arity{arity}_Success_ShouldReturnDefault", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MatchErrorValueTask_Arity{arity}_Failure_ShouldMatchError", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateMatchErrorSyncCall(arity) };
        var then = new[] { "Assert.Equal(\"default\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var given = new[] { GenerateErrorTypeFailureResultCreation(arity) };
        var when = new[] { GenerateMatchErrorSyncCall(arity) };
        var then = new[] { "Assert.Equal(\"matched\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateTaskResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateTaskResultCreation(arity) };
        var when = new[] { GenerateMatchErrorTaskCall(arity) };
        var then = new[] { "Assert.Equal(\"default\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var given = new[] { GenerateTaskErrorTypeFailureResultCreation(arity) };
        var when = new[] { GenerateMatchErrorTaskCall(arity) };
        var then = new[] { "Assert.Equal(\"matched\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateValueTaskResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateValueTaskResultCreation(arity) };
        var when = new[] { GenerateMatchErrorValueTaskCall(arity) };
        var then = new[] { "Assert.Equal(\"default\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var given = new[] { GenerateValueTaskErrorTypeFailureResultCreation(arity) };
        var when = new[] { GenerateMatchErrorValueTaskCall(arity) };
        var then = new[] { "Assert.Equal(\"matched\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateMatchErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var matchResult = result.MatchError<Error, string>(error => \"matched\", () => \"default\");";
        }

        var typeParams = string.Join(", ", new[] { "Error" }.Concat(Enumerable.Range(1, arity)
                                                                              .Select(GetTestType))
                                                            .Append("string"));
        return $"var matchResult = result.MatchError<{typeParams}>(error => \"matched\", () => \"default\");";
    }

    private string GenerateMatchErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var matchResult = await taskResult.MatchErrorAsync<Error, string>(error => Task.FromResult(\"matched\"), () => Task.FromResult(\"default\"));";
        }

        var typeParams = string.Join(", ", new[] { "Error" }.Concat(Enumerable.Range(1, arity)
                                                                              .Select(GetTestType))
                                                            .Append("string"));
        return $"var matchResult = await taskResult.MatchErrorAsync<{typeParams}>(error => Task.FromResult(\"matched\"), () => Task.FromResult(\"default\"));";
    }

    private string GenerateMatchErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var matchResult = await valueTaskResult.MatchErrorAsync<Error, string>(error => ValueTask.FromResult(\"matched\"), () => ValueTask.FromResult(\"default\"));";
        }

        var typeParams = string.Join(", ", new[] { "Error" }.Concat(Enumerable.Range(1, arity)
                                                                              .Select(GetTestType))
                                                            .Append("string"));
        return $"var matchResult = await valueTaskResult.MatchErrorAsync<{typeParams}>(error => ValueTask.FromResult(\"matched\"), () => ValueTask.FromResult(\"default\"));";
    }
}
