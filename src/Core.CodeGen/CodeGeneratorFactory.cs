using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Generators;
using UnambitiousFx.Core.CodeGen.Generators.Transformations;
using UnambitiousFx.Core.CodeGen.Generators.Validations;
using UnambitiousFx.Core.CodeGen.Generators.ValueAccess;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
/// Factory for creating code generators.
/// Implements Factory pattern for better maintainability and testability.
/// </summary>
internal static class CodeGeneratorFactory
{
    /// <summary>
    /// Creates generators for OneOf types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateOneOfGenerators(
        string baseNamespace,
        FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
    {
        yield return new OneOfCodeGenerator(baseNamespace, fileOrganization);
        yield return new OneOfTestsGenerator(baseNamespace);
    }

    /// <summary>
    /// Creates generators for Result types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateResultGenerators(
        string baseNamespace,
        FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
    {
        yield return new ResultCodeGenerator(baseNamespace, fileOrganization);
        yield return new ResultTestGenerator(baseNamespace);

        // ValueAccess extensions
        yield return new ResultToNullableExtensionsCodeGenerator(baseNamespace);
        yield return new ResultValueOrExtensionsCodeGenerator(baseNamespace);
        yield return new ResultValueOrThrowExtensionsCodeGenerator(baseNamespace);
        yield return new ResultMatchExtensionsCodeGenerator(baseNamespace);

        // Validation extensions
        yield return new ResultEnsureExtensionsCodeGenerator(baseNamespace);

        // Transformation extensions
        yield return new ResultMapExtensionsCodeGenerator(baseNamespace);
        yield return new ResultThenExtensionsCodeGenerator(baseNamespace);
        yield return new ResultTryExtensionsCodeGenerator(baseNamespace);
        yield return new ResultZipExtensionsCodeGenerator(baseNamespace);
    }
}
