namespace UnambitiousFx.Core.CodeGen.Design;

[Flags]
internal enum ClassModifier {
    None     = 0,
    Abstract = 1 << 0,
    Static   = 1 << 1,
    Sealed   = 1 << 2,
    Partial  = 1 << 3
}