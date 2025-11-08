namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a metadata reference to an attribute in code generation.
/// </summary>
/// <remarks>
///     The <c>AttributeReference</c> can be used to describe an attribute with its name,
///     optional arguments, and optional namespace using directives.
/// </remarks>
public record AttributeReference(string Name,
                                 string? Arguments = null,
                                 string? Using = null);
