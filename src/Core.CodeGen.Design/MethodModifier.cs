namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents method modifiers that can be applied to methods.
/// </summary>
[Flags]
public enum MethodModifier {
    /// <summary>
    ///     No modifier applied.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Virtual method modifier.
    /// </summary>
    Virtual = 1 << 0,

    /// <summary>
    ///     Override method modifier.
    /// </summary>
    Override = 1 << 1,

    /// <summary>
    ///     Sealed method modifier.
    /// </summary>
    Sealed = 1 << 2,

    /// <summary>
    ///     Static method modifier.
    /// </summary>
    Static = 1 << 3,

    /// <summary>
    ///     Async method modifier.
    /// </summary>
    Async = 1 << 4
}
