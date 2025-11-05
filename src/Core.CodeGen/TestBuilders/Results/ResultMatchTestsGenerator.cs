using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultMatchTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultMatchTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultMatchTestsGenerator(string               baseNamespace,
                                     FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultMatch{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Tests" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ValueAccess.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Match{asyncType}_Arity{arity}_Success_ShouldReturnSuccessValue",
                                "async Task",
                                GenerateAsyncSuccessTestBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Match{asyncType}_Arity{arity}_Failure_ShouldReturnFailureValue",
                                "async Task",
                                GenerateAsyncFailureTestBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateAsyncSuccessTestBody(ushort arity,
                                                string asyncType) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var methodCall     = GenerateAsyncMatchCall(arity, asyncType, "success");
        return BuildTestBody([$"// Given: A successful {asyncType}<Result>", testValues, resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return success value", "Assert.Equal(\"success\", matchResult);"]);
    }

    private string GenerateAsyncFailureTestBody(ushort arity,
                                                string asyncType) {
        var resultCreation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var methodCall     = GenerateAsyncMatchCall(arity, asyncType, "success");
        return BuildTestBody([$"// Given: A failed {asyncType}<Result>", resultCreation],
                             ["// When: Calling MatchAsync", methodCall],
                             ["// Then: Should return failure value", "Assert.Equal(\"failure\", matchResult);"]);
    }

    private string GenerateAsyncMatchCall(ushort arity,
                                          string asyncType,
                                          string successValue) {
        var successLambda = GenerateSuccessLambda(arity, asyncType, successValue);
        return $"var matchResult = await taskResult.MatchAsync({successLambda}, errors => {asyncType}.FromResult(\"failure\"));";
    }

    private string GenerateSuccessLambda(ushort arity,
                                         string asyncType,
                                         string successValue) {
        if (arity == 1) {
            return $"x => {asyncType}.FromResult(\"{successValue}\")";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"({parameters}) => {asyncType}.FromResult(\"{successValue}\")";
    }
}
