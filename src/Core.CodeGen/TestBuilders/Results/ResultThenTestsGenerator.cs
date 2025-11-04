using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Then extension methods (sync only).
/// </summary>
internal sealed class ResultThenTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultThenTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultThenTestsGenerator(string baseNamespace,
                                    FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
        => GenerateVariants(arity, ClassName, (GenerateSyncTests, false));

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultThenSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Then" };
        cw.AddMethod(new MethodWriter($"Then_Arity{arity}_Success_ShouldTransform",
                                      "void",
                                      GenerateSyncSuccessTestBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Then_Arity{arity}_Failure_ShouldReturnOriginal",
                                      "void",
                                      GenerateSyncFailureTestBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private string GenerateSyncSuccessTestBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var transformedValues = GenerateTransformedValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateThenCallSuccess(arity);
        var assertions = GenerateSuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, transformedValues, creation], [call], assertions);
    }

    private string GenerateSyncFailureTestBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateThenCallFailure(arity);
        var assertions = new[] { "Assert.False(actualResult.IsSuccess);" };
        return BuildTestBody([creation], [call], assertions);
    }

    // Helper methods restored to satisfy body generation
    private string GenerateTransformedValues(ushort arity) {
        return string.Join('\n', Enumerable.Range(1, arity).Select(i => $"var transformed{i} = {GetTransformedValue(i)};"));
    }
    private string GenerateThenCallSuccess(ushort arity) {
        if (arity == 1) return "var actualResult = result.Then(v1 => Result.Success(transformed1));";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"v{i}"));
        var transformedArgs = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"transformed{i}"));
        return $"var actualResult = result.Then(({parameters}) => Result.Success({transformedArgs}));";
    }
    private string GenerateThenCallFailure(ushort arity) {
        if (arity == 1) return "var actualResult = result.Then(v1 => Result.Success(100));";
        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"v{i}"));
        var dummyValues = string.Join(", ", Enumerable.Range(1, arity).Select(GetTransformedValue));
        return $"var actualResult = result.Then(({parameters}) => Result.Success({dummyValues}));";
    }
    private string GenerateSuccessAssertions(ushort arity) {
        var lines = new List<string> { "Assert.True(actualResult.IsSuccess);" };
        if (arity == 1) {
            lines.Add("Assert.True(actualResult.TryGet(out var actual));");
            lines.Add("Assert.Equal(transformed1, actual);");
        } else {
            var outParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"out var actual{i}"));
            lines.Add($"Assert.True(actualResult.TryGet({outParams}));");
            for (var i = 1; i <= arity; i++) lines.Add($"Assert.Equal(transformed{i}, actual{i});");
        }
        return string.Join('\n', lines);
    }
    private string GetTransformedValue(int index) => index switch {
        1 => "100",
        2 => "\"World\"",
        3 => "6.28",
        4 => "false",
        5 => "'B'",
        6 => "456L",
        7 => "15.0f",
        8 => "198m",
        _ => $"{index * 20}"
    };
}
