namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Defines the different styles for generating properties.
/// </summary>
public enum PropertyStyle
{
    /// <summary>
    ///     Generates an auto-implemented property.
    /// </summary>
    AutoProperty,

    /// <summary>
    ///     Generates a property with an expression body.
    /// </summary>
    Expression,

    /// <summary>
    ///     Generates an override property.
    /// </summary>
    Override,

    /// <summary>
    ///     Generates an abstract property.
    /// </summary>
    Abstract
}
