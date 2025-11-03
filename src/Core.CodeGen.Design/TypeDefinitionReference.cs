namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a reference to a type definition, including its name and optional namespace.
/// </summary>
/// <param name="Name">The name of the type.</param>
/// <param name="Using">The namespace or using directive associated with the type, if any.</param>
public sealed record TypeDefinitionReference(string  Name,
                                             string? Using = null);
