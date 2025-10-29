using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.TestBuilders;

/// <summary>
/// Builds tests for Result state properties (IsSuccess, IsFaulted).
/// </summary>
internal static class StateTestBuilder {
    public static MethodWriter BuildIsSuccessTrueTest(ushort arity) {
        var successCall = ResultTestHelpers.GenerateSuccessCall(arity);
        var body = $$"""
                     var r = {{successCall}};

                     Assert.True(r.IsSuccess);
                     Assert.False(r.IsFaulted);
                     """;

        return new MethodWriter(
            name: "Success_IsSuccess_ReturnsTrue",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }

    public static MethodWriter BuildIsSuccessFalseTest(ushort arity) {
        var failureCall = ResultTestHelpers.GenerateFailureCall(arity);
        var body = $$"""
                     var r = {{failureCall}};

                     Assert.False(r.IsSuccess);
                     Assert.True(r.IsFaulted);
                     """;

        return new MethodWriter(
            name: "Failure_IsSuccess_ReturnsFalse",
            returnType: "void",
            body: body,
            visibility: Visibility.Public,
            attributes: [new AttributeReference("Fact")]
        );
    }
}
