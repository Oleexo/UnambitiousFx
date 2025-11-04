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

    public ResultMapTestsGenerator(string baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Map" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Map" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMapValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Map" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Map_Arity{arity}_Success_ShouldTransform", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Map_Arity{arity}_Failure_ShouldNotTransform", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"MapTask_Arity{arity}_Success_ShouldTransform", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"MapTask_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"MapValueTask_Arity{arity}_Success_ShouldTransform", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"MapValueTask_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

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

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateMapTaskCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateMapTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateMapValueTaskCall(arity);
        var assertions = GenerateMapSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateMapValueTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateMapSyncCall(ushort arity) {
        if (arity == 1) return "var transformedResult = result.Map(x => x * 2);";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = result.Map(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapTaskCall(ushort arity) {
        if (arity == 1) return "var transformedResult = await taskResult.MapAsync(x => x * 2);";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = await taskResult.MapAsync(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapValueTaskCall(ushort arity) {
        if (arity == 1) return "var transformedResult = await valueTaskResult.MapAsync(x => x * 2);";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_mapped\""));
        return $"var transformedResult = await valueTaskResult.MapAsync(({parameters}) => ({tupleItems}));";
    }

    private string GenerateMapSuccessAssertions(ushort arity) {
        if (arity == 1) return "Assert.True(transformedResult.IsSuccess);\nAssert.Equal(84, transformedResult.TryGet(out var v) ? v : 0);"; // preserves original semantics
        return "Assert.True(transformedResult.IsSuccess);"; // original multi-arity assertion style minimal
    }
}
