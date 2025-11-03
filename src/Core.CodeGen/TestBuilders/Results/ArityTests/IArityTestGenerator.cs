namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests;

/// <summary>
///     Interface for generating arity-specific test methods for Result types.
///     Extends the base ICodeGenerator to provide specialized test generation capabilities.
/// </summary>
internal interface IArityTestGenerator : ICodeGenerator {
    /// <summary>
    ///     Gets the name of the test class that will be generated.
    /// </summary>
    /// <returns>The test class name.</returns>
    string GetTestClassName();

    /// <summary>
    ///     Gets the required using statements for the generated test class.
    /// </summary>
    /// <returns>Collection of using statements.</returns>
    IEnumerable<string> GetRequiredUsings();

    /// <summary>
    ///     Gets the category of tests this generator produces.
    /// </summary>
    /// <returns>The test generation category.</returns>
    TestGenerationCategory GetCategory();
}
