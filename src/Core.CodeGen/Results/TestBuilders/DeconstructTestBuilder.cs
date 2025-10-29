using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.TestBuilders;

/// <summary>
/// Builds tests for Result.Deconstruct() operations.
/// </summary>
internal static class DeconstructTestBuilder {
    public static MethodWriter BuildDeconstructSuccessTest(ushort arity) {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);
        var outParams   = GenerateDeconstructOutParams(arity);
        var assertions  = GenerateDeconstructSuccessAssertions(arity);

        var body = $"""
                    var r = {successCall};

                    r.Deconstruct({outParams});
                    {assertions}
                    """;

        return new MethodWriter(
            name: "Success_Deconstruct_ReturnsValue",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildDeconstructFailureTest(ushort arity) {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);
        var outParams   = GenerateDeconstructOutParams(arity);
        var assertions  = GenerateDeconstructFailureAssertions(arity);

        var body = $"""
                    var r = {failureCall};

                    r.Deconstruct({outParams});
                    {assertions}
                    """;

        return new MethodWriter(
            name: "Failure_Deconstruct_ReturnsErrorMessage",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    private static string GenerateDeconstructOutParams(ushort arity) {
        if (arity == 1) {
            return "out var ok, out var value, out var errors";
        }

        return "out var ok, out var values, out var errors";
    }

    private static string GenerateDeconstructSuccessAssertions(ushort arity) {
        var result = "Assert.True(ok);\n";

        if (arity == 1) {
            result += ResultTestHelpers.GenerateValueAssertion(1, "value") + "\n";
        }
        else {
            result += "Assert.NotNull(values);\n";
            for (int i = 1; i <= arity; i++) {
                result += ResultTestHelpers.GenerateValueAssertion(i, $"values.Value.Item{i}") + "\n";
            }
        }

        result += "Assert.Null(errors);";
        return result;
    }

    private static string GenerateDeconstructFailureAssertions(ushort arity) {
        var result = "Assert.False(ok);\n";

        if (arity == 1) {
            var defaultValue = ResultTestHelpers.GetDefaultValue(1);
            result += $"Assert.Equal({defaultValue}, value);\n";
        }
        else {
            result += "Assert.Null(values);\n";
        }

        result += "Assert.NotNull(errors);\n";
        result += "Assert.Single(errors);\n";
        result += "Assert.Equal(\"boom\", errors.First().Message);";
        return result;
    }
}
