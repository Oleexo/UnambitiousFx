using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultValueOrTestsGenerator : ResultTestGeneratorBase { // base changed
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultValueOrTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrTestsGenerator(string baseNamespace,
                                       FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOr" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateSyncFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ValueOr" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateTaskFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ValueOr" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateValueTaskFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"ValueOr_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"ValueOr_Arity{arity}_Failure_ShouldReturnFallback", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity) => new($"ValueOrWithFactory_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity) => new($"ValueOrWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue", "void", GenerateSyncFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"ValueOrTask_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"ValueOrTask_Arity{arity}_Failure_ShouldReturnFallback", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessWithFactoryTest(ushort arity) => new($"ValueOrTaskWithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateTaskSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureWithFactoryTest(ushort arity) => new($"ValueOrTaskWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue", "async Task", GenerateTaskFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"ValueOrValueTask_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"ValueOrValueTask_Arity{arity}_Failure_ShouldReturnFallback", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessWithFactoryTest(ushort arity) => new($"ValueOrValueTaskWithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateValueTaskSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureWithFactoryTest(ushort arity) => new($"ValueOrValueTaskWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue", "async Task", GenerateValueTaskFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrSyncCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var fallbackValues  = GenerateFallbackValues(arity);
        var call            = GenerateValueOrSyncCall(arity);
        var assertions      = GenerateValueOrFailureAssertions(arity);
        return BuildTestBody([failureCreation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity) {
        var failureCreation   = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return BuildTestBody([failureCreation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateTaskResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrTaskCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation       = GenerateTaskFailureResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrTaskCall(arity);
        var assertions     = GenerateValueOrFailureAssertions(arity);
        return BuildTestBody([creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateTaskResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return BuildTestBody([creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var creation       = GenerateValueTaskResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrValueTaskCall(arity);
        var assertions     = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation       = GenerateValueTaskFailureResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call           = GenerateValueOrValueTaskCall(arity);
        var assertions     = GenerateValueOrFailureAssertions(arity);
        return BuildTestBody([creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateValueTaskResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateValueTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call              = GenerateValueOrValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrFactoryFailureAssertions(arity);
        return BuildTestBody([creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueOrSyncCall(ushort arity) => arity == 1 ? "var actualValue = result.ValueOr(fallback1);" : $"var actualValue = result.ValueOr({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))});";
    private string GenerateValueOrSyncWithFactoryCall(ushort arity) => "var actualValue = result.ValueOr(factory);";
    private string GenerateValueOrTaskCall(ushort arity) => arity == 1 ? "var actualValue = await taskResult.ValueOrAsync(fallback1);" : $"var actualValue = await taskResult.ValueOrAsync({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))});";
    private string GenerateValueOrTaskWithFactoryCall(ushort arity) => "var actualValue = await taskResult.ValueOrAsync(factory);";
    private string GenerateValueOrValueTaskCall(ushort arity) => arity == 1 ? "var actualValue = await valueTaskResult.ValueOrAsync(fallback1);" : $"var actualValue = await valueTaskResult.ValueOrAsync({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))});";
    private string GenerateValueOrValueTaskWithFactoryCall(ushort arity) => "var actualValue = await valueTaskResult.ValueOrAsync(factory);";

    private string GenerateValueOrSuccessAssertions(ushort arity) => arity == 1 ? "Assert.Equal(value1, actualValue);" : string.Join('\n', Enumerable.Range(1, arity).Select(i => $"Assert.Equal(value{i}, actualValue.Item{i});"));
    private string GenerateValueOrFailureAssertions(ushort arity) => arity == 1 ? "Assert.Equal(fallback1, actualValue);" : string.Join('\n', Enumerable.Range(1, arity).Select(i => $"Assert.Equal(fallback{i}, actualValue.Item{i});"));
    private string GenerateValueOrFactoryFailureAssertions(ushort arity) => GenerateValueOrFailureAssertions(arity);

    private string GenerateFallbackValues(ushort arity) => string.Join('\n', Enumerable.Range(1, arity).Select(i => $"var fallback{i} = {GetFallbackValue(i)};"));

    private string GenerateFactoryDefinition(ushort arity) {
        if (arity == 1) return "var fallback1 = 999;\nFunc<int> factory = () => fallback1;";
        var fallbackDecls = string.Join('\n', Enumerable.Range(1, arity).Select(i => $"var fallback{i} = {GetFallbackValue(i)};"));
        var tupleType     = $"({string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType))})";
        var tupleValues   = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))})";
        return $"{fallbackDecls}\nFunc<{tupleType}> factory = () => {tupleValues};";
    }

    private string GetFallbackValue(int index) => index switch { 1 => "999", 2 => "\"fallback\"", 3 => "false", 4 => "9.99", 5 => "999L", _ => $"\"fallback{index}\"" };
}
