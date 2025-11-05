using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Map extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMapTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultMapTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultMapTestsGenerator(string               baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Map" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultMap{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Map" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Map_Arity{arity}_Success_ShouldTransform", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Map_Arity{arity}_Failure_ShouldNotTransform", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Map{asyncType}_Arity{arity}_Success_ShouldTransform", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Map{asyncType}_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateMapSyncCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GenerateMapSyncCall(arity);
        return BuildTestBody([failureCreation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateMapSyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Map(x => x * 2);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = result.Map(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return "Assert.True(transformedResult.IsSuccess);\nAssert.Equal(84, transformedResult.TryGet(out var v) ? v : 0);"; // preserves original semantics
        }

        return "Assert.True(transformedResult.IsSuccess);"; // original multi-arity assertion style minimal
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call       = GenerateMapAsyncCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call     = GenerateMapAsyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateAsyncFailureResultCreation(ushort arity,
                                                      string asyncType) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var core = $"Result.Failure<{typeParams}>(\"Test error\")";
        return $"var taskResult = {asyncType}.FromResult({core});";
    }

    private string GenerateMapAsyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await taskResult.MapAsync(x => x * 2);";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = await taskResult.MapAsync(({parameters}) => ({tupleItems}));";
    }
}
