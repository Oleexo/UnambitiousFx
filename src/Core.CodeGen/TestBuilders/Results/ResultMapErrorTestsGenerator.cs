using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MapError extension methods (sync) and MapErrorAsync (Task, ValueTask).
/// </summary>
internal sealed class ResultMapErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0;
    private const string ClassName           = "ResultMapErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorTestsGenerator(string               baseNamespace,
                                        FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MapError" };
        
        // Basic tests without policy
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
        
        // Policy tests - Success scenarios
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_Success_ShortCircuit_ShouldNotMapError",
                                      "void",
                                      GenerateSyncSuccessWithPolicyBody(arity, "ShortCircuit"),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_Success_Accumulate_ShouldNotMapError",
                                      "void",
                                      GenerateSyncSuccessWithPolicyBody(arity, "Accumulate"),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        
        // Accumulate policy - single error test
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_Accumulate_SingleError_ShouldMapError",
                                      "void",
                                      GenerateAccumulateSingleErrorBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        
        // Accumulate policy - multiple errors test
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_Accumulate_MultipleErrors_ShouldMapAllErrors",
                                      "void",
                                      GenerateAccumulateMultipleErrorsBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        
        // ShortCircuit policy - single error test
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_ShortCircuit_SingleError_ShouldMapError",
                                      "void",
                                      GenerateShortCircuitSingleErrorBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        
        // ShortCircuit policy - multiple errors test
        cw.AddMethod(new MethodWriter($"MapErrorWithPolicy_Arity{arity}_ShortCircuit_MultipleErrors_ShouldMapAllErrors",
                                      "void",
                                      GenerateShortCircuitMultipleErrorsBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.AddUsing("UnambitiousFx.Core.Results.Types");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultMapError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} MapErrorAsync" };
        cw.AddMethod(new MethodWriter($"MapError{asyncType}_Arity{arity}_Success_ShouldNotMapError",
                                      "async Task",
                                      GenerateAsyncSuccessBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"MapError{asyncType}_Arity{arity}_Failure_ShouldMapError",
                                      "async Task",
                                      GenerateAsyncFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        if (arity == 0) {
            cw.AddMethod(new MethodWriter($"MapErrorAsync{asyncType}Awaitable_Arity{arity}_Failure_ShouldMapErrors",
                                          "async Task",
                                          GenerateAsyncAwaitableFailureBody(arity, asyncType),
                                          attributes: [new FactAttributeReference()],
                                          usings: GetUsings()));
        }

        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var givenLines = new List<string>();
        if (arity > 0) {
            givenLines.Add(GenerateTestValues(arity));
        }
        givenLines.Add(GenerateResultCreation(arity));
        
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")));"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")));";
        
        return BuildTestBody(givenLines, [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")));"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")));";
        
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);

        var call = arity == 0
                       ? $"var mappedResult = await taskResult.MapErrorAsync(errors => {asyncType}.FromResult(errors.Select(e => e.WithMessage(e.Message + \" MAPPED\"))));"
                       : $"var mappedResult = await taskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(errors => {asyncType}.FromResult(errors.Select(e => e.WithMessage(e.Message + \" MAPPED\"))));";
        return arity == 0
                   ? BuildTestBody([creation],                            [call], ["Assert.True(mappedResult.IsSuccess);"])
                   : BuildTestBody([GenerateTestValues(arity), creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = arity == 0
                       ? $"var mappedResult = await taskResult.MapErrorAsync(errors => {asyncType}.FromResult(errors.Select(e => e.WithMessage(e.Message + \" MAPPED\"))));"
                       : $"var mappedResult = await taskResult.MapErrorAsync<{GenerateTypeParams(arity)}>(errors => {asyncType}.FromResult(errors.Select(e => e.WithMessage(e.Message + \" MAPPED\"))));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateAsyncAwaitableFailureBody(ushort arity,
                                                     string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call =
            $"var mappedResult = await taskResult.MapErrorAsync(errors => {asyncType}.FromResult(errors.Select(e => e.WithMessage(e.Message + \" MAPPED\"))));";
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    #region Policy-based tests

    private string GenerateSyncSuccessWithPolicyBody(ushort arity,
                                                     string policy) {
        var givenLines = new List<string>();
        if (arity > 0) {
            givenLines.Add(GenerateTestValues(arity));
        }
        givenLines.Add(GenerateResultCreation(arity));
        
        var call = arity == 0
            ? $"var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.{policy});"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.{policy});";
        
        return BuildTestBody(givenLines, [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureWithPolicyBody(ushort arity,
                                                     string policy) {
        var creation = GenerateFailureResultCreation(arity);
        var call = arity == 0
            ? $"var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.{policy});"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.{policy});";
        
        return BuildTestBody([creation], [call], ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateAccumulateSingleErrorBody(ushort arity) {
        var givenLines = new List<string>
        {
            GenerateFailureResultCreation(arity),
            "// Testing Accumulate policy with single error - preserves original error in Reasons"
        };
        
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.Accumulate);"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.Accumulate);";
        
        var assertions = new List<string>
        {
            "Assert.False(mappedResult.IsSuccess);",
            "// Accumulate preserves original errors + adds mapped errors to Reasons",
            "Assert.Equal(2, mappedResult.Errors.Count()); // Original + Mapped",
            "Assert.Contains(mappedResult.Errors, e => e.Message.Contains(\"MAPPED\"));",
            "Assert.Contains(mappedResult.Errors, e => !e.Message.Contains(\"MAPPED\"));"
        };
        
        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateAccumulateMultipleErrorsBody(ushort arity) {
        var givenLines = new List<string>
        {
            "var error1 = new Error(\"Error 1\");",
            "var error2 = new Error(\"Error 2\");"
        };
        
        if (arity == 0) {
            givenLines.Add("var result = Result.Failure(new[] { error1, error2 });");
        } else {
            var typeParams = GenerateTypeParams(arity);
            givenLines.Add($"var result = Result.Failure<{typeParams}>(new[] {{ error1, error2 }});");
        }
        
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.Accumulate);"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.Accumulate);";
        
        var assertions = new List<string>
        {
            "Assert.False(mappedResult.IsSuccess);",
            "// Accumulate preserves original errors + adds mapped errors",
            "Assert.Equal(4, mappedResult.Errors.Count()); // 2 original + 2 mapped",
            "Assert.Equal(2, mappedResult.Errors.Count(e => e.Message.Contains(\"MAPPED\")));",
            "Assert.Equal(2, mappedResult.Errors.Count(e => !e.Message.Contains(\"MAPPED\")));"
        };
        
        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateShortCircuitSingleErrorBody(ushort arity) {
        var givenLines = new List<string>
        {
            GenerateFailureResultCreation(arity),
            "// Testing ShortCircuit policy with single error"
        };
        
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.ShortCircuit);"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.ShortCircuit);";
        
        var assertions = new List<string>
        {
            "Assert.False(mappedResult.IsSuccess);",
            "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);",
            "Assert.Single(mappedResult.Errors);"
        };
        
        return BuildTestBody(givenLines, [call], assertions);
    }

    private string GenerateShortCircuitMultipleErrorsBody(ushort arity) {
        var givenLines = new List<string>
        {
            "var error1 = new Error(\"Error 1\");",
            "var error2 = new Error(\"Error 2\");"
        };
        
        if (arity == 0) {
            givenLines.Add("var result = Result.Failure(new[] { error1, error2 });");
        } else {
            var typeParams = GenerateTypeParams(arity);
            givenLines.Add($"var result = Result.Failure<{typeParams}>(new[] {{ error1, error2 }});");
        }
        
        var call = arity == 0
            ? "var mappedResult = result.MapError(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.ShortCircuit);"
            : $"var mappedResult = result.MapError<{GenerateTypeParams(arity)}>(errors => errors.Select(e => e.WithMessage(e.Message + \" MAPPED\")), MapErrorChainPolicy.ShortCircuit);";
        
        var assertions = new List<string>
        {
            "Assert.False(mappedResult.IsSuccess);",
            "// All errors should be mapped",
            "Assert.Equal(2, mappedResult.Errors.Count());",
            "Assert.All(mappedResult.Errors, e => Assert.Contains(\"MAPPED\", e.Message));"
        };
        
        return BuildTestBody(givenLines, [call], assertions);
    }

    #endregion
}
