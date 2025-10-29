using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.TestBuilders;

/// <summary>
/// Builds tests for Result.IfSuccess() and Result.IfFailure() conditional operations.
/// </summary>
internal static class ConditionalTestBuilder {
    public static MethodWriter BuildIfSuccessCallsActionTest(ushort arity) {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);
        var valueParams = ResultTestHelpers.GenerateValueParameters(arity);
        var assertions  = ResultTestHelpers.GenerateValueAssertions(arity, "    ");

        var body = $$"""
                     var r = {{successCall}};

                     var isCalled = false;
                     r.IfSuccess(({{valueParams}}) => {
                     {{assertions}}    isCalled = true;
                     });

                     Assert.True(isCalled);
                     isCalled = false;

                     r.IfSuccess(() => { isCalled = true; });
                     """;

        return new MethodWriter(
            name: "Success_IfSuccess_CallsSuccessAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildIfSuccessDoesNotCallActionTest(ushort arity) {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);
        var wildcards   = ResultTestHelpers.GenerateWildcardParameters(arity);

        var body = $"""
                    var r = {failureCall};

                    r.IfSuccess(({wildcards}) => Assert.Fail("Expected failure"));

                    r.IfSuccess(() => Assert.Fail("Expected failure"));
                    """;

        return new MethodWriter(
            name: "Failure_IfSuccess_DoesNotCallSuccessAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildIfFailureDoesNotCallActionTest(ushort arity) {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);

        var body = $"""
                    var r = {successCall};

                    r.IfFailure(_ => Assert.Fail("Expected success"));
                    """;

        return new MethodWriter(
            name: "Success_IfFailure_DoesNotCallFailureAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildIfFailureCallsActionTest(ushort arity) {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);

        var body = $$"""
                     var r = {{failureCall}};

                     var called = false;
                     r.IfFailure(errors => {
                         called = true;
                         Assert.Single(errors);
                         Assert.Equal("boom", errors.First().Message);
                     });
                     Assert.True(called);
                     """;

        return new MethodWriter(
            name: "Failure_IfFailure_CallsFailureAction",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }
}
