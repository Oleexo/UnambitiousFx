using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultTryGetTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultTryGetTests";
    private const string ExtensionsNamespace = "Results";

    public ResultTryGetTestsGenerator(string baseNamespace,
                                      FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) { }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false));

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var classWriter = new ClassWriter(ClassName, Visibility.Public, ClassModifier.Abstract) { Region = $"Arity {arity}" };
        classWriter.AddMethod(GenerateSyncSuccessTest(arity));
        classWriter.AddMethod(GenerateSyncFailureTest(arity));
        return classWriter;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"TryGet_Arity{arity}_Success_ShouldReturnValues", "void", GenerateSyncSuccessTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());
    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"TryGet_Arity{arity}_Failure_ShouldReturnFalse", "void", GenerateSyncFailureTestBody(arity), attributes: [new FactAttributeReference()], usings: GetRequiredUsings());

    private string GenerateSyncSuccessTestBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreationInternal(arity);
        var methodCall = GenerateTryGetCall(arity);
        var assertions = GenerateTryGetSuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(["// Given: A successful Result", testValues, resultCreation],
                             ["// When: Calling TryGet", methodCall],
                             new[] { "// Then: Should retrieve values" }.Concat(assertions));
    }

    private string GenerateSyncFailureTestBody(ushort arity)
    {
        var resultCreation = GenerateFailureResultCreationInternal(arity);
        var methodCall = GenerateTryGetCall(arity);
        return BuildTestBody(["// Given: A failed Result", resultCreation],
                             ["// When: Calling TryGet", methodCall],
                             ["// Then: Should return false", "Assert.False(tryGetResult);"]);
    }

    private string GenerateTryGetCall(ushort arity)
    {
        var outParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"out var outValue{i}"));
        return $"var tryGetResult = result.TryGet({outParams});";
    }

    private string GenerateTryGetSuccessAssertions(ushort arity)
    {
        var lines = new List<string> { "Assert.True(tryGetResult);" };
        for (var i = 1; i <= arity; i++)
        {
            var expected = GetTestValue(i);
            lines.Add(expected is "true" or "false" ? $"Assert.True(outValue{i});" : $"Assert.Equal({expected}, outValue{i});");
        }
        return string.Join('\n', lines);
    }

    private string GenerateResultCreationInternal(ushort arity)
    {
        if (arity == 1) return "var result = Result.Success(value1);";
        var values = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateFailureResultCreationInternal(ushort arity)
    {
        var typeParams = GenerateTypeParams(arity);
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private IEnumerable<string> GetRequiredUsings() => ["System", "Xunit", "UnambitiousFx.Core", "UnambitiousFx.Core.Results"]; // original list
}
