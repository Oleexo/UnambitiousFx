namespace UnambitiousFx.Core.CodeGen.Design;

[Flags]
internal enum MethodModifier {
    None     = 0,
    Virtual  = 1 << 0,
    Override = 1 << 1,
    Sealed   = 1 << 2,
    Static   = 1 << 3,
    Async    = 1 << 4
}
