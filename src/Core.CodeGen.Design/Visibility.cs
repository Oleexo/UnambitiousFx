namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents the accessibility levels that can be applied to types and type members in C#.
/// </summary>
public enum Visibility {
    /// <summary>
    ///     The type or member can be accessed by any other code in the same assembly or another assembly that references it.
    /// </summary>
    Public,

    /// <summary>
    ///     The type or member can be accessed only by code in the same assembly.
    /// </summary>
    Internal,

    /// <summary>
    ///     The type or member can be accessed only by code in the same class or struct.
    /// </summary>
    Private,

    /// <summary>
    ///     The type or member can be accessed only by code in the same class, or in a class that is derived from that class.
    /// </summary>
    Protected,

    /// <summary>
    ///     The type or member can be accessed by any code in the assembly in which it's declared, or from within a derived
    ///     class in another assembly.
    /// </summary>
    ProtectedInternal,

    /// <summary>
    ///     The type or member can be accessed only within its declaring assembly, by code in the same class or in a type that
    ///     is derived from that class.
    /// </summary>
    PrivateProtected
}
