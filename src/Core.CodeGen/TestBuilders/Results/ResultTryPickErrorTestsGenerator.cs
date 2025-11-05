using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TryPickError extension methods (Sync, Task, ValueTask).
/// </summary>
internal sealed class ResultTryPickErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0;
    private const string ClassName           = "ResultTryPickErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultTryPickErrorTestsGenerator(string               baseNamespace,
                                            FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity,
                                ClassName,
                                (x => GenerateSyncTests(x), false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultTryPickErrorTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync TryPickError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureFoundTest(arity));
        cw.AddMethod(GenerateSyncFailureNotFoundTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultTryPickError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} TryPickError" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureFoundTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureNotFoundTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"TryPickError{asyncType}_Arity{arity}_Success_ShouldReturnFalse", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureFoundTest(ushort arity,
                                                       string asyncType) {
        return new MethodWriter($"TryPickError{asyncType}_Arity{arity}_Failure_ShouldReturnFoundError", "async Task", GenerateAsyncFailureFoundBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureNotFoundTest(ushort arity,
                                                          string asyncType) {
        return new MethodWriter($"TryPickError{asyncType}_Arity{arity}_Failure_ShouldReturnNotFound", "async Task", GenerateAsyncFailureNotFoundBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var testValues = arity == 0
                             ? string.Empty
                             : GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call     = GenerateTryPickErrorAsyncCall(arity, asyncType);
        var given = arity == 0
                        ? new[] { creation }
                        : new[] { testValues, creation };
        return BuildTestBody(given,
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateAsyncFailureFoundBody(ushort arity,
                                                 string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call     = GenerateTryPickErrorAsyncCall(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.True(result.Success);", "Assert.NotNull(result.Error);", "Assert.Equal(\"Test error\", result.Error.Message);"]);
    }

    private string GenerateAsyncFailureNotFoundBody(ushort arity,
                                                    string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call     = GenerateTryPickErrorAsyncCallNotFound(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.False(result.Success);", "Assert.Null(result.Error);"]);
    }

    private string GenerateTryPickErrorAsyncCall(ushort arity,
                                                 string asyncType) {
        return arity == 0
                   ? $"var result = await taskResult.TryPickErrorAsync(e => {asyncType}.FromResult(e is ExceptionalError));"
                   : $"var result = await taskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => {asyncType}.FromResult(e is ExceptionalError));";
    }

    private string GenerateTryPickErrorAsyncCallNotFound(ushort arity,
                                                         string asyncType) {
        return arity == 0
                   ? $"var result = await taskResult.TryPickErrorAsync(e => {asyncType}.FromResult(e is Error));"
                   : $"var result = await taskResult.TryPickErrorAsync<{GenerateTypeParams(arity)}>(e => {asyncType}.FromResult(e is Error));";
    }

    // Synchronous test generation methods
    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"TryPickError_Arity{arity}_Success_ShouldReturnFalse", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureFoundTest(ushort arity) {
        return new MethodWriter($"TryPickError_Arity{arity}_Failure_ShouldReturnFoundError", "void", GenerateSyncFailureFoundBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureNotFoundTest(ushort arity) {
        return new MethodWriter($"TryPickError_Arity{arity}_Failure_ShouldReturnNotFound", "void", GenerateSyncFailureNotFoundBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity == 0
                             ? string.Empty
                             : GenerateTestValues(arity);
        var creation = GenerateSyncResultCreation(arity);
        var call     = GenerateTryPickErrorSyncCall(arity);
        var given = arity == 0
                        ? new[] { creation }
                        : new[] { testValues, creation };
        return BuildTestBody(given,
                             [call],
                             ["Assert.False(found);", "Assert.Null(error);"]);
    }

    private string GenerateSyncFailureFoundBody(ushort arity) {
        var creation = GenerateSyncErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorSyncCall(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.True(found);", "Assert.NotNull(error);", "Assert.Equal(\"Test error\", error.Message);"]);
    }

    private string GenerateSyncFailureNotFoundBody(ushort arity) {
        var creation = GenerateSyncErrorTypeFailureResultCreation(arity);
        var call     = GenerateTryPickErrorSyncCallNotFound(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.False(found);", "Assert.Null(error);"]);
    }

    private string GenerateSyncResultCreation(ushort arity) {
        if (arity == 0) {
            return "var result = Result.Success();";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateSyncErrorTypeFailureResultCreation(ushort arity) {
        if (arity == 0) {
            return "var result = Result.Failure(new Error(\"Test error\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var result = Result.Failure<{typeParams}>(new Error(\"Test error\"));";
    }

    private string GenerateTryPickErrorSyncCall(ushort arity) {
        return arity == 0
                   ? "var found = result.TryPickError(e => e is Error, out var error);"
                   : $"var found = result.TryPickError<{GenerateTypeParams(arity)}>(e => e is Error, out var error);";
    }

    private string GenerateTryPickErrorSyncCallNotFound(ushort arity) {
        return arity == 0
                   ? "var found = result.TryPickError(e => e is ExceptionalError, out var error);"
                   : $"var found = result.TryPickError<{GenerateTypeParams(arity)}>(e => e is ExceptionalError, out var error);";
    }
}
