using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
/// Builds tests for Result.Match() pattern matching operations.
/// </summary>
internal static class MatchTestBuilder
{
    public static MethodWriter BuildMatchBaseWithoutResponseSuccessTest(ushort arity)
    {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);
        var body = $$"""
                     var r = {{successCall}};

                     r.Match(
                         success: () => { },
                         failure: _ => Assert.Fail("Expected success")
                     );
                     """;

        return new MethodWriter(
            name: "Success_MatchBaseWithoutResponse_CallsSuccessAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")],
            usings: ["UnambitiousFx.Core.Results"]
        );
    }

    public static MethodWriter BuildMatchBaseWithoutResponseFailureTest(ushort arity)
    {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);
        var body = $$"""
                     var r = {{failureCall}};

                     r.Match(
                         success: () => Assert.Fail("Expected failure"),
                         failure: e => Assert.Equal("boom", e.First().Message)
                     );
                     """;

        return new MethodWriter(
            name: "Failure_MatchBaseWithoutResponse_CallsFailureAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")],
            usings: ["UnambitiousFx.Core.Results"]
        );
    }

    public static MethodWriter BuildMatchBaseWithResponseSuccessTest(ushort arity)
    {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);
        var body = $$"""
                     var r = {{successCall}};

                     var v = r.Match(
                         success: () => { return 24; },
                         failure: _ => {
                             Assert.Fail("Expected success");
                             return 0;
                         }
                     );

                     Assert.Equal(24, v);
                     """;

        return new MethodWriter(
            name: "Success_MatchBaseWithResponse_CallsSuccessAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")],
            usings: ["UnambitiousFx.Core.Results"]
        );
    }

    public static MethodWriter BuildMatchBaseWithResponseFailureTest(ushort arity)
    {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);
        var body = $$"""
                     var r = {{failureCall}};

                     var msg = r.Match(
                         success: () => {
                             Assert.Fail("Expected failure");
                             return "";
                         },
                         failure: errors => {
                             Assert.NotNull(errors);
                             Assert.Single(errors);
                             var error = errors.First();
                             Assert.Equal("boom", error.Message);
                             Assert.Equal("EXCEPTION", error.Code);
                             Assert.NotNull(error.Exception);
                             return error.Message;
                         }
                     );

                     Assert.Equal("boom", msg);
                     """;

        return new MethodWriter(
            name: "Failure_MatchBaseWithResponse_CallsFailureAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")],
            usings: ["UnambitiousFx.Core.Results"]
        );
    }

    public static MethodWriter BuildMatchWithoutResponseSuccessTest(ushort arity)
    {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{successCall}};

                         var called = false;
                         r.Match(
                             success: () => { called = true; },
                             failure: _ => Assert.Fail("Expected success")
                         );

                         Assert.True(called);
                         """;

            return new MethodWriter(
                name: "Success_MatchWithoutResponse_CallsSuccessAction",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var valueParams = ResultTestHelpers.GenerateValueParameters(arity);
        var assertions = ResultTestHelpers.GenerateValueAssertions(arity);

        var body2 = $$"""
                      var r = {{successCall}};

                      r.Match(
                          success: ({{valueParams}}) => {
                      {{assertions}}                    Assert.True(true);
                          },
                          failure: _ => Assert.Fail("Expected success")
                      );
                      """;

        return new MethodWriter(
            name: "Success_MatchWithoutResponse_CallsSuccessAction",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildMatchWithoutResponseFailureTest(ushort arity)
    {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{failureCall}};

                         r.Match(
                             success: () => Assert.Fail("Expected failure"),
                             failure: e => Assert.Equal("boom", e.First().Message)
                         );
                         """;

            return new MethodWriter(
                name: "Failure_MatchWithoutResponse_CallsFailureAction",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var wildcards = ResultTestHelpers.GenerateWildcardParameters(arity);

        var body2 = $$"""
                      var r = {{failureCall}};

                      var called = false;
                      r.Match(
                          success: ({{wildcards}}) => Assert.Fail("Expected failure"),
                          failure: errors => {
                              called = true;
                              Assert.Single(errors);
                              Assert.Equal("boom", errors.First().Message);
                          }
                      );
                      Assert.True(called);
                      """;

        return new MethodWriter(
            name: "Failure_MatchWithoutResponse_CallsFailureAction",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildMatchWithResponseSuccessTest(ushort arity)
    {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{successCall}};

                         var v = r.Match(
                             success: () => 24,
                             failure: _ => {
                                 Assert.Fail("Expected success");
                                 return 0;
                             }
                         );

                         Assert.Equal(24, v);
                         """;

            return new MethodWriter(
                name: "Success_MatchWithResponse_CallsSuccessAction",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var valueParams = ResultTestHelpers.GenerateValueParameters(arity);
        var returnValue = arity == 1 ? "v" : "v1";
        var defaultValue = ResultTestHelpers.GetDefaultValue(1);
        var testValue = ResultTestHelpers.GetTestValue(1);

        var body2 = $$"""
                      var r = {{successCall}};

                      var e = r.Match(
                          success: ({{valueParams}}) => {{returnValue}},
                          failure: _ => {
                              Assert.Fail("Expected failure");
                              return {{defaultValue}};
                          }
                      );

                      Assert.Equal({{testValue}}, e);
                      """;

        return new MethodWriter(
            name: "Success_MatchWithResponse_CallsSuccessAction",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildMatchWithResponseFailureTest(ushort arity)
    {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);

        if (arity == 0)
        {
            var body = $$"""
                         var r = {{failureCall}};

                         var result = r.Match(
                             success: () => {
                                 Assert.Fail("Expected failure");
                                 return "";
                             },
                             failure: errors => errors.First().Message
                         );

                         Assert.Equal("boom", result);
                         """;

            return new MethodWriter(
                name: "Failure_MatchWithResponse_CallsFailureAction",
                returnType: "void",
                body: body,
                visibility: Visibility.Public,
                attributes: [new AttributeReference("Fact")]
            );
        }

        var wildcards = ResultTestHelpers.GenerateWildcardParameters(arity);

        var body2 = $$"""
                      var r = {{failureCall}};

                      var result = r.Match(
                          success: ({{wildcards}}) => {
                              Assert.Fail("Expected failure");
                              return "";
                          },
                          failure: errors => errors.First().Message
                      );

                      Assert.Equal("boom", result);
                      """;

        return new MethodWriter(
            name: "Failure_MatchWithResponse_CallsFailureAction",
            returnType: "void",
            body: body2,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }
}
