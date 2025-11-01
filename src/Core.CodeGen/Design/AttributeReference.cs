namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed record AttributeReference(string  Name,
                                          string? Arguments = null,
                                          string? Using     = null);
