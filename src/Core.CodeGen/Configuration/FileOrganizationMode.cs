namespace UnambitiousFx.Core.CodeGen.Configuration;

/// <summary>
///     Defines how generated code files should be organized.
/// </summary>
internal enum FileOrganizationMode {
    /// <summary>
    ///     Each arity is generated in a separate file with pattern: *.{x}.g.cs
    ///     Example: Result.1.g.cs, Result.2.g.cs, etc.
    /// </summary>
    SeparateFiles,

    /// <summary>
    ///     All arities are generated in a single file with regions separating each arity.
    ///     Regions are formatted as: #region Arity {x}
    ///     Example: Single Result.g.cs file containing all arities.
    /// </summary>
    SingleFile
}
