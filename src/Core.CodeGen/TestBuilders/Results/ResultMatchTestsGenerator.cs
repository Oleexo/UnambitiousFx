using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultMatchTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultMatchTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultMatchTestsGenerator(string baseNamespace,
                                     FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (GenerateTaskTests, true), (GenerateValueTaskTests, true));
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Tests" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Tests" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"MatchTask_Arity{arity}_Success_ShouldReturnSuccessValue", "async Task", GenerateTaskSuccessTestBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"MatchTask_Arity{arity}_Failure_ShouldReturnFailureValue", "async Task", GenerateTaskFailureTestBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"MatchValueTask_Arity{arity}_Success_ShouldReturnSuccessValue", "async Task", GenerateValueTaskSuccessTestBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());
    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"MatchValueTask_Arity{arity}_Failure_ShouldReturnFailureValue", "async Task", GenerateValueTaskFailureTestBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateTaskSuccessTestBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateTaskResultCreation(arity);
        var methodCall     = GenerateTaskMatchCall(arity, "success");
        return BuildTestBody(["// Given: A successful Task<Result>", testValues, resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return success value", "Assert.Equal(\"success\", matchResult);"]);
    }

    private string GenerateTaskFailureTestBody(ushort arity) {
        var resultCreation = GenerateTaskFailureResultCreation(arity);
        var methodCall     = GenerateTaskMatchCall(arity, "success");
        return BuildTestBody(["// Given: A failed Task<Result>", resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return failure value", "Assert.Equal(\"failure\", matchResult);"]);
    }

    private string GenerateValueTaskSuccessTestBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateValueTaskResultCreation(arity);
        var methodCall     = GenerateValueTaskMatchCall(arity, "success");
        return BuildTestBody(["// Given: A successful ValueTask<Result>", testValues, resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return success value", "Assert.Equal(\"success\", matchResult);"]);
    }

    private string GenerateValueTaskFailureTestBody(ushort arity) {
        var resultCreation = GenerateValueTaskFailureResultCreation(arity);
        var methodCall     = GenerateValueTaskMatchCall(arity, "success");
        return BuildTestBody(["// Given: A failed ValueTask<Result>", resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return failure value", "Assert.Equal(\"failure\", matchResult);"]);
    }

    private string GenerateTaskMatchCall(ushort arity, string successValue) {
        var successLambda = GenerateSuccessLambda(arity, false, successValue);
        return $"var matchResult = await taskResult.MatchAsync({successLambda}, errors => Task.FromResult(\"failure\"));";
    }

    private string GenerateValueTaskMatchCall(ushort arity, string successValue) {
        var successLambda = GenerateSuccessLambda(arity, true, successValue);
        return $"var matchResult = await valueTaskResult.MatchAsync({successLambda}, errors => ValueTask.FromResult(\"failure\"));";
    }

    private string GenerateSuccessLambda(ushort arity, bool isValueTask, string successValue) {
        var asyncType = isValueTask ? "ValueTask" : "Task";
        if (arity == 1) return $"x => {asyncType}.FromResult(\"{successValue}\")";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        return $"({parameters}) => {asyncType}.FromResult(\"{successValue}\")";
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
