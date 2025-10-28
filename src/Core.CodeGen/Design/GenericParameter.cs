namespace UnambitiousFx.Core.CodeGen.Design;

internal sealed record GenericParameter(string  Name,
                                        string  Constraint,
                                        string? Using = null);