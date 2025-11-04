using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Flatten extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFlattenTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultFlattenTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultFlattenTestsGenerator(string               baseNamespace,
                                       FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultFlattenSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Flatten" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFlattenTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Flatten" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultFlattenValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Flatten" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Flatten_Arity{arity}_Success_ShouldFlatten",
                                                                      "void",
                                                                      GenerateSyncSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Flatten_Arity{arity}_Failure_ShouldNotFlatten",
                                                                      "void",
                                                                      GenerateSyncFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"FlattenTask_Arity{arity}_Success_ShouldFlatten",
                                                                      "async Task",
                                                                      GenerateTaskSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"FlattenTask_Arity{arity}_Failure_ShouldNotFlatten",
                                                                      "async Task",
                                                                      GenerateTaskFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"FlattenValueTask_Arity{arity}_Success_ShouldFlatten",
                                                                      "async Task",
                                                                      GenerateValueTaskSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"FlattenValueTask_Arity{arity}_Failure_ShouldNotFlatten",
                                                                      "async Task",
                                                                      GenerateValueTaskFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateNestedResultCreation(arity);
        var call       = "var flattenedResult = nestedResult.Flatten();";
        var assertions = GenerateFlattenSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureNestedResultCreation(arity);
        var call     = "var flattenedResult = nestedResult.Flatten();";
        return BuildTestBody([creation], [call], ["Assert.False(flattenedResult.IsSuccess);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskNestedResultCreation(arity);
        var call       = "var flattenedResult = await taskNestedResult.FlattenAsync();";
        var assertions = GenerateFlattenSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureNestedResultCreation(arity);
        var call     = "var flattenedResult = await taskNestedResult.FlattenAsync();";
        return BuildTestBody([creation], [call], ["Assert.False(flattenedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskNestedResultCreation(arity);
        var call       = "var flattenedResult = await valueTaskNestedResult.FlattenAsync();";
        var assertions = GenerateFlattenSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureNestedResultCreation(arity);
        var call     = "var flattenedResult = await valueTaskNestedResult.FlattenAsync();";
        return BuildTestBody([creation], [call], ["Assert.False(flattenedResult.IsSuccess);"]);
    }

    // Helpers reintroduced after refactor
    private string GenerateNestedResultCreation(ushort arity) {
        if (arity == 1) return "var nestedResult = Result.Success(Result.Success(value1));";
        var values = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"));
        return $"var nestedResult = Result.Success(Result.Success({values}));";
    }
    private string GenerateFailureNestedResultCreation(ushort arity) {
        var typeParams = GenerateTypeParams(arity);
        return $"var nestedResult = Result.Success(Result.Failure<{typeParams}>(\"Test error\"));";
    }
    private string GenerateTaskNestedResultCreation(ushort arity) {
        return $"var taskNestedResult = Task.FromResult({GenerateNestedResultCreation(arity).Replace("var nestedResult = ", string.Empty)});";
    }
    private string GenerateTaskFailureNestedResultCreation(ushort arity) {
        return $"var taskNestedResult = Task.FromResult({GenerateFailureNestedResultCreation(arity).Replace("var nestedResult = ", string.Empty)});";
    }
    private string GenerateValueTaskNestedResultCreation(ushort arity) {
        return $"var valueTaskNestedResult = ValueTask.FromResult({GenerateNestedResultCreation(arity).Replace("var nestedResult = ", string.Empty)});";
    }
    private string GenerateValueTaskFailureNestedResultCreation(ushort arity) {
        return $"var valueTaskNestedResult = ValueTask.FromResult({GenerateFailureNestedResultCreation(arity).Replace("var nestedResult = ", string.Empty)});";
    }
    private string GenerateFlattenSuccessAssertions(ushort arity) {
        if (arity == 1) return "Assert.True(flattenedResult.IsSuccess);";
        return "Assert.True(flattenedResult.IsSuccess);"; // multi-arity minimal assertion
    }
}
