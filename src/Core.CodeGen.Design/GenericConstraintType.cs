namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents the different types of generic constraints that can be applied to type parameters.
/// </summary>
public enum GenericConstraintType {
    /// <summary>
    ///     The type parameter must be a reference type (class constraint).
    /// </summary>
    Class,

    /// <summary>
    ///     The type parameter must be a non-nullable value type (struct constraint).
    /// </summary>
    Struct,

    /// <summary>
    ///     The type parameter must be a non-nullable type (notnull constraint).
    /// </summary>
    NotNull,

    /// <summary>
    ///     The type parameter must be an unmanaged type (unmanaged constraint).
    /// </summary>
    Unmanaged,

    /// <summary>
    ///     The type parameter must have a public parameterless constructor (new() constraint).
    /// </summary>
    New,

    /// <summary>
    ///     The type parameter must inherit from or be the specified base class.
    /// </summary>
    BaseClass,

    /// <summary>
    ///     The type parameter must implement the specified interface.
    /// </summary>
    Interface
}
