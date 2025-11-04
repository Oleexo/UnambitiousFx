using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.HasError extension methods (sync, Task, ValueTask).
///     Refactored to use common variant generation helper for homogeneity.
/// </summary>
internal sealed class ResultHasErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0; // Supports non-generic Result
    private const string ClassName           = "ResultHasErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultHasErrorTestsGenerator(string              baseNamespace,
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
                                (GenerateSyncTests,       false),
                                (GenerateTaskTests,       true),
                                (GenerateValueTaskTests,  true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync HasError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task HasError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask HasError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"HasError_Arity{arity}_Success_ShouldReturnFalse",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"HasError_Arity{arity}_Failure_ShouldReturnTrue",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"HasErrorTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"HasErrorTask_Arity{arity}_Failure_ShouldReturnTrue",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"HasErrorValueTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"HasErrorValueTask_Arity{arity}_Failure_ShouldReturnTrue",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateHasErrorSyncCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasError);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateErrorTypeFailureResultCreation(arity);
        var call            = GenerateHasErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.True(hasError);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateHasErrorTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasError);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateHasErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.True(hasError);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateHasErrorValueTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasError);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateHasErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.True(hasError);
                """;
    }

    private string GenerateHasErrorSyncCall(ushort arity) {
        if (arity == 0) {
            return "var hasError = result.HasError<Error>();";
        }

        var typeParams = GenerateTypeParamsWithPrefix(arity, "Error");
        return $"var hasError = result.HasError<{typeParams}>();";
    }

    private string GenerateHasErrorTaskCall(ushort arity) {
        if (arity == 0) {
            return "var hasError = await taskResult.HasErrorAsync<Error>();";
        }

        var typeParams = GenerateTypeParamsWithPrefix(arity, "Error");
        return $"var hasError = await taskResult.HasErrorAsync<{typeParams}>();";
    }

    private string GenerateHasErrorValueTaskCall(ushort arity) {
        if (arity == 0) {
            return "var hasError = await valueTaskResult.HasErrorAsync<Error>();";
        }

        var typeParams = GenerateTypeParamsWithPrefix(arity, "Error");
        return $"var hasError = await valueTaskResult.HasErrorAsync<{typeParams}>();";
    }
}
