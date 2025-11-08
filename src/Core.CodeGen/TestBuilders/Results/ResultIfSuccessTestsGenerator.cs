using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.IfSuccess direct method.
///     Generates sync tests for all arities.
/// </summary>
internal sealed class ResultIfSuccessTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultIfSuccessTests";
    private const string ExtensionsNamespace = "Results";

    public ResultIfSuccessTestsGenerator(string baseNamespace,
                                         FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var classWriter = new ClassWriter(ClassName, Visibility.Public, ClassModifier.Abstract) { Region = $"Arity {arity}" };
        classWriter.AddMethod(GenerateSyncSuccessTest(arity));
        classWriter.AddMethod(GenerateSyncFailureTest(arity));
        return classWriter;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) =>
        new($"IfSuccess_Arity{arity}_Success_ShouldExecuteCorrectly", "void", GenerateSyncSuccessTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) =>
        new($"IfSuccess_Arity{arity}_Failure_ShouldNotExecute", "void", GenerateSyncFailureTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());

    private string GenerateSyncSuccessTestBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity);
        var methodCall = GenerateIfSuccessCall(arity);
        return BuildTestBody(["// Given: A successful Result with test values", testValues, resultCreation],
                             ["// When: Calling IfSuccess", methodCall],
                             ["// Then: Should execute success handler", "Assert.True(executed);"]);
    }

    private string GenerateSyncFailureTestBody(ushort arity)
    {
        var resultCreation = GenerateFailureResultCreation(arity);
        var methodCall = GenerateIfSuccessCall(arity);
        return BuildTestBody(["// Given: A failed Result", resultCreation],
                             ["// When: Calling IfSuccess", methodCall],
                             ["// Then: Should NOT execute success handler", "Assert.False(executed);"]);
    }

    private string GenerateIfSuccessCall(ushort arity)
    {
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(_ => "_"));
        var lambda = arity == 1 ? "_" : $"({parameters})";
        return $"var executed = false;\nresult.IfSuccess({lambda} => executed = true);";
    }

    private string GenerateFailureResultCreation(ushort arity)
    {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private IEnumerable<string> GetRequiredUsings() => ["System", "Xunit", "UnambitiousFx.Core", "UnambitiousFx.Core.Results"];
}
