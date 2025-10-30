namespace UnambitiousFx.Core.CodeGen;

/// <summary>
/// Factory for creating code generators.
/// Implements Factory pattern for better maintainability and testability.
/// </summary>
internal static class CodeGeneratorFactory
{
    /// <summary>
    /// Creates all generators needed for code generation.
    /// </summary>
    /// <param name="baseNamespace">The base namespace for generated code.</param>
    /// <returns>Collection of code generators.</returns>
    public static IEnumerable<ICodeGenerator> CreateGenerators(string baseNamespace)
    {
        yield return new OneOfCodeGenerator(baseNamespace);
        yield return new OneOfTestsGenerator(baseNamespace);
        yield return new ResultCodeGenerator(baseNamespace);
        yield return new ResultTestGenerator(baseNamespace);
        yield return new ResultValueAccessExtensionsCodeGenerator(baseNamespace);
    }

    /// <summary>
    /// Creates generators for OneOf types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateOneOfGenerators(string baseNamespace)
    {
        yield return new OneOfCodeGenerator(baseNamespace);
        yield return new OneOfTestsGenerator(baseNamespace);
    }

    /// <summary>
    /// Creates generators for Result types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateResultGenerators(string baseNamespace)
    {
        yield return new ResultCodeGenerator(baseNamespace);
        yield return new ResultTestGenerator(baseNamespace);
        yield return new ResultValueAccessExtensionsCodeGenerator(baseNamespace);
    }
}
