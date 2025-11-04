using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.IfFailure direct method.
///     Generates sync tests for all arities.
/// </summary>
internal sealed class ResultIfFailureTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultIfFailureTests";
    private const string ExtensionsNamespace = "Results";

    public ResultIfFailureTestsGenerator(string baseNamespace,
                                         FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var classWriter = new ClassWriter(ClassName, Visibility.Public, ClassModifier.Abstract) { Region = $"Arity {arity}" };
        classWriter.AddMethod(GenerateSyncSuccessTest(arity));
        classWriter.AddMethod(GenerateSyncFailureTest(arity));
        return classWriter;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"IfFailure_Arity{arity}_Success_ShouldNotExecute", "void", GenerateSyncSuccessTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"IfFailure_Arity{arity}_Failure_ShouldExecute", "void", GenerateSyncFailureTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());

    private string GenerateSyncSuccessTestBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity);
        var methodCall     = GenerateIfFailureCall();
        return BuildTestBody(["// Given: A successful Result", testValues, resultCreation],
                             ["// When: Calling IfFailure", methodCall],
                             ["// Then: Should NOT execute failure handler", "Assert.False(executed);"]);
    }

    private string GenerateSyncFailureTestBody(ushort arity) {
        var resultCreation = GenerateFailureResultCreation(arity);
        var methodCall     = GenerateIfFailureCall();
        return BuildTestBody(["// Given: A failed Result", resultCreation],
                             ["// When: Calling IfFailure", methodCall],
                             ["// Then: Should execute failure handler", "Assert.True(executed);"]);
    }

    private string GenerateIfFailureCall() => "var executed = false;\nresult.IfFailure(_ => executed = true);";

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private IEnumerable<string> GetRequiredUsings() => ["System", "Xunit", "UnambitiousFx.Core", "UnambitiousFx.Core.Results"];
}
