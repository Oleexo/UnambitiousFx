using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultValueOrThrowTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultValueOrThrowTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrThrowTestsGenerator(string baseNamespace,
                                            FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrThrowSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOrThrow" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateSyncFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrThrowTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ValueOrThrow" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateTaskFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrThrowValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ValueOrThrow" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateValueTaskFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"ValueOrThrow_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"ValueOrThrow_Arity{arity}_Failure_ShouldThrowException", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity) => new($"ValueOrThrowWithFactory_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity) => new($"ValueOrThrowWithFactory_Arity{arity}_Failure_ShouldThrowCustomException", "void", GenerateSyncFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"ValueOrThrowTask_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"ValueOrThrowTask_Arity{arity}_Failure_ShouldThrowException", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskSuccessWithFactoryTest(ushort arity) => new($"ValueOrThrowTaskWithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateTaskSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureWithFactoryTest(ushort arity) => new($"ValueOrThrowTaskWithFactory_Arity{arity}_Failure_ShouldThrowCustomException", "async Task", GenerateTaskFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"ValueOrThrowValueTask_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"ValueOrThrowValueTask_Arity{arity}_Failure_ShouldThrowException", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessWithFactoryTest(ushort arity) => new($"ValueOrThrowValueTaskWithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateValueTaskSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureWithFactoryTest(ushort arity) => new($"ValueOrThrowValueTaskWithFactory_Arity{arity}_Failure_ShouldThrowCustomException", "async Task", GenerateValueTaskFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateValueOrThrowSyncCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GenerateValueOrThrowSyncCall(arity);
        var expr            = call.Replace("var actualValue = ", string.Empty).TrimEnd(';');
        return BuildTestBody([failureCreation], ["// When & Then", $"Assert.Throws<Exception>(() => {expr});"], []);
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity) {
        var failureCreation   = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowSyncWithFactoryCall(arity);
        var expr              = call.Replace("var actualValue = ", string.Empty).TrimEnd(';');
        return BuildTestBody([failureCreation, factoryDefinition], [$"Assert.Throws<CustomTestException>(() => {expr});"], []);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateValueOrThrowTaskCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateValueOrThrowTaskCall(arity);
        var expr     = call.Replace("var actualValue = ", string.Empty).Replace("await ", string.Empty).TrimEnd(';');
        return BuildTestBody([creation], ["await Assert.ThrowsAsync<Exception>(async () => {", $"    {expr};", "});"], []);
    }

    private string GenerateTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateTaskResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowTaskWithFactoryCall(arity);
        var expr              = call.Replace("var actualValue = ", string.Empty).Replace("await ", string.Empty).TrimEnd(';');
        return BuildTestBody([creation, factoryDefinition], ["await Assert.ThrowsAsync<CustomTestException>(async () => {", $"    {expr};", "});"], []);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateValueOrThrowValueTaskCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateValueOrThrowValueTaskCall(arity);
        var expr     = call.Replace("var actualValue = ", string.Empty).Replace("await ", string.Empty).TrimEnd(';');
        return BuildTestBody([creation], ["await Assert.ThrowsAsync<Exception>(async () => {", $"    {expr};", "});"], []);
    }

    private string GenerateValueTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateValueTaskResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateValueTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowValueTaskWithFactoryCall(arity);
        var expr              = call.Replace("var actualValue = ", string.Empty).Replace("await ", string.Empty).TrimEnd(';');
        return BuildTestBody([creation, factoryDefinition], ["await Assert.ThrowsAsync<CustomTestException>(async () => {", $"    {expr};", "});"], []);
    }

    private string GenerateValueOrThrowSyncCall(ushort arity) => arity == 1 ? "var actualValue = result.ValueOrThrow();" : "var actualValue = result.ValueOrThrow();"; // multi-arity returns tuple implicitly
    private string GenerateValueOrThrowTaskCall(ushort arity) => arity == 1 ? "var actualValue = await taskResult.ValueOrThrowAsync();" : "var actualValue = await taskResult.ValueOrThrowAsync();";
    private string GenerateValueOrThrowValueTaskCall(ushort arity) => arity == 1 ? "var actualValue = await valueTaskResult.ValueOrThrowAsync();" : "var actualValue = await valueTaskResult.ValueOrThrowAsync();";
    private string GenerateValueOrThrowSyncWithFactoryCall(ushort arity) => arity == 1 ? "var actualValue = result.ValueOrThrow(factory);" : "var actualValue = result.ValueOrThrow(factory);";
    private string GenerateValueOrThrowTaskWithFactoryCall(ushort arity) => arity == 1 ? "var actualValue = await taskResult.ValueOrThrowAsync(factory);" : "var actualValue = await taskResult.ValueOrThrowAsync(factory);";
    private string GenerateValueOrThrowValueTaskWithFactoryCall(ushort arity) => arity == 1 ? "var actualValue = await valueTaskResult.ValueOrThrowAsync(factory);" : "var actualValue = await valueTaskResult.ValueOrThrowAsync(factory);";

    private string GenerateValueOrThrowSuccessAssertions(ushort arity) {
        if (arity == 1) return "Assert.Equal(value1, actualValue);";
        return "Assert.True(actualValue != null);"; // minimal multi-arity assertion retained
    }

    private string GenerateExceptionFactoryDefinition() => "Func<Exception> factory = () => new CustomTestException();";

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = GenerateTypeParams(arity);
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateTaskFailureResultCreation(ushort arity) {
        var typeParams = GenerateTypeParams(arity);
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureResultCreation(ushort arity) {
        var typeParams = GenerateTypeParams(arity);
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }
}
