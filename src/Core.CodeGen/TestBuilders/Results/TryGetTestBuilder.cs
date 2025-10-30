using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
/// Builds tests for Result.TryGet() operations.
/// </summary>
internal static class TryGetTestBuilder
{
    public static MethodWriter BuildTryGetSuccessTest(ushort arity)
    {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{successCall}};

                         if (r.TryGet(out var errors)) {
                             Assert.Null(errors);
                         }
                         else {
                             Assert.Fail("Expected success");
                         }
                         """;

            return new MethodWriter(
                name: "Success_TryGet_ReturnsTrue",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var outParams = GenerateTryGetOutParams(arity);
        var assertions = GenerateTryGetSuccessAssertions(arity);

        var body2 = $$"""
                      var r = {{successCall}};

                      if (r.TryGet({{outParams}})) {
                         {{assertions}}
                      }
                      else {
                          Assert.Fail("Expected success");
                      }
                      """;

        return new MethodWriter(
            name: "Success_Ok_ReturnsValue",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildTryGetFailureTest(ushort arity)
    {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{failureCall}};

                         if (!r.TryGet(out var errors)) {
                             Assert.NotNull(errors);
                             Assert.Single(errors);
                             Assert.Equal("boom", errors.First().Message);
                         }
                         else {
                             Assert.Fail("Expected failure");
                         }
                         """;

            return new MethodWriter(
                name: "Failure_TryGet_ReturnsErrorMessage",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var outParamsWithError = GenerateTryGetOutParamsWithError(arity);
        var wildcards = string.Join(", ", Enumerable.Repeat("out _", arity));

        var body2 = $$"""
                      var r = {{failureCall}};

                      // Test TryGet with both value and error out parameters
                      if (!r.TryGet({{outParamsWithError}})) {
                          Assert.NotNull(errors);
                          Assert.Single(errors);
                          Assert.Equal("boom", errors.First().Message);
                      }
                      else {
                          Assert.Fail("Expected failure");
                      }

                      // Test TryGet with just value out parameter
                      if (r.TryGet({{wildcards}})) {
                          Assert.Fail("Expected failure");
                      }

                      // Test TryGet with just error out parameter
                      if (!r.TryGet(out IEnumerable<IError>? errors2)) {
                          Assert.NotNull(errors2);
                          Assert.Single(errors2);
                          Assert.Equal("boom", errors2.First().Message);
                      }
                      else {
                          Assert.Fail("Expected failure");
                      }
                      """;

        return new MethodWriter(
            name: "Failure_Ok_ReturnsErrorMessage",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")],
            usings: ["UnambitiousFx.Core.Results", "UnambitiousFx.Core.Results.Reasons"]       
        );
    }

    private static string GenerateTryGetOutParams(ushort arity)
    {
        return string.Join(", ", Enumerable.Range(1, arity)
                                           .Select(i => $"out var value{i}"));
    }

    private static string GenerateTryGetOutParamsWithError(ushort arity)
    {
        var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(i => "out _"));
        return $"{valueParams}, out var errors";
    }

    private static string GenerateTryGetSuccessAssertions(ushort arity)
    {
        var result = "";
        for (int i = 1; i <= arity; i++)
        {
            result += $"    {ResultTestHelpers.GenerateValueAssertion(i, $"value{i}")}\n";
        }

        return result;
    }
}
