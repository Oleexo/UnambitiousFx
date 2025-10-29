using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.TestBuilders;

/// <summary>
/// Orchestrates the creation of a complete Result test class by coordinating all test builders.
/// </summary>
internal static class ResultTestClassBuilder {
    /// <summary>
    /// Builds a complete test class for a Result type with the specified arity.
    /// </summary>
    /// <param name="arity">The number of generic type parameters (1-8).</param>
    /// <returns>A ClassWriter configured with all test methods.</returns>
    public static ClassWriter Build(ushort arity) {
        if (arity < 1 ||
            arity > 8) {
            throw new ArgumentOutOfRangeException(nameof(arity), "Arity must be between 1 and 8.");
        }

        var classWriter = new ClassWriter(
            name: $"ResultArity{arity}Tests",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Sealed
        );

        // State tests
        classWriter.AddMethod(StateTestBuilder.BuildIsSuccessTrueTest(arity));
        classWriter.AddMethod(StateTestBuilder.BuildIsSuccessFalseTest(arity));

        // Match tests - base overloads (without value parameters)
        classWriter.AddMethod(MatchTestBuilder.BuildMatchBaseWithoutResponseSuccessTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchBaseWithoutResponseFailureTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchBaseWithResponseSuccessTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchBaseWithResponseFailureTest(arity));

        // Match tests - value overloads (with value parameters)
        classWriter.AddMethod(MatchTestBuilder.BuildMatchWithoutResponseSuccessTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchWithoutResponseFailureTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchWithResponseSuccessTest(arity));
        classWriter.AddMethod(MatchTestBuilder.BuildMatchWithResponseFailureTest(arity));

        // Conditional tests
        classWriter.AddMethod(ConditionalTestBuilder.BuildIfSuccessCallsActionTest(arity));
        classWriter.AddMethod(ConditionalTestBuilder.BuildIfSuccessDoesNotCallActionTest(arity));
        classWriter.AddMethod(ConditionalTestBuilder.BuildIfFailureDoesNotCallActionTest(arity));
        classWriter.AddMethod(ConditionalTestBuilder.BuildIfFailureCallsActionTest(arity));

        // Deconstruct tests
        classWriter.AddMethod(DeconstructTestBuilder.BuildDeconstructSuccessTest(arity));
        classWriter.AddMethod(DeconstructTestBuilder.BuildDeconstructFailureTest(arity));

        // TryGet tests
        classWriter.AddMethod(TryGetTestBuilder.BuildTryGetSuccessTest(arity));
        classWriter.AddMethod(TryGetTestBuilder.BuildTryGetFailureTest(arity));

        return classWriter;
    }
}
