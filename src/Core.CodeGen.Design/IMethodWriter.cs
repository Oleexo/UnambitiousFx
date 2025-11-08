namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Defines a contract for writing method code with associated metadata.
/// </summary>
/// <remarks>
///     This interface provides the necessary components to generate method code including
///     required using statements, generic parameters, and the actual method implementation.
/// </remarks>
public interface IMethodWriter : ICodeWriter
{
    /// <summary>
    ///     Gets the collection of generic parameters associated with the method.
    /// </summary>
    /// <value>
    ///     An enumerable collection of <see cref="GenericParameter" /> objects representing
    ///     the generic type parameters for the method.
    /// </value>
    IEnumerable<GenericParameter> GenericParameters { get; }
}
