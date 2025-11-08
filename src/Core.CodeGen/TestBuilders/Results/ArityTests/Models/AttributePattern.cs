namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results.ArityTests.Models;

/// <summary>
///     Represents attribute patterns found in existing test files.
/// </summary>
internal sealed class AttributePattern
{
    /// <summary>
    ///     Initializes a new instance of the AttributePattern class.
    /// </summary>
    /// <param name="attributeName">The attribute name.</param>
    /// <param name="parameters">The attribute parameters.</param>
    /// <param name="usageCount">The usage count.</param>
    public AttributePattern(string attributeName,
                            IEnumerable<string> parameters,
                            int usageCount = 1)
    {
        AttributeName = attributeName ?? throw new ArgumentNullException(nameof(attributeName));
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        UsageCount = usageCount;
    }

    /// <summary>
    ///     Gets the name of the attribute.
    /// </summary>
    public string AttributeName { get; init; }

    /// <summary>
    ///     Gets the parameters used with the attribute.
    /// </summary>
    public IEnumerable<string> Parameters { get; init; }

    /// <summary>
    ///     Gets the usage frequency of this attribute pattern.
    /// </summary>
    public int UsageCount { get; init; }
}
