namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents modifiers that can be applied to a class.
/// </summary>
[Flags]
public enum ClassModifier
{
    /// <summary>
    ///     No modifiers applied.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The class is abstract.
    /// </summary>
    Abstract = 1 << 0,

    /// <summary>
    ///     The class is static.
    /// </summary>
    Static = 1 << 1,

    /// <summary>
    ///     The class is sealed.
    /// </summary>
    Sealed = 1 << 2,

    /// <summary>
    ///     The class is partial.
    /// </summary>
    Partial = 1 << 3
}
