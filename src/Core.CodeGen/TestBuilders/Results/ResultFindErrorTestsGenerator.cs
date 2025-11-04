using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.FindError extension methods (sync, Task, ValueTask).
///     Refactored for homogeneity using variant helper.
/// </summary>
internal sealed class ResultFindErrorTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 0;
    private const string ClassName           = "ResultFindErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFindErrorTestsGenerator(string              baseNamespace,
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
        var cw = new ClassWriter($"ResultFindErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync FindError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFindErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task FindError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFindErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask FindError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"FindError_Arity{arity}_Success_ShouldReturnNull",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"FindError_Arity{arity}_Failure_ShouldReturnError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"FindErrorTask_Arity{arity}_Success_ShouldReturnNull",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"FindErrorTask_Arity{arity}_Failure_ShouldReturnError",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"FindErrorValueTask_Arity{arity}_Success_ShouldReturnNull",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"FindErrorValueTask_Arity{arity}_Failure_ShouldReturnError",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues   = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation     = GenerateResultCreation(arity);
        var call         = GenerateFindErrorSyncCall(arity);
        var givenLines   = arity == 0 ? new[] { creation } : new[] { testValues, creation };
        return BuildTestBody(givenLines,
                             [call],
                             ["Assert.Null(foundError);"]);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateErrorTypeFailureResultCreation(arity);
        var call            = GenerateFindErrorSyncCall(arity);
        return BuildTestBody([failureCreation],
                             [call],
                             ["Assert.NotNull(foundError);", "Assert.Equal(\"Test error\", foundError.Message);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues   = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation     = GenerateTaskResultCreation(arity);
        var call         = GenerateFindErrorTaskCall(arity);
        var givenLines   = arity == 0 ? new[] { creation } : new[] { testValues, creation };
        return BuildTestBody(givenLines,
                             [call],
                             ["Assert.Null(foundError);"]);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateFindErrorTaskCall(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.NotNull(foundError);", "Assert.Equal(\"Test error\", foundError.Message);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues   = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation     = GenerateValueTaskResultCreation(arity);
        var call         = GenerateFindErrorValueTaskCall(arity);
        var givenLines   = arity == 0 ? new[] { creation } : new[] { testValues, creation };
        return BuildTestBody(givenLines,
                             [call],
                             ["Assert.Null(foundError);"]);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call     = GenerateFindErrorValueTaskCall(arity);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.NotNull(foundError);", "Assert.Equal(\"Test error\", foundError.Message);"]);
    }

    private string GenerateFindErrorSyncCall(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        if (arity == 0) {
            return "var foundError = result.FindError(e => e is Error);";
        }
        return $"var foundError = result.FindError<{typeParams}>(e => e is Error);";
    }

    private string GenerateFindErrorTaskCall(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        if (arity == 0) {
            return "var foundError = await taskResult.FindErrorAsync(e => Task.FromResult(e is Error));";
        }
        return $"var foundError = await taskResult.FindErrorAsync<{typeParams}>(e => Task.FromResult(e is Error));";
    }

    private string GenerateFindErrorValueTaskCall(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        if (arity == 0) {
            return "var foundError = await valueTaskResult.FindErrorAsync(e => ValueTask.FromResult(e is Error));";
        }
        return $"var foundError = await valueTaskResult.FindErrorAsync<{typeParams}>(e => ValueTask.FromResult(e is Error));";
    }
}
