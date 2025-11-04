using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.AppendError extension methods (sync, Task, ValueTask, direct async).
///     Refactored for homogeneity using shared variant generation helper.
/// </summary>
internal sealed class ResultAppendErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0; // Includes non-generic Result
    private const string ClassName           = "ResultAppendErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultAppendErrorTestsGenerator(string              baseNamespace,
                                           FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity,
                                ClassName,
                                (GenerateSyncTests,      false),
                                (GenerateAsyncTests,     true),
                                (GenerateTaskTests,      true),
                                (GenerateValueTaskTests, true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultAppendErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync AppendError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultAppendErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task AppendError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultAppendErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask AppendError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultAppendErrorAsyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Async AppendErrorAsync on Result" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity));
        cw.AddMethod(GenerateAsyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"AppendError_Arity{arity}_Success_ShouldNotAppendError",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"AppendError_Arity{arity}_Failure_ShouldAppendError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"AppendErrorTask_Arity{arity}_Success_ShouldNotAppendError",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"AppendErrorTask_Arity{arity}_Failure_ShouldAppendError",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"AppendErrorValueTask_Arity{arity}_Success_ShouldNotAppendError",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"AppendErrorValueTask_Arity{arity}_Failure_ShouldAppendError",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity) {
        return new MethodWriter($"AppendErrorAsync_Arity{arity}_Success_ShouldNotAppendError",
                                "async Task",
                                GenerateAsyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity) {
        return new MethodWriter($"AppendErrorAsync_Arity{arity}_Failure_ShouldAppendError",
                                "async Task",
                                GenerateAsyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateAppendErrorSyncCall(arity);
        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GenerateAppendErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateAppendErrorTaskCall(arity);
        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateAppendErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateAppendErrorValueTaskCall(arity);
        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateAppendErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateAsyncSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateAppendErrorAsyncCall(arity);
        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateAsyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateAppendErrorAsyncCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateAppendErrorSyncCall(ushort arity) {
        if (arity == 0) {
            return "var appendedResult = result.AppendError(\"Appended error\");";
        }
        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = result.AppendError<{typeParams}>(\"Appended error\");";
    }

    private string GenerateAppendErrorTaskCall(ushort arity) {
        if (arity == 0) {
            return "var appendedResult = await Task.FromResult(result).AppendErrorAsync(\"Appended error\");";
        }
        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = await Task.FromResult(result).AppendErrorAsync<{typeParams}>(\"Appended error\");";
    }

    private string GenerateAppendErrorValueTaskCall(ushort arity) {
        if (arity == 0) {
            return "var appendedResult = await ValueTask.FromResult(result).AppendErrorAsync(\"Appended error\");";
        }
        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = await ValueTask.FromResult(result).AppendErrorAsync<{typeParams}>(\"Appended error\");";
    }

    private string GenerateAppendErrorAsyncCall(ushort arity) {
        if (arity == 0) {
            return "var appendedResult = await UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks.ResultExtensions.AppendErrorAsync(result, \"Appended error\");";
        }
        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = await UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks.ResultExtensions.AppendErrorAsync<{typeParams}>(result, \"Appended error\");";
    }
}
