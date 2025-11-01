namespace UnambitiousFx.Core.CodeGen.Common;

/// <summary>
/// Provides utility methods for working with test types and values.
/// </summary>
internal static class TestTypeHelper
{
    /// <summary>
    /// Gets the C# type name for a given position in test generation.
    /// </summary>
    /// <param name="position">The position (1-8).</param>
    /// <returns>The C# type name.</returns>
    public static string GetTestType(int position)
    {
        return position switch
        {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "decimal",
            6 => "long",
            7 => "char",
            8 => "float",
            _ => throw new ArgumentOutOfRangeException(nameof(position),
                $"Position must be between 1 and 8, but was {position}.")
        };
    }

    /// <summary>
    /// Gets a test value literal for a given position.
    /// </summary>
    /// <param name="position">The position (1-8).</param>
    /// <returns>The test value as a string literal.</returns>
    public static string GetTestValue(int position)
    {
        return position switch
        {
            1 => "42",
            2 => "\"hello\"",
            3 => "true",
            4 => "3.14",
            5 => "99.99m",
            6 => "1000L",
            7 => "'x'",
            8 => "2.5f",
            _ => throw new ArgumentOutOfRangeException(nameof(position),
                $"Position must be between 1 and 8, but was {position}.")
        };
    }

    /// <summary>
    /// Determines if a type at the given position can be tested with multiple values.
    /// </summary>
    /// <param name="position">The position (1-8).</param>
    /// <returns>True if the type supports multiple test values.</returns>
    public static bool IsValueTestable(int position)
    {
        return position switch
        {
            1 => true,  // int - can use Theory with InlineData
            2 => true,  // string - can use Theory with InlineData
            _ => false  // other types - use Fact with single value
        };
    }
}
