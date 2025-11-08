namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
///     Represents a generic constraint that can be applied to a type parameter.
/// </summary>
/// <param name="ParameterName">The name of the type parameter this constraint applies to.</param>
/// <param name="Type">The type of constraint.</param>
/// <param name="TypeName">The name of the type for BaseClass or Interface constraints. Null for other constraint types.</param>
public sealed record GenericConstraint(string ParameterName,
                                       GenericConstraintType Type,
                                       string? TypeName = null)
{
    /// <summary>
    ///     Creates a class constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <returns>A new GenericConstraint representing a class constraint.</returns>
    public static GenericConstraint Class(string parameterName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.Class);
    }

    /// <summary>
    ///     Creates a struct constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <returns>A new GenericConstraint representing a struct constraint.</returns>
    public static GenericConstraint Struct(string parameterName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.Struct);
    }

    /// <summary>
    ///     Creates a notnull constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <returns>A new GenericConstraint representing a notnull constraint.</returns>
    public static GenericConstraint NotNull(string parameterName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.NotNull);
    }

    /// <summary>
    ///     Creates an unmanaged constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <returns>A new GenericConstraint representing an unmanaged constraint.</returns>
    public static GenericConstraint Unmanaged(string parameterName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.Unmanaged);
    }

    /// <summary>
    ///     Creates a new() constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <returns>A new GenericConstraint representing a new() constraint.</returns>
    public static GenericConstraint New(string parameterName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.New);
    }

    /// <summary>
    ///     Creates a base class constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <param name="baseClassName">The name of the base class.</param>
    /// <returns>A new GenericConstraint representing a base class constraint.</returns>
    public static GenericConstraint BaseClass(string parameterName,
                                              string baseClassName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.BaseClass, baseClassName);
    }

    /// <summary>
    ///     Creates an interface constraint for the specified type parameter.
    /// </summary>
    /// <param name="parameterName">The name of the type parameter.</param>
    /// <param name="interfaceName">The name of the interface.</param>
    /// <returns>A new GenericConstraint representing an interface constraint.</returns>
    public static GenericConstraint Interface(string parameterName,
                                              string interfaceName)
    {
        return new GenericConstraint(parameterName, GenericConstraintType.Interface, interfaceName);
    }

    /// <summary>
    ///     Gets the string representation of this constraint for use in C# code.
    /// </summary>
    /// <returns>The constraint string.</returns>
    public string ToConstraintString()
    {
        return Type switch
        {
            GenericConstraintType.Class => "class",
            GenericConstraintType.Struct => "struct",
            GenericConstraintType.NotNull => "notnull",
            GenericConstraintType.Unmanaged => "unmanaged",
            GenericConstraintType.New => "new()",
            GenericConstraintType.BaseClass => TypeName ?? throw new InvalidOperationException("BaseClass constraint requires TypeName"),
            GenericConstraintType.Interface => TypeName ?? throw new InvalidOperationException("Interface constraint requires TypeName"),
            _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "Unknown constraint type")
        };
    }
}
