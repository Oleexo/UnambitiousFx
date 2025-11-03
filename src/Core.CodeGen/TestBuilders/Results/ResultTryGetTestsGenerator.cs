using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TryGet direct method.
///     Generates sync tests for all arities.
/// </summary>
internal sealed class ResultTryGetTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultTryGetTests";
    private const string ExtensionsNamespace = "Results";

    public ResultTryGetTestsGenerator(string               baseNamespace,
                                      FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var result    = new List<ClassWriter>();
        var syncClass = GenerateSyncTests(arity);
        if (syncClass != null) {
            result.Add(syncClass);
        }

        return result;
    }

    private ClassWriter? GenerateSyncTests(ushort arity) {
        var classWriter = new ClassWriter(ClassName,
                                          Visibility.Public,
                                          ClassModifier.Abstract) {
            Region = $"Arity {arity}"
        };
        classWriter.AddMethod(GenerateSyncSuccessTest(arity));
        classWriter.AddMethod(GenerateSyncFailureTest(arity));
        return classWriter;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        var body = GenerateSyncSuccessTestBody(arity);
        return new MethodWriter($"TryGet_Arity{arity}_Success_ShouldReturnValues",
                                "void",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        var body = GenerateSyncFailureTestBody(arity);
        return new MethodWriter($"TryGet_Arity{arity}_Failure_ShouldReturnFalse",
                                "void",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private string GenerateSyncSuccessTestBody(ushort arity) {
        var testValues     = GenerateTestValues(arity);
        var resultCreation = GenerateResultCreation(arity);
        var methodCall     = GenerateTryGetCall(arity);
        var assertions     = GenerateTryGetSuccessAssertions(arity);
        return $"""
                // Given: A successful Result
                {testValues}
                {resultCreation}

                // When: Calling TryGet
                {methodCall}

                // Then: Should retrieve values
                {assertions}
                """;
    }

    private string GenerateSyncFailureTestBody(ushort arity) {
        var          resultCreation = GenerateFailureResultCreation(arity);
        var          methodCall     = GenerateTryGetCall(arity);
        const string assertions     = "Assert.False(tryGetResult);";
        return $"""
                // Given: A failed Result
                {resultCreation}

                // When: Calling TryGet
                {methodCall}

                // Then: Should return false
                {assertions}
                """;
    }

    private string GenerateTryGetCall(ushort arity) {
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var outValue{i}"));
        return $"var tryGetResult = result.TryGet({outParams});";
    }

    private string GenerateTryGetSuccessAssertions(ushort arity) {
        var lines = new List<string> { "Assert.True(tryGetResult);" };
        for (var i = 1; i <= arity; i++) {
            var expected = GetTestValue(i);
            if (expected is "true" or "false") {
                lines.Add($"Assert.True(outValue{i});");
            }
            else {
                lines.Add($"Assert.Equal({expected}, outValue{i});");
            }
        }

        return string.Join("\n", lines);
    }

    private string GenerateResultCreation(ushort arity) {
        if (arity == 1) {
            return "var result = Result.Success(value1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    private string GetTestValue(int index) {
        return index switch {
            1 => "42",
            2 => "\"test\"",
            3 => "true",
            4 => "3.14",
            5 => "123L",
            _ => $"\"value{index}\""
        };
    }

    private string GetTestType(int index) {
        return index switch {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "long",
            _ => "string"
        };
    }

    private IEnumerable<string> GetRequiredUsings() {
        return [
            "System",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results"
        ];
    }
}
