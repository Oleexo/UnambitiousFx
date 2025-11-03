using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Generators;
using UnambitiousFx.Core.CodeGen.Generators.ErrorHandling;
using UnambitiousFx.Core.CodeGen.Generators.SideEffects;
using UnambitiousFx.Core.CodeGen.Generators.Transformations;
using UnambitiousFx.Core.CodeGen.Generators.Validations;
using UnambitiousFx.Core.CodeGen.Generators.ValueAccess;
using UnambitiousFx.Core.CodeGen.TestBuilders.Results;

namespace UnambitiousFx.Core.CodeGen;

/// <summary>
///     Factory for creating code generators.
///     Implements Factory pattern for better maintainability and testability.
/// </summary>
internal static class CodeGeneratorFactory
{
    /// <summary>
    ///     Creates generators for OneOf types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateOneOfGenerators(string baseNamespace,
                                                                    FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
    {
        yield return new OneOfCodeGenerator(baseNamespace, fileOrganization);
        yield return new OneOfTestsGenerator(baseNamespace);
    }

    /// <summary>
    ///     Creates generators for Result types only.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateResultGenerators(string baseNamespace,
                                                                     FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
    {
        yield return new ResultCodeGenerator(baseNamespace, fileOrganization);


        // ValueAccess extensions
        yield return new ResultToNullableExtensionsCodeGenerator(baseNamespace);
        yield return new ResultValueOrExtensionsCodeGenerator(baseNamespace);
        yield return new ResultValueOrThrowExtensionsCodeGenerator(baseNamespace);
        yield return new ResultMatchExtensionsCodeGenerator(baseNamespace);

        // Validation extensions
        yield return new ResultEnsureExtensionsCodeGenerator(baseNamespace);

        // Transformation extensions
        yield return new ResultBindExtensionsCodeGenerator(baseNamespace);
        yield return new ResultMapExtensionsCodeGenerator(baseNamespace);
        yield return new ResultThenExtensionsCodeGenerator(baseNamespace);
        yield return new ResultTryExtensionsCodeGenerator(baseNamespace);
        yield return new ResultZipExtensionsCodeGenerator(baseNamespace);
        yield return new ResultFlattenExtensionsCodeGenerator(baseNamespace);

        // SideEffects extensions
        yield return new ResultTapExtensionsCodeGenerator(baseNamespace);
        yield return new ResultTapBothExtensionsCodeGenerator(baseNamespace);
        yield return new ResultTapErrorExtensionsCodeGenerator(baseNamespace);

        // ErrorHandling extensions
        yield return new ResultMapErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultMapErrorsExtensionsCodeGenerator(baseNamespace);
        yield return new ResultHasErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultFilterErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultHasExceptionExtensionsCodeGenerator(baseNamespace);
        yield return new ResultRecoveryExtensionsCodeGenerator(baseNamespace);
        yield return new ResultAppendErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultPrependErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultAccumulateExtensionsCodeGenerator(baseNamespace);
        yield return new ResultFindErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultMatchErrorExtensionsCodeGenerator(baseNamespace);
        yield return new ResultShapeErrorExtensionsCodeGenerator(baseNamespace);
    }

    /// <summary>
    ///     Creates test generators for Result types.
    /// </summary>
    public static IEnumerable<ICodeGenerator> CreateResultTestGenerators(string baseNamespace,
                                                                         FileOrganizationMode fileOrganization = FileOrganizationMode.SingleFile)
    {
        // Direct method specific generators
        yield return new ResultIfSuccessTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultIfFailureTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultTryGetTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultMatchTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);

        // Transformation method specific generators
        yield return new ResultMapTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultBindTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultThenTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultTryTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultFlattenTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultZipTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);

        // Validation method specific generators
        yield return new ResultEnsureTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);

        // Side effects method specific generators
        yield return new ResultTapTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultTapBothTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultTapErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);

        // Error handling method specific generators
        yield return new ResultMapErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultMapErrorsTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultPrependErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultAppendErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultHasErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultHasExceptionTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultFindErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultMatchErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultFilterErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultRecoverTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultTryPickErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultShapeErrorTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);

        // Value access method specific generators
        yield return new ResultValueOrTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultValueOrThrowTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
        yield return new ResultToNullableTestsGenerator(baseNamespace, FileOrganizationMode.SingleFile);
    }
}
