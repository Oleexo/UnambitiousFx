using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.HasException extension methods (sync, Task, ValueTask).
///     Refactored to use common variant generation helper for homogeneity.
/// </summary>
internal sealed class ResultHasExceptionTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0; // Includes non-generic Result
    private const string ClassName           = "ResultHasExceptionTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultHasExceptionTestsGenerator(string              baseNamespace,
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
                                (GenerateTaskTests,      true),
                                (GenerateValueTaskTests, true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasExceptionSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync HasException" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasExceptionTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task HasException" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultHasExceptionValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask HasException" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"HasException_Arity{arity}_Success_ShouldReturnFalse",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"HasException_Arity{arity}_Failure_ShouldReturnTrue",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"HasExceptionTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"HasExceptionTask_Arity{arity}_Failure_ShouldReturnTrue",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"HasExceptionValueTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"HasExceptionValueTask_Arity{arity}_Failure_ShouldReturnTrue",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = arity > 0 ? GenerateResultCreation(arity) : "var result = Result.Success();";
        var call       = GenerateHasExceptionSyncCall(arity);

        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.False(hasException);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasException);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateExceptionalFailureResultCreation(arity);
        var call            = GenerateHasExceptionSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.True(hasException);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateHasExceptionTaskCall(arity);

        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.False(hasException);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasException);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var failureCreation = GenerateTaskExceptionalFailureResultCreation(arity);
        var call            = GenerateHasExceptionTaskCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.True(hasException);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateHasExceptionValueTaskCall(arity);

        if (arity == 0) {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.False(hasException);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.False(hasException);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var failureCreation = GenerateValueTaskExceptionalFailureResultCreation(arity);
        var call            = GenerateHasExceptionValueTaskCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.True(hasException);
                """;
    }

    private string GenerateHasExceptionSyncCall(ushort arity) {
        if (arity == 0) {
            return "var hasException = result.HasException();";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var hasException = result.HasException<{typeParams}>();";
    }

    private string GenerateHasExceptionTaskCall(ushort arity) {
        if (arity == 0) {
            return "var hasException = await Task.FromResult(result).HasExceptionAsync();";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var hasException = await Task.FromResult(result).HasExceptionAsync<{typeParams}>();";
    }

    private string GenerateHasExceptionValueTaskCall(ushort arity) {
        if (arity == 0) {
            return "var hasException = await ValueTask.FromResult(result).HasExceptionAsync();";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var hasException = await ValueTask.FromResult(result).HasExceptionAsync<{typeParams}>();";
    }
}
