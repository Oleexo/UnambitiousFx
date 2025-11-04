using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultBindTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultBindTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultBindTestsGenerator(string baseNamespace,
                                    FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Bind" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Bind" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Bind" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Bind_Arity{arity}_Success_ShouldTransform", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Bind_Arity{arity}_Failure_ShouldNotTransform", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"BindTask_Arity{arity}_Success_ShouldTransform", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"BindTask_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"BindValueTask_Arity{arity}_Success_ShouldTransform", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"BindValueTask_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateBindSyncCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateBindSyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateBindTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateBindTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateBindValueTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateBindValueTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateBindSyncCall(ushort arity) {
        if (arity == 1) return "var transformedResult = result.Bind(x => Result.Success(x * 2));";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var mapped     = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = result.Bind(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindTaskCall(ushort arity) {
        if (arity == 1) return "var transformedResult = await taskResult.BindAsync(x => Result.Success(x * 2));";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var mapped     = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await taskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindValueTaskCall(ushort arity) {
        if (arity == 1) return "var transformedResult = await valueTaskResult.BindAsync(x => Result.Success(x * 2));";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var mapped     = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await valueTaskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindSuccessAssertions(ushort arity) {
        if (arity == 1) return "Assert.True(transformedResult.IsSuccess);"; // keep simple
        return "Assert.True(transformedResult.IsSuccess);";
    }
}
