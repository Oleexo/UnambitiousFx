namespace UnambitiousFx.Core.CodeGen.Common;

/// <summary>
/// Provides utility methods for converting numeric positions to ordinal names.
/// </summary>
internal static class OrdinalHelper
{
    /// <summary>
    /// Converts a numeric position to its ordinal name.
    /// </summary>
    /// <param name="position">The numeric position (1-8).</param>
    /// <returns>The ordinal name (e.g., "First", "Second", etc.).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when position is not in range 1-8.</exception>
    public static string GetOrdinalName(int position)
    {
        return position switch
        {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            _ => throw new ArgumentOutOfRangeException(nameof(position),
                $"Position must be between 1 and 8, but was {position}.")
        };
    }
}
