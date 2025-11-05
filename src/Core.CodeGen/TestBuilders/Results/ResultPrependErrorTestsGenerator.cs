using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.PrependError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultPrependErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0; // Changed to 0 to support non-generic Result
    private const string ClassName           = "ResultPrependErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultPrependErrorTestsGenerator(string               baseNamespace,
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
                                (GenerateSyncTests, false),
                                (x => GenerateAwaitableAsyncTests(x, "Task"), true),
                                (x => GenerateAwaitableAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultPrependErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync PrependError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAwaitableAsyncTests(ushort arity,
                                                    string asyncType) {
        var cw = new ClassWriter($"ResultPrependError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} PrependError" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAwaitableAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAwaitableAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"PrependError_Arity{arity}_Success_ShouldNotPrependError", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"PrependError_Arity{arity}_Failure_ShouldPrependError", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAwaitableAsyncSuccessTest(ushort arity,
                                                           string asyncType) {
        return new MethodWriter($"PrependError{asyncType}_Arity{arity}_Success_ShouldNotPrependError", "async Task",
                                GenerateAwaitableAsyncSuccessBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAwaitableAsyncFailureTest(ushort arity,
                                                           string asyncType) {
        return new MethodWriter($"PrependError{asyncType}_Arity{arity}_Failure_ShouldPrependError", "async Task",
                                GenerateAwaitableAsyncFailureBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"PrependErrorAsync_Arity{arity}_Success_ShouldNotPrependError", "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType), attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"PrependErrorAsync_Arity{arity}_Failure_ShouldPrependError", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call     = GeneratePrependErrorSyncCall(arity);
        var given = arity == 0
                        ? new[] { creation }
                        : new[] { testValues, creation };
        return BuildTestBody(given, [call], ["Assert.True(prependedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GeneratePrependErrorSyncCall(arity);
        return BuildTestBody([failureCreation],
                             [call],
                             [
                                 "Assert.False(prependedResult.IsSuccess);",
                                 "Assert.Single(prependedResult.Errors);",
                                 "Assert.Contains(\"Prepended error\", prependedResult.Errors.First().Message);",
                                 "Assert.EndsWith(\"Test error\", prependedResult.Errors.First().Message);"
                             ]);
    }

    private string GenerateAwaitableAsyncSuccessBody(ushort arity,
                                                     string asyncType) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call     = GeneratePrependErrorAwaitableAsyncCall(arity, asyncType);
        var given = arity == 0
                        ? new[] { creation }
                        : new[] { testValues, creation };
        return BuildTestBody(given, [call], ["Assert.True(prependedResult.IsSuccess);"]);
    }

    private string GenerateAwaitableAsyncFailureBody(ushort arity,
                                                     string asyncType) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GeneratePrependErrorAwaitableAsyncCall(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             [
                                 "Assert.False(prependedResult.IsSuccess);",
                                 "Assert.Single(prependedResult.Errors);",
                                 "Assert.Contains(\"Prepended error\", prependedResult.Errors.First().Message);",
                                 "Assert.EndsWith(\"Test error\", prependedResult.Errors.First().Message);"
                             ]);
    }

    private string GeneratePrependErrorAwaitableAsyncCall(ushort arity,
                                                          string asyncType) {
        if (arity == 0) {
            return $"var prependedResult = await {asyncType}.FromResult(result).PrependErrorAsync(\"Prepended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var prependedResult = await {asyncType}.FromResult(result).PrependErrorAsync<{typeParams}>(\"Prepended error\");";
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call     = GeneratePrependErrorDirectAsyncCall(arity, asyncType);
        var given = arity == 0
                        ? new[] { creation }
                        : new[] { testValues, creation };
        return BuildTestBody(given, [call], ["Assert.True(prependedResult.IsSuccess);"]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GeneratePrependErrorDirectAsyncCall(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             [
                                 "Assert.False(prependedResult.IsSuccess);",
                                 "Assert.Single(prependedResult.Errors);",
                                 "Assert.Contains(\"Prepended error\", prependedResult.Errors.First().Message);",
                                 "Assert.EndsWith(\"Test error\", prependedResult.Errors.First().Message);"
                             ]);
    }

    private string GeneratePrependErrorSyncCall(ushort arity) {
        return arity == 0
                   ? "var prependedResult = result.PrependError(\"Prepended error\");"
                   : $"var prependedResult = result.PrependError<{GenerateTypeParams(arity)}>(\"Prepended error\");";
    }

    private string GeneratePrependErrorDirectAsyncCall(ushort arity,
                                                       string asyncType) {
        return arity == 0
                   ? "var prependedResult = await result.PrependErrorAsync(\"Prepended error\");"
                   : $"var prependedResult = await result.PrependErrorAsync<{GenerateTypeParams(arity)}>(\"Prepended error\");";
    }
}
