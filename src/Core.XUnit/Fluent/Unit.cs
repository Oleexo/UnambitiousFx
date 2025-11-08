namespace UnambitiousFx.Core.XUnit.Fluent;

/// <summary>
///     Represents a unit type indicating the absence of a meaningful value.
///     Commonly used in scenarios where a method or operation does not return a specific value but must provide a
///     placeholder type.
/// </summary>
public readonly struct Unit
{
    /// <summary>
    ///     Represents the singleton instance of the <see cref="Unit" /> type, used to denote
    ///     the absence of a value in fluent assertions.
    /// </summary>
    public static readonly Unit Value = new();
}
