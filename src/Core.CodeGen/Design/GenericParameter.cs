namespace UnambitiousFx.Core.CodeGen.Design;

/// <summary>
/// Represents a generic type parameter with optional constraints.
/// </summary>
/// <param name="Name">The name of the generic parameter.</param>
/// <param name="Constraints">The constraints applied to this parameter.</param>
/// <param name="Using">Optional using directive required for this parameter.</param>
internal sealed record GenericParameter(
    string Name,
    IReadOnlyList<GenericConstraint>? Constraints = null,
    string? Using = null)
{
    /// <summary>
    /// Backward-compatible constructor with constraint string.
    /// </summary>
    /// <param name="name">The name of the generic parameter.</param>
    /// <param name="constraint">The constraint string.</param>
    /// <param name="using">Optional using directive.</param>
    public GenericParameter(string name, string constraint, string? @using = null)
        : this(name, ParseConstraintString(name, constraint), @using)
    {
    }

    /// <summary>
    /// Creates a generic parameter with a single constraint (for backward compatibility).
    /// </summary>
    /// <param name="name">The name of the generic parameter.</param>
    /// <param name="constraint">The constraint string.</param>
    /// <param name="using">Optional using directive.</param>
    /// <returns>A new GenericParameter with the specified constraint.</returns>
    public static GenericParameter WithConstraint(string name, string constraint, string? @using = null)
    {
        // Parse the constraint string and create appropriate GenericConstraint objects
        var constraints = ParseConstraintString(name, constraint);
        return new GenericParameter(name, constraints, @using);
    }

    /// <summary>
    /// Gets the constraint string for this parameter (for backward compatibility).
    /// </summary>
    public string? Constraint => Constraints?.Count > 0 
        ? string.Join(", ", Constraints.Select(c => c.ToConstraintString()))
        : null;

    private static IReadOnlyList<GenericConstraint> ParseConstraintString(string parameterName, string constraintString)
    {
        if (string.IsNullOrWhiteSpace(constraintString))
            return Array.Empty<GenericConstraint>();

        var constraints = new List<GenericConstraint>();
        var parts = constraintString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            var constraint = part.Trim() switch
            {
                "class" => GenericConstraint.Class(parameterName),
                "struct" => GenericConstraint.Struct(parameterName),
                "notnull" => GenericConstraint.NotNull(parameterName),
                "unmanaged" => GenericConstraint.Unmanaged(parameterName),
                "new()" => GenericConstraint.New(parameterName),
                _ when part.StartsWith("I") && char.IsUpper(part[1]) => GenericConstraint.Interface(parameterName, part),
                _ => GenericConstraint.BaseClass(parameterName, part)
            };
            constraints.Add(constraint);
        }

        return constraints.ToArray();
    }
}