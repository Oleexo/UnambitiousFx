using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MatchError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMatchErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0; // Changed to 0 to support non-generic Result
    private const string ClassName           = "ResultMatchErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMatchErrorTestsGenerator(string               baseNamespace,
                                          FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MatchError" };
        cw.AddMethod(new MethodWriter($"MatchError_Arity{arity}_Success_ShouldReturnDefault", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MatchError_Arity{arity}_Failure_ShouldMatchError", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultMatchError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} MatchError" };
        cw.AddMethod(new MethodWriter($"MatchError{asyncType}_Arity{arity}_Success_ShouldReturnDefault", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MatchError{asyncType}_Arity{arity}_Failure_ShouldMatchError", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var given = arity == 0
                        ? new[] { GenerateResultCreation(arity) }
                        : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateMatchErrorSyncCall(arity) };
        var then = new[] { "Assert.Equal(\"default\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var given = new[] { GenerateErrorTypeFailureResultCreation(arity) };
        var when  = new[] { GenerateMatchErrorSyncCall(arity) };
        var then  = new[] { "Assert.Equal(\"matched\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var given = arity == 0
                        ? new[] { GenerateAsyncSuccessResultCreation(arity,                            asyncType) }
                        : new[] { GenerateTestValues(arity), GenerateAsyncSuccessResultCreation(arity, asyncType) };
        var when = new[] { GenerateMatchErrorAsyncCall(arity, asyncType) };
        var then = new[] { "Assert.Equal(\"default\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var given = new[] { GenerateAsyncFailureResultCreation(arity, asyncType) };
        var when  = new[] { GenerateMatchErrorAsyncCall(arity, asyncType) };
        var then  = new[] { "Assert.Equal(\"matched\", matchResult);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateMatchErrorAsyncCall(ushort arity,
                                               string asyncType) {
        if (arity == 0) {
            return $"var matchResult = await taskResult.MatchErrorAsync<ExceptionalError, string>(error => {asyncType}.FromResult(\"matched\"), () => {asyncType}.FromResult(\"default\"));";
        }

        var typeParams = string.Join(", ", new[] { "ExceptionalError" }.Concat(Enumerable.Range(1, arity)
                                                                              .Select(GetTestType))
                                                            .Append("string"));
        return $"var matchResult = await taskResult.MatchErrorAsync<{typeParams}>(error => {asyncType}.FromResult(\"matched\"), () => {asyncType}.FromResult(\"default\"));";
    }

    private string GenerateMatchErrorSyncCall(ushort arity) {
        if (arity == 0) {
            return "var matchResult = result.MatchError<Error, string>(error => \"matched\", () => \"default\");";
        }

        var typeParams = string.Join(", ", new[] { "Error" }.Concat(Enumerable.Range(1, arity)
                                                                              .Select(GetTestType))
                                                            .Append("string"));
        return $"var matchResult = result.MatchError<{typeParams}>(error => \"matched\", () => \"default\");";
    }
}
