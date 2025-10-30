using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Common;

/// <summary>
/// Provides utility methods for working with generic types in code generation.
/// </summary>
internal static class GenericTypeHelper
{
    /// <summary>
    /// Creates generic type parameters for a given arity.
    /// </summary>
    /// <param name="arity">Number of generic type parameters.</param>
    /// <param name="prefix">Prefix for type parameter names (e.g., "T" or "TValue").</param>
    /// <param name="constraint">Generic constraint (e.g., "notnull" or empty string).</param>
    /// <returns>Array of GenericParameter objects.</returns>
    public static GenericParameter[] CreateGenericParameters(int arity, string prefix = "T", string constraint = "")
    {
        return Enumerable.Range(1, arity)
            .Select(i => new GenericParameter($"{prefix}{i}", constraint))
            .ToArray();
    }

    /// <summary>
    /// Creates generic type parameters with ordinal names.
    /// </summary>
    /// <param name="arity">Number of generic type parameters.</param>
    /// <param name="prefix">Prefix for type parameter names (e.g., "T").</param>
    /// <param name="constraint">Generic constraint.</param>
    /// <returns>Array of GenericParameter objects with ordinal names.</returns>
    public static GenericParameter[] CreateOrdinalGenericParameters(int arity, string prefix = "T", string constraint = "notnull")
    {
        return Enumerable.Range(1, arity)
            .Select(i => new GenericParameter($"{prefix}{OrdinalHelper.GetOrdinalName(i)}", constraint))
            .ToArray();
    }

    /// <summary>
    /// Builds a comma-separated string of generic type names.
    /// </summary>
    /// <param name="arity">Number of generic types.</param>
    /// <param name="prefix">Prefix for type names (e.g., "T" or "TValue").</param>
    /// <returns>Comma-separated generic type string (e.g., "T1, T2, T3").</returns>
    public static string BuildGenericTypeString(int arity, string prefix = "T")
    {
        return string.Join(", ", Enumerable.Range(1, arity).Select(n => $"{prefix}{n}"));
    }

    /// <summary>
    /// Builds a tuple type string for multiple generic types.
    /// </summary>
    /// <param name="arity">Number of types in the tuple.</param>
    /// <param name="prefix">Prefix for type names.</param>
    /// <returns>Tuple type string (e.g., "(T1, T2, T3)").</returns>
    public static string BuildTupleTypeString(int arity, string prefix = "T")
    {
        if (arity == 1)
        {
            return $"{prefix}1";
        }
        return $"({BuildGenericTypeString(arity, prefix)})";
    }
}
