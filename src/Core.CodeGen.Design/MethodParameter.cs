namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a method parameter with its type and name.
/// </summary>
/// <param name="Type">The type of the parameter.</param>
/// <param name="Name">The name of the parameter.</param>
public sealed record MethodParameter(string Type,
                                     string Name);
